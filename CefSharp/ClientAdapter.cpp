#include "stdafx.h"

#include "BindingHandler.h"
#include "ClientAdapter.h"
#include "CefSharp.h"
#include "IBeforeCreated.h"
#include "IBeforeResourceLoad.h"
#include "StreamAdapter.h"

namespace CefSharp 
{
    bool ClientAdapter::OnBeforePopup(CefRefPtr<CefBrowser> parentBrowser, const CefPopupFeatures& popupFeatures, CefWindowInfo& windowInfo, const CefString& url, CefRefPtr<CefClient>& client, CefBrowserSettings& settings)
    {
        IBeforeCreated^ beforeCreatedHandler = _browserControl->BeforeCreatedHandler;
        return beforeCreatedHandler == nullptr || beforeCreatedHandler->HandleBeforeCreated(true, toClr(url));
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

    void ClientAdapter::OnAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url)
    {
        if(frame->IsMain())
        {
            _browserControl->SetAddress(toClr(url));
        }
    }

    void ClientAdapter::OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title)
    {
        _browserControl->SetTitle(toClr(title));
    }

    bool ClientAdapter::OnConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line)
    {
        String^ messageStr = toClr(message);
        String^ sourceStr = toClr(source);
        _browserControl->RaiseConsoleMessage(messageStr, sourceStr, line);

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

        _browserControl->AddFrame(frame);
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

        _browserControl->FrameLoadComplete(frame);
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
                //mimeType = toNative(requestResponse->MimeType);
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

    void ClientAdapter::OnJSBinding(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Value> object)
    {
        for each(KeyValuePair<String^, Object^>^ kvp in CEF::GetBoundObjects())
        {
            BindingHandler::Bind(kvp->Key, kvp->Value, object);
        }
    }
}