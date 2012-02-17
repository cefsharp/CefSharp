#include "stdafx.h"

#include "WebBrowser.h"

namespace CefSharp
{
    void WebBrowser::Load(String^ url)
    {
        WaitForInitialized();

        _loadCompleted->Reset();
        _clientAdapter->GetCefBrowser()->GetMainFrame()->LoadURL(toNative(url));
    }

    void WebBrowser::WaitForLoadCompletion()
    {
        WaitForLoadCompletion(-1);
    }

    void WebBrowser::WaitForLoadCompletion(int timeout)
    {
        _loadCompleted->Wait(timeout);
    }

    void WebBrowser::Stop()
    {
    	WaitForInitialized();

        _clientAdapter->GetCefBrowser()->StopLoad();
    }

    void WebBrowser::Back()
    {
    	WaitForInitialized();

        _clientAdapter->GetCefBrowser()->GoBack();
    }

    void WebBrowser::Forward()
    {
    	WaitForInitialized();

        _clientAdapter->GetCefBrowser()->GoForward();
    }

    void WebBrowser::Reload()
    {
        Reload(false);
    }

    void WebBrowser::Reload(bool ignoreCache)
    {
    	WaitForInitialized();

        if(ignoreCache)
        {
            _clientAdapter->GetCefBrowser()->ReloadIgnoreCache();
        }
        else
        {
            _clientAdapter->GetCefBrowser()->Reload();
        }
    }

    void WebBrowser::Print()
    {
        WaitForInitialized();

        _clientAdapter->GetCefBrowser()->GetMainFrame()->Print();
    }

    String^ WebBrowser::RunScript(String^ script)
    {
    	WaitForInitialized();

        CefRefPtr<CefBrowser> browser = _clientAdapter->GetCefBrowser();
        CefRefPtr<CefFrame> frame = browser->GetMainFrame();

        return _scriptCore->Evaluate(frame, script);
    }

    void WebBrowser::OnInitialized()
    {
        _browserInitialized->Set();
    }

    void WebBrowser::SetTitle(String^ title)
    {
        _title = title;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Title"));
    }

    void WebBrowser::SetToolTip(String^ text)
    {

    }

    void WebBrowser::SetAddress(String^ address)
    {
        _address = address;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"Address"));
    }

    void WebBrowser::SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
    {
        if(isLoading != _isLoading) 
        {
            _isLoading = isLoading;
            PropertyChanged(this, gcnew PropertyChangedEventArgs(L"IsLoading"));
        }

        if(canGoBack != _canGoBack) 
        {
            _canGoBack = canGoBack;
            PropertyChanged(this, gcnew PropertyChangedEventArgs(L"CanGoBack"));
        }

        if(canGoForward != _canGoForward)
        {
            _canGoForward = canGoForward;
            PropertyChanged(this, gcnew PropertyChangedEventArgs(L"CanGoForward"));
        }
    }

    void WebBrowser::AddFrame(CefRefPtr<CefFrame> frame)
    {
        _loadCompleted->AddCount();
    }

    void WebBrowser::FrameLoadComplete(CefRefPtr<CefFrame> frame)
    {
        _loadCompleted->Signal();
    }

    void WebBrowser::SetJsResult(String^ result)
    {
        _jsResult = result;
        _runJsFinished->Set();
    }

    void WebBrowser::SetJsError()
    {
        _jsError = true;
        _runJsFinished->Set();
    }

    void WebBrowser::RaiseConsoleMessage(String^ message, String^ source, int line)
    {
        ConsoleMessage(this, gcnew ConsoleMessageEventArgs(message, source, line));
    }

    void WebBrowser::WaitForInitialized()
    {
        if (IsInitialized) return;

        // TODO: risk of infinite lock
        _browserInitialized->WaitOne();
    }

    void WebBrowser::OnApplyTemplate()
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

    Size WebBrowser::ArrangeOverride(Size size)
    {
		if(_clientAdapter->GetIsInitialized()) {
			_clientAdapter->GetCefBrowser()->SetSize(PET_VIEW, (int)size.Width, (int)size.Height);
		} else {
			Dispatcher->BeginInvoke(DispatcherPriority::Loaded, gcnew ActionDelegate(this, &WebBrowser::InvalidateArrange));
		}

        return ContentControl::ArrangeOverride(size);
    }

    void WebBrowser::OnGotFocus(RoutedEventArgs^ e)
    {
        _clientAdapter->GetCefBrowser()->SendFocusEvent(true);
        ContentControl::OnGotFocus(e);
    }

    void WebBrowser::OnLostFocus(RoutedEventArgs^ e)
    {
        _clientAdapter->GetCefBrowser()->SendFocusEvent(false);
        ContentControl::OnLostFocus(e);
    }

    void WebBrowser::OnMouseMove(MouseEventArgs^ e)
    {
        Point point = e->GetPosition(this);
        _clientAdapter->GetCefBrowser()->SendMouseMoveEvent((int)point.X, (int)point.Y, false);
    }

    void WebBrowser::OnMouseWheel(MouseWheelEventArgs^ e)
    {
        Point point = e->GetPosition(this);
        _clientAdapter->GetCefBrowser()->SendMouseWheelEvent((int)point.X, (int)point.Y, e->Delta);
    }

    void WebBrowser::OnMouseLeave(MouseEventArgs^ e)
    {
        _clientAdapter->GetCefBrowser()->SendMouseMoveEvent(0, 0, true);
    }

    void WebBrowser::OnMouseDown(MouseButtonEventArgs^ e)
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

    void WebBrowser::OnMouseUp(MouseButtonEventArgs^ e)
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

    void WebBrowser::SetCursor(CefCursorHandle cursor)
    {
        SafeFileHandle^ handle = gcnew SafeFileHandle((IntPtr)cursor, false);
        Dispatcher->BeginInvoke(DispatcherPriority::Render,
            gcnew Action<SafeFileHandle^>(this, &WebBrowser::SetCursor), handle);
    }

    void WebBrowser::SetCursor(SafeFileHandle^ handle)
    {
        Cursor = CursorInteropHelper::Create(handle);
    }

    IntPtr WebBrowser::SourceHook(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, bool% handled)
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

    void WebBrowser::SetBuffer(int width, int height, const std::vector<CefRect>& dirtyRects, const void* buffer)
    {
        /*
		if (dirtyRect.width == 0 || dirtyRect.height == 0 || width == 0 || height == 0)
        {
            return;
        }
        */

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
    
	void WebBrowser::SetBitmap()
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

	void WebBrowser::Close() 
	{
		_clientAdapter->GetCefBrowser()->CloseBrowser();
	}
}