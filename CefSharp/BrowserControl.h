#include "stdafx.h"
#pragma once

#include "CefSharp.h"
#include "HandlerAdapter.h"
#include "IBeforeResourceLoad.h"
#include "ConsoleMessageEventArgs.h"

using namespace System;
using namespace System::ComponentModel;
using namespace System::Windows::Forms;
using namespace System::Threading;

namespace CefSharp
{
    interface class IBeforeResourceLoad;

    public ref class BrowserControl sealed : public Control, INotifyPropertyChanged
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

        ManualResetEvent^ _browserReady;

    protected:
        virtual void OnHandleCreated(EventArgs^ e) override;
        virtual void OnSizeChanged(EventArgs^ e) override;

    public protected:
        virtual void OnReady(EventArgs^ e);

    internal:
        
        void SetTitle(String^ title);
        void SetAddress(String^ address);
        void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);
        void SetJsResult(const CefString& result);
        void SetJsError(const CefString& error);
        void RaiseConsoleMessage(String^ message, String^ source, int line);

    private:
        void Construct(String^ address)
        {
            _address = address;
            _runJsFinished = gcnew AutoResetEvent(false);
            _browserReady = gcnew ManualResetEvent(false);

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