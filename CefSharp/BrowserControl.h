#include "stdafx.h"
#pragma once

#include "HandlerAdapter.h"
#include "CefSharp.h"

using namespace System;
using namespace System::ComponentModel;
using namespace System::Windows::Forms;
using namespace System::Threading;

namespace CefSharp 
{
    interface class IBeforeResourceLoad;

    public ref class BrowserControl sealed : public UserControl, INotifyPropertyChanged
    {
        bool _canGoForward;
        bool _canGoBack;
        bool _isLoading;

        String^ _address;
        String^ _title;
        IBeforeResourceLoad^ _beforeResourceLoadHandler;
        
        MCefRefPtr<HandlerAdapter> _handlerAdapter;
        AutoResetEvent^ _runJsFinished;
        String^ _jsResult;
        String^ _jsError;

    public:

        void Load(String^ url)
        {
            pin_ptr<const wchar_t> charPtr = PtrToStringChars(url);
            CefString urlStr = charPtr;
            _handlerAdapter->GetCefBrowser()->GetMainFrame()->LoadURL(urlStr);
        }

        void Stop()
        {
            _handlerAdapter->GetCefBrowser()->StopLoad();
        }

        void Back()
        {
            _handlerAdapter->GetCefBrowser()->GoBack();
        }

        void Forward()
        {
            _handlerAdapter->GetCefBrowser()->GoForward();
        }

        String^ RunScript(String^ script, String^ scriptUrl, int startLine)
        {
            return RunScript(script, scriptUrl, startLine, -1);
        }

        String^ RunScript(String^ script, String^ scriptUrl, int startLine, int timeout)
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

    protected:
        

        virtual void OnHandleCreated(EventArgs^ e) override
        {
            if(DesignMode == false) 
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

        virtual void OnSizeChanged(EventArgs^ e) override
        {
            if(DesignMode == false) 
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

    internal:
        void SetTitle(String^ title)
        {
            _title = title;
            PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Title"));
        }

        void SetAddress(String^ address)
        {
            _address = address;
            PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Address"));
        }

        void SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
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

        void SetJsResult(const CefString& result)
        {
            _jsResult = gcnew String(result.c_str());
            _runJsFinished->Set();
        }

        void SetJsError(const CefString& error)
        {
            _jsError = gcnew String(error.c_str());
            _runJsFinished->Set();
        }


    public:

        BrowserControl() : 
            _address(gcnew String("about:blank")), 
            _runJsFinished(gcnew AutoResetEvent(false)) {}


        BrowserControl(String^ initialUrl) : 
            _address(initialUrl), 
            _runJsFinished(gcnew AutoResetEvent(false)) {}

        property String^ Title
        {
            String^ get()
            {
                return _title;
            }
        }

        property String^ Address
        {
            String^ get()
            {
                return _address;
            }
        }

        property IBeforeResourceLoad^ BeforeResourceLoadHandler
        {
            IBeforeResourceLoad^ get()
            {
                return _beforeResourceLoadHandler;
            }

            void set(IBeforeResourceLoad^ handler)
            {
                _beforeResourceLoadHandler = handler;
            }
        }

        property bool CanGoForward
        { 
            bool get() { return _canGoForward; } 
        }

        property bool CanGoBack
        { 
            bool get() { return _canGoBack; } 
        }

        property bool IsLoading
        {
            bool get() { return _canGoBack; }
        }

        virtual event PropertyChangedEventHandler^ PropertyChanged;
    };

}