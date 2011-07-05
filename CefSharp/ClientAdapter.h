#include "stdafx.h"
#pragma once

namespace CefSharp 
{
    using namespace System;

    interface class ICefWebBrowser;

    class ClientAdapter : public CefClient,
                          public CefLifeSpanHandler,
                          public CefLoadHandler,
                          public CefRequestHandler,
                          public CefDisplayHandler,
                          public CefJSBindingHandler,
                          public CefMenuHandler
    {
    private:
        gcroot<ICefWebBrowser^> _browserControl;
        HWND _browserHwnd;
        CefRefPtr<CefBrowser> _cefBrowser;

        gcroot<String^> _tooltip;

    public:
        ~ClientAdapter() { _browserControl = nullptr; }
        ClientAdapter(ICefWebBrowser^ browserControl) : _browserControl(browserControl) {}

        HWND GetBrowserHwnd() { return _browserHwnd; }

        CefRefPtr<CefBrowser> GetCefBrowser()
        {
            if (_cefBrowser == nullptr)
            {
                //TODO: make own exception type?
                throw gcnew InvalidOperationException("CefBrowser is not initialized now.");
            }

            return _cefBrowser;
        }

        bool GetIsInitialized()
        {
            return _cefBrowser != nullptr;
        }

        // CefClient
        virtual CefRefPtr<CefLifeSpanHandler> GetLifeSpanHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefLoadHandler> GetLoadHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefRequestHandler> GetRequestHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefDisplayHandler> GetDisplayHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefJSBindingHandler> GetJSBindingHandler() OVERRIDE { return this; }
        virtual CefRefPtr<CefMenuHandler> GetMenuHandler() OVERRIDE { return this; }

        // CefLifeSpanHandler
        virtual bool OnBeforePopup(CefRefPtr<CefBrowser> parentBrowser, const CefPopupFeatures& popupFeatures, CefWindowInfo& windowInfo, const CefString& url, CefRefPtr<CefClient>& client, CefBrowserSettings& settings) OVERRIDE;
        virtual void OnAfterCreated(CefRefPtr<CefBrowser> browser) OVERRIDE;
        virtual void OnBeforeClose(CefRefPtr<CefBrowser> browser) OVERRIDE;

        // CefLoadHandler
        virtual void OnLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame) OVERRIDE;
        virtual void OnLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int httpStatusCode) OVERRIDE;

        // CefRequestHandler
        virtual bool OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefRefPtr<CefResponse> response, int loadFlags) OVERRIDE;
        virtual void OnResourceResponse(CefRefPtr<CefBrowser> browser, const CefString& url, CefRefPtr<CefResponse> response, CefRefPtr<CefContentFilter>& filter) OVERRIDE;

        // CefDisplayHandler
        virtual void OnAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url) OVERRIDE;
        virtual void OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title) OVERRIDE;
        virtual bool OnTooltip(CefRefPtr<CefBrowser> browser, CefString& text) OVERRIDE;
        virtual bool OnConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line) OVERRIDE;

        // CefJSBindingHandler
        virtual void OnJSBinding(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Value> object) OVERRIDE;

        // CefMenuHandler
        virtual bool OnBeforeMenu(CefRefPtr<CefBrowser> browser, const MenuInfo& menuInfo) OVERRIDE;

        IMPLEMENT_LOCKING(ClientAdapter);
        IMPLEMENT_REFCOUNTING(ClientAdapter);
    };
}
