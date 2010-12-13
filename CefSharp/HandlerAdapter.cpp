#include "stdafx.h"

#include "HandlerAdapter.h"
#include "BrowserControl.h"
#include "Request.h"
#include "ReturnValue.h"
#include "StreamAdapter.h"
#include "JsResultHandler.h"

namespace CefSharp 
{
    CefHandler::RetVal HandlerAdapter::HandleAfterCreated(CefRefPtr<CefBrowser> browser) 
    { 
        if(!browser->IsPopup()) 
        {
            _browserHwnd = browser->GetWindowHandle();
            _cefBrowser = browser;

            _browserControl->OnReady();
        }
        return RV_CONTINUE; 
    }

    CefHandler::RetVal HandlerAdapter::HandleTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title) 
    { 
          _browserControl->SetTitle(gcnew String(title.c_str()));
          return RV_CONTINUE; 
    }

    CefHandler::RetVal HandlerAdapter::HandleAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url)
    {
        if(frame->IsMain())
        {
            _browserControl->SetAddress(gcnew String(url.c_str()));
        }
        return RV_CONTINUE; 
    }

    CefHandler::RetVal HandlerAdapter::HandleBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, NavType navType, bool isRedirect) 
    { 
        return RV_CONTINUE; 
    }

    CefHandler::RetVal HandlerAdapter::HandleLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
    {
        if(!browser->IsPopup())
        {
            Lock();
            if(!frame.get())
            {
                _browserControl->SetNavState(true, false, false);
            }
            _browserControl->AddFrame(frame);
            Unlock();
        }
        return RV_CONTINUE;
    }

    CefHandler::RetVal HandlerAdapter::HandleLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
    {
        if(!browser->IsPopup())
        {
            Lock();
            if(!frame.get())
            {
                _browserControl->SetNavState(false, browser->CanGoBack(), browser->CanGoForward());        
            }
            _browserControl->FrameLoadComplete(frame);
            Unlock();
        }
        
        return RV_CONTINUE;
    }

    CefHandler::RetVal HandlerAdapter::HandleBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefString& mimeType, int loadFlags) 
    {
        IBeforeResourceLoad^ handler = _browserControl->BeforeResourceLoadHandler;
        if(handler != nullptr)
        {
            String^ mRedirectUrl = nullptr;
            Stream^ mResourceStream = nullptr;
            String^ mMimeType = nullptr;
            CefRequestWrapper^ wrapper = gcnew CefRequestWrapper(request);
            
            handler->HandleBeforeResourceLoad(_browserControl, wrapper, mRedirectUrl, 
                mResourceStream, mMimeType, loadFlags);

            if(mResourceStream != nullptr)
            {
                CefRefPtr<StreamAdapter> adapter = new StreamAdapter(mResourceStream);
                resourceStream = CefStreamReader::CreateForHandler(static_cast<CefRefPtr<CefReadHandler>>(adapter));
                pin_ptr<const wchar_t> pStr = PtrToStringChars(mMimeType);
                CefString str(pStr);
                mimeType = str;
            }
        }
        return RV_CONTINUE; 
    }

    CefHandler::RetVal HandlerAdapter::HandleJSBinding(CefRefPtr<CefBrowser> browser,
                                     CefRefPtr<CefFrame> frame,
                                     CefRefPtr<CefV8Value> object)
    {
        JsResultHandler::Bind(_browserControl, object);

        return RV_CONTINUE;
    }

    CefHandler::RetVal HandlerAdapter::HandleConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line)
    {
        String^ messageStr = convertToString(message);
        String^ sourceStr = convertToString(source);
        _browserControl->RaiseConsoleMessage(messageStr, sourceStr, line);
        return RV_CONTINUE;
    }

}