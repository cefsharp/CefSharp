// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "RtzCountdownEvent.h"

using namespace System::ComponentModel;
using namespace System::Collections::Generic;

namespace CefSharp
{
    interface class ILifeSpanHandler;
    interface class IRequestHandler;
    interface class IMenuHandler;
    interface class IKeyboardHandler;
    interface class IJsDialogHandler;

    public ref class BrowserCore : INotifyPropertyChanged
    {
    private:
        RtzCountdownEvent^ _loadCompleted;

        bool _isBrowserInitialized;
        bool _canGoBack;
        bool _canGoForward;

        int _contentsWidth;
        int _contentsHeight;

        String^ _tooltip;
        String^ _address;
        String^ _title;

        ILifeSpanHandler^ _lifeSpanHandler;
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
                if (_address != nullptr &&
                    _address->Equals(address))
                {
                    // The URI:s are identical, so nothing needs to be done.
                    return;
                }

                _address = address;
                PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Address"));

				
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

        virtual property ILifeSpanHandler^ LifeSpanHandler
        {
            ILifeSpanHandler^ get() { return _lifeSpanHandler; }
            void set(ILifeSpanHandler^ handler) { _lifeSpanHandler = handler; }
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

        void SetNavState(bool canGoBack, bool canGoForward)
        {
            if (canGoBack != _canGoBack) 
            {
                _canGoBack = canGoBack;
                PropertyChanged(this, gcnew PropertyChangedEventArgs(L"CanGoBack"));
            }

            if (canGoForward != _canGoForward)
            {
                _canGoForward = canGoForward;
                PropertyChanged(this, gcnew PropertyChangedEventArgs(L"CanGoForward"));
            }
        }

        void OnInitialized();
        void OnLoad();
        void OnFrameLoadStart();
        void OnFrameLoadEnd();
    };
}
