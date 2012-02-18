#include "stdafx.h"
#pragma once

#include "ClientAdapter.h"
#include "RtzCountdownEvent.h"

using namespace System::ComponentModel;

namespace CefSharp
{
    interface class IBeforePopup;
    interface class IBeforeResourceLoad;
    interface class IBeforeMenu;
    interface class IAfterResponse;

                                   // XXX: needs to become IWebBrowser
    public ref class BrowserCore// : INotifyPropertyChanged
    {
    private:
        MCefRefPtr<ClientAdapter> _clientAdapter;
        ManualResetEvent^ _browserInitialized;
        RtzCountdownEvent^ _loadCompleted;

        bool _isLoading;
        bool _canGoBack;
        bool _canGoForward;

        String^ _tooltip;
        String^ _address;
        String^ _title;

        IBeforePopup^ _beforePopupHandler;
        IBeforeResourceLoad^ _beforeResourceLoadHandler;
        IBeforeMenu^ _beforeMenuHandler;
        IAfterResponse^ _afterResponseHandler;

    internal:
        property CefRefPtr<ClientAdapter> Adapter
        {
            CefRefPtr<ClientAdapter> get() { return _clientAdapter.get(); }
            void set(CefRefPtr<ClientAdapter> clientAdapter) { _clientAdapter = clientAdapter; }
        }

    public:
        BrowserCore()
        {
            _browserInitialized = gcnew ManualResetEvent(false);
            _loadCompleted = gcnew RtzCountdownEvent();
        }

        property bool IsInitialized
        {
            bool get()
            {
                return
                    _clientAdapter.get() != nullptr &&
                    _clientAdapter->GetIsInitialized();
            }
        }

        property bool IsLoading
        {
            bool get() { return _isLoading; }
        }

        property bool CanGoBack
        {
            bool get() { return _canGoBack; }
        }

        property bool CanGoForward
        {
            bool get() { return _canGoForward; }
        }

        property String^ Address
        {
            String^ get() { return _address; }

            void set(String^ address)
            {
                _address = address;
                //PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Address"));

            }
        }

        property String^ Title
        {
            String^ get() { return _title; }

            void set(String^ title)
            {
                _title = title;
                //PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Title"));
            }
        }

        property String^ Tooltip
        {
            String^ get() { return _tooltip; }

            void set(String^ tooltip)
            {
                _tooltip = tooltip;
               //PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Tooltip"));
            }
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

        void WaitForInitialized();
        void OnInitialized();

        void Load(String^ url);
        void Stop();
        void Back();
        void Forward();
        void Reload();
        void Reload(bool ignoreCache);
        void Print();

        void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);

        void AddFrame(CefRefPtr<CefFrame> frame);
        void FrameLoadComplete(CefRefPtr<CefFrame> frame);


    };
}
