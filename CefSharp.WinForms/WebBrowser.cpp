#include "stdafx.h"

#include "WebBrowser.h"
#include "ScriptException.h"

using namespace System::Drawing;

namespace CefSharp
{
    void WebBrowser::Load(String^ url)
    {
        WaitForInitialized();

        _loadCompleted->Reset();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->LoadURL(toNative(url));
    }

    void WebBrowser::Stop()
    {
    	WaitForInitialized();

        _clientAdapter->GetCefBrowser()->StopLoad();
    }

    void WebBrowser::Back()
    {
    	WaitForInitialized();

        _clientAdapter->GetCefBrowser()->GoBack();
    }

    void WebBrowser::Forward()
    {
    	WaitForInitialized();

        _clientAdapter->GetCefBrowser()->GoForward();
    }

    void WebBrowser::Reload()
    {
        Reload(false);
    }

    void WebBrowser::Reload(bool ignoreCache)
    {
    	WaitForInitialized();

        if(ignoreCache)
        {
            _clientAdapter->GetCefBrowser()->ReloadIgnoreCache();
        }
        else
        {
            _clientAdapter->GetCefBrowser()->Reload();
        }
    }

    void WebBrowser::Print()
    {
        WaitForInitialized();

        _clientAdapter->GetCefBrowser()->GetMainFrame()->Print();
    }

    String^ WebBrowser::RunScript(String^ script)
    {
	    WaitForInitialized();

        CefRefPtr<CefBrowser> browser = _clientAdapter->GetCefBrowser();
        CefRefPtr<CefFrame> frame = browser->GetMainFrame();

        return _scriptCore->Evaluate(frame, script);
    }

    void WebBrowser::OnInitialized()
    {
        BeginInvoke(gcnew Action<EventArgs^>(this, &WebBrowser::OnSizeChanged), EventArgs::Empty);
        _browserInitialized->Set();
    }

    void WebBrowser::OnHandleCreated(EventArgs^ e)
    {
        if (DesignMode == false) 
        {
            _clientAdapter = new ClientAdapter(this);
            CefRefPtr<ClientAdapter> ptr = _clientAdapter.get();

            CefString urlStr = toNative(_address);

            CefWindowInfo windowInfo;

            HWND hWnd = static_cast<HWND>(Handle.ToPointer());
            RECT rect;
            GetClientRect(hWnd, &rect);
            windowInfo.SetAsChild(hWnd, rect);


            CefBrowser::CreateBrowser(windowInfo, static_cast<CefRefPtr<CefClient>>(ptr), urlStr, *_settings->_browserSettings);
        }
    }

    void WebBrowser::OnSizeChanged(EventArgs^ e)
    {
        if (IsInitialized && !DesignMode)
        {
            HWND hWnd = static_cast<HWND>(Handle.ToPointer());
            RECT rect;
            GetClientRect(hWnd, &rect);
            HDWP hdwp = BeginDeferWindowPos(1);

            HWND browserHwnd = _clientAdapter->GetBrowserHwnd();
            hdwp = DeferWindowPos(hdwp, browserHwnd, NULL, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, SWP_NOZORDER);
            EndDeferWindowPos(hdwp);
        }
    }

    void WebBrowser::OnGotFocus(EventArgs^ e)
    {
        if (IsInitialized && !DesignMode)
        {
            _clientAdapter->GetCefBrowser()->SetFocus(true);
        }
    }

    void WebBrowser::SetTitle(String^ title)
    {
        _title = title;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Title"));
    }

    void WebBrowser::SetToolTip(String^ text)
    {
        _tooltip = text;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"ToolTip"));
    }

    void WebBrowser::SetAddress(String^ address)
    {
        _address = address;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Address"));
    }

    void WebBrowser::SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
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

    void WebBrowser::AddFrame(CefRefPtr<CefFrame> frame)
    {
        _loadCompleted->AddCount();
    }

    void WebBrowser::FrameLoadComplete(CefRefPtr<CefFrame> frame)
    {
        _loadCompleted->Signal();
    }

    void WebBrowser::WaitForLoadCompletion()
    {
        WaitForLoadCompletion(-1);
    }

    void WebBrowser::WaitForLoadCompletion(int timeout)
    {
        _loadCompleted->Wait(timeout);
    }

    void WebBrowser::SetJsResult(String^ result)
    {
        _jsResult = result;
        _runJsFinished->Set();
    }

    void WebBrowser::SetJsError()
    {
        _jsError = true;
        _runJsFinished->Set();
    }

    void WebBrowser::RaiseConsoleMessage(String^ message, String^ source, int line)
    {
        ConsoleMessage(this, gcnew ConsoleMessageEventArgs(message, source, line));
    }

    void WebBrowser::WaitForInitialized()
    {
        if (IsInitialized) return;

        // TODO: risk of infinite lock
        _browserInitialized->WaitOne();
    }
}