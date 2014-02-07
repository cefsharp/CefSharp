#include "Stdafx.h"

#include "Internals/JavascriptBinding/BindingHandler.h"
#include "ClientAdapter.h"
#include "CefSharp.h"
#include "StreamAdapter.h"
#include "DownloadAdapter.h"
#include "IWebBrowser.h"
#include "ILifeSpanHandler.h"
#include "ILoadHandler.h"
#include "IRequestHandler.h"
#include "IMenuHandler.h"
#include "IKeyboardHandler.h"
#include "IJsDialogHandler.h"

using namespace std;
using namespace CefSharp::Internals::JavascriptBinding;

namespace CefSharp
{
    bool ClientAdapter::_OnBeforePopup(ClientAdapter* const _this, CefWindowInfo* const _windowInfo, const CefString* const _url)
    {
        CefWindowInfo& windowInfo = *_windowInfo;

        ILifeSpanHandler^ handler = _this->_browserControl->LifeSpanHandler;
        if (handler == nullptr)
        {
            return false;
        }

        return handler->OnBeforePopup(_this->_browserControl, toClr(*_url),
            windowInfo.m_x, windowInfo.m_y, windowInfo.m_nWidth, windowInfo.m_nHeight);
    }

    bool ClientAdapter::OnBeforePopup(CefRefPtr<CefBrowser> parentBrowser, const CefPopupFeatures& popupFeatures, CefWindowInfo& windowInfo, const CefString& url, CefRefPtr<CefClient>& client, CefBrowserSettings& settings)
    {
        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_OnBeforePopup, this, &windowInfo, &url);
        } else {
            return _OnBeforePopup(this, &windowInfo, &url);
        }
    }

    void ClientAdapter::_OnInitialized(ClientAdapter* const _this)
    {
        _this->_browserControl->OnInitialized();
    }

    void ClientAdapter::OnAfterCreated(CefRefPtr<CefBrowser> browser)
    {
        if(!browser->IsPopup())
        {
            _browserHwnd = browser->GetWindowHandle();
            _cefBrowser = browser;

            if (IsCrossDomainCallRequired()) {
                msclr::call_in_appdomain(GetAppDomainId(), &_OnInitialized, this);
            } else {
                _OnInitialized(this);
            }
        }
    }

    void ClientAdapter::_OnBeforeClose(ClientAdapter* const _this)
    {
        ILifeSpanHandler^ handler = _this->_browserControl->LifeSpanHandler;
        if (handler != nullptr)
        {
            handler->OnBeforeClose(_this->_browserControl);
        }
    }

    void ClientAdapter::OnBeforeClose(CefRefPtr<CefBrowser> browser)
    {
        if (_browserHwnd == browser->GetWindowHandle())
        {
            if (IsCrossDomainCallRequired()) {
                msclr::call_in_appdomain(GetAppDomainId(), &_OnBeforeClose, this);
            } else {
                _OnBeforeClose(this);
            }

            _cefBrowser = nullptr;
        }
    }

    void ClientAdapter::_SetAddress(ClientAdapter* const _this, const CefString* const _url)
    {
        _this->_browserControl->Address = toClr(*_url);
    }

    void ClientAdapter::OnAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url)
    {
        if (frame->IsMain())
        {
            if (IsCrossDomainCallRequired()) {
                msclr::call_in_appdomain(GetAppDomainId(), &_SetAddress, this, &url);
            } else {
                _SetAddress(this, &url);
            }
        }
    }

    void ClientAdapter::_SetSize(ClientAdapter* const _this, int width, int height)
    {
        _this->_browserControl->ContentsWidth = width;
        _this->_browserControl->ContentsHeight = height;
    }

    void ClientAdapter::OnContentsSizeChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int width, int height)
    {
        if (frame->IsMain())
        {
            if (IsCrossDomainCallRequired()) {
                msclr::call_in_appdomain(GetAppDomainId(), &_SetSize, this, width, height);
            } else {
                _SetSize(this, width, height);
            }
        }
    }

    void ClientAdapter::_SetTitle(ClientAdapter* const _this, const CefString* const _title)
    {
        _this->_browserControl->Title = toClr(*_title);
    }

    void ClientAdapter::OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title)
    {
        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_SetTitle, this, &title);
        } else {
            _SetTitle(this, &title);
        }
    }

    void ClientAdapter::_OnTooltip(ClientAdapter* const _this, CefString* const _text)
    {
        String^ tooltip = toClr(*_text);

        if (tooltip != _this->_tooltip)
        {
            _this->_tooltip = tooltip;
            _this->_browserControl->TooltipText = tooltip;
        }
    }

    bool ClientAdapter::OnTooltip(CefRefPtr<CefBrowser> browser, CefString& text)
    {
        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_OnTooltip, this, &text);
        } else {
            _OnTooltip(this, &text);
        }
        
        return true;
    }

    bool ClientAdapter::_OnConsoleMessage(ClientAdapter* const _this, const CefString* const _message, const CefString* const _source, int line)
    {
        String^ messageStr = toClr(*_message);
        String^ sourceStr = toClr(*_source);
        _this->_browserControl->OnConsoleMessage(messageStr, sourceStr, line);

        return true;
    }

    bool ClientAdapter::OnConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line)
    {
        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_OnConsoleMessage, this, &message, &source, line);
        } else {
            return _OnConsoleMessage(this, &message, &source, line);
        }
    }

    bool ClientAdapter::_OnKeyEvent(ClientAdapter* const _this, KeyEventType type, int code, int modifiers, bool isSystemKey, bool isAfterJavaScript)
    {
        IKeyboardHandler^ handler = _this->_browserControl->KeyboardHandler;
        if (handler == nullptr)
        {
            return false;
        }

        return handler->OnKeyEvent(_this->_browserControl, (KeyType)type, code, modifiers, isSystemKey, isAfterJavaScript);
    }

    bool ClientAdapter::OnKeyEvent(CefRefPtr<CefBrowser> browser, KeyEventType type, int code, int modifiers, bool isSystemKey, bool isAfterJavaScript)
    {
        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_OnKeyEvent, this, type, code, modifiers, isSystemKey, isAfterJavaScript);
        } else {
            return _OnKeyEvent(this, type, code, modifiers, isSystemKey, isAfterJavaScript);
        }
    }

    void ClientAdapter::OnLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
    {
        if (browser->IsPopup())
        {
            return;
        }

        AutoLock lock_scope(this);
        if (frame->IsMain())
        {
            if (IsCrossDomainCallRequired()) {
                msclr::call_in_appdomain(GetAppDomainId(), &_SetNavState, this, true, false, false);
            } else {
                _SetNavState(this, true, false, false);
            }
        }

        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_OnFrameLoadStart, this, frame);
        } else {
            _OnFrameLoadStart(this, frame);
        }
    }

    void ClientAdapter::_SetNavState(ClientAdapter* const _this, bool isLoading, bool canGoBack, bool canGoForward)
    {
        _this->_browserControl->SetNavState(isLoading, canGoBack, canGoForward);
    }

    void ClientAdapter::_OnFrameLoadStart(ClientAdapter* const _this, CefRefPtr<CefFrame> frame)
    {
        _this->_browserControl->OnFrameLoadStart(toClr(frame->GetURL()));
    }

    void ClientAdapter::OnLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int httpStatusCode)
    {
        if(browser->IsPopup())
        {
            return;
        }

        AutoLock lock_scope(this);
        if (frame->IsMain())
        {
            if (IsCrossDomainCallRequired()) {
                msclr::call_in_appdomain(GetAppDomainId(), &_SetNavState, this, false, browser->CanGoBack(), browser->CanGoForward());
            } else {
                _SetNavState(this, false, browser->CanGoBack(), browser->CanGoForward());
            }
        }

        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_OnFrameLoadEnd, this, frame);
        } else {
            _OnFrameLoadEnd(this, frame);
        }
    }

    void ClientAdapter::_OnFrameLoadEnd(ClientAdapter* const _this, CefRefPtr<CefFrame> frame)
    {
        _this->_browserControl->OnFrameLoadEnd(toClr(frame->GetURL()));
    }

    bool ClientAdapter::_OnLoadError(ClientAdapter* const _this, ErrorCode errorCode, const CefString* const _failedUrl, CefString* const _errorText)
    {
        CefString& errorText = *_errorText;

        ILoadHandler^ handler = _this->_browserControl->LoadHandler;
        if (handler == nullptr)
        {
            return false;
        }

        String^ errorString = nullptr;
        handler->OnLoadError(_this->_browserControl, toClr(*_failedUrl), errorCode, errorString);

        if (errorString == nullptr)
        {
            return false;
        }
        else
        {
            errorText = toNative(errorString);
            return true;
        }
    }

    bool ClientAdapter::OnLoadError(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, ErrorCode errorCode, const CefString& failedUrl, CefString& errorText)
    {
       if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_OnLoadError, this, errorCode, &failedUrl, &errorText);
        } else {
            return _OnLoadError(this, errorCode, &failedUrl, &errorText);
        }
    }

    bool ClientAdapter::_OnBeforeBrowse(ClientAdapter* const _this, CefRefPtr<CefRequest> request, NavType navType, bool isRedirect)
    {
        IRequestHandler^ handler = _this->_browserControl->RequestHandler;
        if (handler == nullptr)
        {
            return false;
        }

        CefRequestWrapper^ wrapper = gcnew CefRequestWrapper(request);
        NavigationType navigationType = (NavigationType)navType;

        return handler->OnBeforeBrowse(_this->_browserControl, wrapper, navigationType, isRedirect);
    }

    bool ClientAdapter::OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, NavType navType, bool isRedirect)
    {
        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_OnBeforeBrowse, this, request, navType, isRedirect);
        } else {
            return _OnBeforeBrowse(this, request, navType, isRedirect);
        }
    }

    bool ClientAdapter::_OnBeforeResourceLoad(ClientAdapter* const _this, CefRefPtr<CefRequest> request, CefString* const _redirectUrl, CefRefPtr<CefStreamReader>* const _resourceStream, CefRefPtr<CefResponse> response)
    {
        CefString& redirectUrl = *_redirectUrl;
        CefRefPtr<CefStreamReader>&  resourceStream = *_resourceStream;

        IRequestHandler^ handler = _this->_browserControl->RequestHandler;
        if (handler == nullptr)
        {
            return false;
        }

        CefRequestWrapper^ wrapper = gcnew CefRequestWrapper(request);
        RequestResponse^ requestResponse = gcnew RequestResponse(wrapper);

        bool ret = handler->OnBeforeResourceLoad(_this->_browserControl, requestResponse);

        if (requestResponse->Action == ResponseAction::Redirect)
        {
            redirectUrl = toNative(requestResponse->RedirectUrl);
        }
        else if (requestResponse->Action == ResponseAction::Respond)
        {
            CefRefPtr<StreamAdapter> adapter = new StreamAdapter(requestResponse->ResponseStream);

            resourceStream = CefStreamReader::CreateForHandler(static_cast<CefRefPtr<CefReadHandler>>(adapter));
            response->SetMimeType(toNative(requestResponse->MimeType));
            response->SetStatus(requestResponse->StatusCode);
            response->SetStatusText(toNative(requestResponse->StatusText));

            CefResponse::HeaderMap map;
            if(requestResponse->ResponseHeaders != nullptr)
            {
                for each(KeyValuePair<String^, String^>^ kvp in requestResponse->ResponseHeaders)
                {
                    map.insert(pair<CefString,CefString>(toNative(kvp->Key),toNative(kvp->Value)));
                }
            }
            response->SetHeaderMap(map);
        }

        return ret;
    }

    bool ClientAdapter::OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefRefPtr<CefResponse> response, int loadFlags)
    {
        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_OnBeforeResourceLoad, this, request, &redirectUrl, &resourceStream, response);
        } else {
            return _OnBeforeResourceLoad(this, request, &redirectUrl, &resourceStream, response);
        }
    }

    bool ClientAdapter::_GetDownloadHandler(ClientAdapter* const _this, const CefString* const _mimeType, const CefString* const _fileName, int64 contentLength, CefRefPtr<CefDownloadHandler>* const _handler)
    {
        CefRefPtr<CefDownloadHandler>& handler = *_handler;

        IRequestHandler^ requestHandler = _this->_browserControl->RequestHandler;
        if (requestHandler == nullptr)
        {
            return false;
        }

        IDownloadHandler^ downloadHandler;
        bool ret = requestHandler->GetDownloadHandler(_this->_browserControl, toClr(*_mimeType), toClr(*_fileName), contentLength, downloadHandler);
        if (ret)
        {
            handler = new DownloadAdapter(downloadHandler);
        }

        return ret;
    }

    bool ClientAdapter::GetDownloadHandler(CefRefPtr<CefBrowser> browser, const CefString& mimeType, const CefString& fileName, int64 contentLength, CefRefPtr<CefDownloadHandler>& handler)
    {
        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_GetDownloadHandler, this, &mimeType, &fileName, contentLength, &handler);
        } else {
            return _GetDownloadHandler(this, &mimeType, &fileName, contentLength, &handler);
        }
    }

    bool ClientAdapter::_GetAuthCredentials(ClientAdapter* const _this, bool isProxy, const CefString* const _host, int port, const CefString* const _realm, const CefString* const _scheme, CefString* const _username, CefString* const _password)
    {
        CefString& username = *_username;
        CefString& password = *_password;

        IRequestHandler^ handler = _this->_browserControl->RequestHandler;
        if (handler == nullptr)
        {
            return false;
        }

        String^ usernameString = nullptr;
        String^ passwordString = nullptr;
        bool handled = handler->GetAuthCredentials(_this->_browserControl, isProxy, toClr(*_host), port, toClr(*_realm), toClr(*_scheme), usernameString, passwordString);
        
        if (usernameString != nullptr)
        {
            username = toNative(usernameString);
        }
        if (passwordString != nullptr)
        {
            password = toNative(passwordString);
        }
    
        return handled;
    }

    bool ClientAdapter::GetAuthCredentials(CefRefPtr<CefBrowser> browser, bool isProxy, const CefString& host, int port, const CefString& realm, const CefString& scheme, CefString& username, CefString& password)
    {
        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_GetAuthCredentials, this, isProxy, &host, port, &realm, &scheme, &username, &password);
        } else {
            return _GetAuthCredentials(this, isProxy, &host, port, &realm, &scheme, &username, &password);
        }
    }

    void ClientAdapter::_OnResourceResponse(ClientAdapter* const _this, const CefString* const _url, CefRefPtr<CefResponse> response)
    {
        IRequestHandler^ handler = _this->_browserControl->RequestHandler;
        if (handler == nullptr)
        {
            return;
        }

        WebHeaderCollection^ headers = gcnew WebHeaderCollection();
        CefResponse::HeaderMap map;
        response->GetHeaderMap(map);
        for (CefResponse::HeaderMap::iterator it = map.begin(); it != map.end(); ++it)
        {
            try
            {
                headers->Add(toClr(it->first), toClr(it->second));
            }
            catch (Exception ^ex)
            {
                // adding a header with invalid characters can cause an exception to be
                // thrown. we will drop those headers for now.
                // we could eventually use reflection to call headers->AddWithoutValidate().
            }
        }

        handler->OnResourceResponse(
            _this->_browserControl,
            toClr(*_url),
            response->GetStatus(),
            toClr(response->GetStatusText()),
            toClr(response->GetMimeType()),
            headers);
    }

    void ClientAdapter::OnResourceResponse(CefRefPtr<CefBrowser> browser, const CefString& url, CefRefPtr<CefResponse> response, CefRefPtr<CefContentFilter>& filter)
    {
        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_OnResourceResponse, this, &url, response);
        } else {
            _OnResourceResponse(this, &url, response);
        }
    }

    void ClientAdapter::_OnContextCreated(ClientAdapter* const _this, CefRefPtr<CefV8Context> context)
    {
        for each(KeyValuePair<String^, Object^>^ kvp in _this->_browserControl->GetBoundObjects())
        {
            BindingHandler::Bind(kvp->Key, kvp->Value, context->GetGlobal());
        }
    }

    void ClientAdapter::OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context)
    {
        for each(KeyValuePair<String^, Object^>^ kvp in CEF::GetBoundObjects())
        {
            BindingHandler::Bind(kvp->Key, kvp->Value, context->GetGlobal());
        }

        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_OnContextCreated, this, context);
        } else {
            _OnContextCreated(this, context);
        }
    }

    bool ClientAdapter::_OnBeforeMenu(ClientAdapter* const _this)
    {
        IMenuHandler^ handler = _this->_browserControl->MenuHandler;
        if (handler == nullptr)
        {
            return false;
        }

        return handler->OnBeforeMenu(_this->_browserControl);
    }

    bool ClientAdapter::OnBeforeMenu(CefRefPtr<CefBrowser> browser, const CefMenuInfo& menuInfo)
    {
        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_OnBeforeMenu, this);
        } else {
            return _OnBeforeMenu(this);
        }
    }

    void ClientAdapter::_OnTakeFocus(ClientAdapter* const _this, bool next)
    {
        _this->_browserControl->OnTakeFocus(next);
    }

    void ClientAdapter::OnTakeFocus(CefRefPtr<CefBrowser> browser, bool next)
    {
        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_OnTakeFocus, this, next);
        } else {
            _OnTakeFocus(this, next);
        }
    }

    bool ClientAdapter::OnJSAlert(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message)
    {
        IJsDialogHandler^ handler = _browserControl->JsDialogHandler;
        if (handler == nullptr)
        {
            return false;
        }

        bool handled = handler->OnJSAlert(_browserControl, toClr(frame->GetURL()), toClr(message));

        return handled;
    }

    bool ClientAdapter::OnJSConfirm(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message, bool& retval)
    {
        IJsDialogHandler^ handler = _browserControl->JsDialogHandler;
        if (handler == nullptr)
        {
            return false;
        }

        bool handled = handler->OnJSConfirm(_browserControl, toClr(frame->GetURL()), toClr(message), retval);

        return handled;
    }

    bool ClientAdapter::OnJSPrompt(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& message, const CefString& defaultValue, bool& retval, CefString& result)
    {
        IJsDialogHandler^ handler = _browserControl->JsDialogHandler;
        if (handler == nullptr)
        {
            return false;
        }

        String^ resultString = nullptr;

        bool handled = handler->OnJSPrompt(_browserControl, toClr(frame->GetURL()), toClr(message), toClr(defaultValue), retval, resultString);

        if(resultString != nullptr)
        {
            result = toNative(resultString);
        }

        return handled;
    }
}
