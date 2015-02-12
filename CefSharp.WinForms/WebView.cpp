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

    bool WebView::TryGetCefBrowser(CefRefPtr<CefBrowser>& browser)
    {
        if (_browserCore->IsBrowserInitialized)
        {
            browser = _clientAdapter->GetCefBrowser();
            return browser != nullptr;
        }
        else
        {
            return false;
        }
    }

    void WebView::OnHandleCreated(EventArgs^ e)
    {
        if (DesignMode == false) 
        {
            _clientAdapter = new ClientAdapter(this);

            CefWindowInfo window;
            CefString url = toNative(_browserCore->Address);

            HWND hWnd = static_cast<HWND>(Handle.ToPointer());
            RECT rect;
            GetClientRect(hWnd, &rect);
            window.SetAsChild(hWnd, rect);

            CefBrowser::CreateBrowser(window, _clientAdapter.get(),
                url, *(CefBrowserSettings*)_settings->_internalBrowserSettings);
        }
    }

    void WebView::OnSizeChanged(EventArgs^ e)
    {
        CefRefPtr<CefBrowser> browser;
        if (!DesignMode &&
            TryGetCefBrowser(browser))
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
        CefRefPtr<CefBrowser> browser;
        if (!DesignMode &&
            TryGetCefBrowser(browser))
        {
            browser->SetFocus(true);
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

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->GetMainFrame()->LoadURL(toNative(url));
        }
    }

    void WebView::LoadHtml(String^ html)
    {
        LoadHtml(html, "about:blank");
    }

    void WebView::LoadHtml(String^ html, String^ url)
    {
        _browserCore->CheckBrowserInitialization();
        _browserCore->OnLoad();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->LoadString(toNative(html), toNative(url));
    }

    void WebView::Stop()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->StopLoad();
        }
    }

    void WebView::Back()
    {
        _browserCore->CheckBrowserInitialization();


        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->GoBack();
        }
    }

    void WebView::Forward()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->GoForward();
        }
    }

    void WebView::Reload()
    {
        Reload(false);
    }

    void WebView::Reload(bool ignoreCache)
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (!TryGetCefBrowser(browser))
        {
            return;
        }

        if (ignoreCache)
        {
            browser->ReloadIgnoreCache();
        }
        else
        {
            browser->Reload();
        }
    }

    void WebView::ClearHistory()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->ClearHistory();
        }
    }

    void WebView::ShowDevTools()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->ShowDevTools();
        }
    }

    void WebView::CloseDevTools()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->CloseDevTools();
        }
    }

    void WebView::Undo()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->GetMainFrame()->Undo();
        }
    }

    void WebView::Redo()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->GetMainFrame()->Redo();
        }
    }

    void WebView::Cut()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->GetMainFrame()->Cut();
        }
    }

    void WebView::Copy()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->GetMainFrame()->Copy();
        }
    }

    void WebView::Paste()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->GetMainFrame()->Paste();
        }
    }

    void WebView::Delete()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->GetMainFrame()->Delete();
        }
    }

    void WebView::SelectAll()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->GetMainFrame()->SelectAll();
        }
    }

    void WebView::Print()
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            browser->GetMainFrame()->Print();
        }
    }

    void WebView::ExecuteScript(String^ script)
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            _scriptCore->Execute(browser, toNative(script));
        }
    }

    Object^ WebView::EvaluateScript(String^ script)
    {
        return EvaluateScript(script, TimeSpan::MaxValue);
    }

    Object^ WebView::EvaluateScript(String^ script, TimeSpan timeout)
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser;
        if (TryGetCefBrowser(browser))
        {
            return _scriptCore->Evaluate(browser, toNative(script),
                timeout.TotalMilliseconds);
        }
        else
        {
            return nullptr;
        }
    }

    void WebView::SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
    {
        _browserCore->SetNavState(isLoading, canGoBack, canGoForward);
    }

    void WebView::OnConsoleMessage(String^ message, String^ source, int line)
    {
        ConsoleMessage(this, gcnew ConsoleMessageEventArgs(message, source, line));
    }

    void WebView::OnFrameLoadStart(String^ url, bool isMainFrame)
    {
        _browserCore->OnFrameLoadStart();

        LoadStartedEventArgs^ args = gcnew LoadStartedEventArgs(url, isMainFrame);
        LoadStarted(this, args);
    }

    void WebView::OnFrameLoadEnd(String^ url, bool isMainFrame)
    {
        _browserCore->OnFrameLoadEnd();
        
        LoadCompletedEventArgs^ args = gcnew LoadCompletedEventArgs(url, isMainFrame);
        LoadCompleted(this, args);
    }

    void WebView::OnTakeFocus(bool next)
    {
        SelectNextControl(this, next, true, true, true);
    }

    void WebView::RegisterJsObject(String^ name, Object^ objectToBind)
    {
        _browserCore->RegisterJsObject(name, objectToBind);
    }

    IDictionary<String^, Object^>^ WebView::GetBoundObjects()
    {
        return _browserCore->GetBoundObjects();
    }
}}