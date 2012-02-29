#include "WebView.h"

namespace CefSharp
{
namespace Wpf
{
    void WebView::WaitForInitialized()
    {
        if (IsInitialized)
        {
            return;
        }

        // TODO: risk of infinite lock
        _initialized->WaitOne();
    }

    void WebView::SetCursor(SafeFileHandle^ handle)
    {
        Cursor = CursorInteropHelper::Create(handle);
    }

    void WebView::BrowserCore_PropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
    {
        if (e->PropertyName == "Tooltip")
        {
            _timer->Stop();

            if (String::IsNullOrEmpty(_browserCore->Tooltip))
            {
                Dispatcher->BeginInvoke(DispatcherPriority::Render,
                    gcnew Action<String^>(this, &WebView::SetTooltip), nullptr);
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
        SetTooltip(_browserCore->Tooltip);
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
			bitmap = (InteropBitmap^) System::Windows::Interop::Imaging::CreateBitmapSourceFromMemorySection((IntPtr) _fileMappingHandle, _width, _height, PixelFormats::Bgr32, stride, 0);
			_image->Source = bitmap;
			_ibitmap = bitmap;
		}

		bitmap->Invalidate();
    }

    Size WebView::ArrangeOverride(Size size)
    {
		if(_clientAdapter->GetIsInitialized()) {
			_clientAdapter->GetCefBrowser()->SetSize(PET_VIEW, (int)size.Width, (int)size.Height);
		} else {
			Dispatcher->BeginInvoke(DispatcherPriority::Loaded, gcnew ActionDelegate(this, &WebView::InvalidateArrange));
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
        _initialized->Set();
    }

    void WebView::Load(String^ url)
    {
        WaitForInitialized();
        _browserCore->OnLoad();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->LoadURL(toNative(url));
    }

    void WebView::Stop()
    {
        WaitForInitialized();
        _clientAdapter->GetCefBrowser()->StopLoad();

    }

    void WebView::Back()
    {
        WaitForInitialized();
        _clientAdapter->GetCefBrowser()->GoBack();
    }

    void WebView::Forward()
    {
        WaitForInitialized();
        _clientAdapter->GetCefBrowser()->GoForward();
    }

    void WebView::Reload()
    {
        Reload(false);
    }

    void WebView::Reload(bool ignoreCache)
    {
        WaitForInitialized();
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
        WaitForInitialized();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->Print();
    }

    void WebView::ExecuteScript(String^ script)
    {
        WaitForInitialized();

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
	    WaitForInitialized();

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

    void WebView::OnApplyTemplate()
    {
        ContentControl::OnApplyTemplate();

        _image = (Image^)GetTemplateChild("PART_Image");

        if (_image == nullptr)
        {
            Content = _image = gcnew Image();

            _image->Stretch = Stretch::None;
            _image->HorizontalAlignment = System::Windows::HorizontalAlignment::Left;
            _image->VerticalAlignment = System::Windows::VerticalAlignment::Top;
        }
    }

    void WebView::SetCursor(CefCursorHandle cursor)
    {
        SafeFileHandle^ handle = gcnew SafeFileHandle((IntPtr)cursor, false);
        Dispatcher->BeginInvoke(DispatcherPriority::Render,
            gcnew Action<SafeFileHandle^>(this, &WebView::SetCursor), handle);
    }

    void WebView::SetTooltip(String^ text)
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