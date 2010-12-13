#include "stdafx.h"
#pragma once

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
        IBeforeResourceLoad^ _beforeResourceLoadHandler;       
        String^ _jsResult;
        String^ _jsError;

        MCefRefPtr<HandlerAdapter> _handlerAdapter;

        initonly AutoResetEvent^ _runJsFinished;
        initonly RtzCountdownEvent^ _loadCompleted;

    protected:
        
        virtual void OnHandleCreated(EventArgs^ e) override;
        virtual void OnSizeChanged(EventArgs^ e) override;

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

    public:

        BrowserControl() : 
            _address(gcnew String("about:blank")), 
            _runJsFinished(gcnew AutoResetEvent(false)),
            _loadCompleted(gcnew RtzCountdownEvent()) {}


        BrowserControl(String^ initialUrl) : 
            _address(initialUrl), 
            _runJsFinished(gcnew AutoResetEvent(false)),
            _loadCompleted(gcnew RtzCountdownEvent()) {}

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

        virtual event PropertyChangedEventHandler^ PropertyChanged;

        event ConsoleMessageEventHandler^ ConsoleMessage;
    };

}