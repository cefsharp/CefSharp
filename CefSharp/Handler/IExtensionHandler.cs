// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle events related to browser extensions.
    /// The methods of this class will be called on the CEF UI thread.
    /// See <see cref="IRequestContext.LoadExtension"/> for information about extension loading.
    /// </summary>
    public interface IExtensionHandler
    {
        /// <summary>
        /// Called if the IRequestContext.LoadExtension request fails.
        /// </summary>
        /// <param name="errorCode">error code</param>
        void OnExtensionLoadFailed(CefErrorCode errorCode);

        /// <summary>
        /// Called if the IRequestContext.LoadExtension request succeeds.
        /// </summary>
        /// <param name="extension">is the loaded extension.</param>
        void OnExtensionLoaded(IExtension extension);

        /// <summary>
        /// Called after the IExtension.Unload request has completed.
        /// </summary>
        /// <param name="extension">is the unloaded extension</param>
        void OnExtensionUnloaded(IExtension extension);

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
        bool OnBeforeBackgroundBrowser(IExtension extension, string url, IBrowserSettings settings);

        ///
        // Called when an extension API (e.g. chrome.tabs.create) requests creation of
        // a new browser. |extension| and |browser| are the source of the API call.
        // |active_browser| may optionally be specified via the windowId property or
        // returned via the GetActiveBrowser() callback and provides the default
        // |client| and |settings| values for the new browser. |index| is the position
        // value optionally specified via the index property. |url| is the URL that
        // will be loaded in the browser. |active| is true if the new browser should
        // be active when opened.  To allow creation of the browser optionally modify
        // |windowInfo|, |client| and |settings| and return false. To cancel creation
        // of the browser return true. Successful creation will be indicated by a call
        // to CefLifeSpanHandler::OnAfterCreated. Any modifications to |windowInfo|
        // will be ignored if |active_browser| is wrapped in a CefBrowserView.
        ///
        bool OnBeforeBrowser(IExtension extension, IBrowser browser, IBrowser activeBrowser, int index, string url, bool active, IWindowInfo windowInfo, IBrowserSettings settings);

        ///
        // Called when no tabId is specified to an extension API call that accepts a
        // tabId parameter (e.g. chrome.tabs.*). |extension| and |browser| are the
        // source of the API call. Return the browser that will be acted on by the API
        // call or return NULL to act on |browser|. The returned browser must share
        // the same CefRequestContext as |browser|. Incognito browsers should not be
        // considered unless the source extension has incognito access enabled, in
        // which case |include_incognito| will be true.
        ///
        /*--cef()--*/
        IBrowser GetActiveBrowser(IExtension extension, IBrowser browser, bool includeIncognito);
        ///
        // Called when the tabId associated with |target_browser| is specified to an
        // extension API call that accepts a tabId parameter (e.g. chrome.tabs.*).
        // |extension| and |browser| are the source of the API call. Return true
        // to allow access of false to deny access. Access to incognito browsers
        // should not be allowed unless the source extension has incognito access
        // enabled, in which case |include_incognito| will be true.
        ///
        /*--cef()--*/
        bool CanAccessBrowser(IExtension extension, IBrowser browser, bool includeIncognito, IBrowser targetBrowser);

        ///
        // Called to retrieve an extension resource that would normally be loaded from
        // disk (e.g. if a file parameter is specified to chrome.tabs.executeScript).
        // |extension| and |browser| are the source of the resource request. |file| is
        // the requested relative file path. To handle the resource request return
        // true and execute |callback| either synchronously or asynchronously. For the
        // default behavior which reads the resource from the extension directory on
        // disk return false. Localization substitutions will not be applied to
        // resources handled via this method.
        ///
        /*--cef()--*/
        bool GetExtensionResource(IExtension extension, IBrowser browser, string file, IGetExtensionResourceCallback callback);
    }
}
