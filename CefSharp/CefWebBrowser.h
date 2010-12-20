#include "stdafx.h"
#pragma once

#include "CefSharp.h"
#include "HandlerAdapter.h"
#include "IBeforeResourceLoad.h"
#include "ConsoleMessageEventArgs.h"
#include "RtzCountdownEvent.h"

using namespace System;
using namespace System::ComponentModel;
using namespace System::Windows::Forms;
using namespace System::Threading;

namespace CefSharp
{
    public ref class CefWebBrowser sealed : public Control, INotifyPropertyChanged
    {
        bool _canGoForward;
        bool _canGoBack;
        bool _isLoading;

        String^ _address;
        String^ _title;
        String^ _jsResult;
        String^ _jsError;

        IBeforeResourceLoad^ _beforeResourceLoadHandler;       
        MCefRefPtr<HandlerAdapter> _handlerAdapter;

        AutoResetEvent^ _runJsFinished;
        RtzCountdownEvent^ _loadCompleted;
        ManualResetEvent^ _browserInitialized;

    protected:
        virtual void OnHandleCreated(EventArgs^ e) override;
        virtual void OnSizeChanged(EventArgs^ e) override;
        virtual void OnGotFocus(EventArgs^ e) override;

    internal:

        virtual void OnInitialized();
        
        void SetTitle(String^ title);
        void SetAddress(String^ address);
        void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);
        
        void AddFrame(CefRefPtr<CefFrame> frame);
        void FrameLoadComplete(CefRefPtr<CefFrame> frame);

        
        void SetJsResult(const CefString& result);
        void SetJsError(const CefString& error);
        void RaiseConsoleMessage(String^ message, String^ source, int line);

    private:
        void Construct(String^ address)
        {
            _address = address;
            _runJsFinished = gcnew AutoResetEvent(false);
            _browserInitialized = gcnew ManualResetEvent(false);
            _loadCompleted = gcnew RtzCountdownEvent();

            if(!CEF::IsInitialized)
            {
                if(!CEF::Initialize(gcnew Settings(), gcnew BrowserSettings()))
                {
                    throw gcnew InvalidOperationException("CEF initialization failed.");
                }
            }
        }

    public:

        CefWebBrowser()
        {
            Construct("about:blank");
        }

        CefWebBrowser(String^ initialUrl)
        {
            Construct(initialUrl);
        }

        void Load(String^ url);
        void WaitForLoadCompletion();
        void WaitForLoadCompletion(int timeout);
        void Stop();
        void Back();
        void Forward();
        void Reload();
        void Reload(bool ignoreCache);
        String^ RunScript(String^ script, String^ scriptUrl, int startLine);
        String^ RunScript(String^ script, String^ scriptUrl, int startLine, int timeout);

        property String^ Title
        {
            String^ get() { return _title; }
        }

        property String^ Address
        {
            String^ get() { return _address; }
        }

        property IBeforeResourceLoad^ BeforeResourceLoadHandler
        {
            IBeforeResourceLoad^ get() { return _beforeResourceLoadHandler; }
            void set(IBeforeResourceLoad^ handler) { _beforeResourceLoadHandler = handler; }
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
            bool get() { return _isLoading; }
        }

        property bool IsInitialized
        {
            bool get()
            {
                return _handlerAdapter.get() != nullptr && _handlerAdapter->GetIsInitialized();
            }
        }

        void WaitForInitialized();

        virtual event PropertyChangedEventHandler^ PropertyChanged;

        // TODO: Initialized event can be subscribed by user code after actual Initialized event happens,
        // so we must handle this situation and in on add event handler raise event.
        // event EventHandler^ Initialized;

        event ConsoleMessageEventHandler^ ConsoleMessage;
    };

}