#include "stdafx.h"

#include "WpfHandlerAdapter.h"
#include "CefWpfWebBrowser.h"

namespace CefSharp
{
    CefHandler::RetVal WpfHandlerAdapter::HandleAfterCreated(CefRefPtr<CefBrowser> browser) 
    { 
        if(!browser->IsPopup()) 
        {
            _cefBrowser = browser;
            _browserControl->OnInitialized();
        }

        return RV_CONTINUE; 
    }

    CefHandler::RetVal WpfHandlerAdapter::HandleGetRect(CefRefPtr<CefBrowser> browser, bool screen, CefRect& rect)
    {
        rect.x = rect.y = 0;
        rect.width = rect.height = 256; // XXX

        return RV_CONTINUE;
    }

    CefHandler::RetVal WpfHandlerAdapter::HandleGetScreenPoint(CefRefPtr<CefBrowser> browser, int viewX, int viewY, int& screenX, int& screenY)
    {
        return RV_CONTINUE;
    }

        
    CefHandler::RetVal WpfHandlerAdapter::HandleCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor)
    {
        _browserControl->SetCursor(cursor);
        return RV_CONTINUE;
    }

    CefHandler::RetVal WpfHandlerAdapter::HandlePaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const CefRect& dirtyRect, const void* buffer)
    {
        Console::WriteLine("HandlePaint: {0},{1} {2}x{3}", dirtyRect.x, dirtyRect.y, dirtyRect.width, dirtyRect.height);
        _browserControl->SetBuffer(dirtyRect, buffer);
        return RV_CONTINUE;
    }
}

/*
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
    CefHandler::RetVal HandlerAdapter::HandleAfterCreated(CefRefPtr<CefBrowser> browser) 
    { 
        if(!browser->IsPopup()) 
        {
            _browserHwnd = browser->GetWindowHandle();
            _cefBrowser = browser;

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

    CefHandler::RetVal HandlerAdapter::HandleLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
    {
        if(!browser->IsPopup())
        {
            Lock();
            if (frame->IsMain())
            {
                _browserControl->SetNavState(true, false, false);
            }
            _browserControl->AddFrame(frame);
            Unlock();
        }
        return RV_CONTINUE;
    }

    CefHandler::RetVal HandlerAdapter::HandleLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int httpStatusCode)
    {
        if(!browser->IsPopup())
        {
            Lock();
            if (frame->IsMain())
            {
                _browserControl->SetNavState(false, browser->CanGoBack(), browser->CanGoForward());        
            }
            _browserControl->FrameLoadComplete(frame);
            Unlock();
        }
        
        return RV_CONTINUE;
    }

    CefHandler::RetVal HandlerAdapter::HandleBeforeCreated(CefRefPtr<CefBrowser> parentBrowser, CefWindowInfo& createInfo, bool popup, const CefPopupFeatures& popupFeatures, CefRefPtr<CefHandler>& handler, CefString& url, CefBrowserSettings& settings)
    {
        IBeforeCreated^ beforeCreatedHandler = _browserControl->BeforeCreatedHandler;
        if (beforeCreatedHandler == nullptr || beforeCreatedHandler->HandleBeforeCreated(popup, toClr(url)))
        {
          return RV_CONTINUE;
        }
        else
        {
          return RV_HANDLED;
        }
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
*/