// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#ifndef CEFSHARP_CORE_INTERNALS_CLIENTADAPTER_H_
#define CEFSHARP_CORE_INTERNALS_CLIENTADAPTER_H_

#pragma once

#include "Stdafx.h"

#include "include/cef_app.h"
#include "include/cef_client.h"
#include "include/cef_render_process_handler.h"
#include "include/internal/cef_types.h"

using namespace System::Threading::Tasks;

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
            public CefDownloadHandler,
            public CefFindHandler
        {
        private:
            gcroot<IWebBrowserInternal^> _browserControl;
            HWND _browserHwnd;
            CefRefPtr<CefBrowser> _cefBrowser;

            gcroot<IBrowser^> _browser;
            gcroot<Dictionary<int, IBrowser^>^> _popupBrowsers;
            gcroot<String^> _tooltip;
            gcroot<IBrowserAdapter^> _browserAdapter;
            //contains in-progress eval script tasks
            gcroot<PendingTaskRepository<JavascriptResponse^>^> _pendingTaskRepository;
            //contains js callback factories for each browser
            IBrowser^ GetBrowserWrapper(int browserId, bool isPopup);

        public:
            ClientAdapter(IWebBrowserInternal^ browserControl, IBrowserAdapter^ browserAdapter) :
                _browserControl(browserControl),
                _popupBrowsers(gcnew Dictionary<int, IBrowser^>()),
                _pendingTaskRepository(gcnew PendingTaskRepository<JavascriptResponse^>()),
                _browserAdapter(browserAdapter),
                _browserHwnd(NULL)
            {

            }

            ~ClientAdapter()
            {
                CloseAllPopups(true);

                //this will dispose the repository and cancel all pending tasks
                delete _pendingTaskRepository;

                _browser = nullptr;
                _browserControl = nullptr;
                _browserHwnd = nullptr;
                _cefBrowser = NULL;
                _tooltip = nullptr;
                _browserAdapter = nullptr;
                _popupBrowsers = nullptr;
            }

            HWND GetBrowserHwnd() { return _browserHwnd; }
            PendingTaskRepository<JavascriptResponse^>^ GetPendingTaskRepository();
            void CloseAllPopups(bool forceClose);
            void MethodInvocationComplete(MethodInvocationResult^ result);
            IBrowser^ GetBrowserWrapper(int browserId);

            // CefClient
            virtual DECL CefRefPtr<CefLifeSpanHandler> GetLifeSpanHandler() OVERRIDE { return this; }
            virtual DECL CefRefPtr<CefLoadHandler> GetLoadHandler() OVERRIDE { return this; }
            virtual DECL CefRefPtr<CefRequestHandler> GetRequestHandler() OVERRIDE { return this; }
            virtual DECL CefRefPtr<CefDisplayHandler> GetDisplayHandler() OVERRIDE { return this; }
            virtual DECL CefRefPtr<CefDownloadHandler> GetDownloadHandler() OVERRIDE { return this; }
            virtual DECL CefRefPtr<CefContextMenuHandler> GetContextMenuHandler() OVERRIDE { return this; }
            virtual DECL CefRefPtr<CefFocusHandler> GetFocusHandler() OVERRIDE { return this; }
            virtual DECL CefRefPtr<CefKeyboardHandler> GetKeyboardHandler() OVERRIDE { return this; }
            virtual DECL CefRefPtr<CefJSDialogHandler> GetJSDialogHandler() OVERRIDE { return this; }
            virtual DECL CefRefPtr<CefDialogHandler> GetDialogHandler() OVERRIDE { return this; }
            virtual DECL CefRefPtr<CefDragHandler> GetDragHandler() OVERRIDE { return this; }
            virtual DECL CefRefPtr<CefFindHandler> GetFindHandler() OVERRIDE { return this; }
            virtual DECL bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefProcessId source_process, CefRefPtr<CefProcessMessage> message) OVERRIDE;


            // CefLifeSpanHandler
            virtual DECL bool OnBeforePopup(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
                const CefString& target_url, const CefString& target_frame_name,
                CefLifeSpanHandler::WindowOpenDisposition target_disposition, bool user_gesture,
                const CefPopupFeatures& popupFeatures,
                CefWindowInfo& windowInfo, CefRefPtr<CefClient>& client, CefBrowserSettings& settings, CefRefPtr<CefDictionaryValue>& extraInfo, bool* no_javascript_access) OVERRIDE;
            virtual DECL void OnAfterCreated(CefRefPtr<CefBrowser> browser) OVERRIDE;
            virtual DECL bool DoClose(CefRefPtr<CefBrowser> browser) OVERRIDE;
            virtual DECL void OnBeforeClose(CefRefPtr<CefBrowser> browser) OVERRIDE;

            // CefLoadHandler
            virtual DECL void OnLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, TransitionType transitionType) OVERRIDE;
            virtual DECL void OnLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int httpStatusCode) OVERRIDE;
            virtual DECL void OnLoadError(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, ErrorCode errorCode, const CefString& errorText, const CefString& failedUrl) OVERRIDE;

            // CefRequestHandler
            virtual DECL bool OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool userGesture, bool isRedirect) OVERRIDE;
            virtual DECL bool OnOpenURLFromTab(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& targetUrl,
                CefRequestHandler::WindowOpenDisposition targetDisposition, bool userGesture) OVERRIDE;
            virtual DECL CefRefPtr<CefResourceRequestHandler> GetResourceRequestHandler(CefRefPtr<CefBrowser> browser,
                CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool isNavigation, bool isDownload, const CefString& requestInitiator, bool& disableDefaultHandling) OVERRIDE;
            virtual DECL bool GetAuthCredentials(CefRefPtr<CefBrowser> browser, const CefString& originUrl, bool isProxy,
                const CefString& host, int port, const CefString& realm, const CefString& scheme, CefRefPtr<CefAuthCallback> callback) OVERRIDE;
            virtual DECL bool OnQuotaRequest(CefRefPtr<CefBrowser> browser, const CefString& originUrl, int64 newSize, CefRefPtr<CefRequestCallback> callback) OVERRIDE;
            virtual DECL bool OnCertificateError(CefRefPtr<CefBrowser> browser, cef_errorcode_t cert_error, const CefString& request_url, CefRefPtr<CefSSLInfo> ssl_info, CefRefPtr<CefRequestCallback> callback) OVERRIDE;
            virtual DECL bool OnSelectClientCertificate(CefRefPtr<CefBrowser> browser, bool isProxy, const CefString& host, int port,
                const CefRequestHandler::X509CertificateList& certificates, CefRefPtr<CefSelectClientCertificateCallback> callback) OVERRIDE;
            virtual DECL void OnPluginCrashed(CefRefPtr<CefBrowser> browser, const CefString& plugin_path) OVERRIDE;
            virtual DECL void OnRenderViewReady(CefRefPtr<CefBrowser> browser) OVERRIDE;
            virtual DECL void OnRenderProcessTerminated(CefRefPtr<CefBrowser> browser, TerminationStatus status) OVERRIDE;
            virtual DECL void OnDocumentAvailableInMainFrame(CefRefPtr<CefBrowser> browser) OVERRIDE;

            // CefDisplayHandler
            virtual DECL void OnLoadingStateChange(CefRefPtr<CefBrowser> browser, bool isLoading, bool canGoBack, bool canGoForward) OVERRIDE;
            virtual DECL void OnAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url) OVERRIDE;
            virtual DECL bool OnAutoResize(CefRefPtr< CefBrowser > browser, const CefSize& new_size) OVERRIDE;
            virtual DECL void OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title) OVERRIDE;
            virtual DECL void OnFaviconURLChange(CefRefPtr<CefBrowser> browser, const std::vector<CefString>& iconUrls) OVERRIDE;
            virtual DECL void OnFullscreenModeChange(CefRefPtr<CefBrowser> browser, bool fullscreen) OVERRIDE;
            virtual DECL void OnLoadingProgressChange(CefRefPtr<CefBrowser> browser, double progress) OVERRIDE;
            virtual DECL bool OnTooltip(CefRefPtr<CefBrowser> browser, CefString& text) OVERRIDE;
            virtual DECL bool OnConsoleMessage(CefRefPtr<CefBrowser> browser, cef_log_severity_t level, const CefString& message, const CefString& source, int line) OVERRIDE;
            virtual DECL void OnStatusMessage(CefRefPtr<CefBrowser> browser, const CefString& message) OVERRIDE;

            // CefContextMenuHandler
            virtual DECL void OnBeforeContextMenu(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
                CefRefPtr<CefContextMenuParams> params, CefRefPtr<CefMenuModel> model) OVERRIDE;
            virtual DECL bool OnContextMenuCommand(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
                CefRefPtr<CefContextMenuParams> params, int commandId, EventFlags eventFlags) OVERRIDE;
            virtual DECL void OnContextMenuDismissed(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame) OVERRIDE;
            virtual DECL bool RunContextMenu(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefContextMenuParams> params, CefRefPtr<CefMenuModel> model, CefRefPtr<CefRunContextMenuCallback> callback) OVERRIDE;

            // CefFocusHandler
            virtual DECL void OnGotFocus(CefRefPtr<CefBrowser> browser) OVERRIDE;
            virtual DECL bool OnSetFocus(CefRefPtr<CefBrowser> browser, FocusSource source) OVERRIDE;
            virtual DECL void OnTakeFocus(CefRefPtr<CefBrowser> browser, bool next) OVERRIDE;

            // CefKeyboardHandler
            virtual DECL bool OnKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event) OVERRIDE;
            virtual DECL bool OnPreKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event, bool* is_keyboard_shortcut) OVERRIDE;

            // CefJSDialogHandler
            virtual DECL bool OnJSDialog(CefRefPtr<CefBrowser> browser, const CefString& origin_url,
                JSDialogType dialog_type, const CefString& message_text, const CefString& default_prompt_text,
                CefRefPtr<CefJSDialogCallback> callback, bool& suppress_message) OVERRIDE;
            virtual DECL bool OnBeforeUnloadDialog(CefRefPtr<CefBrowser> browser, const CefString& message_text, bool is_reload,
                CefRefPtr<CefJSDialogCallback> callback) OVERRIDE;
            virtual DECL void OnResetDialogState(CefRefPtr<CefBrowser> browser) OVERRIDE;
            virtual DECL void OnDialogClosed(CefRefPtr<CefBrowser> browser) OVERRIDE;

            // CefDialogHandler
            virtual DECL bool OnFileDialog(CefRefPtr<CefBrowser> browser, FileDialogMode mode, const CefString& title,
                const CefString& default_file_path, const std::vector<CefString>& accept_filters, int selected_accept_filter,
                CefRefPtr<CefFileDialogCallback> callback) OVERRIDE;

            //CefDragHandler
            virtual DECL bool OnDragEnter(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDragData> dragData, DragOperationsMask mask) OVERRIDE;
            virtual DECL void OnDraggableRegionsChanged(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const std::vector<CefDraggableRegion>& regions) OVERRIDE;

            //CefDownloadHandler
            virtual DECL void OnBeforeDownload(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
                const CefString& suggested_name, CefRefPtr<CefBeforeDownloadCallback> callback) OVERRIDE;
            virtual DECL void OnDownloadUpdated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
                CefRefPtr<CefDownloadItemCallback> callback) OVERRIDE;

            //CefFindHandler
            virtual DECL void OnFindResult(CefRefPtr<CefBrowser> browser, int identifier, int count, const CefRect& selectionRect, int activeMatchOrdinal, bool finalUpdate) OVERRIDE;

            IMPLEMENT_REFCOUNTING(ClientAdapter);
        };
    }
}
#endif  // CEFSHARP_CORE_INTERNALS_CLIENTADAPTER_H_
