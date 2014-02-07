#include "Stdafx.h"
#pragma once

#include "include/cef_client.h"

namespace CefSharp
{
    using namespace System;

    interface class IWebBrowser;

    class ClientAdapter : public CefClient,
                          public CefLifeSpanHandler,
                          public CefLoadHandler,
                          public CefRequestHandler,
                          public CefDisplayHandler,
                          public CefV8ContextHandler,
                          public CefMenuHandler,
                          public CefFocusHandler,
                          public CefKeyboardHandler,
                          public CefJSDialogHandler,
                          public AppDomainSafeCefBase
    {

    private:
        static void _OnInitialized(ClientAdapter* const _this);
        static void _OnBeforeClose(ClientAdapter* const _this);
        static bool _OnBeforeBrowse(ClientAdapter* const _this, CefRefPtr<CefRequest> request, NavType navType, bool isRedirect);
        static void _OnContextCreated(ClientAdapter* const _this, CefRefPtr<CefV8Context> context);
        static void _SetTitle(ClientAdapter* const _this, const CefString* const _title);
        static void _SetAddress(ClientAdapter* const _this, const CefString* const _url);
        static void _SetNavState(ClientAdapter* const _this, bool isLoading, bool canGoBack, bool canGoForward);
        static void _OnFrameLoadStart(ClientAdapter* const _this, CefRefPtr<CefFrame> frame);
        static void _SetSize(ClientAdapter* const _this, int width, int height);
        static void _OnFrameLoadEnd(ClientAdapter* const _this, CefRefPtr<CefFrame> frame);
        static void _OnTooltip(ClientAdapter* const _this, CefString* const _text);
        static void _OnTakeFocus(ClientAdapter* const _this, bool next);
        static bool _OnBeforeMenu(ClientAdapter* const _this);
        static bool _OnBeforePopup(ClientAdapter* const _this, CefWindowInfo* const _windowInfo, const CefString* const _url);
        static bool _OnConsoleMessage(ClientAdapter* const _this, const CefString* const _message, const CefString* const _source, int line);
        static bool _OnKeyEvent(ClientAdapter* const _this, KeyEventType type, int code, int modifiers, bool isSystemKey, bool isAfterJavaScript);
        static bool _OnLoadError(ClientAdapter* const _this, ErrorCode errorCode, const CefString* const _failedUrl, CefString* const _errorText);
        static bool _OnBeforeResourceLoad(ClientAdapter* const _this, CefRefPtr<CefRequest> request, CefString* const _redirectUrl, CefRefPtr<CefStreamReader>* const _resourceStream, CefRefPtr<CefResponse> response);
        static bool _GetDownloadHandler(ClientAdapter* const _this, const CefString* const _mimeType, const CefString* const _fileName, int64 contentLength, CefRefPtr<CefDownloadHandler>* const _handler);
        static bool _GetAuthCredentials(ClientAdapter* const _this, bool isProxy, const CefString* const _host, int port, const CefString* const _realm, const CefString* const _scheme, CefString* const _username, CefString* const _password);
        static void _OnResourceResponse(ClientAdapter* const _this, const CefString* const _url, CefRefPtr<CefResponse> response);

    private:
        gcroot<IWebBrowser^> _browserControl;
        HWND _browserHwnd;
        CefRefPtr<CefBrowser> _cefBrowser;

        gcroot<String^> _tooltip;

    public:
        ~ClientAdapter() { _browserControl = nullptr; }
        ClientAdapter(IWebBrowser^ browserControl) : _browserControl(browserControl) {}

        HWND GetBrowserHwnd() { return _browserHwnd; }
        CefRefPtr<CefBrowser> GetCefBrowser() { return _cefBrowser; }

        // CefClient
        virtual CefRefPtr<CefLifeSpanHandler> GetLifeSpanHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefLoadHandler> GetLoadHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefRequestHandler> GetRequestHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefDisplayHandler> GetDisplayHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefV8ContextHandler> GetV8ContextHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefMenuHandler> GetMenuHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefFocusHandler> GetFocusHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefKeyboardHandler> GetKeyboardHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefJSDialogHandler> GetJSDialogHandler() OVERRIDE { return this; }

        // CefLifeSpanHandler
        virtual DECL bool OnBeforePopup(CefRefPtr<CefBrowser> parentBrowser, const CefPopupFeatures& popupFeatures, CefWindowInfo& windowInfo, const CefString& url, CefRefPtr<CefClient>& client, CefBrowserSettings& settings) OVERRIDE;
        virtual DECL void OnAfterCreated(CefRefPtr<CefBrowser> browser) OVERRIDE;
        virtual DECL void OnBeforeClose(CefRefPtr<CefBrowser> browser) OVERRIDE;

        // CefLoadHandler
        virtual DECL void OnLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame) OVERRIDE;
        virtual DECL void OnLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int httpStatusCode) OVERRIDE;
        virtual DECL bool OnLoadError(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, ErrorCode errorCode, const CefString& failedUrl, CefString& errorText) OVERRIDE;

        // CefRequestHandler
        virtual DECL bool OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, NavType navType, bool isRedirect) OVERRIDE;
        virtual DECL bool OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefRefPtr<CefResponse> response, int loadFlags) OVERRIDE;
        virtual DECL void OnResourceResponse(CefRefPtr<CefBrowser> browser, const CefString& url, CefRefPtr<CefResponse> response, CefRefPtr<CefContentFilter>& filter) OVERRIDE;
        virtual DECL bool GetDownloadHandler(CefRefPtr<CefBrowser> browser, const CefString& mimeType, const CefString& fileName, int64 contentLength, CefRefPtr<CefDownloadHandler>& handler) OVERRIDE;

        virtual DECL bool GetAuthCredentials(CefRefPtr<CefBrowser> browser, bool isProxy, const CefString& host, int port, const CefString& realm, const CefString& scheme, CefString& username, CefString& password) OVERRIDE;

        // CefDisplayHandler
        virtual DECL void OnAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url) OVERRIDE;
        virtual DECL void OnContentsSizeChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int width, int height) OVERRIDE;
        virtual DECL void OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title) OVERRIDE;
        virtual DECL bool OnTooltip(CefRefPtr<CefBrowser> browser, CefString& text) OVERRIDE;
        virtual DECL bool OnConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line) OVERRIDE;

        // CefV8ContextHandler
        virtual DECL void OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context) OVERRIDE;

        // CefMenuHandler
        virtual DECL bool OnBeforeMenu(CefRefPtr<CefBrowser> browser, const CefMenuInfo& menuInfo) OVERRIDE;

        // CefFocusHandler
        virtual DECL void OnTakeFocus(CefRefPtr<CefBrowser> browser, bool next) OVERRIDE;

        // CefKeyboardHandler
        virtual DECL bool OnKeyEvent(CefRefPtr<CefBrowser> browser, KeyEventType type, int code, int modifiers, bool isSystemKey, bool isAfterJavaScript) OVERRIDE;

        // CefJSDialogHandler
        virtual DECL bool OnJSAlert(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message) OVERRIDE;
        virtual DECL bool OnJSConfirm(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message, bool& retval) OVERRIDE;
        virtual DECL bool OnJSPrompt(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message, const CefString& defaultValue, bool& retval,  CefString& result) OVERRIDE;

        IMPLEMENT_LOCKING(ClientAdapter);
        IMPLEMENT_SAFE_REFCOUNTING(ClientAdapter);
    };
}
