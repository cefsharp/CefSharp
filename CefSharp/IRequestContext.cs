// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CefSharp
{
    /// <summary>
    /// A request context provides request handling for a set of related browser or URL request objects.
    /// A request context can be specified when creating a new browser by setting the 
    /// <see cref="IWebBrowser.RequestContext"/> property (Passing in via the constructor for the OffScreen
    /// control is preferred).
    /// Browser objects with different request contexts will never be hosted in the same render process.
    /// Browser objects with the same request context may or may not be hosted in the same render process
    /// depending on the process model.Browser objects created indirectly via the JavaScript window.open
    /// function or targeted links will share the same render process and the same request context as
    /// the source browser.
    /// </summary>
    public interface IRequestContext : IDisposable
    {
        /// <summary>
        /// Returns true if this object is pointing to the same context object.
        /// </summary>
        /// <param name="context">context to compare</param>
        /// <returns>Returns true if the same</returns>
        bool IsSame(IRequestContext context);

        /// <summary>
        /// Returns true if this object is sharing the same storage as the specified context.
        /// </summary>
        /// <param name="context">context to compare</param>
        /// <returns>Returns true if same storage</returns>
        bool IsSharingWith(IRequestContext context);

        /// <summary>
        /// Returns true if this object is the global context. The global context is
        /// used by default when creating a browser or URL request with a NULL context
        /// argument.
        /// </summary>
        bool IsGlobal { get; }

        /// <summary>
        /// Returns the default cookie manager for this object. This will be the global
        /// cookie manager if this object is the global request context. Otherwise,
        /// this will be the default cookie manager used when this request context does
        /// not receive a value via IRequestContextHandler.GetCookieManager(). 
        /// </summary>
        /// <param name="callback">If callback is non-NULL it will be executed asnychronously on the CEF IO thread
        /// after the manager's storage has been initialized.</param>
        /// <returns>Returns the default cookie manager for this object</returns>
        ICookieManager GetDefaultCookieManager(ICompletionCallback callback);

        /// <summary>
        /// Register a scheme handler factory for the specified schemeName and optional domainName.
        /// An empty domainName value for a standard scheme will cause the factory to match all domain
        /// names. The domainName value will be ignored for non-standard schemes. If schemeName is
        /// a built-in scheme and no handler is returned by factory then the built-in scheme handler
        /// factory will be called. If schemeName is a custom scheme then you must also implement the
        /// CefApp::OnRegisterCustomSchemes() method in all processes. This function may be called multiple
        /// times to change or remove the factory that matches the specified schemeName and optional
        /// domainName.
        /// </summary>
        /// <param name="schemeName">Scheme Name</param>
        /// <param name="domainName">Optional domain name</param>
        /// <param name="factory">Scheme handler factory</param>
        /// <returns>Returns false if an error occurs.</returns>
        bool RegisterSchemeHandlerFactory(string schemeName, string domainName, ISchemeHandlerFactory factory);

        /// <summary>
        /// Clear all registered scheme handler factories. 
        /// </summary>
        /// <returns>Returns false on error.</returns>
        bool ClearSchemeHandlerFactories();

        /// <summary>
        /// Returns the cache path for this object. If empty an "incognito mode"
        /// in-memory cache is being used.
        /// </summary>
        string CachePath { get; }

        /// <summary>
        /// Tells all renderer processes associated with this context to throw away
        /// their plugin list cache. If reloadPages is true they will also reload
        /// all pages with plugins. RequestContextHandler.OnBeforePluginLoad may
        /// be called to rebuild the plugin list cache.
        /// </summary>
        /// <param name="reloadPages">reload any pages with pluginst</param>
        void PurgePluginListCache(bool reloadPages);

        /// <summary>
        /// Returns true if a preference with the specified name exists. This method
        /// must be called on the CEF UI thread.
        /// </summary>
        /// <param name="name">name of preference</param>
        /// <returns>bool if the preference exists</returns>
        /// <remarks>Use Cef.UIThreadTaskFactory to execute this method if required,
        /// Cef.OnContextInitialized and ChromiumWebBrowser.IsBrowserInitializedChanged are both
        /// executed on the CEF UI thread, so can be called directly.
        /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
        /// application thread will be the CEF UI thread.</remarks>
        bool HasPreference(string name);

        /// <summary>
        /// Returns the value for the preference with the specified name. Returns
        /// NULL if the preference does not exist. The returned object contains a copy
        /// of the underlying preference value and modifications to the returned object
        /// will not modify the underlying preference value. This method must be called
        /// on the CEF UI thread.
        /// </summary>
        /// <param name="name">preference name</param>
        /// <returns>Returns the value for the preference with the specified name</returns>
        /// <remarks>Use Cef.UIThreadTaskFactory to execute this method if required,
        /// Cef.OnContextInitialized and ChromiumWebBrowser.IsBrowserInitializedChanged are both
        /// executed on the CEF UI thread, so can be called directly.
        /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
        /// application thread will be the CEF UI thread.</remarks>
        object GetPreference(string name);

        /// <summary>
        /// Returns all preferences as a dictionary. The returned
        /// object contains a copy of the underlying preference values and
        /// modifications to the returned object will not modify the underlying
        /// preference values. This method must be called on the browser process UI
        /// thread.
        /// </summary>
        /// <param name="includeDefaults">If true then
        /// preferences currently at their default value will be included.</param>
        /// <returns>Preferences (dictionary can have sub dictionaries)</returns>
        IDictionary<string, object> GetAllPreferences(bool includeDefaults);

        /// <summary>
        /// Returns true if the preference with the specified name can be modified
        /// using SetPreference. As one example preferences set via the command-line
        /// usually cannot be modified. This method must be called on the CEF UI thread.
        /// </summary>
        /// <param name="name">preference key</param>
        /// <returns>Returns true if the preference with the specified name can be modified
        /// using SetPreference</returns>
        /// <remarks>Use Cef.UIThreadTaskFactory to execute this method if required,
        /// Cef.OnContextInitialized and ChromiumWebBrowser.IsBrowserInitializedChanged are both
        /// executed on the CEF UI thread, so can be called directly.
        /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
        /// application thread will be the CEF UI thread.</remarks>
        bool CanSetPreference(string name);

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
        /// /// <remarks>Use Cef.UIThreadTaskFactory to execute this method if required,
        /// Cef.OnContextInitialized and ChromiumWebBrowser.IsBrowserInitializedChanged are both
        /// executed on the CEF UI thread, so can be called directly.
        /// When CefSettings.MultiThreadedMessageLoop == false (the default is true) then the main
        /// application thread will be the CEF UI thread.</remarks>
        bool SetPreference(string name, object value, out string error);

        /// <summary>
        /// Clears all certificate exceptions that were added as part of handling
        /// <see cref="IRequestHandler.OnCertificateError"/>. If you call this it is
        /// recommended that you also call <see cref="IRequestContext.CloseAllConnections"/> or you risk not
        /// being prompted again for server certificates if you reconnect quickly.
        /// </summary>
        /// <param name="callback">If is non-NULL it will be executed on the CEF UI thread after
        /// completion. This param is optional</param>
        void ClearCertificateExceptions(ICompletionCallback callback);

        /// <summary>
        /// Clears all active and idle connections that Chromium currently has.
        /// This is only recommended if you have released all other CEF objects but
        /// don't yet want to call Cef.Shutdown().
        /// </summary>
        /// <param name="callback">If is non-NULL it will be executed on the CEF UI thread after
        /// completion. This param is optional</param>
        void CloseAllConnections(ICompletionCallback callback);

        /// <summary>
        /// Attempts to resolve origin to a list of associated IP addresses.
        /// </summary>
        /// <param name="origin">host name to resolve</param>
        /// <returns>A task that represents the Resoolve Host operation. The value of the TResult parameter contains ResolveCallbackResult.</returns>
        Task<ResolveCallbackResult> ResolveHostAsync(Uri origin);

        /// <summary>
        /// Attempts to resolve origin to a list of associated IP addresses using
        /// cached data. This method must be called on the CEF IO thread. Use
        /// Cef.IOThreadTaskFactory to execute on that thread.
        /// </summary>
        /// <param name="origin">host name to resolve</param>
        /// <param name="resolvedIpAddresses">list of resolved IP
        /// addresses or empty list if no cached data is available.</param>
        /// <returns> Returns <see cref="CefErrorCode.None"/> on success</returns>
        CefErrorCode ResolveHostCached(Uri origin, out IList<string> resolvedIpAddresses);
    }
}
