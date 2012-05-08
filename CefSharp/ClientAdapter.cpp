#include "stdafx.h"

#include "BindingHandler.h"
#include "ClientAdapter.h"
#include "CefSharp.h"
#include "IBeforePopup.h"
#include "IBeforeBrowse.h"
#include "IBeforeResourceLoad.h"
#include "IBeforeMenu.h"
#include "IAfterResponse.h"
#include "IAfterLoadError.h"
#include "StreamAdapter.h"

namespace CefSharp
{
    bool ClientAdapter::OnBeforePopup(CefRefPtr<CefBrowser> parentBrowser, const CefPopupFeatures& popupFeatures, CefWindowInfo& windowInfo, const CefString& url, CefRefPtr<CefClient>& client, CefBrowserSettings& settings)
    {
        IBeforePopup^ handler = _browserControl->BeforePopupHandler;
        return handler != nullptr &&
            handler->HandleBeforePopup(toClr(url), windowInfo.m_x, windowInfo.m_y, windowInfo.m_nWidth, windowInfo.m_nHeight);
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

        _browserControl->OnFrameLoadStart();
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

        _browserControl->OnFrameLoadEnd();
    }

    bool ClientAdapter::OnLoadError(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, ErrorCode errorCode, const CefString& failedUrl, CefString& errorText)
    {
        IAfterLoadError^ handler = _browserControl->AfterLoadErrorHandler;
        return handler != nullptr &&
            handler->HandleLoadError();
    }

    bool ClientAdapter::OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, NavType navType, bool isRedirect)
    {
        IBeforeBrowse^ handler = _browserControl->BeforeBrowseHandler;
        if (handler != nullptr)
        {
            CefRequestWrapper^ wrapper = gcnew CefRequestWrapper(request);
            NavigationType navigationType = (NavigationType)navType;

            return handler->HandleBeforeBrowse(_browserControl, wrapper, navigationType, isRedirect);
        }
        else
        {
            return false;
        }
    }

    bool ClientAdapter::OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefRefPtr<CefResponse> response, int loadFlags)
    {
        IBeforeResourceLoad^ handler = _browserControl->BeforeResourceLoadHandler;
        if(handler != nullptr)
        {
            CefRequestWrapper^ wrapper = gcnew CefRequestWrapper(request);
            RequestResponse^ requestResponse = gcnew RequestResponse(wrapper);

            handler->HandleBeforeResourceLoad(_browserControl, requestResponse);

            if(requestResponse->Action == ResponseAction::Respond)
            {
                CefRefPtr<StreamAdapter> adapter = new StreamAdapter(requestResponse->ResponseStream);
                resourceStream = CefStreamReader::CreateForHandler(static_cast<CefRefPtr<CefReadHandler>>(adapter));
                response->SetMimeType(toNative(requestResponse->MimeType));
                return false;
            }
            else if(requestResponse->Action == ResponseAction::Cancel)
            {
                return true;
            }
            else if(requestResponse->Action == ResponseAction::Redirect)
            {
                redirectUrl = toNative(requestResponse->RedirectUrl);
            }
        }

        return false;
    }

    void ClientAdapter::OnResourceResponse(CefRefPtr<CefBrowser> browser, const CefString& url, CefRefPtr<CefResponse> response, CefRefPtr<CefContentFilter>& filter)
    {
        IAfterResponse^ handler = _browserControl->AfterResponseHandler;
        if (handler != nullptr)
        {
            String^ cookie = toClr(response->GetHeader("Set-Cookie"));
            if (!String::IsNullOrEmpty(cookie))
            {
                handler->HandleSetCookie(cookie);
            }
        }
    }

    void ClientAdapter::OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context)
    {
        for each(KeyValuePair<String^, Object^>^ kvp in CEF::GetBoundObjects())
        {
            BindingHandler::Bind(kvp->Key, kvp->Value, context->GetGlobal());
        }
    }

    bool ClientAdapter::OnBeforeMenu(CefRefPtr<CefBrowser> browser, const CefMenuInfo& menuInfo)
    {
        IBeforeMenu^ beforeMenuHandler = _browserControl->BeforeMenuHandler;
        return beforeMenuHandler != nullptr &&
            beforeMenuHandler->HandleBeforeMenu();
    }

    void ClientAdapter::OnTakeFocus(CefRefPtr<CefBrowser> browser, bool next)
    {
        _browserControl->OnTakeFocus(next);
    }
}