// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_extension.h"
#include "include\cef_extension_handler.h"
#include "BrowserSettings.h"
#include "CefExtensionWrapper.h"
#include "CefSharpBrowserWrapper.h"
#include "CefGetExtensionResourceCallbackWrapper.h"
#include "WindowInfo.h"

using namespace CefSharp;

namespace CefSharp
{
    namespace Internals
    {
        private class CefExtensionHandlerAdapter : public CefExtensionHandler
        {
            gcroot<IExtensionHandler^> _handler;

        public:
            CefExtensionHandlerAdapter(IExtensionHandler^ handler)
                : _handler(handler)
            {
            }

            ~CefExtensionHandlerAdapter()
            {
                delete _handler;
                _handler = nullptr;
            }

            ///
            // Called if the CefRequestContext::LoadExtension request fails. |result| will
            // be the error code.
            ///
            /*--cef()--*/
            void OnExtensionLoadFailed(cef_errorcode_t result) OVERRIDE
            {
                _handler->OnExtensionLoadFailed((CefErrorCode)result);
            }

            ///
            // Called if the CefRequestContext::LoadExtension request succeeds.
            // |extension| is the loaded extension.
            ///
            /*--cef()--*/
            void OnExtensionLoaded(CefRefPtr<CefExtension> extension) OVERRIDE
            {
                //TODO: Should this be auto disposed?
                _handler->OnExtensionLoaded(gcnew CefExtensionWrapper(extension));
            }

            ///
            // Called after the CefExtension::Unload request has completed.
            ///
            /*--cef()--*/
            void OnExtensionUnloaded(CefRefPtr<CefExtension> extension) OVERRIDE
            {
                //TODO: Add comment to interface saying extension is only valid within the scope
                //of this method as it's auto disposed
                CefExtensionWrapper wrapper(extension);
                _handler->OnExtensionUnloaded(%wrapper);
            }

            ///
            // Called when an extension needs a browser to host a background script
            // specified via the "background" manifest key. The browser will have no
            // visible window and cannot be displayed. |extension| is the extension that
            // is loading the background script. |url| is an internally generated
            // reference to an HTML page that will be used to load the background script
            // via a <script> src attribute. To allow creation of the browser optionally
            // modify |client| and |settings| and return false. To cancel creation of the
            // browser (and consequently cancel load of the background script) return
            // true. Successful creation will be indicated by a call to
            // CefLifeSpanHandler::OnAfterCreated, and CefBrowserHost::IsBackgroundHost
            // will return true for the resulting browser. See
            // https://developer.chrome.com/extensions/event_pages for more information
            // about extension background script usage.
            ///
            /*--cef()--*/
            bool OnBeforeBackgroundBrowser(CefRefPtr<CefExtension> extension,
                const CefString& url,
                CefRefPtr<CefClient>& client,
                CefBrowserSettings& settings) OVERRIDE
            {
                BrowserSettings browserSettingsWrapper(&settings);

                return _handler->OnBeforeBackgroundBrowser(gcnew CefExtensionWrapper(extension),
                    StringUtils::ToClr(url),
                    %browserSettingsWrapper);
            }

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
            /*--cef()--*/
            bool OnBeforeBrowser(CefRefPtr<CefExtension> extension,
                CefRefPtr<CefBrowser> browser,
                CefRefPtr<CefBrowser> active_browser,
                int index,
                const CefString& url,
                bool active,
                CefWindowInfo& windowInfo,
                CefRefPtr<CefClient>& client,
                CefBrowserSettings& settings) OVERRIDE
            {
                BrowserSettings browserSettingsWrapper(&settings);

                return _handler->OnBeforeBrowser(gcnew CefExtensionWrapper(extension),
                    gcnew CefSharpBrowserWrapper(browser),
                    gcnew CefSharpBrowserWrapper(active_browser),
                    index,
                    StringUtils::ToClr(url),
                    active,
                    gcnew WindowInfo(&windowInfo),
                    %browserSettingsWrapper);
            }

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
            CefRefPtr<CefBrowser> GetActiveBrowser(
                CefRefPtr<CefExtension> extension,
                CefRefPtr<CefBrowser> browser,
                bool includeIncognito) OVERRIDE
            {
                //TODO: Should extension be auto disposed?
                auto activeBrowser = _handler->GetActiveBrowser(gcnew CefExtensionWrapper(extension),
                    gcnew CefSharpBrowserWrapper(browser),
                    includeIncognito);

                if (activeBrowser == nullptr)
                {
                    return NULL;
                }

                //TODO: CLean this up
                auto wrapper = static_cast<CefSharpBrowserWrapper^>(activeBrowser);

                return wrapper->Browser.get();
            }

            ///
            // Called when the tabId associated with |target_browser| is specified to an
            // extension API call that accepts a tabId parameter (e.g. chrome.tabs.*).
            // |extension| and |browser| are the source of the API call. Return true
            // to allow access of false to deny access. Access to incognito browsers
            // should not be allowed unless the source extension has incognito access
            // enabled, in which case |include_incognito| will be true.
            ///
            /*--cef()--*/
            bool CanAccessBrowser(CefRefPtr<CefExtension> extension,
                CefRefPtr<CefBrowser> browser,
                bool includeIncognito,
                CefRefPtr<CefBrowser> target_browser) OVERRIDE
            {
                return _handler->CanAccessBrowser(gcnew CefExtensionWrapper(extension),
                    gcnew CefSharpBrowserWrapper(browser),
                    includeIncognito,
                    gcnew CefSharpBrowserWrapper(target_browser));
            }

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
            bool GetExtensionResource(
                CefRefPtr<CefExtension> extension,
                CefRefPtr<CefBrowser> browser,
                const CefString& file,
                CefRefPtr<CefGetExtensionResourceCallback> callback) OVERRIDE
            {
                return _handler->GetExtensionResource(gcnew CefExtensionWrapper(extension),
                    gcnew CefSharpBrowserWrapper(browser),
                    StringUtils::ToClr(file),
                    gcnew CefGetExtensionResourceCallbackWrapper(callback));
            }

            IMPLEMENT_REFCOUNTING(CefExtensionHandlerAdapter);
        };
    }
}
