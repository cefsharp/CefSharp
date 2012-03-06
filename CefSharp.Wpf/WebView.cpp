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

        _browserCore = gcnew BrowserCore(address);
        _browserCore->PropertyChanged +=
            gcnew PropertyChangedEventHandler(this, &WebView::BrowserCore_PropertyChanged);

        _scriptCore = new ScriptCore();

		_paintDelegate = gcnew ActionDelegate(this, &WebView::SetBitmap);

        ToolTip = _toolTip =
            gcnew System::Windows::Controls::ToolTip();
        _toolTip->StaysOpen = true;

        _timer = gcnew DispatcherTimer(DispatcherPriority::Render);
        _timer->Interval = TimeSpan::FromSeconds(0.5);
        _timer->Tick +=
            gcnew EventHandler(this, &WebView::Timer_Tick);
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
                if (!IsFocused)
                {
                    break;
                }

                CefBrowser::KeyType type = KT_CHAR;
                bool sysChar = false, imeChar = false;

                if (message == WM_KEYDOWN || message == WM_SYSKEYDOWN)
                {
                    type = KT_KEYDOWN;
                }
                else if (message == WM_KEYUP || message == WM_SYSKEYUP)
                {
                    type = KT_KEYUP;
                }

                if (message == WM_SYSKEYDOWN || message == WM_SYSKEYUP || message == WM_SYSCHAR)
                {
                    sysChar = true;
                }

                if (message == WM_IME_CHAR)
                {
                    imeChar = true;
                }

                _clientAdapter->GetCefBrowser()->SendKeyEvent(type, wParam.ToInt32(), lParam.ToInt32(), sysChar, imeChar);
                handled = true;
        }

        return IntPtr::Zero;
    }

    void WebView::SetBitmap()
    {
		InteropBitmap^ bitmap = _ibitmap;

		if(bitmap == nullptr) 
		{
			_image->Source = nullptr;
			GC::Collect(1);

			int stride = _width * PixelFormats::Bgr32.BitsPerPixel / 8;
            bitmap = (InteropBitmap^)Interop::Imaging::CreateBitmapSourceFromMemorySection(
                (IntPtr)_fileMappingHandle, _width, _height, PixelFormats::Bgr32, stride, 0);
			_image->Source = bitmap;
			_ibitmap = bitmap;
		}

		bitmap->Invalidate();
    }

    void WebView::OnPreviewKey(KeyEventArgs^ e)
    {
        if (e->Key == Key::Tab)
        {
            CefBrowser::KeyType type = e->IsDown ? KT_KEYDOWN : KT_KEYUP;
            _clientAdapter->GetCefBrowser()->SendKeyEvent(type, 9, 0, false, false);
            e->Handled = true;
        }
    }

    Size WebView::ArrangeOverride(Size size)
    {
		if(_clientAdapter->GetIsInitialized()) {
			_clientAdapter->GetCefBrowser()->SetSize(PET_VIEW,
                (int)size.Width, (int)size.Height);
		} else {
			Dispatcher->BeginInvoke(DispatcherPriority::Loaded,
                gcnew ActionDelegate(this, &WebView::InvalidateArrange));
		}

        return ContentControl::ArrangeOverride(size);
    }

    void WebView::OnGotFocus(RoutedEventArgs^ e)
    {
        _clientAdapter->GetCefBrowser()->SendFocusEvent(true);
        ContentControl::OnGotFocus(e);
    }

    void WebView::OnLostFocus(RoutedEventArgs^ e)
    {
        _clientAdapter->GetCefBrowser()->SendFocusEvent(false);
        ContentControl::OnLostFocus(e);
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
        Point point = e->GetPosition(this);
        _clientAdapter->GetCefBrowser()->SendMouseMoveEvent((int)point.X, (int)point.Y, false);
    }

    void WebView::OnMouseWheel(MouseWheelEventArgs^ e)
    {
        Point point = e->GetPosition(this);
        _clientAdapter->GetCefBrowser()->SendMouseWheelEvent((int)point.X, (int)point.Y, e->Delta);
    }

    void WebView::OnMouseDown(MouseButtonEventArgs^ e)
    {
        Keyboard::Focus(this); // XXX: temporary

        Point point = e->GetPosition(this);
        CefBrowser::MouseButtonType mbt;
        if (e->RightButton == MouseButtonState::Pressed)
        {
            mbt = CefBrowser::MouseButtonType::MBT_RIGHT;
        }
        else if (e->LeftButton == MouseButtonState::Pressed)
        {
            mbt = CefBrowser::MouseButtonType::MBT_LEFT;
        }

        _clientAdapter->GetCefBrowser()->SendMouseClickEvent((int)point.X, (int)point.Y, mbt, false, 1);
    }

    void WebView::OnMouseLeave(MouseEventArgs^ e)
    {
        _clientAdapter->GetCefBrowser()->SendMouseMoveEvent(0, 0, true);
    }

    void WebView::OnMouseUp(MouseButtonEventArgs^ e)
    {
        Point point = e->GetPosition(this);
        CefBrowser::MouseButtonType mbt;
        if (e->RightButton == MouseButtonState::Pressed)
        {
            mbt = CefBrowser::MouseButtonType::MBT_RIGHT;
        }
        else if (e->LeftButton == MouseButtonState::Pressed)
        {
            mbt = CefBrowser::MouseButtonType::MBT_LEFT;
        }

        _clientAdapter->GetCefBrowser()->SendMouseClickEvent((int)point.X, (int)point.Y, mbt, true, 1);
    }

    void WebView::OnInitialized()
    {
        _browserCore->OnInitialized();
    }

    void WebView::Load(String^ url)
    {
        _browserCore->CheckBrowserInitialization();
        _browserCore->OnLoad();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->LoadURL(toNative(url));
    }

    void WebView::Stop()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->StopLoad();
    }

    void WebView::Back()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GoBack();
    }

    void WebView::Forward()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GoForward();
    }

    void WebView::Reload()
    {
        Reload(false);
    }

    void WebView::Reload(bool ignoreCache)
    {
        _browserCore->CheckBrowserInitialization();
        if (ignoreCache)
        {
            _clientAdapter->GetCefBrowser()->ReloadIgnoreCache();
        }
        else
        {
            _clientAdapter->GetCefBrowser()->Reload();
        }
    }

    void WebView::Print()
    {
        _browserCore->CheckBrowserInitialization();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->Print();
    }

    void WebView::ExecuteScript(String^ script)
    {
        _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser = _clientAdapter->GetCefBrowser();
        CefRefPtr<CefFrame> frame = browser->GetMainFrame();

        _scriptCore->Execute(frame, toNative(script));
    }

    Object^ WebView::EvaluateScript(String^ script)
    {
        return EvaluateScript(script, TimeSpan::MaxValue);
    }

    Object^ WebView::EvaluateScript(String^ script, TimeSpan timeout)
    {
	    _browserCore->CheckBrowserInitialization();

        CefRefPtr<CefBrowser> browser = _clientAdapter->GetCefBrowser();
        CefRefPtr<CefFrame> frame = browser->GetMainFrame();

        return _scriptCore->Evaluate(frame, toNative(script), timeout.TotalMilliseconds);
    }

    void WebView::SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
    {
        _browserCore->SetNavState(isLoading, canGoBack, canGoForward);
    }

    void WebView::RaiseConsoleMessage(String^ message, String^ source, int line)
    {
        ConsoleMessage(this, gcnew ConsoleMessageEventArgs(message, source, line));
    }

    void WebView::OnFrameLoadStart()
    {
        _browserCore->OnFrameLoadStart();
    }

    void WebView::OnFrameLoadEnd()
    {
        _browserCore->OnFrameLoadEnd();
    }

    void WebView::OnTakeFocus(bool next)
    {
        FocusNavigationDirection direction = next ?
            FocusNavigationDirection::Next :
            FocusNavigationDirection::Previous;
        TraversalRequest^ request = gcnew TraversalRequest(direction);

        Dispatcher->BeginInvoke(DispatcherPriority::Input,
            gcnew MoveFocusDelegate(this, &WebView::MoveFocus), request);
    }

    void WebView::OnApplyTemplate()
    {
        ContentControl::OnApplyTemplate();

        _clientAdapter = new RenderClientAdapter(this);

        Visual^ parent = (Visual^)VisualTreeHelper::GetParent(this);
        HwndSource^ source = (HwndSource^)PresentationSource::FromVisual(parent);
        HWND hwnd = static_cast<HWND>(source->Handle.ToPointer());

        CefWindowInfo window;
        window.SetAsOffScreen(hwnd);
        CefRefPtr<RenderClientAdapter> ptr = _clientAdapter.get();
        CefString url = toNative(_browserCore->Address);

        CefBrowser::CreateBrowser(window, static_cast<CefRefPtr<CefClient>>(ptr),
            url, *_settings->_browserSettings);

        source->AddHook(gcnew Interop::HwndSourceHook(this, &WebView::SourceHook));

        _image = (Image^)GetTemplateChild("PART_Image");
        Content = _image = gcnew Image();
        _image->Stretch = Stretch::None;
        _image->HorizontalAlignment = ::HorizontalAlignment::Left;
        _image->VerticalAlignment = ::VerticalAlignment::Top;
    }

    void WebView::SetCursor(CefCursorHandle cursor)
    {
        SafeFileHandle^ handle = gcnew SafeFileHandle((IntPtr)cursor, false);
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
            _toolTip->IsOpen = true;
        }
    }

    void WebView::SetBuffer(int width, int height, const void* buffer)
    {
        if (!_backBufferHandle || _width != width || _height != height)
        {
			_ibitmap = nullptr;

			if (_backBufferHandle)
            {
                UnmapViewOfFile(_backBufferHandle);
                _backBufferHandle = NULL;
            }

            if (_fileMappingHandle)
            {
                CloseHandle(_fileMappingHandle);
                _fileMappingHandle = NULL;
            }

			int pixels = width * height;
            int bytes = pixels * PixelFormats::Bgr32.BitsPerPixel / 8;

			_fileMappingHandle = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, bytes, NULL);
			if(!_fileMappingHandle) 
			{
				return;
			}

			_backBufferHandle = MapViewOfFile(_fileMappingHandle, FILE_MAP_ALL_ACCESS, 0, 0, bytes);
			if(!_backBufferHandle) 
			{
				return;
			}

			_width = width;
			_height = height;
        }
	
		int stride = width * PixelFormats::Bgr32.BitsPerPixel / 8;
		CopyMemory(_backBufferHandle, (void*) buffer, height * stride);
		
		if(!Dispatcher->HasShutdownStarted) {
			Dispatcher->BeginInvoke(DispatcherPriority::Render, _paintDelegate);
		}
    }
}}