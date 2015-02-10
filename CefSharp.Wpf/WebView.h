#pragma once

#include "RenderClientAdapter.h"
#include "ScriptCore.h"

using namespace Microsoft::Win32::SafeHandles;
using namespace System;
using namespace System::Collections::Generic;
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

namespace CefSharp
{
    namespace Wpf
    {
        public ref class WebView sealed : public ContentControl, IRenderWebBrowser
        {
        private:
            delegate void ActionHandler();
            delegate bool MoveFocusHandler(TraversalRequest^ request);

            BrowserSettings^ _settings;

            MCefRefPtr<RenderClientAdapter> _clientAdapter;
            BrowserCore^ _browserCore;
            MCefRefPtr<ScriptCore> _scriptCore;

            Object^ _sync;
            HwndSource^ _source;
            Matrix^ _matrix;
            HwndSourceHook^ _hook;
            ::ToolTip^ _toolTip;
            Popup^ _popup;
            DispatcherTimer^ _timer;

            Image^ _image;
            int _width, _height;
            InteropBitmap^ _ibitmap;
            HANDLE _fileMappingHandle, _backBufferHandle;
            ActionHandler^ _paintDelegate;

            Image^ _popupImage;
            int _popupWidth, _popupHeight, _popupX, _popupY;
            int _popupImageWidth, _popupImageHeight;
            Transform^ _popupOffsetTransform;
            InteropBitmap^ _popupIbitmap;
            HANDLE _popupFileMappingHandle, _popupBackBufferHandle;
            ActionHandler^ _paintPopupDelegate;
            ActionHandler^ _resizePopupDelegate;

            Window^ _currentWindow;

            void Initialize(String^ address, BrowserSettings^ settings);
            bool TryGetCefBrowser(CefRefPtr<CefBrowser>& browser);
            void BrowserCore_PropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
            void Timer_Tick(Object^ sender, EventArgs^ e);
            void ToolTip_Closed(Object^ sender, RoutedEventArgs^ e);
            void SetCursor(SafeFileHandle^ handle);
            void SetTooltipText(String^ text);
            IntPtr SourceHook(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, bool% handled);
            void SetBitmap();

            void SetBuffer(int& currentWidth, int& currentHeight, int width, int height,
                HANDLE& fileMappingHandle, HANDLE& backBufferHandle,
                InteropBitmap^& ibitmap, ActionHandler^ paintDelegate,
                const void* buffer);

            void SetPopupBitmap();
            void OnPreviewKey(KeyEventArgs^ e);
            void OnMouseButton(MouseButtonEventArgs^ e);

            void ShowHidePopup(bool isOpened);
            void SetPopupSizeAndPositionImpl();

            void OnLoaded(Object^ sender, RoutedEventArgs^ e);
            void OnUnloaded(Object^ sender, RoutedEventArgs^ e);
            void OnGotKeyboardFocus(Object^ sender, KeyboardFocusChangedEventArgs^ e);
            void OnLostKeyboardFocus(Object^ sender, KeyboardFocusChangedEventArgs^ e);
            void OnPopupMouseMove(Object^ sender, MouseEventArgs^ e);
            void OnPopupMouseWheel(Object^ sender,MouseWheelEventArgs^ e);
            void OnPopupMouseDown(Object^ sender,MouseButtonEventArgs^ e);
            void OnPopupMouseUp(Object^ sender, MouseButtonEventArgs^ e);
            void OnPopupMouseLeave(Object^ sender, MouseEventArgs^ e);
            void OnHidePopup(Object^ sender, EventArgs^ e);

            void HidePopup();
            void AddSourceHook();
            bool IsNonStandardDpi();
            Transform^ GetScaleTransform();

        public protected: // a.k.a protected internal
            virtual void OnVisualParentChanged(DependencyObject^ oldParent) override;

        protected:
            virtual Size ArrangeOverride(Size size) override;
            virtual void OnPreviewKeyDown(KeyEventArgs^ e) override;
            virtual void OnPreviewKeyUp(KeyEventArgs^ e) override;
            virtual void OnPreviewTextInput(TextCompositionEventArgs^ e) override;

            virtual void OnMouseMove(MouseEventArgs^ e) override;
            virtual void OnMouseWheel(MouseWheelEventArgs^ e) override;
            virtual void OnMouseDown(MouseButtonEventArgs^ e) override;
            virtual void OnMouseUp(MouseButtonEventArgs^ e) override;
            virtual void OnMouseLeave(MouseEventArgs^ e) override;

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
            virtual event KeyEventHandler^ BrowserKey;
            virtual event LoadCompletedEventHandler^ LoadCompleted;
            virtual event LoadStartedEventHandler^ LoadStarted;

            WebView()
            {
                Initialize(String::Empty, gcnew BrowserSettings);
            }

            WebView(String^ address, BrowserSettings^ settings)
            {
                Initialize(address, settings);
            }

            ~WebView()
            {
                if (_source && _hook)
                {
                    _source->RemoveHook(_hook);
                }

                CefRefPtr<CefBrowser> browser;
                if (TryGetCefBrowser(browser))
                {
                    browser->CloseBrowser();
                }
            }

            virtual property bool IsBrowserInitialized
            {
                bool get() { return _browserCore->IsBrowserInitialized; }
            }

            virtual property bool IsLoading
            {
                bool get() { return _browserCore->IsLoading; }
            }

            virtual property bool CanGoBack
            { 
                bool get() { return _browserCore->CanGoBack; } 
            }

            virtual property bool CanGoForward
            { 
                bool get() { return _browserCore->CanGoForward; } 
            }

            virtual property int ContentsWidth
            {
                int get() { return _browserCore->ContentsWidth; }
                void set(int contentsWidth) { _browserCore->ContentsWidth = contentsWidth; }
            }

            virtual property int ContentsHeight
            {
                int get() { return _browserCore->ContentsHeight; }
                void set(int contentsHeight) { _browserCore->ContentsHeight = contentsHeight; }
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

            virtual property String^ TooltipText
            {
                String^ get() { return _browserCore->TooltipText; }
                void set(String^ text) { _browserCore->TooltipText = text; }
            }

            virtual property ILifeSpanHandler^ LifeSpanHandler
            {
                ILifeSpanHandler^ get() { return _browserCore->LifeSpanHandler; }
                void set(ILifeSpanHandler^ handler) { _browserCore->LifeSpanHandler = handler; }
            }

            virtual property ILoadHandler^ LoadHandler
            {
                ILoadHandler^ get() { return _browserCore->LoadHandler; }
                void set(ILoadHandler^ handler) { _browserCore->LoadHandler = handler; }
            }

            virtual property IRequestHandler^ RequestHandler
            {
                IRequestHandler^ get() { return _browserCore->RequestHandler; }
                void set(IRequestHandler^ handler) { _browserCore->RequestHandler = handler; }
            }

            virtual property IMenuHandler^ MenuHandler
            {
                IMenuHandler^ get() { return _browserCore->MenuHandler; }
                void set(IMenuHandler^ handler) { _browserCore->MenuHandler = handler; }
            }

            virtual property IKeyboardHandler^ KeyboardHandler
            {
                IKeyboardHandler^ get() { return _browserCore->KeyboardHandler; }
                void set(IKeyboardHandler^ handler) { _browserCore->KeyboardHandler = handler; }
            }

            virtual property IJsDialogHandler^ JsDialogHandler
            {
                IJsDialogHandler^ get() { return _browserCore->JsDialogHandler; }
                void set(IJsDialogHandler^ handler) { _browserCore->JsDialogHandler = handler; }
            }

            virtual property double ZoomLevel
            {
                double get()
                {
                    CefRefPtr<CefBrowser> browser;
                    if(!TryGetCefBrowser(browser))
                    {
                        return 0;
                    }
                    return browser->GetZoomLevel();
                }

                void set(double zoomLevel)
                {
                    CefRefPtr<CefBrowser> browser;
                    if(!TryGetCefBrowser(browser))
                    {
                        return;
                    }
                    browser->SetZoomLevel(zoomLevel);
                }
            }

            virtual void OnInitialized();

            virtual void Load(String^ url);
            virtual void LoadHtml(String^ html);
            virtual void LoadHtml(String^ html, String^ url);
            virtual void Stop();
            virtual void Back();
            virtual void Forward();
            virtual void Reload();
            virtual void Reload(bool ignoreCache);
            virtual void ClearHistory();
            virtual void ShowDevTools();
            virtual void CloseDevTools();

            virtual void Undo();
            virtual void Redo();
            virtual void Cut();
            virtual void Copy();
            virtual void Paste();
            virtual void Delete();
            virtual void SelectAll();
            virtual void Print();

            void ExecuteScript(String^ script);
            Object^ EvaluateScript(String^ script);
            Object^ EvaluateScript(String^ script, TimeSpan timeout);

            virtual void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);

            virtual void OnFrameLoadStart(String^ url, bool isMainFrame);
            virtual void OnFrameLoadEnd(String^ url, bool isMainFrame);
            virtual void OnTakeFocus(bool next);
            virtual void OnConsoleMessage(String^ message, String^ source, int line);

            virtual void RegisterJsObject(String^ name, Object^ objectToBind);
            virtual IDictionary<String^, Object^>^ GetBoundObjects();

            virtual void OnApplyTemplate() override;
            virtual void SetCursor(IntPtr cursor);
            virtual void SetBuffer(int width, int height, const void* buffer);
            virtual void SetPopupBuffer(int width, int height, const void* buffer);

            virtual void SetPopupIsOpen(bool isOpen);
            virtual void SetPopupSizeAndPosition(const void* rect);
        };
    }
}
