#include "Stdafx.h"

#include <msclr/lock.h>
#include "WebView.h"

namespace CefSharp
{
    namespace Wpf
    {
        void WebView::Initialize(String^ address, BrowserSettings^ settings)
        {
            if (!CEF::IsInitialized &&
                !CEF::Initialize(gcnew Settings))
            {
                throw gcnew InvalidOperationException("CEF::Initialize() failed");
            }

            Focusable = true;
            FocusVisualStyle = nullptr;
            IsTabStop = true;

            _settings = settings;
            _sync = gcnew Object();

            _browserCore = gcnew BrowserCore(address);
            _browserCore->PropertyChanged +=
                gcnew PropertyChangedEventHandler(this, &WebView::BrowserCore_PropertyChanged);

            _scriptCore = new ScriptCore();
            _paintDelegate = gcnew ActionHandler(this, &WebView::SetBitmap);
            _paintPopupDelegate = gcnew ActionHandler(this, &WebView::SetPopupBitmap);
            _resizePopupDelegate = gcnew ActionHandler(this, &WebView::SetPopupSizeAndPositionImpl);

            ToolTip = _toolTip =
                gcnew System::Windows::Controls::ToolTip();
            _toolTip->StaysOpen = true;
            _toolTip->Visibility = ::Visibility::Collapsed;
            _toolTip->Closed +=
                gcnew RoutedEventHandler(this, &WebView::ToolTip_Closed);

            _timer = gcnew DispatcherTimer(DispatcherPriority::Render);
            _timer->Interval = TimeSpan::FromSeconds(0.5);
            _timer->Tick +=
                gcnew EventHandler(this, &WebView::Timer_Tick);

            this->Loaded +=	gcnew RoutedEventHandler(this, &WebView::OnLoaded);	
            this->Unloaded += gcnew RoutedEventHandler(this, &WebView::OnUnloaded);	
            this->GotKeyboardFocus += gcnew KeyboardFocusChangedEventHandler(this, &WebView::OnGotKeyboardFocus);
            this->LostKeyboardFocus += gcnew KeyboardFocusChangedEventHandler(this, &WebView::OnLostKeyboardFocus);
        }

        bool WebView::TryGetCefBrowser(CefRefPtr<CefBrowser>& browser)
        {
            if (_browserCore->IsBrowserInitialized)
            {
                browser = _clientAdapter->GetCefBrowser();
                return browser != nullptr;
            }
            else
            {
                return false;
            }
        }

        void WebView::SetCursor(SafeFileHandle^ handle)
        {
            Cursor = CursorInteropHelper::Create(handle);
        }

        void WebView::BrowserCore_PropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
        {
            if (e->PropertyName == "TooltipText")
            {
                _timer->Stop();

                if (String::IsNullOrEmpty(_browserCore->TooltipText))
                {
                    Dispatcher->BeginInvoke(DispatcherPriority::Render,
                        gcnew Action<String^>(this, &WebView::SetTooltipText), nullptr);
                }
                else
                {
                    _timer->Start();
                }
            }
        }

        void WebView::Timer_Tick(Object^ sender, EventArgs^ e)
        {
            _timer->Stop();
            SetTooltipText(_browserCore->TooltipText);
        }

        void WebView::ToolTip_Closed(Object^ sender, RoutedEventArgs^ e)
        {
            _toolTip->Visibility = ::Visibility::Collapsed;
            // set Placement to something other than PlacementMode::Mouse,
            // so that when we re-show the tooltip in SetTooltipText(),
            // the tooltip will be repositioned to the new mouse point.
            _toolTip->Placement = PlacementMode::Absolute;
        }

        IntPtr WebView::SourceHook(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, bool% handled)
        {
            handled = false;

            switch(message)
            {
            case WM_KEYDOWN:
            case WM_KEYUP:
            case WM_SYSKEYDOWN:
            case WM_SYSKEYUP:
            case WM_CHAR:
            case WM_SYSCHAR:
            case WM_IME_CHAR:
                CefRefPtr<CefBrowser> browser;
                if (!IsKeyboardFocused ||
                    !TryGetCefBrowser(browser))
                {
                    break;
                }

                CefBrowser::KeyType type;
                if (message == WM_CHAR || message == WM_IME_CHAR)
                    type = KT_CHAR;
                else if (message == WM_KEYDOWN || message == WM_SYSKEYDOWN)
                    type = KT_KEYDOWN;
                else if (message == WM_KEYUP || message == WM_SYSKEYUP)
                    type = KT_KEYUP;

                CefKeyInfo keyInfo;
                keyInfo.key =
                    wParam.ToInt32();
                keyInfo.sysChar =
                    message == WM_SYSKEYDOWN ||
                    message == WM_SYSKEYUP ||
                    message == WM_SYSCHAR;
                keyInfo.imeChar =
                    message == WM_IME_CHAR;

                browser->SendKeyEvent(type, keyInfo, lParam.ToInt32());
                handled = true;
            }

            return IntPtr::Zero;
        }

        void WebView::SetBitmap()
        {
            InteropBitmap^ bitmap = _ibitmap;
            msclr::lock l(_sync);

            if(bitmap == nullptr) 
            {
                _image->Source = nullptr;
                GC::Collect(1);

                int stride = _width * PixelFormats::Bgra32.BitsPerPixel / 8;
                bitmap = (InteropBitmap^)Interop::Imaging::CreateBitmapSourceFromMemorySection(
                    (IntPtr)_fileMappingHandle, _width, _height, PixelFormats::Bgra32, stride, 0);
                _image->Source = bitmap;
                _ibitmap = bitmap;
            }

            bitmap->Invalidate();
        }

        void WebView::SetPopupBitmap()
        {
            InteropBitmap^ bitmap = _popupIbitmap;

            if(bitmap == nullptr) 
            {
                _popupImage->Source = nullptr;
                GC::Collect(1);

                int stride = _popupImageWidth * PixelFormats::Bgra32.BitsPerPixel / 8;
                bitmap = (InteropBitmap^)Interop::Imaging::CreateBitmapSourceFromMemorySection(
                    (IntPtr)_popupFileMappingHandle, _popupImageWidth, _popupImageHeight, PixelFormats::Bgra32, stride, 0);
                _popupImage->Source = bitmap;
                _popupIbitmap = bitmap;
            }

            bitmap->Invalidate();
        }

        void WebView::OnPreviewKey(KeyEventArgs^ e)
        {
            CefRefPtr<CefBrowser> browser;
            if (!TryGetCefBrowser(browser))
            {
                return;
            }

            if (e->Key == Key::Tab ||
                (e->Key >= Key::Left && e->Key <= Key::Down))
            {
                CefBrowser::KeyType type = e->IsDown ? KT_KEYDOWN : KT_KEYUP;
                CefKeyInfo keyInfo;
                keyInfo.key = KeyInterop::VirtualKeyFromKey(e->Key);
                browser->SendKeyEvent(type, keyInfo, 0);

                e->Handled = true;
            }
        }

        void WebView::OnPreviewTextInput(TextCompositionEventArgs^ e)
        {
            CefRefPtr<CefBrowser> browser;
            if (!TryGetCefBrowser(browser))
            {
                return;
            }

            CefBrowser::KeyType type;
            for (int i = 0; i < e->Text->Length; i++)
            {
                CefKeyInfo keyInfo;
                keyInfo.key = (int)e->Text[i];
                type = KT_CHAR; 
                browser->SendKeyEvent(type, keyInfo, 0);
            }
            e->Handled = true;
        }

        void WebView::OnMouseButton(MouseButtonEventArgs^ e)
        {
            CefRefPtr<CefBrowser> browser;
            if (!TryGetCefBrowser(browser))
            {
                return;
            }

            auto deviceIndependentPosition = e->GetPosition(this);
            auto pixelPosition = _matrix->Transform(deviceIndependentPosition);

            CefBrowser::MouseButtonType type;
            if (e->ChangedButton == MouseButton::Left)
                type = CefBrowser::MouseButtonType::MBT_LEFT;
            else if (e->ChangedButton == MouseButton::Middle)
                type = CefBrowser::MouseButtonType::MBT_MIDDLE;
            else
                type = CefBrowser::MouseButtonType::MBT_RIGHT;

            bool mouseUp = e->ButtonState == MouseButtonState::Released;

            browser->SendMouseClickEvent((int)pixelPosition.X, (int)pixelPosition.Y,
                type, mouseUp, e->ClickCount);
        }

        void WebView::OnVisualParentChanged(DependencyObject^ oldParent)
        {
            EventHandler^ _handler = gcnew EventHandler(this, &WebView::OnHidePopup);

            if (_currentWindow != nullptr)
            {
                _currentWindow->LocationChanged -= _handler;
                _currentWindow->Deactivated -= _handler;
            }

            _currentWindow = Window::GetWindow(this);
            if (_currentWindow != nullptr)
            {
                _currentWindow->LocationChanged += _handler;
                _currentWindow->Deactivated += _handler;
            }

            ContentControl::OnVisualParentChanged(oldParent);
        }

        Size WebView::ArrangeOverride(Size size)
        {
            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                Point point = _matrix->Transform(Point(size.Width, size.Height));
                browser->SetSize(PET_VIEW, (int)point.X, (int)point.Y);
                HidePopup();
            }
            else
            {
                Dispatcher->BeginInvoke(DispatcherPriority::Loaded,
                    gcnew ActionHandler(this, &WebView::InvalidateArrange));
            }

            return ContentControl::ArrangeOverride(size);
        }

        void WebView::OnGotKeyboardFocus(Object^ sender, KeyboardFocusChangedEventArgs^ e)
        {
            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->SendFocusEvent(true);
            }
        }

        void WebView::OnLostKeyboardFocus(Object^ sender, KeyboardFocusChangedEventArgs^ e)
        {
            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->SendFocusEvent(false);
            }

            HidePopup();
        }

        void WebView::OnPreviewKeyDown(KeyEventArgs^ e)
        {
            OnPreviewKey(e);
        }

        void WebView::OnPreviewKeyUp(KeyEventArgs^ e)
        {
            OnPreviewKey(e);
        }

        void WebView::OnMouseMove(MouseEventArgs^ e)
        {
            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                auto deviceIndependentPosition = e->GetPosition(this);
                auto pixelPosition = _matrix->Transform(deviceIndependentPosition);
                browser->SendMouseMoveEvent((int)pixelPosition.X, (int)pixelPosition.Y, false);
            }
        }

        void WebView::OnMouseWheel(MouseWheelEventArgs^ e)
        {
            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                auto deviceIndependentPosition = e->GetPosition(this);
                auto pixelPosition = _matrix->Transform(deviceIndependentPosition);
                browser->SendMouseWheelEvent((int)pixelPosition.X, (int)pixelPosition.Y, 0, e->Delta);
            }
        }

        void WebView::OnMouseDown(MouseButtonEventArgs^ e)
        {
            Focus();
            OnMouseButton(e);
            Mouse::Capture(this);
        }

        void WebView::OnMouseUp(MouseButtonEventArgs^ e)
        {
            OnMouseButton(e);
            Mouse::Capture(nullptr);
        }

        void WebView::OnMouseLeave(MouseEventArgs^ e)
        {
            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->SendMouseMoveEvent(0, 0, true);
            }

            _toolTip->IsOpen = false;
        }

        void WebView::OnInitialized()
        {
            _browserCore->OnInitialized();
        }

        void WebView::Load(String^ url)
        {
            _browserCore->CheckBrowserInitialization();
            _browserCore->OnLoad();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->GetMainFrame()->LoadURL(toNative(url));
            }
        }

        void WebView::LoadHtml(String^ html)
        {
            LoadHtml(html, "about:blank");
        }

        void WebView::LoadHtml(String^ html, String^ url)
        {
            _browserCore->CheckBrowserInitialization();
            _browserCore->OnLoad();
            _clientAdapter->GetCefBrowser()->GetMainFrame()->LoadString(toNative(html), toNative(url));
        }

        void WebView::Stop()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->StopLoad();
            }
        }

        void WebView::Back()
        {
            _browserCore->CheckBrowserInitialization();


            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->GoBack();
            }
        }

        void WebView::Forward()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->GoForward();
            }
        }

        void WebView::Reload()
        {
            Reload(false);
        }

        void WebView::Reload(bool ignoreCache)
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (!TryGetCefBrowser(browser))
            {
                return;
            }

            if (ignoreCache)
            {
                browser->ReloadIgnoreCache();
            }
            else
            {
                browser->Reload();
            }
        }

        void WebView::ClearHistory()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->ClearHistory();
            }
        }

        void WebView::ShowDevTools()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->ShowDevTools();
            }
        }

        void WebView::CloseDevTools()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->CloseDevTools();
            }
        }

        void WebView::Undo()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->GetMainFrame()->Undo();
            }
        }

        void WebView::Redo()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->GetMainFrame()->Redo();
            }
        }

        void WebView::Cut()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->GetMainFrame()->Cut();
            }
        }

        void WebView::Copy()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->GetMainFrame()->Copy();
            }
        }

        void WebView::Paste()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->GetMainFrame()->Paste();
            }
        }

        void WebView::Delete()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->GetMainFrame()->Delete();
            }
        }

        void WebView::SelectAll()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->GetMainFrame()->SelectAll();
            }
        }

        void WebView::Print()
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->GetMainFrame()->Print();
            }
        }

        void WebView::ExecuteScript(String^ script)
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                _scriptCore->Execute(browser, toNative(script));
            }
        }

        Object^ WebView::EvaluateScript(String^ script)
        {
            return EvaluateScript(script, TimeSpan::MaxValue);
        }

        Object^ WebView::EvaluateScript(String^ script, TimeSpan timeout)
        {
            _browserCore->CheckBrowserInitialization();

            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                return _scriptCore->Evaluate(browser, toNative(script),
                    timeout.TotalMilliseconds);
            }
            else
            {
                return nullptr;
            }
        }

        void WebView::SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
        {
            _browserCore->SetNavState(isLoading, canGoBack, canGoForward);
        }

        void WebView::OnConsoleMessage(String^ message, String^ source, int line)
        {
            ConsoleMessage(this, gcnew ConsoleMessageEventArgs(message, source, line));
        }

        void WebView::RegisterJsObject(String^ name, Object^ objectToBind)
        {
            _browserCore->RegisterJsObject(name, objectToBind);
        }

        IDictionary<String^, Object^>^ WebView::GetBoundObjects()
        {
            return _browserCore->GetBoundObjects();
        }

        void WebView::OnFrameLoadStart(String^ url, bool isMainFrame)
        {
            _browserCore->OnFrameLoadStart();

            LoadStartedEventArgs^ args = gcnew LoadStartedEventArgs(url, isMainFrame);
            LoadStarted(this, args);
        }

        void WebView::OnFrameLoadEnd(String^ url, bool isMainFrame)
        {
            _browserCore->OnFrameLoadEnd();

            LoadCompletedEventArgs^ args = gcnew LoadCompletedEventArgs(url, isMainFrame);
            LoadCompleted(this, args);
        }

        void WebView::OnTakeFocus(bool next)
        {
            FocusNavigationDirection direction = next ?
                FocusNavigationDirection::Next :
            FocusNavigationDirection::Previous;
            TraversalRequest^ request = gcnew TraversalRequest(direction);

            Dispatcher->BeginInvoke(DispatcherPriority::Input,
                gcnew MoveFocusHandler(this, &WebView::MoveFocus), request);
        }

        void WebView::OnApplyTemplate()
        {
            ContentControl::OnApplyTemplate();

            _clientAdapter = new RenderClientAdapter(this);

            AddSourceHook();

            HWND hwnd = static_cast<HWND>(_source->Handle.ToPointer());
            CefWindowInfo window;
            window.SetAsOffScreen(hwnd);
            window.SetTransparentPainting(TRUE);
            CefString url = toNative(_browserCore->Address);

            CefBrowser::CreateBrowser(window, _clientAdapter.get(),
                url, *(CefBrowserSettings*)_settings->_internalBrowserSettings);

            Content = _image = gcnew Image();
            RenderOptions::SetBitmapScalingMode(_image, BitmapScalingMode::NearestNeighbor);

            _popup = gcnew Popup();
            _popup->Child = _popupImage = gcnew Image();

            _popup->MouseDown += gcnew MouseButtonEventHandler(this, &WebView::OnPopupMouseDown);
            _popup->MouseUp += gcnew MouseButtonEventHandler(this, &WebView::OnPopupMouseUp);
            _popup->MouseMove += gcnew MouseEventHandler(this, &WebView::OnPopupMouseMove);
            _popup->MouseLeave += gcnew MouseEventHandler(this, &WebView::OnPopupMouseLeave);
            _popup->MouseWheel += gcnew MouseWheelEventHandler(this, &WebView::OnPopupMouseWheel);

            _popup->PlacementTarget = this;
            _popup->Placement = PlacementMode::Relative;

            _image->Stretch = Stretch::None;
            _image->HorizontalAlignment = ::HorizontalAlignment::Left;
            _image->VerticalAlignment = ::VerticalAlignment::Top;

            _popupImage->Stretch = Stretch::None;
            _popupImage->HorizontalAlignment = ::HorizontalAlignment::Left;
            _popupImage->VerticalAlignment = ::VerticalAlignment::Top;

            if (IsNonStandardDpi())
            {
                auto transform = GetScaleTransform();
                _image->LayoutTransform = transform;
                _popup->LayoutTransform = transform;
                _popupOffsetTransform = transform;
            }
        }

        bool WebView::IsNonStandardDpi()
        {
            // If the display properties is set to e.g. 125%, M11 and M22 will be 1.25.
            return _matrix->M11 != 1 ||
                _matrix->M22 != 1;
        }

        Transform^ WebView::GetScaleTransform()
        {
            auto factorX = _matrix->M11;
            auto factorY = _matrix->M22;
            auto scaleX = 1 / factorX;
            auto scaleY = 1 / factorY;
            return gcnew ScaleTransform(scaleX, scaleY);
        }

        void WebView::SetCursor(IntPtr cursor)
        {
            SafeFileHandle^ handle = gcnew SafeFileHandle(cursor, false);
            Dispatcher->BeginInvoke(DispatcherPriority::Render,
                gcnew Action<SafeFileHandle^>(this, &WebView::SetCursor), handle);
        }

        void WebView::SetTooltipText(String^ text)
        {
            if (String::IsNullOrEmpty(text))
            {
                _toolTip->IsOpen = false;
            }
            else
            {
                _toolTip->Content = text;
                _toolTip->Placement = PlacementMode::Mouse;
                _toolTip->Visibility = ::Visibility::Visible;
                _toolTip->IsOpen = true;
            }
        }

        void WebView::SetBuffer(int width, int height, const void* buffer)
        {
            msclr::lock l(_sync);

            int currentWidth = _width, currentHeight = _height;
            HANDLE fileMappingHandle = _fileMappingHandle, backBufferHandle = _backBufferHandle;
            InteropBitmap^ ibitmap = _ibitmap;

            SetBuffer(currentWidth, currentHeight, width, height, fileMappingHandle, backBufferHandle, ibitmap,
                _paintDelegate, buffer);

            _ibitmap = ibitmap;
            _fileMappingHandle = fileMappingHandle;
            _backBufferHandle = backBufferHandle;

            _width = currentWidth;
            _height = currentHeight;
        }

        void WebView::SetPopupBuffer(int width, int height, const void* buffer)
        {
            int currentWidth = _popupImageWidth, currentHeight = _popupImageHeight;
            HANDLE fileMappingHandle = _popupFileMappingHandle, backBufferHandle = _popupBackBufferHandle;
            InteropBitmap^ ibitmap = _popupIbitmap;

            SetBuffer(currentWidth, currentHeight, width, height, fileMappingHandle, backBufferHandle, ibitmap,
                _paintPopupDelegate, buffer);

            _popupIbitmap = ibitmap;
            _popupFileMappingHandle = fileMappingHandle;
            _popupBackBufferHandle = backBufferHandle;

            _popupImageWidth = currentWidth;
            _popupImageHeight = currentHeight;
        }

        void WebView::SetBuffer(int &currentWidth, int& currentHeight, int width, int height,
            HANDLE& fileMappingHandle, HANDLE& backBufferHandle,
            InteropBitmap^& ibitmap, ActionHandler^ paintDelegate,
            const void* buffer)
        {
            if (!backBufferHandle || currentWidth != width || currentHeight != height)
            {
                ibitmap = nullptr;

                if (backBufferHandle)
                {
                    UnmapViewOfFile(backBufferHandle);
                    backBufferHandle = NULL;
                }

                if (fileMappingHandle)
                {
                    CloseHandle(fileMappingHandle);
                    fileMappingHandle = NULL;
                }

                int pixels = width * height;
                int bytes = pixels * PixelFormats::Bgr32.BitsPerPixel / 8;

                fileMappingHandle = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, bytes, NULL);
                if(!fileMappingHandle) 
                {
                    return;
                }

                backBufferHandle = MapViewOfFile(fileMappingHandle, FILE_MAP_ALL_ACCESS, 0, 0, bytes);
                if(!backBufferHandle) 
                {
                    return;
                }

                currentWidth = width;
                currentHeight = height;
            }

            int stride = width * PixelFormats::Bgr32.BitsPerPixel / 8;
            CopyMemory(backBufferHandle, (void*) buffer, height * stride);

            if(!Dispatcher->HasShutdownStarted) {
                Dispatcher->BeginInvoke(DispatcherPriority::Render, paintDelegate);
            }
        }

        void WebView::SetPopupSizeAndPosition(const void* rect)
        {
            auto cefRect = (const CefRect*) rect;

            _popupX = cefRect->x;
            _popupY = cefRect->y;
            _popupWidth = cefRect->width;
            _popupHeight = cefRect->height;

            if(!Dispatcher->HasShutdownStarted) {
                Dispatcher->BeginInvoke(DispatcherPriority::Render, _resizePopupDelegate);
            }
        }

        void WebView::SetPopupIsOpen(bool isOpen)
        {
            if(!Dispatcher->HasShutdownStarted) {
                Dispatcher->BeginInvoke(gcnew Action<bool>(this, &WebView::ShowHidePopup), DispatcherPriority::Render, isOpen);
            }
        }

        void WebView::SetPopupSizeAndPositionImpl()
        {
            _popup->Width = _popupWidth;
            _popup->Height = _popupHeight;

            auto popupOffset = Point(_popupX, _popupY);
            if(_popupOffsetTransform != nullptr) 
            {
                popupOffset = _popupOffsetTransform->GeneralTransform::Transform(popupOffset);
            }

            _popup->HorizontalOffset = popupOffset.X;
            _popup->VerticalOffset = popupOffset.Y;
        }

        void WebView::ShowHidePopup(bool isOpened)
        {
            _popup->IsOpen = isOpened;
        }

        void WebView::OnLoaded(Object^ sender, RoutedEventArgs^ e)
        {
            AddSourceHook();
        }

        void WebView::OnUnloaded(Object^ sender, RoutedEventArgs^ e)
        {  
            if (_source && _hook)
            {
                _source->RemoveHook(_hook);
                _source = nullptr;
                _hook = nullptr;
            }
        }

        void WebView::OnPopupMouseMove(Object^ sender, MouseEventArgs^ e)
        {
            OnMouseMove(e);
        }

        void WebView::OnPopupMouseWheel(Object^ sender, MouseWheelEventArgs^ e)
        {
            OnMouseWheel(e);
        }

        void WebView::OnPopupMouseDown(Object^ sender,MouseButtonEventArgs^ e)
        {
            OnMouseDown(e);
        }

        void WebView::OnPopupMouseUp(Object^ sender,MouseButtonEventArgs^ e)
        {
            OnMouseUp(e);
        }

        void WebView::OnPopupMouseLeave(Object^ sender,MouseEventArgs^ e)
        {
            OnMouseLeave(e);
        }

        void WebView::OnHidePopup(Object^ sender, EventArgs^ e)
        { 
            HidePopup();
        }

        void WebView::HidePopup()
        {
            CefRefPtr<CefBrowser> browser;
            if (_popup != nullptr && _popup->IsOpen && TryGetCefBrowser(browser))
            {           
                // This is the only decent way I found to hide popup properly so that clicking again on the a drop down (<select>) 
                // opens it. 
                browser->SendMouseClickEvent(-1,-1, CefBrowser::MouseButtonType::MBT_LEFT, false, 1 );
            }
        }

        void WebView::AddSourceHook()
        {
            if (_source == nullptr)
            {
                _source = (HwndSource^)PresentationSource::FromVisual(this);
                if (_source != nullptr)
                {
                    _matrix = _source->CompositionTarget->TransformToDevice;

                    _hook = gcnew Interop::HwndSourceHook(this, &WebView::SourceHook);
                    _source->AddHook(_hook);
                }
            }
        }
    }
}
