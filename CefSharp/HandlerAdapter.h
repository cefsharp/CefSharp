#include "stdafx.h"
#pragma once

#include "Handler.h"

namespace CefSharp
{
	ref class CefWebBrowser;

	public ref class HandlerAdapter
	{
	private:
		Handler* _handler;
		CefWebBrowser^ _browserControl;

	private:
        void ThrowIfIsNotInitialized()
        {
            if (!GetIsInitialized())
            {
                //TODO: make own exception type?
                throw gcnew InvalidOperationException("CefBrowser is not initialized now.");
            }
        }

	public:
		HandlerAdapter(CefWebBrowser^ browserControl);

		~HandlerAdapter()
		{
			_handler->Release();
			_handler = nullptr;
		}

		!HandlerAdapter()
		{
			_handler->Release();
			_handler = nullptr;
		}

		bool GetIsInitialized()
        {
			return _handler != nullptr && _handler->GetIsInitialized();
        }

	internal:
		CefHandler::RetVal HandleAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url);
        CefHandler::RetVal HandleAfterCreated(CefRefPtr<CefBrowser> browser);
        CefHandler::RetVal HandleTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title);
        CefHandler::RetVal HandleLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame);
        CefHandler::RetVal HandleLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame);
        CefHandler::RetVal HandleJSBinding(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Value> object);
        CefHandler::RetVal HandleConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line);
        CefHandler::RetVal HandleBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefHandler::NavType navType, bool isRedirect);
		CefHandler::RetVal HandleBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefString& mimeType,int loadFlags);

		Handler* GetHandler()
		{
			return _handler;
		}

		HWND GetBrowserHwnd()
		{
			return _handler->GetBrowserHwnd();
		}

        CefRefPtr<CefBrowser> GetCefBrowser()
        {
            ThrowIfIsNotInitialized();
			return _handler->GetCefBrowser();
        }
	};
};