#include "stdafx.h"

#include "HandlerAdapter.h"
#include "CefWebBrowser.h"
#include "Request.h"
#include "ReturnValue.h"
#include "StreamAdapter.h"
#include "CefSharp.h"
#include "BindingHandler.h"

namespace CefSharp
{
	using namespace System;

	HandlerAdapter::HandlerAdapter(CefWebBrowser^ browserControl) : _browserControl(browserControl)
	{
		_handler = new Handler();
		_handler->RegisterAfterCreated(gcnew HandleAfterCreatedDelegate(this, &HandlerAdapter::HandleAfterCreated));
		_handler->RegisterTitleChange(gcnew HandleTitleChangeDelegate(this, &HandlerAdapter::HandleTitleChange));
		_handler->RegisterAdressChange(gcnew HandleAddressChangeDelegate(this, &HandlerAdapter::HandleAddressChange));
		_handler->RegisterBeforeBrowse(gcnew HandleBeforeBrowseDelegate(this, &HandlerAdapter::HandleBeforeBrowse));
		_handler->RegisterLoadStart(gcnew HandleLoadStartDelegate(this, &HandlerAdapter::HandleLoadStart));
		_handler->RegisterLoadEnd(gcnew HandleLoadEndDelegate(this, &HandlerAdapter::HandleLoadEnd));
		_handler->RegisterBeforeResourceLoad(gcnew HandleBeforeResourceLoadDelegate(this, &HandlerAdapter::HandleBeforeResourceLoad));
		_handler->RegisterJSBinding(gcnew HandleJSBindingDelegate(this, &HandlerAdapter::HandleJSBinding));
		_handler->RegisterConsoleMessage(gcnew HandleConsoleMessageDelegate(this, &HandlerAdapter::HandleConsoleMessage));
	}

	CefHandler::RetVal HandlerAdapter::HandleAfterCreated(CefRefPtr<CefBrowser> browser)
    {
        if(!browser->IsPopup())
        {
            _browserControl->OnInitialized();
        }
        return RV_CONTINUE;
    }

    CefHandler::RetVal HandlerAdapter::HandleTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title) 
    { 
          _browserControl->SetTitle(toClr(title));
          return RV_CONTINUE; 
    }

    CefHandler::RetVal HandlerAdapter::HandleAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url)
    {
        if(frame->IsMain())
        {
            _browserControl->SetAddress(toClr(url));
        }
        return RV_CONTINUE;
    }

    CefHandler::RetVal HandlerAdapter::HandleBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefHandler::NavType navType, bool isRedirect)
    {
        return RV_CONTINUE;
    }

    CefHandler::RetVal HandlerAdapter::HandleLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
    {
        if(!browser->IsPopup())
        {
            _handler->Lock();
            if(!frame.get())
            {
                _browserControl->SetNavState(true, false, false);
            }
            _browserControl->AddFrame(frame);
            _handler->Unlock();
        }
        return RV_CONTINUE;
    }

    CefHandler::RetVal HandlerAdapter::HandleLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
    {
        if(!browser->IsPopup())
        {
            _handler->Lock();
            if(!frame.get())
            {
                _browserControl->SetNavState(false, browser->CanGoBack(), browser->CanGoForward());
            }
            _browserControl->FrameLoadComplete(frame);
            _handler->Unlock();
        }

        return RV_CONTINUE;
    }

    CefHandler::RetVal HandlerAdapter::HandleBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefRequest> request, CefString& redirectUrl, CefRefPtr<CefStreamReader>& resourceStream, CefString& mimeType, int loadFlags) 
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
                mimeType = toNative(requestResponse->MimeType);
                return RV_CONTINUE;
            }
            else if(requestResponse->Action == ResponseAction::Cancel)
            {
                return RV_HANDLED;
            }
            else if(requestResponse->Action == ResponseAction::Redirect)
            {
                redirectUrl = toNative(requestResponse->RedirectUrl);
            }
        }
        return RV_CONTINUE; 
    }

    CefHandler::RetVal HandlerAdapter::HandleJSBinding(CefRefPtr<CefBrowser> browser,
                                     CefRefPtr<CefFrame> frame,
                                     CefRefPtr<CefV8Value> object)
    {
        for each(KeyValuePair<String^, Object^>^ kvp in CEF::GetBoundObjects())
        {
            BindingHandler::Bind(kvp->Key, kvp->Value, object);
        }
        return RV_CONTINUE;
    }

    CefHandler::RetVal HandlerAdapter::HandleConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line)
    {
        String^ messageStr = toClr(message);
        String^ sourceStr = toClr(source);
        _browserControl->RaiseConsoleMessage(messageStr, sourceStr, line);
        return RV_CONTINUE;
    }
}