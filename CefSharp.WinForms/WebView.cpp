#include "WebView.h"

using namespace System::Drawing;

namespace CefSharp
{
namespace WinForms
{
    void WebView::Initialize(String^ address, BrowserSettings^ settings)
    {
        if (!CEF::IsInitialized &&
            !CEF::Initialize(gcnew Settings))
        {
            throw gcnew InvalidOperationException("CEF::Initialize() failed");
        }

        TabStop = true;

        _settings = settings;
        _browserCore = gcnew BrowserCore(address);
        _scriptCore = new ScriptCore();
    }

    void WebView::OnHandleCreated(EventArgs^ e)
    {
        if (DesignMode == false) 
        {
            _clientAdapter = new ClientAdapter(this);

            CefWindowInfo window;
            CefRefPtr<ClientAdapter> ptr = _clientAdapter.get();
            CefString url = toNative(_browserCore->Address);

            HWND hWnd = static_cast<HWND>(Handle.ToPointer());
            RECT rect;
            GetClientRect(hWnd, &rect);
            window.SetAsChild(hWnd, rect);

            CefBrowser::CreateBrowser(window, static_cast<CefRefPtr<CefClient>>(ptr),
                url, *_settings->_browserSettings);
        }
    }

    void WebView::OnSizeChanged(EventArgs^ e)
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

    void WebView::OnGotFocus(EventArgs^ e)
    {
        if (IsInitialized && !DesignMode)
        {
            _clientAdapter->GetCefBrowser()->SetFocus(true);
        }
    }

    void WebView::OnInitialized()
    {
        BeginInvoke(gcnew Action<EventArgs^>(this, &WebView::OnSizeChanged), EventArgs::Empty);
        _browserCore->OnInitialized();
    }

    void WebView::Load(String^ url)
    {
        _browserCore->CheckBrowserInitialization();
        _browserCore->OnLoad();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->LoadURL(toNative(url));
    }

    void WebView::Stop()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->StopLoad();

    }

    void WebView::Back()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GoBack();
    }

    void WebView::Forward()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GoForward();
    }

    void WebView::Reload()
    {
        Reload(false);
    }

    void WebView::Reload(bool ignoreCache)
    {
        _browserCore->CheckBrowserInitialization();
        if (ignoreCache)
        {
            _clientAdapter->GetCefBrowser()->ReloadIgnoreCache();
        }
        else
        {
            _clientAdapter->GetCefBrowser()->Reload();
        }
    }

    void WebView::ClearHistory()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->ClearHistory();
    }

    void WebView::ShowDevTools()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->ShowDevTools();
    }

    void WebView::CloseDevTools()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->CloseDevTools();
    }

    void WebView::Undo()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->Undo();
    }

    void WebView::Redo()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->Redo();
    }

    void WebView::Cut()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->Cut();
    }

    void WebView::Copy()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->Copy();
    }

    void WebView::Paste()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->Paste();
    }

    void WebView::Delete()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->Delete();
    }

    void WebView::SelectAll()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->SelectAll();
    }

    void WebView::Print()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->Print();
    }

    void WebView::ExecuteScript(String^ script)
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser = _clientAdapter->GetCefBrowser();
        CefRefPtr<CefFrame> frame = browser->GetMainFrame();

        _scriptCore->Execute(frame, toNative(script));
    }

    Object^ WebView::EvaluateScript(String^ script)
    {
        return EvaluateScript(script, TimeSpan::MaxValue);
    }

    Object^ WebView::EvaluateScript(String^ script, TimeSpan timeout)
    {
	    _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser = _clientAdapter->GetCefBrowser();
        CefRefPtr<CefFrame> frame = browser->GetMainFrame();

        return _scriptCore->Evaluate(frame, toNative(script), timeout.TotalMilliseconds);
    }

    void WebView::SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
    {
        _browserCore->SetNavState(isLoading, canGoBack, canGoForward);
    }

    void WebView::OnConsoleMessage(String^ message, String^ source, int line)
    {
        ConsoleMessage(this, gcnew ConsoleMessageEventArgs(message, source, line));
    }

    void WebView::OnFrameLoadStart()
    {
        _browserCore->OnFrameLoadStart();
    }

    void WebView::OnFrameLoadEnd()
    {
        _browserCore->OnFrameLoadEnd();
    }

    void WebView::OnTakeFocus(bool next)
    {
        SelectNextControl(this, next, true, true, true);
    }
}}