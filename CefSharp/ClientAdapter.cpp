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
    bool ClientAdapter::OnBeforePopup(CefRefPtr<CefBrowser> parentBrowser, const CefPopupFeatures& popupFeatures, CefWindowInfo& windowInfo, const CefString& url, CefRefPtr<CefClient>& client, CefBrowserSettings& settings)
    {
        ILifeSpanHandler^ handler = _browserControl->LifeSpanHandler;
        if (handler == nullptr)
        {
            return false;
        }

        return handler->OnBeforePopup(_browserControl, toClr(url),
            windowInfo.m_x, windowInfo.m_y, windowInfo.m_nWidth, windowInfo.m_nHeight);
    }

    void ClientAdapter::OnAfterCreated(CefRefPtr<CefBrowser> browser)
    {
        if(!browser->IsPopup())
        {
            _browserHwnd = browser->GetWindowHandle();
            _cefBrowser = browser;

            _browserControl->OnInitialized();
        }
    }

    void ClientAdapter::OnBeforeClose(CefRefPtr<CefBrowser> browser)
    {
        if (_browserHwnd == browser->GetWindowHandle())
        {
            ILifeSpanHandler^ handler = _browserControl->LifeSpanHandler;
            if (handler != nullptr)
            {
                handler->OnBeforeClose(_browserControl);
            }

            _cefBrowser = nullptr;
        }
    }

    void ClientAdapter::OnAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url)
    {
        if (frame->IsMain())
        {
            _browserControl->Address = toClr(url);
        }
    }

    void ClientAdapter::OnContentsSizeChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int width, int height)
    {
        if (frame->IsMain())
        {
            _browserControl->ContentsWidth = width;
            _browserControl->ContentsHeight = height;
        }
    }

    void ClientAdapter::OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title)
    {
        _browserControl->Title = toClr(title);
    }

    bool ClientAdapter::OnTooltip(CefRefPtr<CefBrowser> browser, CefString& text)
    {
        String^ tooltip = toClr(text);

        if (tooltip != _tooltip)
        {
            _tooltip = tooltip;
            _browserControl->TooltipText = _tooltip;
        }

        return true;
    }

    bool ClientAdapter::OnConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line)
    {
        String^ messageStr = toClr(message);
        String^ sourceStr = toClr(source);
        _browserControl->OnConsoleMessage(messageStr, sourceStr, line);

        return true;
    }

    bool ClientAdapter::OnKeyEvent(CefRefPtr<CefBrowser> browser, KeyEventType type, int code, int modifiers, bool isSystemKey, bool isAfterJavaScript)
    {
        IKeyboardHandler^ handler = _browserControl->KeyboardHandler;
        if (handler == nullptr)
        {
            return false;
        }

        return handler->OnKeyEvent(_browserControl, (KeyType)type, code, modifiers, isSystemKey, isAfterJavaScript);
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
            _browserControl->SetNavState(true, false, false);
        }

        _browserControl->OnFrameLoadStart(toClr(frame->GetURL()), frame->IsMain());
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
            _browserControl->SetNavState(false, browser->CanGoBack(), browser->CanGoForward());
        }

        _browserControl->OnFrameLoadEnd(toClr(frame->GetURL()), frame->IsMain());
    }

    bool ClientAdapter::OnLoadError(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, ErrorCode errorCode, const CefString& failedUrl, CefString& errorText)
    {
        ILoadHandler^ handler = _browserControl->LoadHandler;
        if (handler == nullptr)
        {
            return false;
        }

        String^ errorString = nullptr;
        handler->OnLoadError(_browserControl, toClr(failedUrl), errorCode, errorString);

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

    bool ClientAdapter::OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, NavType navType, bool isRedirect)
    {
        IRequestHandler^ handler = _browserControl->RequestHandler;
        if (handler == nullptr)
        {
            return false;
        }

        CefRequestWrapper^ wrapper = gcnew CefRequestWrapper(request);
        NavigationType navigationType = (NavigationType)navType;

        return handler->OnBeforeBrowse(_browserControl, wrapper, navigationType, isRedirect);
    }

    bool ClientAdapter::OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefRefPtr<CefResponse> response, int loadFlags)
    {
        IRequestHandler^ handler = _browserControl->RequestHandler;
        if (handler == nullptr)
        {
            return false;
        }

        CefRequestWrapper^ wrapper = gcnew CefRequestWrapper(request);
        RequestResponse^ requestResponse = gcnew RequestResponse(wrapper);

        bool ret = handler->OnBeforeResourceLoad(_browserControl, requestResponse);

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

    bool ClientAdapter::GetDownloadHandler(CefRefPtr<CefBrowser> browser, const CefString& mimeType, const CefString& fileName, int64 contentLength, CefRefPtr<CefDownloadHandler>& handler)
    {
        IRequestHandler^ requestHandler = _browserControl->RequestHandler;
        if (requestHandler == nullptr)
        {
            return false;
        }

        IDownloadHandler^ downloadHandler;
        bool ret = requestHandler->GetDownloadHandler(_browserControl, toClr(mimeType), toClr(fileName), contentLength, downloadHandler);
        if (ret)
        {
            handler = new DownloadAdapter(downloadHandler);
        }

        return ret;
    }

    bool ClientAdapter::GetAuthCredentials(CefRefPtr<CefBrowser> browser, bool isProxy, const CefString& host, int port, const CefString& realm, const CefString& scheme, CefString& username, CefString& password)
    {
        IRequestHandler^ handler = _browserControl->RequestHandler;
        if (handler == nullptr)
        {
            return false;
        }

        String^ usernameString = nullptr;
        String^ passwordString = nullptr;
        bool handled = handler->GetAuthCredentials(_browserControl, isProxy, toClr(host), port, toClr(realm), toClr(scheme), usernameString, passwordString);
        
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

    void ClientAdapter::OnResourceResponse(CefRefPtr<CefBrowser> browser, const CefString& url, CefRefPtr<CefResponse> response, CefRefPtr<CefContentFilter>& filter)
    {
        IRequestHandler^ handler = _browserControl->RequestHandler;
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
            _browserControl,
            toClr(url),
            response->GetStatus(),
            toClr(response->GetStatusText()),
            toClr(response->GetMimeType()),
            headers);
    }

    void ClientAdapter::OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context)
    {
        for each(KeyValuePair<String^, Object^>^ kvp in CEF::GetBoundObjects())
        {
            BindingHandler::Bind(kvp->Key, kvp->Value, context->GetGlobal());
        }

        for each(KeyValuePair<String^, Object^>^ kvp in _browserControl->GetBoundObjects())
        {
            BindingHandler::Bind(kvp->Key, kvp->Value, context->GetGlobal());
        }
    }

    bool ClientAdapter::OnBeforeMenu(CefRefPtr<CefBrowser> browser, const CefMenuInfo& menuInfo)
    {
        IMenuHandler^ handler = _browserControl->MenuHandler;
        if (handler == nullptr)
        {
            return false;
        }

        return handler->OnBeforeMenu(_browserControl);
    }

    void ClientAdapter::OnTakeFocus(CefRefPtr<CefBrowser> browser, bool next)
    {
        _browserControl->OnTakeFocus(next);
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
