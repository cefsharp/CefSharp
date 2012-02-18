#include "stdafx.h"

#include "WebBrowser.h"
#include "ScriptException.h"

using namespace System::Drawing;

namespace CefSharp
{
    void WebBrowser::OnHandleCreated(EventArgs^ e)
    {
        if (DesignMode == false) 
        {
            CefRefPtr<ClientAdapter> clientAdapter = new ClientAdapter(this);
            _browserCore->Adapter = clientAdapter;
            //CefRefPtr<ClientAdapter> ptr = clientAdapter.get();

            CefString urlStr = toNative(_browserCore->Address);

            CefWindowInfo windowInfo;

            HWND hWnd = static_cast<HWND>(Handle.ToPointer());
            RECT rect;
            GetClientRect(hWnd, &rect);
            windowInfo.SetAsChild(hWnd, rect);


            CefBrowser::CreateBrowser(windowInfo, static_cast<CefRefPtr<CefClient>>(clientAdapter), urlStr, *_settings->_browserSettings);
        }
    }

    void WebBrowser::OnSizeChanged(EventArgs^ e)
    {
        if (_browserCore->IsInitialized && !DesignMode)
        {
            HWND hWnd = static_cast<HWND>(Handle.ToPointer());
            RECT rect;
            GetClientRect(hWnd, &rect);
            HDWP hdwp = BeginDeferWindowPos(1);

            HWND browserHwnd = _browserCore->Adapter->GetBrowserHwnd();
            hdwp = DeferWindowPos(hdwp, browserHwnd, NULL, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, SWP_NOZORDER);
            EndDeferWindowPos(hdwp);
        }
    }

    void WebBrowser::OnGotFocus(EventArgs^ e)
    {
        if (_browserCore->IsInitialized && !DesignMode)
        {
            _browserCore->Adapter->GetCefBrowser()->SetFocus(true);
        }
    }

    void WebBrowser::WaitForInitialized()
    {
        _browserCore->WaitForInitialized();
    }

    void WebBrowser::OnInitialized()
    {
        BeginInvoke(gcnew Action<EventArgs^>(this, &WebBrowser::OnSizeChanged), EventArgs::Empty);
        _browserCore->OnInitialized();
    }

    void WebBrowser::Load(String^ url)
    {
        _browserCore->Load(url);
    }

    void WebBrowser::Stop()
    {
        _browserCore->Stop();
    }

    void WebBrowser::Back()
    {
        _browserCore->Back();
    }

    void WebBrowser::Forward()
    {
        _browserCore->Forward();
    }

    void WebBrowser::Reload()
    {
        _browserCore->Reload();
    }

    void WebBrowser::Reload(bool ignoreCache)
    {
        _browserCore->Reload(ignoreCache);
    }

    void WebBrowser::Print()
    {
        _browserCore->Print();
    }

    String^ WebBrowser::RunScript(String^ script)
    {
	    WaitForInitialized();

        CefRefPtr<CefBrowser> browser = _browserCore->Adapter->GetCefBrowser();
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