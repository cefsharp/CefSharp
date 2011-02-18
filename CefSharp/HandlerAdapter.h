#include "stdafx.h"
#pragma once

namespace CefSharp 
{
    using namespace System;

    ref class CefWebBrowser;

    class HandlerAdapter : public CefThreadSafeBase<CefHandler>
    {
    private:
        gcroot<CefWebBrowser^> _browserControl;
        HWND _browserHwnd;
        CefRefPtr<CefBrowser> _cefBrowser;

    private:
        void ThrowIfIsNotInitialized()
        {
            if (!GetIsInitialized())
            {
                //TODO: make own exception type?
                throw gcnew InvalidOperationException("CefBrowser is not initialized now.");
            }
        }

    protected:
        
    public:
        ~HandlerAdapter() { _browserControl = nullptr; }
        HandlerAdapter(CefWebBrowser^ browserControl) : _browserControl(browserControl) {}

        virtual RetVal HandleAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url);
        virtual RetVal HandleAfterCreated(CefRefPtr<CefBrowser> browser);
        virtual RetVal HandleTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title);
        virtual RetVal HandleLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, bool isMainContent);
        virtual RetVal HandleLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, bool isMainContent, int httpStatusCode);
        virtual RetVal HandleJSBinding(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Value> object);
        virtual RetVal HandleConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line);
		virtual RetVal HandleBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefString& mimeType,int loadFlags);
		
		virtual RetVal HandleBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, NavType navType, bool isRedirect) { return RV_CONTINUE; }
        virtual RetVal HandleBeforeCreated(CefRefPtr<CefBrowser> parentBrowser, CefWindowInfo& createInfo, bool popup, const CefPopupFeatures& popupFeatures, CefRefPtr<CefHandler>& handler, CefString& url, CefBrowserSettings& settings) { return RV_CONTINUE; }
        virtual RetVal HandleLoadError(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, ErrorCode errorCode, const CefString& failedUrl, CefString& errorText) { return RV_CONTINUE; }
        virtual RetVal HandleDownloadResponse(CefRefPtr<CefBrowser> browser, const CefString& mimeType, const CefString& fileName, int64 contentLength, CefRefPtr<CefDownloadHandler>& handler) { return RV_CONTINUE; }
        virtual RetVal HandleAuthenticationRequest(CefRefPtr<CefBrowser> browser, bool isProxy, const CefString& host, const CefString& realm, const CefString& scheme, CefString& username, CefString& password) { return RV_HANDLED; }
        virtual RetVal HandleBeforeMenu(CefRefPtr<CefBrowser> browser, const MenuInfo& menuInfo) { return RV_HANDLED; }
        virtual RetVal HandleGetMenuLabel(CefRefPtr<CefBrowser> browser, MenuId menuId, CefString& label) { return RV_CONTINUE; }
        virtual RetVal HandleMenuAction(CefRefPtr<CefBrowser> browser, MenuId menuId) { return RV_CONTINUE; }
        virtual RetVal HandlePrintOptions(CefRefPtr<CefBrowser> browser, CefPrintOptions& printOptions) { return RV_CONTINUE; }
        virtual RetVal HandlePrintHeaderFooter(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefPrintInfo& printInfo, const CefString& url, const CefString& title, int currentPage, int maxPages, CefString& topLeft, CefString& topCenter, CefString& topRight, CefString& bottomLeft, CefString& bottomCenter, CefString& bottomRight) { return RV_CONTINUE; }
        virtual RetVal HandleJSAlert(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message) { return RV_CONTINUE; }
        virtual RetVal HandleJSConfirm(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message, bool& retval) { return RV_CONTINUE; }
        virtual RetVal HandleJSPrompt(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message, const CefString& defaultValue, bool& retval, CefString& result) { return RV_CONTINUE; }
        
        virtual RetVal HandleBeforeWindowClose(CefRefPtr<CefBrowser> browser) { return RV_CONTINUE; }
        virtual RetVal HandleTakeFocus(CefRefPtr<CefBrowser> browser, bool reverse) { return RV_CONTINUE; }
        virtual RetVal HandleSetFocus(CefRefPtr<CefBrowser> browser, bool isWidget) { return RV_CONTINUE; }
        virtual RetVal HandleTooltip(CefRefPtr<CefBrowser> browser, CefString& text) { return RV_CONTINUE; }
        virtual RetVal HandleKeyEvent(CefRefPtr<CefBrowser> browser, KeyEventType type, int code, int modifiers, bool isSystemKey) { return RV_CONTINUE; }
        virtual RetVal HandleFindResult(CefRefPtr<CefBrowser> browser, int identifier, int count, const CefRect& selectionRect, int activeMatchOrdinal, bool finalUpdate) { return RV_CONTINUE; }
        virtual RetVal HandleProtocolExecution(CefRefPtr<CefBrowser> browser, const CefString& url, bool* allow_os_execution) { return RV_CONTINUE; }
        virtual RetVal HandleStatus(CefRefPtr<CefBrowser> browser, const CefString& value, StatusType type) { return RV_CONTINUE; }

        HWND GetBrowserHwnd() { return _browserHwnd; }
        CefRefPtr<CefBrowser> GetCefBrowser()
        {
            ThrowIfIsNotInitialized();
            return _cefBrowser;
        }

        bool GetIsInitialized()
        {
        	return _cefBrowser != nullptr;
        }
    };
};