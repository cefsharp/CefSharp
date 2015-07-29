// Copyright � 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefBrowserHostWrapper.h"

void CefBrowserHostWrapper::StartDownload(String^ url)
{
    ThrowIfDisposed();

    _browserHost->StartDownload(StringUtils::ToNative(url));
}

void CefBrowserHostWrapper::Print()
{
    ThrowIfDisposed();

    _browserHost->Print();
}

void CefBrowserHostWrapper::SetZoomLevel(double zoomLevel)
{
    ThrowIfDisposed();

    _browserHost->SetZoomLevel(zoomLevel);
}

Task<double>^ CefBrowserHostWrapper::GetZoomLevelAsync()
{
    ThrowIfDisposed();

    if (CefCurrentlyOn(TID_UI))
    {
        TaskCompletionSource<double>^ taskSource = gcnew TaskCompletionSource<double>();
        taskSource->SetResult(GetZoomLevelOnUI());
        return taskSource->Task;
    }
    return Cef::UIThreadTaskFactory->StartNew(gcnew Func<double>(this, &CefBrowserHostWrapper::GetZoomLevelOnUI));
}

IntPtr CefBrowserHostWrapper::GetWindowHandle()
{
    ThrowIfDisposed();

    return IntPtr(_browserHost->GetWindowHandle());
}

void CefBrowserHostWrapper::CloseBrowser(bool forceClose)
{
    ThrowIfDisposed();

    _browserHost->CloseBrowser(forceClose);
}

void CefBrowserHostWrapper::ShowDevTools()
{
    ThrowIfDisposed();

    CefWindowInfo windowInfo;
    CefBrowserSettings settings;

    windowInfo.SetAsPopup(_browserHost->GetWindowHandle(), "DevTools");

    _browserHost->ShowDevTools(windowInfo, _browserHost->GetClient(), settings, CefPoint());
}

void CefBrowserHostWrapper::CloseDevTools()
{
    ThrowIfDisposed();

    _browserHost->CloseDevTools();
}

void CefBrowserHostWrapper::AddWordToDictionary(String^ word)
{
    ThrowIfDisposed();

    _browserHost->AddWordToDictionary(StringUtils::ToNative(word));
}

void CefBrowserHostWrapper::ReplaceMisspelling(String^ word)
{
    ThrowIfDisposed();

    _browserHost->ReplaceMisspelling(StringUtils::ToNative(word));
}

void CefBrowserHostWrapper::Find(int identifier, String^ searchText, bool forward, bool matchCase, bool findNext)
{
    ThrowIfDisposed();

    _browserHost->Find(identifier, StringUtils::ToNative(searchText), forward, matchCase, findNext);
}

void CefBrowserHostWrapper::StopFinding(bool clearSelection)
{
    ThrowIfDisposed();

    _browserHost->StopFinding(clearSelection);
}

void CefBrowserHostWrapper::SetFocus(bool focus)
{
    ThrowIfDisposed();

    _browserHost->SetFocus(focus);
}

void CefBrowserHostWrapper::SendFocusEvent(bool setFocus)
{
    ThrowIfDisposed();

    _browserHost->SendFocusEvent(setFocus);
}

void CefBrowserHostWrapper::SendKeyEvent(KeyEvent keyEvent)
{
    CefKeyEvent nativeKeyEvent;
    nativeKeyEvent.focus_on_editable_field = keyEvent.FocusOnEditableField == 1;
    nativeKeyEvent.is_system_key = keyEvent.IsSystemKey == 1;
    nativeKeyEvent.modifiers = (uint32)keyEvent.Modifiers;
    nativeKeyEvent.native_key_code = keyEvent.NativeKeyCode;
    nativeKeyEvent.windows_key_code = keyEvent.WindowsKeyCode;
        
    _browserHost->SendKeyEvent(nativeKeyEvent);
}

double CefBrowserHostWrapper::GetZoomLevelOnUI()
{
    ThrowIfDisposed();

    CefTaskScheduler::EnsureOn(TID_UI, "CefBrowserHostWrapper::GetZoomLevel");

    //Don't throw exception if no browser host here as it's not easy to handle
    if(_browserHost.get())
    {
        return _browserHost->GetZoomLevel();
    }

    return 0.0;	
}

void CefBrowserHostWrapper::SendMouseWheelEvent(int x, int y, int deltaX, int deltaY, CefEventFlags modifiers)
{
    ThrowIfDisposed();

    if (_browserHost.get())
    {
        CefMouseEvent mouseEvent;
        mouseEvent.x = x;
        mouseEvent.y = y;
        mouseEvent.modifiers = (uint32)modifiers;

        _browserHost->SendMouseWheelEvent(mouseEvent, deltaX, deltaY);
    }
}

void CefBrowserHostWrapper::Invalidate(PaintElementType type)
{
    ThrowIfDisposed();

    _browserHost->Invalidate((CefBrowserHost::PaintElementType)type);
}

void CefBrowserHostWrapper::SendMouseClickEvent(int x, int y, MouseButtonType mouseButtonType, bool mouseUp, int clickCount, CefEventFlags modifiers)
{
    ThrowIfDisposed();

    CefMouseEvent mouseEvent;
    mouseEvent.x = x;
    mouseEvent.y = y;
    mouseEvent.modifiers = (uint32)modifiers;

    _browserHost->SendMouseClickEvent(mouseEvent, (CefBrowserHost::MouseButtonType) mouseButtonType, mouseUp, clickCount);
}

void CefBrowserHostWrapper::SendMouseMoveEvent(int x, int y, bool mouseLeave, CefEventFlags modifiers)
{
    ThrowIfDisposed();

    CefMouseEvent mouseEvent;
    mouseEvent.x = x;
    mouseEvent.y = y;

    mouseEvent.modifiers = (uint32)modifiers;

    _browserHost->SendMouseMoveEvent(mouseEvent, mouseLeave);
}

void CefBrowserHostWrapper::ThrowIfDisposed()
{
    if (_disposed)
    {
        throw gcnew ObjectDisposedException(gcnew String(L"This CefSharp IBrowserHost instance has been disposed!"));
    }
}