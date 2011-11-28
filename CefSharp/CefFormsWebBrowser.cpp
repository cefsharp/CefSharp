#include "stdafx.h"

#include "CefFormsWebBrowser.h"
#include "ScriptException.h"

using namespace System::Drawing;

namespace CefSharp
{
    void CefFormsWebBrowser::Load(String^ url)
    {
        WaitForInitialized();

        _loadCompleted->Reset();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->LoadURL(toNative(url));
    }

    void CefFormsWebBrowser::Stop()
    {
    	WaitForInitialized();

        _clientAdapter->GetCefBrowser()->StopLoad();
    }

    void CefFormsWebBrowser::Back()
    {
    	WaitForInitialized();

        _clientAdapter->GetCefBrowser()->GoBack();
    }

    void CefFormsWebBrowser::Forward()
    {
    	WaitForInitialized();

        _clientAdapter->GetCefBrowser()->GoForward();
    }

    void CefFormsWebBrowser::Reload()
    {
        Reload(false);
    }

    void CefFormsWebBrowser::Reload(bool ignoreCache)
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

    void CefFormsWebBrowser::Print()
    {
        WaitForInitialized();

        _clientAdapter->GetCefBrowser()->GetMainFrame()->Print();
    }

    String^ CefFormsWebBrowser::RunScript(String^ script)
    {
	    WaitForInitialized();

        if (!CefCurrentlyOn(TID_UI))
        {
            return nullptr;
        }

        CefRefPtr<CefBrowser> browser = _clientAdapter->GetCefBrowser();
        CefRefPtr<CefFrame> frame = browser->GetMainFrame();
        CefRefPtr<CefV8Context> context = frame->GetV8Context();
        CefString url = frame->GetURL();

        String^ result;

        if (!context.get())
        {
            return result;
        }
        else if (!context->Enter())
        {
            return result;
        }
        else
        {
            CefRefPtr<CefV8Value> global = context->GetGlobal();
            CefRefPtr<CefV8Value> eval = global->GetValue("eval");
            CefRefPtr<CefV8Value> arg = CefV8Value::CreateString(toNative(script));

            CefV8ValueList args;
            args.push_back(arg);

            CefRefPtr<CefV8Value> value;
            CefRefPtr<CefV8Exception> exception;

            if (eval->ExecuteFunctionWithContext(context, global, args, value, exception, false) &&
                value.get())
            {
                result = toClr(value->GetStringValue());
            }

            context->Exit();
        }

        return result;
    }

    void CefFormsWebBrowser::OnInitialized()
    {
        BeginInvoke(gcnew Action<EventArgs^>(this, &CefFormsWebBrowser::OnSizeChanged), EventArgs::Empty);
        _browserInitialized->Set();
    }

    void CefFormsWebBrowser::OnHandleCreated(EventArgs^ e)
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

    void CefFormsWebBrowser::OnSizeChanged(EventArgs^ e)
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

    void CefFormsWebBrowser::OnGotFocus(EventArgs^ e)
    {
        if (IsInitialized && !DesignMode)
        {
            _clientAdapter->GetCefBrowser()->SetFocus(true);
        }
    }

    void CefFormsWebBrowser::SetTitle(String^ title)
    {
        _title = title;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Title"));
    }

    void CefFormsWebBrowser::SetToolTip(String^ text)
    {
        _tooltip = text;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"ToolTip"));
    }

    void CefFormsWebBrowser::SetAddress(String^ address)
    {
        _address = address;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Address"));
    }

    void CefFormsWebBrowser::SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
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

    void CefFormsWebBrowser::AddFrame(CefRefPtr<CefFrame> frame)
    {
        _loadCompleted->AddCount();
    }

    void CefFormsWebBrowser::FrameLoadComplete(CefRefPtr<CefFrame> frame)
    {
        _loadCompleted->Signal();
    }

    void CefFormsWebBrowser::WaitForLoadCompletion()
    {
        WaitForLoadCompletion(-1);
    }

    void CefFormsWebBrowser::WaitForLoadCompletion(int timeout)
    {
        _loadCompleted->Wait(timeout);
    }

    void CefFormsWebBrowser::SetJsResult(String^ result)
    {
        _jsResult = result;
        _runJsFinished->Set();
    }

    void CefFormsWebBrowser::SetJsError()
    {
        _jsError = true;
        _runJsFinished->Set();
    }

    void CefFormsWebBrowser::RaiseConsoleMessage(String^ message, String^ source, int line)
    {
        ConsoleMessage(this, gcnew ConsoleMessageEventArgs(message, source, line));
    }

    void CefFormsWebBrowser::WaitForInitialized()
    {
        if (IsInitialized) return;

        // TODO: risk of infinite lock
        _browserInitialized->WaitOne();
    }
}