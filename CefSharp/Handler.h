#include "stdafx.h"
#pragma once

namespace CefSharp
{
    using namespace System;
	using namespace System::Text;

	delegate CefHandler::RetVal HandleAddressChangeDelegate(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url);
	delegate CefHandler::RetVal HandleAfterCreatedDelegate(CefRefPtr<CefBrowser> browser);
	delegate CefHandler::RetVal HandleTitleChangeDelegate(CefRefPtr<CefBrowser> browser, const CefString& title);
	delegate CefHandler::RetVal HandleLoadStartDelegate(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame);
	delegate CefHandler::RetVal HandleLoadEndDelegate(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame);
	delegate CefHandler::RetVal HandleJSBindingDelegate(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Value> object);
	delegate CefHandler::RetVal HandleConsoleMessageDelegate(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line);
	delegate CefHandler::RetVal HandleBeforeBrowseDelegate(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefHandler::NavType navType, bool isRedirect);
	delegate CefHandler::RetVal HandleBeforeResourceLoadDelegate(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefString& mimeType, int loadFlags);
	delegate CefHandler::RetVal HandleBeforeCreatedDelegate(CefRefPtr<CefBrowser> parentBrowser, CefWindowInfo& createInfo, bool popup, const CefPopupFeatures& popupFeatures, CefRefPtr<CefHandler>& handler, CefString& url, CefBrowserSettings& settings);
	delegate CefHandler::RetVal HandleLoadErrorDelegate(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefHandler::ErrorCode errorCode, const CefString& failedUrl, CefString& errorText);
	delegate CefHandler::RetVal HandleDownloadResponseDelegate(CefRefPtr<CefBrowser> browser, const CefString& mimeType, const CefString& fileName, int64 contentLength, CefRefPtr<CefDownloadHandler>& handler);
	delegate CefHandler::RetVal HandleAuthenticationRequestDelegate(CefRefPtr<CefBrowser> browser, bool isProxy, const CefString& host, const CefString& realm, const CefString& scheme, CefString& username, CefString& password);
	delegate CefHandler::RetVal HandleBeforeMenuDelegate(CefRefPtr<CefBrowser> browser, const CefHandler::MenuInfo& menuInfo);
	delegate CefHandler::RetVal HandleGetMenuLabelDelegate(CefRefPtr<CefBrowser> browser, CefHandler::MenuId menuId, CefString& label);
	delegate CefHandler::RetVal HandleMenuActionDelegate(CefRefPtr<CefBrowser> browser, CefHandler::MenuId menuId);
	delegate CefHandler::RetVal HandlePrintOptionsDelegate(CefRefPtr<CefBrowser> browser, CefHandler::CefPrintOptions& printOptions);
	delegate CefHandler::RetVal HandlePrintHeaderFooterDelegate(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefPrintInfo& printInfo, const CefString& url, const CefString& title, int currentPage, int maxPages, CefString& topLeft, CefString& topCenter, CefString& topRight, CefString& bottomLeft, CefString& bottomCenter, CefString& bottomRight);
	delegate CefHandler::RetVal HandleJSAlertDelegate(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message);
	delegate CefHandler::RetVal HandleJSConfirmDelegate(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message, bool& retval);
	delegate CefHandler::RetVal HandleJSPromptDelegate(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message, const CefString& defaultValue, bool& retval, CefString& result);
	delegate CefHandler::RetVal HandleBeforeWindowCloseDelegate(CefRefPtr<CefBrowser> browser);
	delegate CefHandler::RetVal HandleTakeFocusDelegate(CefRefPtr<CefBrowser> browser, bool reverse);
	delegate CefHandler::RetVal HandleSetFocusDelegate(CefRefPtr<CefBrowser> browser, bool isWidget);
	delegate CefHandler::RetVal HandleTooltipDelegate(CefRefPtr<CefBrowser> browser, CefString& text);
	delegate CefHandler::RetVal HandleKeyEventDelegate(CefRefPtr<CefBrowser> browser, CefHandler::KeyEventType type, int code, int modifiers, bool isSystemKey);
	delegate CefHandler::RetVal HandleFindResultDelegate(CefRefPtr<CefBrowser> browser, int identifier, int count, const CefRect& selectionRect, int activeMatchOrdinal, bool finalUpdate);

	typedef CefHandler::RetVal (__stdcall *HandleAddressChangeFnPtr)(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url);
	typedef CefHandler::RetVal (__stdcall *HandleAfterCreatedFnPtr)(CefRefPtr<CefBrowser> browser);
	typedef CefHandler::RetVal (__stdcall *HandleTitleChangeFnPtr)(CefRefPtr<CefBrowser> browser, const CefString& title);
	typedef CefHandler::RetVal (__stdcall *HandleLoadStartFnPtr)(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame);
	typedef CefHandler::RetVal (__stdcall *HandleLoadEndFnPtr)(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame);
	typedef CefHandler::RetVal (__stdcall *HandleJSBindingFnPtr)(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Value> object);
	typedef CefHandler::RetVal (__stdcall *HandleConsoleMessageFnPtr)(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line);
	typedef CefHandler::RetVal (__stdcall *HandleBeforeBrowseFnPtr)(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefHandler::NavType navType, bool isRedirect);
	typedef CefHandler::RetVal (__stdcall *HandleBeforeResourceLoadFnPtr)(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefString& mimeType,int loadFlags);
	typedef CefHandler::RetVal (__stdcall *HandleBeforeCreatedFnPtr)(CefRefPtr<CefBrowser> parentBrowser, CefWindowInfo& createInfo, bool popup, const CefPopupFeatures& popupFeatures, CefRefPtr<CefHandler>& handler, CefString& url, CefBrowserSettings& settings);
	typedef CefHandler::RetVal (__stdcall *HandleLoadErrorFnPtr)(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefHandler::ErrorCode errorCode, const CefString& failedUrl, CefString& errorText);
	typedef CefHandler::RetVal (__stdcall *HandleDownloadResponseFnPtr)(CefRefPtr<CefBrowser> browser, const CefString& mimeType, const CefString& fileName, int64 contentLength, CefRefPtr<CefDownloadHandler>& handler);
	typedef CefHandler::RetVal (__stdcall *HandleAuthenticationRequestFnPtr)(CefRefPtr<CefBrowser> browser, bool isProxy, const CefString& host, const CefString& realm, const CefString& scheme, CefString& username, CefString& password);
	typedef CefHandler::RetVal (__stdcall *HandleBeforeMenuFnPtr)(CefRefPtr<CefBrowser> browser, const CefHandler::MenuInfo& menuInfo);
	typedef CefHandler::RetVal (__stdcall *HandleGetMenuLabelFnPtr)(CefRefPtr<CefBrowser> browser, CefHandler::MenuId menuId, CefString& label);
	typedef CefHandler::RetVal (__stdcall *HandleMenuActionFnPtr)(CefRefPtr<CefBrowser> browser, CefHandler::MenuId menuId);
	typedef CefHandler::RetVal (__stdcall *HandlePrintOptionsFnPtr)(CefRefPtr<CefBrowser> browser, CefHandler::CefPrintOptions& printOptions);
	typedef CefHandler::RetVal (__stdcall *HandlePrintHeaderFooterFnPtr)(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefPrintInfo& printInfo, const CefString& url, const CefString& title, int currentPage, int maxPages, CefString& topLeft, CefString& topCenter, CefString& topRight, CefString& bottomLeft, CefString& bottomCenter, CefString& bottomRight);
	typedef CefHandler::RetVal (__stdcall *HandleJSAlertFnPtr)(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message);
	typedef CefHandler::RetVal (__stdcall *HandleJSConfirmFnPtr)(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message, bool& retval);
	typedef CefHandler::RetVal (__stdcall *HandleJSPromptFnPtr)(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message, const CefString& defaultValue, bool& retval, CefString& result);
	typedef CefHandler::RetVal (__stdcall *HandleBeforeWindowCloseFnPtr)(CefRefPtr<CefBrowser> browser);
	typedef CefHandler::RetVal (__stdcall *HandleTakeFocusFnPtr)(CefRefPtr<CefBrowser> browser, bool reverse);
	typedef CefHandler::RetVal (__stdcall *HandleSetFocusFnPtr)(CefRefPtr<CefBrowser> browser, bool isWidget);
	typedef CefHandler::RetVal (__stdcall *HandleTooltipFnPtr)(CefRefPtr<CefBrowser> browser, CefString& text);
	typedef CefHandler::RetVal (__stdcall *HandleKeyEventFnPtr)(CefRefPtr<CefBrowser> browser, CefHandler::KeyEventType type, int code, int modifiers, bool isSystemKey);
	typedef CefHandler::RetVal (__stdcall *HandleFindResultFnPtr)(CefRefPtr<CefBrowser> browser, int identifier, int count, const CefRect& selectionRect, int activeMatchOrdinal, bool finalUpdate);

	class Handler : public CefThreadSafeBase<CefHandler>
    {
	private:
		HWND _browserHwnd;
		CefRefPtr<CefBrowser> _cefBrowser;

		gcroot<HandleAddressChangeDelegate^> _addressChangeDelegate;
		gcroot<HandleAfterCreatedDelegate^> _afterCreatedDelegate;
		gcroot<HandleTitleChangeDelegate^> _titleChangeDelegate;
		gcroot<HandleBeforeBrowseDelegate^> _beforeBrowseDelegate;
		gcroot<HandleLoadStartDelegate^> _loadStartDelegate;
		gcroot<HandleLoadEndDelegate^> _loadEndDelegate;
		gcroot<HandleBeforeResourceLoadDelegate^> _beforeResourceLoadDelegate;
		gcroot<HandleJSBindingDelegate^> _jsBindingDelegate;
		gcroot<HandleConsoleMessageDelegate^> _consoleMessageDelegate;

		HandleAddressChangeFnPtr _addressChangeCallback;
		HandleAfterCreatedFnPtr _afterCreatedCallback;
		HandleTitleChangeFnPtr _titleChangeCallback;
		HandleBeforeBrowseFnPtr _beforeBrowseCallback;
		HandleLoadStartFnPtr _loadStartCallback;
		HandleLoadEndFnPtr _loadEndCallback;
		HandleBeforeResourceLoadFnPtr _beforeResourceLoadCallback;
		HandleJSBindingFnPtr _jsBindingCallback;
		HandleConsoleMessageFnPtr _consoleMessageCallback;

	public:
		Handler()
		{
			_addressChangeCallback = NULL;
			_afterCreatedCallback = NULL;
			_titleChangeCallback = NULL;
			_beforeBrowseCallback = NULL;
			_loadStartCallback = NULL;
			_loadEndCallback = NULL;
			_beforeResourceLoadCallback = NULL;
			_jsBindingCallback = NULL;
			_consoleMessageCallback = NULL;
		}

		// todo: verify this is getting cleaned up
		~Handler()
		{
			_addressChangeDelegate = nullptr;
			_afterCreatedDelegate = nullptr;
			_titleChangeDelegate = nullptr;
			_beforeBrowseDelegate = nullptr;
			_loadStartDelegate = nullptr;
			_loadEndDelegate = nullptr;
			_beforeResourceLoadDelegate = nullptr;
			_jsBindingDelegate = nullptr;
			_consoleMessageDelegate = nullptr;
		}

		void RegisterAdressChange(HandleAddressChangeDelegate^ addressChangeDelegate);
		void RegisterBeforeBrowse(HandleBeforeBrowseDelegate^ beforeBrowseDelegate);
		void RegisterAfterCreated(HandleAfterCreatedDelegate^ afterCreatedDelegate);
		void RegisterTitleChange(HandleTitleChangeDelegate^ titleChangeDelegate);
		void RegisterLoadStart(HandleLoadStartDelegate^ loadStartDelegate);
		void RegisterLoadEnd(HandleLoadEndDelegate^ loadEndDelegate);
		void RegisterJSBinding(HandleJSBindingDelegate^ jsBindingDelegate);
		void RegisterConsoleMessage(HandleConsoleMessageDelegate^ consoleMessageDelegate);
		void RegisterBeforeResourceLoad(HandleBeforeResourceLoadDelegate^ beforeResourceLoadDelegate);

		virtual RetVal HandleAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url);
        virtual RetVal HandleAfterCreated(CefRefPtr<CefBrowser> browser);
        virtual RetVal HandleTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title);
        virtual RetVal HandleLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame);
        virtual RetVal HandleLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame);
        virtual RetVal HandleJSBinding(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Value> object);
        virtual RetVal HandleConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line);
        virtual RetVal HandleBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, NavType navType, bool isRedirect);
		virtual RetVal HandleBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefString& mimeType,int loadFlags);

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

		HWND GetBrowserHwnd()
		{
			return _browserHwnd;
		}

		CefRefPtr<CefBrowser> GetCefBrowser()
        {
            return _cefBrowser;
        }

		bool GetIsInitialized()
        {
			return _cefBrowser != nullptr;
        }
	};
};