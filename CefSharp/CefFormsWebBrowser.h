#include "stdafx.h"
#pragma once

#include "CefSharp.h"
#include "ClientAdapter.h"
#include "ICefWebBrowser.h"
#include "ConsoleMessageEventArgs.h"
#include "RtzCountdownEvent.h"
#include "BrowserSettings.h"

using namespace System;
using namespace System::Windows::Forms;
using namespace System::Threading;

namespace CefSharp
{
    public ref class CefFormsWebBrowser sealed : public Control, ICefWebBrowser
    {
        bool _canGoForward;
        bool _canGoBack;
        bool _isLoading;

        String^ _address;
        String^ _title;
        String^ _tooltip;
        String^ _jsResult;
        bool _jsError;

        BrowserSettings^ _settings;

        IBeforePopup^ _beforePopupHandler;
        IBeforeResourceLoad^ _beforeResourceLoadHandler;
        IBeforeMenu^ _beforeMenuHandler;
        IAfterResponse^ _afterResponseHandler;
        MCefRefPtr<ClientAdapter> _clientAdapter;

        AutoResetEvent^ _runJsFinished;
        RtzCountdownEvent^ _loadCompleted;
        ManualResetEvent^ _browserInitialized;

    protected:
        virtual void OnHandleCreated(EventArgs^ e) override;
        virtual void OnSizeChanged(EventArgs^ e) override;
        virtual void OnGotFocus(EventArgs^ e) override;

    private:
        void Construct(String^ address, BrowserSettings^ settings)
        {
            _address = address;
            _settings = settings;
            _runJsFinished = gcnew AutoResetEvent(false);
            _browserInitialized = gcnew ManualResetEvent(false);
            _loadCompleted = gcnew RtzCountdownEvent();

            if(!CEF::IsInitialized)
            {
                if(!CEF::Initialize(gcnew Settings()))
                {
                    throw gcnew InvalidOperationException("CEF initialization failed.");
                }
            }
        }

    public:

        CefFormsWebBrowser()
        {
            Construct("about:blank", gcnew BrowserSettings);
        }

        CefFormsWebBrowser(String^ initialUrl, BrowserSettings^ settings)
        {
            Construct(initialUrl, settings);
        }

        virtual void OnInitialized();

        void Load(String^ url);
        void WaitForLoadCompletion();
        void WaitForLoadCompletion(int timeout);
        void Stop();
        void Back();
        void Forward();
        void Reload();
        void Reload(bool ignoreCache);
        void Print();
        String^ RunScript(String^ script, String^ scriptUrl, int startLine);
        String^ RunScript(String^ script, String^ scriptUrl, int startLine, int timeout);

        virtual void SetTitle(String^ title);
        virtual void SetToolTip(String^ text);
        virtual void SetAddress(String^ address);
        virtual void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);
        
        virtual void AddFrame(CefRefPtr<CefFrame> frame);
        virtual void FrameLoadComplete(CefRefPtr<CefFrame> frame);

        virtual void SetJsResult(String^ result);
        virtual void SetJsError();
        virtual void RaiseConsoleMessage(String^ message, String^ source, int line);

        property String^ Title
        {
            String^ get() { return _title; }
        }

        property String^ ToolTip
        {
            String^ get() { return _tooltip; }
        }

        property String^ Address
        {
            String^ get() { return _address; }
        }

        virtual property IBeforePopup^ BeforePopupHandler
        {
            IBeforePopup^ get() { return _beforePopupHandler; }
            void set(IBeforePopup^ handler) { _beforePopupHandler = handler; }
        }

        virtual property IBeforeResourceLoad^ BeforeResourceLoadHandler
        {
            IBeforeResourceLoad^ get() { return _beforeResourceLoadHandler; }
            void set(IBeforeResourceLoad^ handler) { _beforeResourceLoadHandler = handler; }
        }

        virtual property IBeforeMenu^ BeforeMenuHandler
        {
            IBeforeMenu^ get() { return _beforeMenuHandler; }
            void set(IBeforeMenu^ handler) { _beforeMenuHandler = handler; }
        }

        virtual property IAfterResponse^ AfterResponseHandler
        {
            IAfterResponse^ get() { return _afterResponseHandler; }
            void set(IAfterResponse^ handler) { _afterResponseHandler = handler; }
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
                return _clientAdapter.get() != nullptr && _clientAdapter->GetIsInitialized();
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
