#include "stdafx.h"
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
                          public CefKeyboardHandler
    {
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

        IMPLEMENT_LOCKING(ClientAdapter);
        IMPLEMENT_REFCOUNTING(ClientAdapter);
    };
}
