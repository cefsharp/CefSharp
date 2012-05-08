#include "stdafx.h"
#pragma once

#include "ClientAdapter.h"
#include "RtzCountdownEvent.h"

using namespace System::ComponentModel;

namespace CefSharp
{
    interface class IBeforePopup;
    interface class IBeforeBrowse;
    interface class IBeforeResourceLoad;
    interface class IBeforeMenu;
    interface class IAfterResponse;
    interface class IAfterLoadError;

    public ref class BrowserCore : INotifyPropertyChanged
    {
    private:
        RtzCountdownEvent^ _loadCompleted;

        bool _isBrowserInitialized;
        bool _isLoading;
        bool _canGoBack;
        bool _canGoForward;

        int _contentsWidth;
        int _contentsHeight;

        String^ _tooltip;
        String^ _address;
        String^ _title;

        IBeforePopup^ _beforePopupHandler;
        IBeforeBrowse^ _beforeBrowseHandler;
        IBeforeResourceLoad^ _beforeResourceLoadHandler;
        IBeforeMenu^ _beforeMenuHandler;
        IAfterResponse^ _afterResponseHandler;
        IAfterLoadError^ _afterLoadErrorHandler;

    public:
        virtual event PropertyChangedEventHandler^ PropertyChanged;

        BrowserCore(String^ address)
        {
            _loadCompleted = gcnew RtzCountdownEvent();
            _address = address;
        }

        property bool IsBrowserInitialized
        {
            bool get() { return _isBrowserInitialized; }
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

        property int ContentsWidth
        {
            int get() { return _contentsWidth; }

            void set(int contentsWidth)
            {
                if (_contentsWidth != contentsWidth)
                {
                    _contentsWidth = contentsWidth;
                    PropertyChanged(this, gcnew PropertyChangedEventArgs(L"ContentsWidth"));
                }
            }
        }

        property int ContentsHeight
        {
            int get() { return _contentsHeight; }

            void set(int contentsHeight)
            {
                if (_contentsHeight != contentsHeight)
                {
                    _contentsHeight = contentsHeight;
                    PropertyChanged(this, gcnew PropertyChangedEventArgs(L"ContentsHeight"));
                }
            }
        }

        property String^ Address
        {
            String^ get() { return _address; }

            void set(String^ address)
            {
                if (_address != address)
                {
                    _address = address;
                    PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Address"));

                    TooltipText = nullptr;
                }
            }
        }

        property String^ Title
        {
            String^ get() { return _title; }

            void set(String^ title)
            {
                if (_title != title)
                {
                    _title = title;
                    PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Title"));
                }
            }
        }

        property String^ TooltipText
        {
            String^ get() { return _tooltip; }

            void set(String^ text)
            {
                if (_tooltip != text)
                {
                    _tooltip = text;
                   PropertyChanged(this, gcnew PropertyChangedEventArgs(L"TooltipText"));
                }
            }
        }

        virtual property IBeforePopup^ BeforePopupHandler
        {
            IBeforePopup^ get() { return _beforePopupHandler; }
            void set(IBeforePopup^ handler) { _beforePopupHandler = handler; }
        }

        virtual property IBeforeBrowse^ BeforeBrowseHandler
        {
            IBeforeBrowse^ get() { return _beforeBrowseHandler; }
            void set(IBeforeBrowse^ handler) { _beforeBrowseHandler = handler; }
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

        virtual property IAfterLoadError^ AfterLoadErrorHandler
        {
            IAfterLoadError^ get() { return _afterLoadErrorHandler; }
            void set(IAfterLoadError^ handler) { _afterLoadErrorHandler = handler; }
        }

        void CheckBrowserInitialization();

        void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);

        void OnInitialized();
        void OnLoad();
        void OnFrameLoadStart();
        void OnFrameLoadEnd();
    };
}
