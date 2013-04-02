#include "stdafx.h"
#pragma once

#include "ClientAdapter.h"
#include "RtzCountdownEvent.h"

using namespace System::ComponentModel;
using namespace System::Collections::Generic;

namespace CefSharp
{
    interface class ILifeSpanHandler;
    interface class ILoadHandler;
    interface class IRequestHandler;
    interface class IMenuHandler;
    interface class IKeyboardHandler;
    interface class IJsDialogHandler;

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

        ILifeSpanHandler^ _lifeSpanHandler;
        ILoadHandler^ _loadHandler;
        IRequestHandler^ _requestHandler;
        IMenuHandler^ _menuHandler;
        IKeyboardHandler^ _keyboardHandler;
        IJsDialogHandler^ _jsDialogHandler;

        IDictionary<String^, Object^>^ _boundObjects;

    public:
        virtual event PropertyChangedEventHandler^ PropertyChanged;

        BrowserCore(String^ address)
        {
            _loadCompleted = gcnew RtzCountdownEvent();
            _address = address;
            _boundObjects = gcnew Dictionary<String^, Object^>();
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

        virtual property ILifeSpanHandler^ LifeSpanHandler
        {
            ILifeSpanHandler^ get() { return _lifeSpanHandler; }
            void set(ILifeSpanHandler^ handler) { _lifeSpanHandler = handler; }
        }

        virtual property ILoadHandler^ LoadHandler
        {
            ILoadHandler^ get() { return _loadHandler; }
            void set(ILoadHandler^ handler) { _loadHandler = handler; }
        }

        virtual property IRequestHandler^ RequestHandler
        {
            IRequestHandler^ get() { return _requestHandler; }
            void set(IRequestHandler^ handler) { _requestHandler = handler; }
        }

        virtual property IMenuHandler^ MenuHandler
        {
            IMenuHandler^ get() { return _menuHandler; }
            void set(IMenuHandler^ handler) { _menuHandler = handler; }
        }

        virtual property IKeyboardHandler^ KeyboardHandler
        {
            IKeyboardHandler^ get() { return _keyboardHandler; }
            void set(IKeyboardHandler^ handler) { _keyboardHandler = handler; }
        }

        virtual property IJsDialogHandler^ JsDialogHandler
        {
            IJsDialogHandler^ get() { return _jsDialogHandler; }
            void set(IJsDialogHandler^ handler) { _jsDialogHandler = handler; }
        }

        void CheckBrowserInitialization();

        void RegisterJsObject(String^ name, Object^ objectToBind);
        IDictionary<String^, Object^>^ GetBoundObjects();

        void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);

        void OnInitialized();
        void OnLoad();
        void OnFrameLoadStart();
        void OnFrameLoadEnd();
    };
}
