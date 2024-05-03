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
            public CefFindHandler,
            public CefAudioHandler,
            public CefFrameHandler,
            public CefPermissionHandler
        {
        private:
            gcroot<IWebBrowserInternal^> _browserControl;
            HWND _browserHwnd;
            CefRefPtr<CefBrowser> _cefBrowser;
            bool _disposed;

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
                _browserHwnd(nullptr),
                _disposed(false)
            {

            }

            ~ClientAdapter()
            {
                _disposed = true;

                CloseAllPopups(true);

                //this will dispose the repository and cancel all pending tasks
                delete _pendingTaskRepository;

                _browser = nullptr;
                _browserControl = nullptr;
                _browserHwnd = nullptr;
                _cefBrowser = nullptr;
                _tooltip = nullptr;
                _browserAdapter = nullptr;
                _popupBrowsers = nullptr;
            }

            HWND GetBrowserHwnd() { return _browserHwnd; }
            PendingTaskRepository<JavascriptResponse^>^ GetPendingTaskRepository();
            void CloseAllPopups(bool forceClose);
            void MethodInvocationComplete(MethodInvocationResult^ result);
            IBrowser^ GetBrowserWrapper(int browserId);
            bool IsMainBrowser(bool isPopup, int browserId);

            // CefClient
            virtual DECL CefRefPtr<CefLifeSpanHandler> GetLifeSpanHandler() override { return this; }
            virtual DECL CefRefPtr<CefLoadHandler> GetLoadHandler() override { return this; }
            virtual DECL CefRefPtr<CefRequestHandler> GetRequestHandler() override { return this; }
            virtual DECL CefRefPtr<CefDisplayHandler> GetDisplayHandler() override { return this; }
            virtual DECL CefRefPtr<CefDownloadHandler> GetDownloadHandler() override { return this; }
            virtual DECL CefRefPtr<CefContextMenuHandler> GetContextMenuHandler() override { return this; }
            virtual DECL CefRefPtr<CefFocusHandler> GetFocusHandler() override { return this; }
            virtual DECL CefRefPtr<CefKeyboardHandler> GetKeyboardHandler() override { return this; }
            virtual DECL CefRefPtr<CefJSDialogHandler> GetJSDialogHandler() override { return this; }
            virtual DECL CefRefPtr<CefDialogHandler> GetDialogHandler() override { return this; }
            virtual DECL CefRefPtr<CefDragHandler> GetDragHandler() override { return this; }
            virtual DECL CefRefPtr<CefFindHandler> GetFindHandler() override { return this; }
            virtual DECL CefRefPtr<CefAudioHandler> GetAudioHandler() override { return this; }
            virtual DECL CefRefPtr<CefFrameHandler> GetFrameHandler() override { return this; }
            virtual DECL CefRefPtr<CefPermissionHandler> GetPermissionHandler() override { return this; }
            virtual DECL bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefProcessId source_process, CefRefPtr<CefProcessMessage> message) override;


            // CefLifeSpanHandler
            virtual DECL bool OnBeforePopup(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
                const CefString& target_url, const CefString& target_frame_name,
                CefLifeSpanHandler::WindowOpenDisposition target_disposition, bool user_gesture,
                const CefPopupFeatures& popupFeatures,
                CefWindowInfo& windowInfo, CefRefPtr<CefClient>& client, CefBrowserSettings& settings, CefRefPtr<CefDictionaryValue>& extraInfo, bool* no_javascript_access) override;
            virtual DECL void OnAfterCreated(CefRefPtr<CefBrowser> browser) override;
            virtual DECL bool DoClose(CefRefPtr<CefBrowser> browser) override;
            virtual DECL void OnBeforeClose(CefRefPtr<CefBrowser> browser) override;

            // CefLoadHandler
            virtual DECL void OnLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, TransitionType transitionType) override;
            virtual DECL void OnLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int httpStatusCode) override;
            virtual DECL void OnLoadError(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, ErrorCode errorCode, const CefString& errorText, const CefString& failedUrl) override;

            // CefRequestHandler
            virtual DECL bool OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool userGesture, bool isRedirect) override;
            virtual DECL bool OnOpenURLFromTab(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& targetUrl,
                CefRequestHandler::WindowOpenDisposition targetDisposition, bool userGesture) override;
            virtual DECL CefRefPtr<CefResourceRequestHandler> GetResourceRequestHandler(CefRefPtr<CefBrowser> browser,
                CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool isNavigation, bool isDownload, const CefString& requestInitiator, bool& disableDefaultHandling) override;
            virtual DECL bool GetAuthCredentials(CefRefPtr<CefBrowser> browser, const CefString& originUrl, bool isProxy,
                const CefString& host, int port, const CefString& realm, const CefString& scheme, CefRefPtr<CefAuthCallback> callback) override;
            virtual DECL bool OnCertificateError(CefRefPtr<CefBrowser> browser, cef_errorcode_t cert_error, const CefString& request_url, CefRefPtr<CefSSLInfo> ssl_info, CefRefPtr<CefCallback> callback) override;
            virtual DECL bool OnSelectClientCertificate(CefRefPtr<CefBrowser> browser, bool isProxy, const CefString& host, int port,
                const CefRequestHandler::X509CertificateList& certificates, CefRefPtr<CefSelectClientCertificateCallback> callback) override;
            virtual DECL void OnRenderViewReady(CefRefPtr<CefBrowser> browser) override;
            virtual DECL void OnRenderProcessTerminated(CefRefPtr<CefBrowser> browser, TerminationStatus status, int errorCode, const CefString& errorString) override;
            virtual DECL void OnDocumentAvailableInMainFrame(CefRefPtr<CefBrowser> browser) override;

            // CefDisplayHandler
            virtual DECL void OnLoadingStateChange(CefRefPtr<CefBrowser> browser, bool isLoading, bool canGoBack, bool canGoForward) override;
            virtual DECL void OnAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url) override;
            virtual DECL bool OnAutoResize(CefRefPtr< CefBrowser > browser, const CefSize& new_size) override;
            virtual DECL bool OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor, cef_cursor_type_t type, const CefCursorInfo & custom_cursor_info) override;
            virtual DECL void OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title) override;
            virtual DECL void OnFaviconURLChange(CefRefPtr<CefBrowser> browser, const std::vector<CefString>& iconUrls) override;
            virtual DECL void OnFullscreenModeChange(CefRefPtr<CefBrowser> browser, bool fullscreen) override;
            virtual DECL void OnLoadingProgressChange(CefRefPtr<CefBrowser> browser, double progress) override;
            virtual DECL bool OnTooltip(CefRefPtr<CefBrowser> browser, CefString& text) override;
            virtual DECL bool OnConsoleMessage(CefRefPtr<CefBrowser> browser, cef_log_severity_t level, const CefString& message, const CefString& source, int line) override;
            virtual DECL void OnStatusMessage(CefRefPtr<CefBrowser> browser, const CefString& message) override;

            virtual DECL void InternalCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor, cef_cursor_type_t type, const CefCursorInfo& custom_cursor_info);

            // CefContextMenuHandler
            virtual DECL void OnBeforeContextMenu(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
                CefRefPtr<CefContextMenuParams> params, CefRefPtr<CefMenuModel> model) override;
            virtual DECL bool OnContextMenuCommand(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
                CefRefPtr<CefContextMenuParams> params, int commandId, EventFlags eventFlags) override;
            virtual DECL void OnContextMenuDismissed(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame) override;
            virtual DECL bool RunContextMenu(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefContextMenuParams> params, CefRefPtr<CefMenuModel> model, CefRefPtr<CefRunContextMenuCallback> callback) override;

            // CefFocusHandler
            virtual DECL void OnGotFocus(CefRefPtr<CefBrowser> browser) override;
            virtual DECL bool OnSetFocus(CefRefPtr<CefBrowser> browser, FocusSource source) override;
            virtual DECL void OnTakeFocus(CefRefPtr<CefBrowser> browser, bool next) override;

            // CefKeyboardHandler
            virtual DECL bool OnKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event) override;
            virtual DECL bool OnPreKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event, bool* is_keyboard_shortcut) override;

            // CefJSDialogHandler
            virtual DECL bool OnJSDialog(CefRefPtr<CefBrowser> browser, const CefString& origin_url,
                JSDialogType dialog_type, const CefString& message_text, const CefString& default_prompt_text,
                CefRefPtr<CefJSDialogCallback> callback, bool& suppress_message) override;
            virtual DECL bool OnBeforeUnloadDialog(CefRefPtr<CefBrowser> browser, const CefString& message_text, bool is_reload,
                CefRefPtr<CefJSDialogCallback> callback) override;
            virtual DECL void OnResetDialogState(CefRefPtr<CefBrowser> browser) override;
            virtual DECL void OnDialogClosed(CefRefPtr<CefBrowser> browser) override;

            // CefDialogHandler
            virtual DECL bool OnFileDialog(CefRefPtr<CefBrowser> browser, FileDialogMode mode, const CefString& title,
                const CefString& default_file_path, const std::vector<CefString>& accept_filters,
                CefRefPtr<CefFileDialogCallback> callback) override;

            //CefDragHandler
            virtual DECL bool OnDragEnter(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDragData> dragData, DragOperationsMask mask) override;
            virtual DECL void OnDraggableRegionsChanged(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const std::vector<CefDraggableRegion>& regions) override;

            //CefDownloadHandler
            virtual DECL bool CanDownload(CefRefPtr<CefBrowser> browser, const CefString & url, const CefString & request_method) override;
            virtual DECL void OnBeforeDownload(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
                const CefString& suggested_name, CefRefPtr<CefBeforeDownloadCallback> callback) override;
            virtual DECL void OnDownloadUpdated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
                CefRefPtr<CefDownloadItemCallback> callback) override;

            //CefFindHandler
            virtual DECL void OnFindResult(CefRefPtr<CefBrowser> browser, int identifier, int count, const CefRect& selectionRect, int activeMatchOrdinal, bool finalUpdate) override;

            //CefAudioHandler
            virtual DECL bool GetAudioParameters(CefRefPtr<CefBrowser> browser, CefAudioParameters & params) override;
            virtual DECL void OnAudioStreamStarted(CefRefPtr<CefBrowser> browser, const CefAudioParameters& params, int channels) override;
            virtual DECL void OnAudioStreamPacket(CefRefPtr<CefBrowser> browser, const float** data, int frames, int64_t pts) override;
            virtual DECL void OnAudioStreamStopped(CefRefPtr<CefBrowser> browser) override;
            virtual DECL void OnAudioStreamError(CefRefPtr<CefBrowser> browser, const CefString& message) override;

            //CefFrameHandler
            virtual DECL void OnFrameCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame) override;
            virtual DECL void OnFrameAttached(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, bool reattached) override;
            virtual DECL void OnFrameDetached(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame) override;
            virtual DECL void OnMainFrameChanged(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> old_frame, CefRefPtr<CefFrame> new_frame) override;

            //CefPermissionHandler
            virtual DECL bool OnShowPermissionPrompt(
                CefRefPtr<CefBrowser> browser,
                uint64_t prompt_id,
                const CefString& requesting_origin,
                uint32_t requested_permissions,
                CefRefPtr<CefPermissionPromptCallback> callback) override;

            virtual DECL void OnDismissPermissionPrompt(
                CefRefPtr<CefBrowser> browser,
                uint64_t prompt_id,
                cef_permission_request_result_t result) override;

            virtual DECL bool OnRequestMediaAccessPermission(
                CefRefPtr<CefBrowser> browser,
                CefRefPtr<CefFrame> frame,
                const CefString& requesting_origin,
                uint32_t requested_permissions,
                CefRefPtr<CefMediaAccessCallback> callback) override;

            IMPLEMENT_REFCOUNTINGM(ClientAdapter);
        };
    }
}
#endif  // CEFSHARP_CORE_INTERNALS_CLIENTADAPTER_H_
