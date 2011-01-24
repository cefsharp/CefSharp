#include "stdafx.h"

#include "Handler.h"
#include "CefWebBrowser.h"
#include "Request.h"
#include "ReturnValue.h"
#include "StreamAdapter.h"
#include "CefSharp.h"
#include "BindingHandler.h"

namespace CefSharp
{
	using namespace System::Runtime::InteropServices;

	void Handler::RegisterAfterCreated(HandleAfterCreatedDelegate^ afterCreatedDelegate)
	{
		_afterCreatedDelegate = afterCreatedDelegate;
		_afterCreatedCallback = (HandleAfterCreatedFnPtr) Marshal::GetFunctionPointerForDelegate(afterCreatedDelegate).ToPointer();
	}

	void Handler::RegisterBeforeBrowse(HandleBeforeBrowseDelegate^ beforeBrowseDelegate)
	{
		_beforeBrowseDelegate = beforeBrowseDelegate;
		_beforeBrowseCallback = (HandleBeforeBrowseFnPtr) Marshal::GetFunctionPointerForDelegate(beforeBrowseDelegate).ToPointer();
	}

	void Handler::RegisterTitleChange(HandleTitleChangeDelegate^ titleChangeDelegate)
	{
		_titleChangeDelegate = titleChangeDelegate;
		_titleChangeCallback = (HandleTitleChangeFnPtr) Marshal::GetFunctionPointerForDelegate(titleChangeDelegate).ToPointer();
	}

	void Handler::RegisterAdressChange(HandleAddressChangeDelegate^ addressChangeDelegate)
	{
		_addressChangeDelegate = addressChangeDelegate;
		_addressChangeCallback = (HandleAddressChangeFnPtr) Marshal::GetFunctionPointerForDelegate(addressChangeDelegate).ToPointer();
	}

	void Handler::RegisterLoadStart(HandleLoadStartDelegate^ loadStartDelegate)
	{
		_loadStartDelegate = loadStartDelegate;
		_loadStartCallback = (HandleLoadStartFnPtr) Marshal::GetFunctionPointerForDelegate(loadStartDelegate).ToPointer();
	}

	void Handler::RegisterLoadEnd(HandleLoadEndDelegate^ loadEndDelegate)
	{
		_loadEndDelegate = loadEndDelegate;
		_loadEndCallback = (HandleLoadEndFnPtr) Marshal::GetFunctionPointerForDelegate(loadEndDelegate).ToPointer();
	}

	void Handler::RegisterJSBinding(HandleJSBindingDelegate^ jsBindingDelegate)
	{
		_jsBindingDelegate = jsBindingDelegate;
		_jsBindingCallback = (HandleJSBindingFnPtr) Marshal::GetFunctionPointerForDelegate(jsBindingDelegate).ToPointer();
	}

	void Handler::RegisterConsoleMessage(HandleConsoleMessageDelegate^ consoleMessageDelegate)
	{
		_consoleMessageDelegate = consoleMessageDelegate;
		_consoleMessageCallback = (HandleConsoleMessageFnPtr) Marshal::GetFunctionPointerForDelegate(consoleMessageDelegate).ToPointer();
	}

	void Handler::RegisterBeforeResourceLoad(HandleBeforeResourceLoadDelegate^ beforeResourceLoadDelegate)
	{
		_beforeResourceLoadDelegate = beforeResourceLoadDelegate;
		_beforeResourceLoadCallback = (HandleBeforeResourceLoadFnPtr) Marshal::GetFunctionPointerForDelegate(beforeResourceLoadDelegate).ToPointer();
	}

	CefHandler::RetVal Handler::HandleAfterCreated(CefRefPtr<CefBrowser> browser)
	{
		if(!browser->IsPopup())
        {
            _browserHwnd = browser->GetWindowHandle();
            _cefBrowser = browser;
        }

		if (_afterCreatedCallback != NULL)
			return _afterCreatedCallback(browser);

		return RV_CONTINUE;
	}

	CefHandler::RetVal Handler::HandleTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title)
	{
		if (_titleChangeCallback != NULL)
			return _titleChangeCallback(browser, title);

		return RV_CONTINUE;
	}

	CefHandler::RetVal Handler::HandleAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url)
	{
		if (_addressChangeCallback != NULL)
			return _addressChangeCallback(browser, frame, url);

		return RV_CONTINUE;
	}

	CefHandler::RetVal Handler::HandleBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, NavType navType, bool isRedirect)
	{
		if (_beforeBrowseCallback != NULL)
			return _beforeBrowseCallback(browser, frame, request, navType, isRedirect);

		return RV_CONTINUE;
	}

    CefHandler::RetVal Handler::HandleLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
	{
		if (_loadStartCallback != NULL)
			return _loadStartCallback(browser, frame);

		return RV_CONTINUE;
	}

    CefHandler::RetVal Handler::HandleLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
	{
		if (_loadEndCallback != NULL)
			return _loadEndCallback(browser, frame);

		return RV_CONTINUE;
	}

    CefHandler::RetVal Handler::HandleJSBinding(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Value> object)
	{
		if (_jsBindingCallback != NULL)
			return _jsBindingCallback(browser, frame, object);

		return RV_CONTINUE;
	}

	CefHandler::RetVal Handler::HandleBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefString& mimeType,int loadFlags)
	{
		if (_beforeResourceLoadCallback != NULL)
			return _beforeResourceLoadCallback(browser, request, redirectUrl, resourceStream, mimeType, loadFlags);

		return RV_CONTINUE;
	}

    CefHandler::RetVal Handler::HandleConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line)
	{
		if (_consoleMessageCallback != NULL)
			return _consoleMessageCallback(browser, message, source, line);

		return RV_CONTINUE;
	}
}