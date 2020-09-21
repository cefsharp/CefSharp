// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Threading.Tasks;
using CefSharp.DevTools;
using CefSharp.Internals;
using CefSharp.Web;

namespace CefSharp
{
    /// <summary>
    /// Extensions for accessing DevTools through <see cref="IBrowserHost"/>
    /// </summary>
    public static class DevToolsExtensions
    {
        /// <summary>
        /// Execute a method call over the DevTools protocol. This is a more structured
        /// version of SendDevToolsMessage.
        /// See the DevTools protocol documentation at https://chromedevtools.github.io/devtools-protocol/ for details
        /// of supported methods and the expected <paramref name="paramsAsJson"/> dictionary contents.
        /// See the SendDevToolsMessage documentation for additional usage information.
        /// </summary>
        /// <param name="messageId">is an incremental number that uniquely identifies the message (pass 0 to have the next number assigned
        /// automatically based on previous values)</param>
        /// <param name="method">is the method name</param>
        /// <param name="parameters">are the method parameters represented as a <see cref="JsonString"/>,
        /// which may be empty.</param>
        /// <returns>return the assigned message Id if called on the CEF UI thread and the message was
        /// successfully submitted for validation, otherwise 0</returns>
        public static int ExecuteDevToolsMethod(this IBrowserHost browserHost, int messageId, string method, JsonString parameters)
        {
            WebBrowserExtensions.ThrowExceptionIfBrowserHostNull(browserHost);

            var json = parameters == null ? null : parameters.Json;

            return browserHost.ExecuteDevToolsMethod(messageId, method, json);
        }

        /// <summary>
        /// Execute a method call over the DevTools protocol. This is a more structured
        /// version of SendDevToolsMessage. <see cref="ExecuteDevToolsMethod"/> can only be called on the
        /// CEF UI Thread, this method can be called on any thread.
        /// See the DevTools protocol documentation at https://chromedevtools.github.io/devtools-protocol/ for details
        /// of supported methods and the expected <paramref name="parameters"/> dictionary contents.
        /// See the SendDevToolsMessage documentation for additional usage information.
        /// </summary>
        /// <param name="browser">the browser instance</param>
        /// <param name="messageId">is an incremental number that uniquely identifies the message (pass 0 to have the next number assigned
        /// automatically based on previous values)</param>
        /// <param name="method">is the method name</param>
        /// <param name="parameters">are the method parameters represented as a dictionary,
        /// which may be empty.</param>
        /// <returns>return a Task that can be awaited to obtain the assigned message Id. If the message was
        /// unsuccessfully submitted for validation, this value will be 0.</returns>
        public static Task<int> ExecuteDevToolsMethodAsync(this IBrowser browser, int messageId, string method, IDictionary<string, object> parameters = null)
        {
            WebBrowserExtensions.ThrowExceptionIfBrowserNull(browser);

            var browserHost = browser.GetHost();

            WebBrowserExtensions.ThrowExceptionIfBrowserHostNull(browserHost);

            if (CefThread.CurrentlyOnUiThread)
            {
                return Task.FromResult(browserHost.ExecuteDevToolsMethod(messageId, method, parameters));
            }

            if (CefThread.CanExecuteOnUiThread)
            {
                return CefThread.ExecuteOnUiThread(() =>
                {
                    return browserHost.ExecuteDevToolsMethod(messageId, method, parameters);
                });
            }

            //CEF returns 0 to signify failure, we'll do the same.
            return Task.FromResult(0);
        }

        /// <summary>
        /// Execute a method call over the DevTools protocol. This is a more structured
        /// version of SendDevToolsMessage. <see cref="ExecuteDevToolsMethod"/> can only be called on the
        /// CEF UI Thread, this method can be called on any thread.
        /// See the DevTools protocol documentation at https://chromedevtools.github.io/devtools-protocol/ for details
        /// of supported methods and the expected <paramref name="parameters"/> dictionary contents.
        /// See the SendDevToolsMessage documentation for additional usage information.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser instance</param>
        /// <param name="messageId">is an incremental number that uniquely identifies the message (pass 0 to have the next number assigned
        /// automatically based on previous values)</param>
        /// <param name="method">is the method name</param>
        /// <param name="parameters">are the method parameters represented as a dictionary,
        /// which may be empty.</param>
        /// <returns>return a Task that can be awaited to obtain the assigned message Id. If the message was
        /// unsuccessfully submitted for validation, this value will be 0.</returns>
        public static Task<int> ExecuteDevToolsMethodAsync(this IWebBrowser chromiumWebBrowser, int messageId, string method, IDictionary<string, object> parameters = null)
        {
            var browser = chromiumWebBrowser.GetBrowser();

            return browser.ExecuteDevToolsMethodAsync(messageId, method, parameters);
        }

        /// <summary>
        /// Gets a new Instance of the DevTools client for the chromiumWebBrowser
        /// instance.
        /// </summary>
        /// <param name="chromiumWebBrowser">the chromiumWebBrowser instance</param>
        /// <returns>DevToolsClient</returns>
        public static DevToolsClient GetDevToolsClient(this IWebBrowser chromiumWebBrowser)
        {
            var browser = chromiumWebBrowser.GetBrowser();

            return browser.GetDevToolsClient();
        }

        /// <summary>
        /// Gets a new Instance of the DevTools client 
        /// </summary>
        /// <param name="browser">the IBrowser instance</param>
        /// <returns>DevToolsClient</returns>
        public static DevToolsClient GetDevToolsClient(this IBrowser browser)
        {
            var browserHost = browser.GetHost();

            WebBrowserExtensions.ThrowExceptionIfBrowserHostNull(browserHost);

            var devToolsClient = new DevToolsClient(browser);

            var observerRegistration = browserHost.AddDevToolsMessageObserver(devToolsClient);

            devToolsClient.SetDevToolsObserverRegistration(observerRegistration);

            return devToolsClient;
        }
    }
}
