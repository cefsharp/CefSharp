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
    public ref class BrowserControl sealed : public Control, INotifyPropertyChanged
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
        ManualResetEvent^ _browserReady;

    protected:
        virtual void OnHandleCreated(EventArgs^ e) override;
        virtual void OnSizeChanged(EventArgs^ e) override;

    public protected:
        virtual void OnReady();

    internal:
        
        void SetTitle(String^ title);
        void SetAddress(String^ address);
        void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);
        
        void ClearFrames();
        void AddFrame(CefRefPtr<CefFrame> frame);
        void FrameLoadComplete(CefRefPtr<CefFrame> frame);
        void BrowserLoadComplete();
        void BrowseStarted();
        
        void SetJsResult(const CefString& result);
        void SetJsError(const CefString& error);
        void RaiseConsoleMessage(String^ message, String^ source, int line);

    private:
        void Construct(String^ address)
        {
            _address = address;
            _runJsFinished = gcnew AutoResetEvent(false);
            _browserReady = gcnew ManualResetEvent(false);
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

        BrowserControl()
        {
            Construct("about:blank");
        }

        BrowserControl(String^ initialUrl)
        {
            Construct(initialUrl);
        }

        void Load(String^ url);
        void WaitForLoadCompletion();
        void WaitForLoadCompletion(int timeout);
        void Stop();
        void Back();
        void Forward();
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
            bool get() { return _canGoBack; }
        }

        property bool IsReady
        {
            bool get()
            {
                return _handlerAdapter.get() != nullptr && _handlerAdapter->GetIsReady();
            }
        }

        void WaitForReady();

        virtual event PropertyChangedEventHandler^ PropertyChanged;

        // TODO: Ready event can be subscribed by user code after actual Ready event happens,
        // so we must handle this situation and in on add event handler raise event.
        // event EventHandler^ Ready;

        event ConsoleMessageEventHandler^ ConsoleMessage;
    };

}