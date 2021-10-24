// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Handler
{
    /// <summary>
    /// Implement this interface to handle events related to browser extensions.
    /// The methods of this class will be called on the CEF UI thread.
    /// See <see cref="IRequestContext.LoadExtension"/> for information about extension loading.
    /// </summary>
    public class ExtensionHandler : IExtensionHandler
    {
        private bool isDisposed;

        /// <summary>
        /// Called if the <see cref="IRequestContext.LoadExtension"/> request fails.
        /// </summary>
        /// <param name="errorCode">error code</param>
        void IExtensionHandler.OnExtensionLoadFailed(CefErrorCode errorCode)
        {
            OnExtensionLoadFailed(errorCode);
        }

        /// <summary>
        /// Called if the <see cref="IRequestContext.LoadExtension"/> request fails.
        /// </summary>
        /// <param name="errorCode">error code</param>
        protected virtual void OnExtensionLoadFailed(CefErrorCode errorCode)
        {

        }

        /// <summary>
        /// Called if the <see cref="IRequestContext.LoadExtension"/> request succeeds.
        /// </summary>
        /// <param name="extension">is the loaded extension.</param>
        void IExtensionHandler.OnExtensionLoaded(IExtension extension)
        {
            OnExtensionLoaded(extension);
        }

        /// <summary>
        /// Called if the <see cref="IRequestContext.LoadExtension"/> request succeeds.
        /// </summary>
        /// <param name="extension">is the loaded extension.</param>
        protected virtual void OnExtensionLoaded(IExtension extension)
        {

        }

        /// <summary>
        /// Called after the IExtension.Unload request has completed.
        /// </summary>
        /// <param name="extension">is the unloaded extension</param>
        void IExtensionHandler.OnExtensionUnloaded(IExtension extension)
        {
            OnExtensionUnloaded(extension);
        }

        /// <summary>
        /// Called after the IExtension.Unload request has completed.
        /// </summary>
        /// <param name="extension">is the unloaded extension</param>
        protected virtual void OnExtensionUnloaded(IExtension extension)
        {

        }

        /// <summary>
        /// Called when an extension needs a browser to host a background script specified via the "background" manifest key.
        /// The browser will have no visible window and cannot be displayed. To allow creation of the browser optionally
        /// modify newBrowser and settings and return false. To cancel creation of the browser
        /// (and consequently cancel load of the background script) return  true. Successful creation will be indicated by a call to
        /// ILifeSpanHandler.OnAfterCreated, and IBrowserHost.IsBackgroundHost
        /// will return true for the resulting browser. See https://developer.chrome.com/extensions/event_pages for more information
        /// about extension background script usage.
        /// </summary>
        /// <param name="extension">is the extension that is loading the background script</param>
        /// <param name="url">is an internally generated reference to an HTML page that will be used to
        /// load the background script via a script src attribute</param>
        /// <param name="settings">browser settings</param>
        /// <returns>To cancel creation of the browser (and consequently cancel load of the background script) return true, otherwise return false.</returns>
        bool IExtensionHandler.OnBeforeBackgroundBrowser(IExtension extension, string url, IBrowserSettings settings)
        {
            return OnBeforeBackgroundBrowser(extension, url, settings);
        }

        /// <summary>
        /// Called when an extension needs a browser to host a background script specified via the "background" manifest key.
        /// The browser will have no visible window and cannot be displayed. To allow creation of the browser optionally
        /// modify newBrowser and settings and return false. To cancel creation of the browser
        /// (and consequently cancel load of the background script) return  true. Successful creation will be indicated by a call to
        /// ILifeSpanHandler.OnAfterCreated, and IBrowserHost.IsBackgroundHost
        /// will return true for the resulting browser. See https://developer.chrome.com/extensions/event_pages for more information
        /// about extension background script usage.
        /// </summary>
        /// <param name="extension">is the extension that is loading the background script</param>
        /// <param name="url">is an internally generated reference to an HTML page that will be used to
        /// load the background script via a script src attribute</param>
        /// <param name="settings">browser settings</param>
        /// <returns>To cancel creation of the browser (and consequently cancel load of the background script) return true, otherwise return false.</returns>
        protected virtual bool OnBeforeBackgroundBrowser(IExtension extension, string url, IBrowserSettings settings)
        {
            return false;
        }

        /// <summary>
        /// Called when an extension API (e.g. chrome.tabs.create) requests creation of a new browser.
        /// Successful creation will be indicated by a call to <see cref="ILifeSpanHandler.OnAfterCreated"/>.
        /// </summary>
        /// <param name="extension">the source of the API call</param>
        /// <param name="browser">the source of the API call</param>
        /// <param name="activeBrowser">may optionally be specified via the windowId property or
        /// returned via the GetActiveBrowser() callback and provides the default for the new browser</param>
        /// <param name="index">is the position value optionally specified via the index property</param>
        /// <param name="url">is the URL that will be loaded in the browser</param>
        /// <param name="active">is true if the new browser should be active when opened</param>
        /// <param name="windowInfo">optionally modify if you are going to allow creation of the browser</param>
        /// <param name="settings">optionally modify browser settings</param>
        /// <returns>To cancel creation of the browser return true. To allow creation return false and optionally modify windowInfo and settings</returns>
        bool IExtensionHandler.OnBeforeBrowser(IExtension extension, IBrowser browser, IBrowser activeBrowser, int index, string url, bool active, IWindowInfo windowInfo, IBrowserSettings settings)
        {
            return OnBeforeBrowser(extension, browser, activeBrowser, index, url, active, windowInfo, settings);
        }

        /// <summary>
        /// Called when an extension API (e.g. chrome.tabs.create) requests creation of a new browser.
        /// Successful creation will be indicated by a call to <see cref="ILifeSpanHandler.OnAfterCreated"/>.
        /// </summary>
        /// <param name="extension">the source of the API call</param>
        /// <param name="browser">the source of the API call</param>
        /// <param name="activeBrowser">may optionally be specified via the windowId property or
        /// returned via the GetActiveBrowser() callback and provides the default for the new browser</param>
        /// <param name="index">is the position value optionally specified via the index property</param>
        /// <param name="url">is the URL that will be loaded in the browser</param>
        /// <param name="active">is true if the new browser should be active when opened</param>
        /// <param name="windowInfo">optionally modify if you are going to allow creation of the browser</param>
        /// <param name="settings">optionally modify browser settings</param>
        /// <returns>To cancel creation of the browser return true. To allow creation return false and optionally modify windowInfo and settings</returns>
        protected virtual bool OnBeforeBrowser(IExtension extension, IBrowser browser, IBrowser activeBrowser, int index, string url, bool active, IWindowInfo windowInfo, IBrowserSettings settings)
        {
            return false;
        }

        /// <summary>
        /// Called when no tabId is specified to an extension API call that accepts a tabId parameter (e.g. chrome.tabs.*).
        /// </summary>
        /// <param name="extension">extension the call originates from</param>
        /// <param name="browser">browser the call originates from</param>
        /// <param name="includeIncognito">Incognito browsers should not be considered unless the source extension has incognito
        /// access enabled, inwhich case this will be true</param>
        /// <returns>Return the browser that will be acted on by the API call or return null to act on <paramref name="browser"/>.
        /// The returned browser must share the same IRequestContext as <paramref name="browser"/></returns>
        IBrowser IExtensionHandler.GetActiveBrowser(IExtension extension, IBrowser browser, bool includeIncognito)
        {
            return GetActiveBrowser(extension, browser, includeIncognito);
        }

        /// <summary>
        /// Called when no tabId is specified to an extension API call that accepts a tabId parameter (e.g. chrome.tabs.*).
        /// </summary>
        /// <param name="extension">extension the call originates from</param>
        /// <param name="browser">browser the call originates from</param>
        /// <param name="includeIncognito">Incognito browsers should not be considered unless the source extension has incognito
        /// access enabled, inwhich case this will be true</param>
        /// <returns>Return the browser that will be acted on by the API call or return null to act on <paramref name="browser"/>.
        /// The returned browser must share the same IRequestContext as <paramref name="browser"/></returns>
        protected virtual IBrowser GetActiveBrowser(IExtension extension, IBrowser browser, bool includeIncognito)
        {
            return null;
        }

        /// <summary>
        /// Called when the tabId associated with <paramref name="targetBrowser"/> is specified to an extension API call that accepts a tabId
        /// parameter (e.g. chrome.tabs.*).
        /// </summary>
        /// <param name="extension">extension the call originates from</param>
        /// <param name="browser">browser the call originates from</param>
        /// <param name="includeIncognito">Access to incognito browsers should not be allowed unless the source extension has
        /// incognito access
        /// enabled, in which case this will be true.</param>
        /// <param name="targetBrowser"></param>
        /// <returns>Return true to allow access of false to deny access.</returns>
        bool IExtensionHandler.CanAccessBrowser(IExtension extension, IBrowser browser, bool includeIncognito, IBrowser targetBrowser)
        {
            return CanAccessBrowser(extension, browser, includeIncognito, targetBrowser);
        }

        /// <summary>
        /// Called when the tabId associated with <paramref name="targetBrowser"/> is specified to an extension API call that accepts a tabId
        /// parameter (e.g. chrome.tabs.*).
        /// </summary>
        /// <param name="extension">extension the call originates from</param>
        /// <param name="browser">browser the call originates from</param>
        /// <param name="includeIncognito">Access to incognito browsers should not be allowed unless the source extension has
        /// incognito access
        /// enabled, in which case this will be true.</param>
        /// <param name="targetBrowser"></param>
        /// <returns>Return true to allow access of false to deny access.</returns>
        protected virtual bool CanAccessBrowser(IExtension extension, IBrowser browser, bool includeIncognito, IBrowser targetBrowser)
        {
            return false;
        }

        /// <summary>
        /// Called to retrieve an extension resource that would normally be loaded from disk
        /// (e.g. if a file parameter is specified to chrome.tabs.executeScript).
        /// Localization substitutions will not be applied to resources handled via this method.
        /// </summary>
        /// <param name="extension">extension the call originates from</param>
        /// <param name="browser">browser the call originates from</param>
        /// <param name="file">is the requested relative file path.</param>
        /// <param name="callback">callback used to handle custom resource requests</param>
        /// <returns>To handle the resource request return true and execute <paramref name="callback"/> either synchronously or asynchronously.
        /// For the default behavior which reads the resource from the extension directory on disk return false</returns>
        bool IExtensionHandler.GetExtensionResource(IExtension extension, IBrowser browser, string file, IGetExtensionResourceCallback callback)
        {
            return GetExtensionResource(extension, browser, file, callback);
        }

        /// <summary>
        /// Called to retrieve an extension resource that would normally be loaded from disk
        /// (e.g. if a file parameter is specified to chrome.tabs.executeScript).
        /// Localization substitutions will not be applied to resources handled via this method.
        /// </summary>
        /// <param name="extension">extension the call originates from</param>
        /// <param name="browser">browser the call originates from</param>
        /// <param name="file">is the requested relative file path.</param>
        /// <param name="callback">callback used to handle custom resource requests</param>
        /// <returns>To handle the resource request return true and execute <paramref name="callback"/> either synchronously or asynchronously.
        /// For the default behavior which reads the resource from the extension directory on disk return false</returns>
        protected virtual bool GetExtensionResource(IExtension extension, IBrowser browser, string file, IGetExtensionResourceCallback callback)
        {
            return false;
        }

        /// <summary>
        /// IsDisposed
        /// </summary>
        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        protected virtual void Dispose(bool disposing)
        {
            isDisposed = true;
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
