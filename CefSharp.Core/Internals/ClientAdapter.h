﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include <list>
#include "include/cef_app.h"
#include "include/cef_client.h"
#include "include/cef_render_process_handler.h"
#include "include/internal/cef_types.h"

using namespace System;

namespace CefSharp
{
    namespace Internals
    {
        private class ClientAdapter : public CefClient,
            public CefLifeSpanHandler,
            public CefLoadHandler,
            public CefRequestHandler,
            public CefDisplayHandler,
            public CefContextMenuHandler,
            public CefFocusHandler,
            public CefKeyboardHandler,
            public CefJSDialogHandler,
            public CefDialogHandler,
            public CefDragHandler,
            public CefGeolocationHandler
        {
        private:
            gcroot<IWebBrowserInternal^> _browserControl;
            gcroot<Action<int>^> _onAfterBrowserCreated;
            HWND _browserHwnd;
            CefRefPtr<CefBrowser> _cefBrowser;
            std::vector<CefRefPtr<CefBrowser>> _popupBrowsers;

            gcroot<String^> _tooltip;

        public:
            ClientAdapter(IWebBrowserInternal^ browserControl, Action<int>^ onAfterBrowserCreated) :
                _browserControl(browserControl), 
                _onAfterBrowserCreated(onAfterBrowserCreated)
            {
            }

            ~ClientAdapter()
            {
                _browserControl = nullptr;
                _onAfterBrowserCreated = nullptr;
                _browserHwnd = nullptr;
                _cefBrowser = NULL;
                _tooltip = nullptr;
            }

            HWND GetBrowserHwnd() { return _browserHwnd; }
            CefRefPtr<CefBrowser> GetCefBrowser() { return _cefBrowser; }
            void GetCefPopupBrowsers(std::vector<CefRefPtr<CefBrowser>>& popupBrowsers);
            void ShowDevTools();
            void CloseDevTools();
            void CloseAllPopups(bool forceClose);

            virtual bool IsOffscreen() { return false; }

            // CefClient
            virtual CefRefPtr<CefLifeSpanHandler> GetLifeSpanHandler() OVERRIDE{ return this; }
            virtual CefRefPtr<CefLoadHandler> GetLoadHandler() OVERRIDE{ return this; }
            virtual CefRefPtr<CefRequestHandler> GetRequestHandler() OVERRIDE{ return this; }
            virtual CefRefPtr<CefDisplayHandler> GetDisplayHandler() OVERRIDE{ return this; }
            virtual CefRefPtr<CefDownloadHandler> GetDownloadHandler() OVERRIDE;
            virtual CefRefPtr<CefContextMenuHandler> GetContextMenuHandler() OVERRIDE{ return this; }
            virtual CefRefPtr<CefFocusHandler> GetFocusHandler() OVERRIDE{ return this; }
            virtual CefRefPtr<CefKeyboardHandler> GetKeyboardHandler() OVERRIDE{ return this; }
            virtual CefRefPtr<CefJSDialogHandler> GetJSDialogHandler() OVERRIDE{ return this; }
            virtual CefRefPtr<CefDialogHandler> GetDialogHandler() OVERRIDE{ return this; }
            virtual CefRefPtr<CefDragHandler> GetDragHandler() OVERRIDE{ return this; }
            virtual CefRefPtr<CefGeolocationHandler> GetGeolocationHandler() OVERRIDE{ return this; }

            // CefLifeSpanHandler
            virtual DECL bool OnBeforePopup(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
                const CefString& target_url, const CefString& target_frame_name, const CefPopupFeatures& popupFeatures,
                CefWindowInfo& windowInfo, CefRefPtr<CefClient>& client, CefBrowserSettings& settings, bool* no_javascript_access) OVERRIDE;
            virtual DECL void OnAfterCreated(CefRefPtr<CefBrowser> browser) OVERRIDE;
            virtual DECL void OnBeforeClose(CefRefPtr<CefBrowser> browser) OVERRIDE;

            // CefLoadHandler
            virtual DECL void OnLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame) OVERRIDE;
            virtual DECL void OnLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int httpStatusCode) OVERRIDE;
            virtual DECL void OnLoadError(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, ErrorCode errorCode, const CefString& errorText, const CefString& failedUrl) OVERRIDE;

            // CefRequestHandler
            virtual DECL CefRefPtr<CefResourceHandler> GetResourceHandler(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request) OVERRIDE;
            virtual DECL cef_return_value_t OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefRequestCallback> callback) OVERRIDE;
            virtual DECL bool GetAuthCredentials(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, bool isProxy,
                const CefString& host, int port, const CefString& realm, const CefString& scheme, CefRefPtr<CefAuthCallback> callback) OVERRIDE;
            virtual DECL bool OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool isRedirect) OVERRIDE;
            virtual DECL bool OnCertificateError(CefRefPtr<CefBrowser> browser, cef_errorcode_t cert_error, const CefString& request_url, CefRefPtr<CefSSLInfo> ssl_info, CefRefPtr<CefRequestCallback> callback) OVERRIDE;
            virtual DECL bool OnQuotaRequest(CefRefPtr<CefBrowser> browser, const CefString& originUrl, int64 newSize, CefRefPtr<CefRequestCallback> callback) OVERRIDE;
            virtual DECL void OnResourceRedirect(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& oldUrl, CefString& newUrl) OVERRIDE;
            virtual DECL void OnProtocolExecution(CefRefPtr<CefBrowser> browser, const CefString& url, bool& allowOSExecution) OVERRIDE;

            virtual DECL bool OnBeforePluginLoad( CefRefPtr< CefBrowser > browser, const CefString& url, const CefString& policy_url, CefRefPtr< CefWebPluginInfo > info ) OVERRIDE;
            virtual DECL void OnPluginCrashed(CefRefPtr<CefBrowser> browser, const CefString& plugin_path) OVERRIDE;
            virtual DECL void OnRenderProcessTerminated(CefRefPtr<CefBrowser> browser, TerminationStatus status) OVERRIDE;

            // CefDisplayHandler
            virtual DECL void OnLoadingStateChange(CefRefPtr<CefBrowser> browser, bool isLoading, bool canGoBack, bool canGoForward) OVERRIDE;
            virtual DECL void OnAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url) OVERRIDE;
            virtual DECL void OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title) OVERRIDE;
            virtual DECL void OnFaviconURLChange(CefRefPtr<CefBrowser> browser, const std::vector<CefString>& iconUrls) OVERRIDE;
            virtual DECL bool OnTooltip(CefRefPtr<CefBrowser> browser, CefString& text) OVERRIDE;
            virtual DECL bool OnConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line) OVERRIDE;
            virtual DECL void OnStatusMessage(CefRefPtr<CefBrowser> browser, const CefString& message) OVERRIDE;

            // CefContextMenuHandler
            virtual DECL void OnBeforeContextMenu(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
                CefRefPtr<CefContextMenuParams> params, CefRefPtr<CefMenuModel> model) OVERRIDE;

            // CefFocusHandler
            virtual DECL void OnGotFocus(CefRefPtr<CefBrowser> browser) OVERRIDE;
            virtual DECL bool OnSetFocus(CefRefPtr<CefBrowser> browser, FocusSource source) OVERRIDE;
            virtual DECL void OnTakeFocus(CefRefPtr<CefBrowser> browser, bool next) OVERRIDE;

            // CefKeyboardHandler
            virtual DECL bool OnKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event) OVERRIDE;
            virtual DECL bool OnPreKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event, bool* is_keyboard_shortcut) OVERRIDE;

            // CefJSDialogHandler
            virtual DECL bool OnJSDialog(CefRefPtr<CefBrowser> browser, const CefString& origin_url, const CefString& accept_lang,
                JSDialogType dialog_type, const CefString& message_text, const CefString& default_prompt_text,
                CefRefPtr<CefJSDialogCallback> callback, bool& suppress_message) OVERRIDE;
            virtual DECL bool OnBeforeUnloadDialog(CefRefPtr<CefBrowser> browser, const CefString& message_text, bool is_reload,
                CefRefPtr<CefJSDialogCallback> callback) OVERRIDE;

            // CefDialogHandler
            virtual DECL bool OnFileDialog(CefRefPtr<CefBrowser> browser, FileDialogMode mode, const CefString& title,
                const CefString& default_file_path, const std::vector<CefString>& accept_filters, int selected_accept_filter,
                CefRefPtr<CefFileDialogCallback> callback) OVERRIDE;

            //CefDragHandler
            virtual DECL bool OnDragEnter(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDragData> dragData, DragOperationsMask mask) OVERRIDE;

            //CefGeolocationHandler
            virtual DECL bool OnRequestGeolocationPermission(CefRefPtr<CefBrowser> browser, const CefString& requesting_url, int request_id,
                CefRefPtr<CefGeolocationCallback> callback) OVERRIDE;
            virtual DECL void OnCancelGeolocationPermission(CefRefPtr<CefBrowser> browser, const CefString& requesting_url, int request_id) OVERRIDE;

            IMPLEMENT_REFCOUNTING(ClientAdapter);
        };
    }
}
