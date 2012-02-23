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

    void WebBrowser::ExecuteScript(String^ script)
    {
        WaitForInitialized();

        CefRefPtr<CefBrowser> browser = _clientAdapter->GetCefBrowser();
        CefRefPtr<CefFrame> frame = browser->GetMainFrame();

        _scriptCore->Execute(frame, script);
    }

    String^ WebBrowser::EvaluateScript(String^ script)
    {
        return EvaluateScript(script, TimeSpan.MaxValue);
    }

    String^ WebBrowser::EvaluateScript(String^ script, TimeSpan timeout)
    {
	    WaitForInitialized();

        CefRefPtr<CefBrowser> browser = _clientAdapter->GetCefBrowser();
        CefRefPtr<CefFrame> frame = browser->GetMainFrame();

        return _scriptCore->Evaluate(frame, script, timeout);
    }

    void WebBrowser::SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
    {
        _browserCore->SetNavState(isLoading, canGoBack, canGoForward);
    }

    void WebBrowser::RaiseConsoleMessage(String^ message, String^ source, int line)
    {
        ConsoleMessage(this, gcnew ConsoleMessageEventArgs(message, source, line));
    }

    void WebBrowser::OnFrameLoadStart()
    {
        _browserCore->OnFrameLoadStart();
    }

    void WebBrowser::OnFrameLoadEnd()
    {
        _browserCore->OnFrameLoadEnd();
    }
}