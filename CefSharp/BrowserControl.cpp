#include "stdafx.h"

#include "BrowserControl.h"

namespace CefSharp
{
    void BrowserControl::Load(String^ url)
    {
        pin_ptr<const wchar_t> charPtr = PtrToStringChars(url);
        CefString urlStr = charPtr;
        _handlerAdapter->GetCefBrowser()->GetMainFrame()->LoadURL(urlStr);
    }

    void BrowserControl::Stop()
    {
        _handlerAdapter->GetCefBrowser()->StopLoad();
    }

    void BrowserControl::Back()
    {
        _handlerAdapter->GetCefBrowser()->GoBack();
    }

    void BrowserControl::Forward()
    {
        _handlerAdapter->GetCefBrowser()->GoForward();
    }

    String^ BrowserControl::RunScript(String^ script, String^ scriptUrl, int startLine)
    {
        return RunScript(script, scriptUrl, startLine, -1);
    }

    String^ BrowserControl::RunScript(String^ script, String^ scriptUrl, int startLine, int timeout)
    {
        _jsError = nullptr;
        _jsResult = nullptr;

        script = 
            "(function() {"
            "   try { "
            "      __js_run_done(" + script + ");"
            "   } catch(e) {"
            "      __js_run_err(e);"
            "   }"
            "})();";

        pin_ptr<const wchar_t> charPtr = PtrToStringChars(script);
        CefString scriptStr = charPtr;

        charPtr = PtrToStringChars(scriptUrl);
        CefString scriptUrlStr = charPtr;
        
        _handlerAdapter->GetCefBrowser()->GetMainFrame()->ExecuteJavaScript(scriptStr, scriptUrlStr, startLine);
        if(!_runJsFinished->WaitOne(timeout))
        {
            throw gcnew TimeoutException(L"Timed out waiting for JavaScript to return");
        }

        if(_jsError == nullptr) 
        {
            return _jsResult;
        }
        throw gcnew Exception("RunScript Exception:" + _jsError);
    }

    void BrowserControl::OnReady(EventArgs^ e)
    {
    	OnSizeChanged(this, EventArgs::Empty);

    	// todo: raise Ready event with control begininvoke
    	Ready(this, e);
    }

    void BrowserControl::OnHandleCreated(EventArgs^ e)
    {
        if (DesignMode == false) 
        {
            _handlerAdapter = new HandlerAdapter(this);
            CefRefPtr<HandlerAdapter> ptr = _handlerAdapter.get();

            pin_ptr<const wchar_t> charPtr = PtrToStringChars(_address);
            CefString urlStr = charPtr;

            CefWindowInfo windowInfo;

            HWND hWnd = static_cast<HWND>(Handle.ToPointer());
            RECT rect;
            GetClientRect(hWnd, &rect);
            windowInfo.SetAsChild(hWnd, rect);

            CefBrowser::CreateBrowser(windowInfo, false, static_cast<CefRefPtr<CefHandler>>(ptr), urlStr);
        }
    }

    void BrowserControl::OnSizeChanged(EventArgs^ e)
    {
        if (DesignMode == false && _handlerAdapter != nullptr && _handlerAdapter->GetIsReady())
        {
            HWND hWnd = static_cast<HWND>(Handle.ToPointer());
            RECT rect;
            GetClientRect(hWnd, &rect);
            HDWP hdwp = BeginDeferWindowPos(1);

            HWND browserHwnd = _handlerAdapter->GetBrowserHwnd();
            hdwp = DeferWindowPos(hdwp, browserHwnd, NULL, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, SWP_NOZORDER);
            EndDeferWindowPos(hdwp);
        }
    }

    void BrowserControl::SetTitle(String^ title)
    {
        _title = title;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Title"));
    }

    void BrowserControl::SetAddress(String^ address)
    {
        _address = address;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Address"));
    }

    void BrowserControl::SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
    {
        if(isLoading != _isLoading) 
        {
            _isLoading = isLoading;
            PropertyChanged(this, gcnew PropertyChangedEventArgs(L"IsLoading"));
        }

        if(canGoBack != _canGoBack) 
        {
            _canGoBack = canGoBack;
            PropertyChanged(this, gcnew PropertyChangedEventArgs(L"CanGoBack"));
        }

        if(canGoForward != _canGoForward)
        {
            _canGoForward = canGoForward;
            PropertyChanged(this, gcnew PropertyChangedEventArgs(L"CanGoForward"));
        }
    }

    void BrowserControl::SetJsResult(const CefString& result)
    {
        _jsResult = gcnew String(result.c_str());
        _runJsFinished->Set();
    }

    void BrowserControl::SetJsError(const CefString& error)
    {
        _jsError = gcnew String(error.c_str());
        _runJsFinished->Set();
    }

    void BrowserControl::RaiseConsoleMessage(String^ message, String^ source, int line)
    {
        ConsoleMessage(this, gcnew ConsoleMessageEventArgs(message, source, line));
    }
}