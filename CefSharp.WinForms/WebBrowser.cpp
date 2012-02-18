#include "stdafx.h"

#include "WebBrowser.h"

using namespace System::Drawing;

namespace CefSharp
{
    void WebBrowser::WaitForInitialized()
    {
        if (IsInitialized)
        {
            return;
        }

        // TODO: risk of infinite lock
        _initialized->WaitOne();
    }

    void WebBrowser::OnHandleCreated(EventArgs^ e)
    {
        if (DesignMode == false) 
        {
            _clientAdapter = new ClientAdapter(this);
            CefRefPtr<ClientAdapter> ptr = _clientAdapter.get();

            CefString urlStr = toNative(_browserCore->Address);

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

    void WebBrowser::OnInitialized()
    {
        BeginInvoke(gcnew Action<EventArgs^>(this, &WebBrowser::OnSizeChanged), EventArgs::Empty);
        _initialized->Set();
    }

    void WebBrowser::Load(String^ url)
    {
        WaitForInitialized();
        _browserCore->OnLoad();
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
        if (ignoreCache)
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

    void WebBrowser::SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
    {
        _browserCore->SetNavState(isLoading, canGoBack, canGoForward);
    }

    void WebBrowser::RaiseConsoleMessage(String^ message, String^ source, int line)
    {
        ConsoleMessage(this, gcnew ConsoleMessageEventArgs(message, source, line));
    }

    void WebBrowser::AddFrame(CefRefPtr<CefFrame> frame)
    {
        _browserCore->AddFrame(frame);
    }

    void WebBrowser::FrameLoadComplete(CefRefPtr<CefFrame> frame)
    {
        _browserCore->FrameLoadComplete(frame);
    }



    /////////////////////

    /*


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






    void WebBrowser::SetTitle(String^ title)
    {
        _browserCore->Title = title;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Title"));
    }



    void WebBrowser::SetAddress(String^ address)
    {
        _browserCore->Address = address;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Address"));
    }





    void WebBrowser::WaitForLoadCompletion()
    {
        WaitForLoadCompletion(-1);
    }

    void WebBrowser::WaitForLoadCompletion(int timeout)
    {
        _loadCompleted->Wait(timeout);
    }




    */
}