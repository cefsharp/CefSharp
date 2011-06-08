#include "stdafx.h"
#pragma once

#include "CefSharp.h"
#include "WpfClientAdapter.h"
#include "ICefWebBrowser.h"
#include "ConsoleMessageEventArgs.h"
#include "RtzCountdownEvent.h"

using namespace Microsoft::Win32::SafeHandles;
using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Windows;
using namespace System::Windows::Controls;
using namespace System::Windows::Input;
using namespace System::Windows::Interop;
using namespace System::Windows::Media;
using namespace System::Windows::Media::Imaging;
using namespace System::Windows::Threading;
using namespace System::Threading;

namespace CefSharp
{
    public ref class CefWpfWebBrowser sealed : public Image, ICefWebBrowser
    {
        bool _canGoForward;
        bool _canGoBack;
        bool _isLoading;

        String^ _address;
        String^ _title;
        String^ _jsResult;
        bool _jsError;

        IBeforeCreated^ _beforeCreatedHandler;
        IBeforeResourceLoad^ _beforeResourceLoadHandler;
        MCefRefPtr<WpfClientAdapter> _clientAdapter;

        AutoResetEvent^ _runJsFinished;
        RtzCountdownEvent^ _loadCompleted;
        ManualResetEvent^ _browserInitialized;

        Matrix _transform;
        int _width, _height;
        void *_buffer;
        WriteableBitmap^ _bitmap;

    private:
        void SetCursor(SafeFileHandle^ handle);

    protected:
        virtual Size ArrangeOverride(Size size) override;
        virtual void OnGotFocus(RoutedEventArgs^ e) override;
        virtual void OnLostFocus(RoutedEventArgs^ e) override;
        virtual void OnKeyDown(System::Windows::Input::KeyEventArgs^ e) override;
        virtual void OnKeyUp(System::Windows::Input::KeyEventArgs^ e) override;
        virtual void OnMouseMove(System::Windows::Input::MouseEventArgs^ e) override;
        virtual void OnMouseLeave(System::Windows::Input::MouseEventArgs^ e) override;
        virtual void OnMouseWheel(MouseWheelEventArgs^ e) override;
        virtual void OnMouseDown(MouseButtonEventArgs^ e) override;
        virtual void OnMouseUp(MouseButtonEventArgs^ e) override;

    public:
        CefWpfWebBrowser(HwndSource^ source, String^ address)
        {
            Focusable = true;

            if (!CEF::IsInitialized)
            {
                throw gcnew InvalidOperationException("CEF is not initialized");
            }

            _address = address;
            _runJsFinished = gcnew AutoResetEvent(false);
            _browserInitialized = gcnew ManualResetEvent(false);
            _loadCompleted = gcnew RtzCountdownEvent();
            _transform = source->CompositionTarget->TransformToDevice;

            HWND hWnd = static_cast<HWND>(source->Handle.ToPointer());
            CefWindowInfo window;
            window.SetAsOffScreen(hWnd);

            _clientAdapter = new WpfClientAdapter(this);
            CefRefPtr<WpfClientAdapter> ptr = _clientAdapter.get();

            CefBrowserSettings settings;
            CefBrowser::CreateBrowser(window, static_cast<CefRefPtr<CefClient>>(ptr), toNative(address), settings);
        }

        virtual void OnInitialized();

        virtual void SetTitle(String^ title);
        virtual void SetAddress(String^ address);
        virtual void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);
        
        virtual void AddFrame(CefRefPtr<CefFrame> frame);
        virtual void FrameLoadComplete(CefRefPtr<CefFrame> frame);

        virtual void SetJsResult(String^ result);
        virtual void SetJsError();
        virtual void RaiseConsoleMessage(String^ message, String^ source, int line);

        virtual property IBeforeCreated^ BeforeCreatedHandler
        {
          IBeforeCreated^ get() { return _beforeCreatedHandler; }
          void set(IBeforeCreated^ handler) { _beforeCreatedHandler = handler; }
        }

        virtual property IBeforeResourceLoad^ BeforeResourceLoadHandler
        {
            IBeforeResourceLoad^ get() { return _beforeResourceLoadHandler; }
            void set(IBeforeResourceLoad^ handler) { _beforeResourceLoadHandler = handler; }
        }

        virtual event PropertyChangedEventHandler^ PropertyChanged;

        // TODO: Initialized event can be subscribed by user code after actual Initialized event happens,
        // so we must handle this situation and in on add event handler raise event.
        // event EventHandler^ Initialized;

        event ConsoleMessageEventHandler^ ConsoleMessage;

        void SetCursor(CefCursorHandle cursor);
        void SetBuffer(int width, int height, const CefRect& dirtyRect, const void* buffer);
        void SetBitmap(WriteableBitmap^ bitmap);
    };
}

/*
#include "CefSharp.h"
#include "WpfClientAdapter.h"

using namespace Microsoft::Win32::SafeHandles;
using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Windows;
using namespace System::Windows::Controls;
using namespace System::Windows::Input;
using namespace System::Windows::Interop;
using namespace System::Windows::Media;
using namespace System::Windows::Media::Imaging;
using namespace System::Windows::Threading;
using namespace System::Threading;

namespace CefSharp
{
    public ref class CefWpfWebBrowser sealed : public Image
    {
        MCefRefPtr<WpfHandlerAdapter> _handlerAdapter;
        ManualResetEvent^ _browserInitialized;
        
        Matrix _transform;
        int _width, _height;
        void *_buffer;
        WriteableBitmap^ _bitmap;

    internal:
        virtual void OnInitialized();

    private:
        void SetCursor(SafeFileHandle^ handle);

    protected:
        virtual Size ArrangeOverride(Size size) override;
        virtual void OnGotFocus(RoutedEventArgs^ e) override;
        virtual void OnLostFocus(RoutedEventArgs^ e) override;
        virtual void OnMouseMove(MouseEventArgs^ e) override;
        virtual void OnMouseLeave(MouseEventArgs^ e) override;
        virtual void OnMouseWheel(MouseWheelEventArgs^ e) override;
        virtual void OnMouseDown(MouseButtonEventArgs^ e) override;
        virtual void OnMouseUp(MouseButtonEventArgs^ e) override;

    public:
        CefWpfWebBrowser(HwndSource^ source, String^ address)
        {
            Focusable = true;

            if (!CEF::IsInitialized)
            {
                throw gcnew InvalidOperationException("CEF is not initialized");
            }

            _transform = source->CompositionTarget->TransformToDevice;
            _browserInitialized = gcnew ManualResetEvent(false);

            CefString url = toNative(address);

            HWND hWnd = static_cast<HWND>(source->Handle.ToPointer());
            CefWindowInfo window;
            window.SetAsOffScreen(hWnd);

            _handlerAdapter = new WpfHandlerAdapter(this);
            CefRefPtr<WpfHandlerAdapter> ptr = _handlerAdapter.get();

            CefBrowser::CreateBrowser(window, false, static_cast<CefRefPtr<CefHandler>>(ptr), url);
        }

        void SetCursor(CefCursorHandle cursor);
        void SetBuffer(int width, int height, const CefRect& dirtyRect, const void* buffer);
        void SetBitmap(WriteableBitmap^ bitmap);
    };
}

/*
#include "stdafx.h"
#pragma once

#include "CefSharp.h"
#include "HandlerAdapter.h"
#include "IBeforeCreated.h"
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
        bool _jsError;

        IBeforeCreated^ _beforeCreatedHandler;
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

        
        void SetJsResult(String^ result);
        void SetJsError();
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

        property IBeforeCreated^ BeforeCreatedHandler
        {
          IBeforeCreated^ get() { return _beforeCreatedHandler; }
          void set(IBeforeCreated^ handler) { _beforeCreatedHandler = handler; }
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
*/