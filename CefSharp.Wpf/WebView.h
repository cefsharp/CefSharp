#pragma once

#include "RenderClientAdapter.h"
#include "ScriptCore.h"
    
using namespace Microsoft::Win32::SafeHandles;
using namespace System;
using namespace System::ComponentModel;
using namespace System::Runtime::InteropServices;
using namespace System::Windows;
using namespace System::Windows::Controls;
using namespace System::Windows::Controls::Primitives;
using namespace System::Windows::Input;
using namespace System::Windows::Interop;
using namespace System::Windows::Media;
using namespace System::Windows::Media::Imaging;
using namespace System::Windows::Threading;
using namespace System::Threading;

namespace CefSharp
{
namespace Wpf
{
    [TemplatePart(Name="PART_Browser", Type=System::Windows::Controls::Image::typeid)]
    public ref class WebView sealed : public ContentControl, IRenderWebBrowser
    {
		delegate void ActionDelegate();

        ManualResetEvent^ _initialized;

        MCefRefPtr<RenderClientAdapter> _clientAdapter;
        BrowserCore^ _browserCore;
        MCefRefPtr<ScriptCore> _scriptCore;

        Image^ _image;
        System::Windows::Controls::ToolTip^ _toolTip;

        int _width, _height;
        InteropBitmap^ _ibitmap;
		HANDLE _fileMappingHandle, _backBufferHandle;
		ActionDelegate^ _paintDelegate;

    private:
        void WaitForInitialized();
        void BrowserCore_PropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
        void SetCursor(SafeFileHandle^ handle);
        void SetTooltip(String^ text);
        IntPtr SourceHook(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, bool% handled);
		void SetBitmap();

    protected:
        virtual Size ArrangeOverride(Size size) override;
        virtual void OnGotFocus(RoutedEventArgs^ e) override;
        virtual void OnLostFocus(RoutedEventArgs^ e) override;
        virtual void OnMouseMove(System::Windows::Input::MouseEventArgs^ e) override;
        virtual void OnMouseWheel(MouseWheelEventArgs^ e) override;
        virtual void OnMouseDown(MouseButtonEventArgs^ e) override;
        virtual void OnMouseUp(MouseButtonEventArgs^ e) override;
        virtual void OnMouseLeave(System::Windows::Input::MouseEventArgs^ e) override;

    public:
        virtual event PropertyChangedEventHandler^ PropertyChanged
        {
            void add(PropertyChangedEventHandler^ handler)
            {
                _browserCore->PropertyChanged += handler;
            }

            void remove(PropertyChangedEventHandler^ handler)
            {
                _browserCore->PropertyChanged -= handler;
            }
        }

        virtual event ConsoleMessageEventHandler^ ConsoleMessage;

        WebView(HwndSource^ source, String^ address)
        {
            Focusable = true;
            FocusVisualStyle = nullptr;

            if (!CEF::IsInitialized)
            {
                throw gcnew InvalidOperationException("CEF is not initialized");
            }

            _initialized  = gcnew ManualResetEvent(false);

            _browserCore = gcnew BrowserCore();
            _browserCore->Address = address;
            _browserCore->PropertyChanged +=
                gcnew PropertyChangedEventHandler(this, &WebView::BrowserCore_PropertyChanged);

            _scriptCore = new ScriptCore();

			_paintDelegate = gcnew ActionDelegate(this, &WebView::SetBitmap);
            source->AddHook(gcnew Interop::HwndSourceHook(this, &WebView::SourceHook));

            ToolTip = _toolTip =
                gcnew System::Windows::Controls::ToolTip();
            _toolTip->StaysOpen = true;

            HWND hWnd = static_cast<HWND>(source->Handle.ToPointer());
            CefWindowInfo window;
            window.SetAsOffScreen(hWnd);

            _clientAdapter = new RenderClientAdapter(this);
            CefRefPtr<RenderClientAdapter> ptr = _clientAdapter.get();

            CefBrowserSettings settings;
            CefBrowser::CreateBrowser(window, static_cast<CefRefPtr<CefClient>>(ptr),
                toNative(address), settings);
        }

        virtual property bool IsLoading
        {
            bool get() { return _browserCore->IsLoading; }
        }

        virtual property bool CanGoForward
        { 
            bool get() { return _browserCore->CanGoForward; } 
        }

        virtual property bool CanGoBack
        { 
            bool get() { return _browserCore->CanGoBack; } 
        }

        virtual property String^ Address
        {
            String^ get() { return _browserCore->Address; }
            void set(String^ address) { _browserCore->Address = address; }
        }

        virtual property String^ Title
        {
            String^ get() { return _browserCore->Title; }
            void set(String^ title) { _browserCore->Title = title; }
        }

        virtual property String^ Tooltip
        {
            String^ get() { return _browserCore->Tooltip; }
            void set(String^ tooltip) { _browserCore->Tooltip = tooltip; }
        }

        virtual property IBeforePopup^ BeforePopupHandler
        {
            IBeforePopup^ get() { return _browserCore->BeforePopupHandler; }
            void set(IBeforePopup^ handler) { _browserCore->BeforePopupHandler = handler; }
        }

        virtual property IBeforeResourceLoad^ BeforeResourceLoadHandler
        {
            IBeforeResourceLoad^ get() { return _browserCore->BeforeResourceLoadHandler; }
            void set(IBeforeResourceLoad^ handler) { _browserCore->BeforeResourceLoadHandler = handler; }
        }

        virtual property IBeforeMenu^ BeforeMenuHandler
        {
            IBeforeMenu^ get() { return _browserCore->BeforeMenuHandler; }
            void set(IBeforeMenu^ handler) { _browserCore->BeforeMenuHandler = handler; }
        }

        virtual property IAfterResponse^ AfterResponseHandler
        {
            IAfterResponse^ get() { return _browserCore->AfterResponseHandler; }
            void set(IAfterResponse^ handler) { _browserCore->AfterResponseHandler = handler; }
        }

        virtual void OnInitialized();

        virtual void Load(String^ url);
        virtual void Stop();
        virtual void Back();
        virtual void Forward();
        virtual void Reload();
        virtual void Reload(bool ignoreCache);
        virtual void Print();

        void ExecuteScript(String^ script);
        Object^ EvaluateScript(String^ script);
        Object^ EvaluateScript(String^ script, TimeSpan timeout);

        virtual void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);

        virtual void OnFrameLoadStart();
        virtual void OnFrameLoadEnd();

        virtual void RaiseConsoleMessage(String^ message, String^ source, int line);

        virtual void OnApplyTemplate() override;
        virtual void SetCursor(CefCursorHandle cursor);
        virtual void SetBuffer(int width, int height, const void* buffer);
    };
}}