// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CefSharp.Internals;
using CefSharp.Preferences;

namespace CefSharp
{
    /// <summary>
    /// RequestContext extensions.
    /// </summary>
    public static class RequestContextExtensions
    {
        /// <summary>
        /// Array of valid proxy schemes
        /// </summary>
        private static string[] ProxySchemes = new string[] { "http", "socks", "socks4", "socks5" };

        /// <summary>
        /// Load an extension from the given directory. To load a crx file you must unzip it first.
        /// For further details see <seealso cref="IRequestContext.LoadExtension(string, string, IExtensionHandler)"/>
        /// </summary>
        /// <param name="requestContext">request context</param>
        /// <param name="rootDirectory">absolute path to the directory that contains the extension to be loaded.</param>
        /// <param name="handler">handle events related to browser extensions</param>
        public static void LoadExtensionFromDirectory(this IRequestContext requestContext, string rootDirectory, IExtensionHandler handler)
        {
            requestContext.LoadExtension(Path.GetFullPath(rootDirectory), null, handler);
        }

        /// <summary>
        /// Load extension(s) from the given directory. This methods obtains all the sub directories of <paramref name="rootDirectory"/>
        /// and calls <see cref="IRequestContext.LoadExtension(string, string, IExtensionHandler)"/> if manifest.json
        /// is found in the sub folder. To load crx file(s) you must unzip them first.
        /// For further details see <seealso cref="IRequestContext.LoadExtension(string, string, IExtensionHandler)"/>
        /// </summary>
        /// <param name="requestContext">request context</param>
        /// <param name="rootDirectory">absolute path to the directory that contains the extension(s) to be loaded.</param>
        /// <param name="handler">handle events related to browser extensions</param>
        public static void LoadExtensionsFromDirectory(this IRequestContext requestContext, string rootDirectory, IExtensionHandler handler)
        {
            var fullPath = Path.GetFullPath(rootDirectory);

            foreach (var dir in Directory.GetDirectories(fullPath))
            {
                //Check the directory has a manifest.json, if so call load
                if (File.Exists(Path.Combine(dir, "manifest.json")))
                {
                    requestContext.LoadExtension(dir, null, handler);
                }
            }
        }

        /// <summary>
        /// Set the value associated with preference name. If value is null the
        /// preference will be restored to its default value. If setting the preference
        /// fails then error will be populated with a detailed description of the
        /// problem. This method must be called on the CEF UI thread.
        /// Preferences set via the command-line usually cannot be modified.
        /// </summary>
        /// <param name="name">preference key</param>
        /// <param name="value">preference value</param>
        /// <param name="error">out error</param>
        /// <returns>Returns true if the value is set successfully and false otherwise.</returns>
        /// <remarks>Use Cef.UIThreadTaskFactory to execute this method if required,
        /// <see cref="IBrowserProcessHandler.OnContextInitialized"/> and ChromiumWebBrowser.IsBrowserInitializedChanged are both
        /// executed on the CEF UI thread, so can be called directly.
        /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
        /// application thread will be the CEF UI thread.</remarks>
        public static Task<SetPreferenceResponse> SetPreferenceAsync(this IRequestContext requestContext, string name, object value)
        {
            if (CefThread.HasShutdown)
            {
                return Task.FromResult(new SetPreferenceResponse(false, "Cef.Shutdown has already been called, it is no longer possible to call SetPreferenceAsync."));
            }

            Func<SetPreferenceResponse> func = () =>
            {
                string error;
                var success = requestContext.SetPreference(name, value, out error);

                return new SetPreferenceResponse(success, error);
            };

            if (CefThread.CurrentlyOnUiThread)
            {
                return Task.FromResult(func());
            }

            return CefThread.ExecuteOnUiThread(func);
        }

        /// <summary>
        /// Sets the proxy server for the specified <see cref="IRequestContext"/>
        /// MUST be called on the CEF UI Thread
        /// </summary>
        /// <param name="requestContext">request context</param>
        /// <param name="scheme">is the protocol of the proxy server, and is one of: 'http', 'socks', 'socks4', 'socks5'. Also note that 'socks' is equivalent to 'socks5'.</param>
        /// <param name="host">proxy host</param>
        /// <param name="port">proxy port</param>
        /// <param name="errorMessage">error message</param>
        /// <returns>returns true if successfull, false otherwise.</returns>
        /// <remarks>Internally calls <seealso cref="IRequestContext.SetPreference(string, object, out string)"/> with
        /// preference 'proxy' and mode of 'fixed_servers'</remarks>
        public static bool SetProxy(this IRequestContext requestContext, string scheme, string host, int? port, out string errorMessage)
        {
            var v = GetProxyDictionary(scheme, host, port);

            if (requestContext.CanSetPreference("proxy"))
            {
                return requestContext.SetPreference("proxy", v, out errorMessage);
            }

            throw new Exception("Unable to set the proxy preference, it is read-only. If you specified the proxy settings with command line args it is not possible to change the proxy settings via this method.");
        }

        /// <summary>
        /// Sets the proxy server for the specified <see cref="IRequestContext"/>.
        /// Protocol for the proxy server is http
        /// MUST be called on the CEF UI Thread
        /// </summary>
        /// <param name="requestContext">request context</param>
        /// <param name="host">proxy host</param>
        /// <param name="port">proxy port</param>
        /// <param name="errorMessage">error message</param>
        /// <returns>returns true if successfull, false otherwise.</returns>
        /// <remarks>Internally calls <seealso cref="IRequestContext.SetPreference(string, object, out string)"/> with
        /// preference 'proxy' and mode of 'fixed_servers'</remarks>
        public static bool SetProxy(this IRequestContext requestContext, string host, int? port, out string errorMessage)
        {
            return requestContext.SetProxy(null, host, port, out errorMessage);
        }

        /// <summary>
        /// Sets the proxy server for the specified <see cref="IRequestContext"/>.
        /// Protocol for the proxy server is http
        /// MUST be called on the CEF UI Thread
        /// </summary>
        /// <param name="requestContext">request context</param>
        /// <param name="host">proxy host</param>
        /// <param name="errorMessage">error message</param>
        /// <returns>returns true if successfull, false otherwise.</returns>
        /// <remarks>Internally calls <seealso cref="IRequestContext.SetPreference(string, object, out string)"/> with
        /// preference 'proxy' and mode of 'fixed_servers'</remarks>
        public static bool SetProxy(this IRequestContext requestContext, string host, out string errorMessage)
        {
            return requestContext.SetProxy(null, host, null, out errorMessage);
        }

        /// <summary>
        /// Creates a Dictionary that can be used with <see cref="IRequestContext.SetPreference(string, object, out string)"/>
        /// </summary>
        /// <param name="scheme">is the protocol of the proxy server, and is one of: 'http', 'socks', 'socks4', 'socks5'. Also note that 'socks' is equivalent to 'socks5'.</param>
        /// <param name="host">proxy host</param>
        /// <param name="port">proxy port</param>
        /// <returns></returns>
        public static IDictionary<string, object> GetProxyDictionary(string scheme, string host, int? port)
        {
            //Default to using http scheme if non provided
            if (string.IsNullOrWhiteSpace(scheme))
            {
                scheme = "http";
            }

            if (!ProxySchemes.Contains(scheme.ToLower()))
            {
                throw new ArgumentException("Invalid Scheme, see https://bitbucket.org/chromiumembedded/cef/wiki/GeneralUsage.md#markdown-header-proxy-resolution for a list of valid schemes.", "scheme");
            }

            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentException("Cannot be null or empty", "host");
            }

            if (port.HasValue && (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort))
            {
                throw new ArgumentOutOfRangeException("port", port, "Invalid TCP Port");
            }

            var dict = new Dictionary<string, object>
            {
                ["mode"] = "fixed_servers",
                ["server"] = scheme + "://" + host + (port.HasValue ? (":" + port) : "")
            };

            return dict;
        }

        /// <summary>
        /// Clears all HTTP authentication credentials that were added as part of handling
        /// <see cref="IRequestHandler.GetAuthCredentials(IWebBrowser, IBrowser, string, bool, string, int, string, string, IAuthCallback)"/>.
        /// </summary>
        /// <param name="requestContext">request context</param>
        /// <returns>A task that represents the ClearHttpAuthCredentials operation.
        /// Result indicates if the credentials cleared successfully.</returns>
        public static Task<bool> ClearHttpAuthCredentialsAsync(this IRequestContext requestContext)
        {
            var handler = new TaskCompletionCallback();

            requestContext.ClearHttpAuthCredentials(handler);

            return handler.Task;
        }
    }
}
