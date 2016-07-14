// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "include\cef_client.h"

#include "CefBrowserHostWrapper.h"
#include "CefDragDataWrapper.h"
#include "CefPdfPrintCallbackWrapper.h"
#include "WindowInfo.h"
#include "CefTaskScheduler.h"
#include "Cef.h"
#include "RequestContext.h"
#include "CefNavigationEntryVisitorAdapter.h"

void CefBrowserHostWrapper::DragTargetDragEnter(IDragData^ dragData, MouseEvent^ mouseEvent, DragOperationsMask allowedOperations)
{
    ThrowIfDisposed();

    auto dragDataWrapper = static_cast<CefDragDataWrapper^>(dragData);
    dragDataWrapper->ResetFileContents(); // Recommended by documentation to reset before calling DragEnter
    _browserHost->DragTargetDragEnter(dragDataWrapper, GetCefMouseEvent(mouseEvent), (CefBrowserHost::DragOperationsMask) allowedOperations);
}

void CefBrowserHostWrapper::DragTargetDragOver(MouseEvent^ mouseEvent, DragOperationsMask allowedOperations)
{
    ThrowIfDisposed();

    _browserHost->DragTargetDragOver(GetCefMouseEvent(mouseEvent), (CefBrowserHost::DragOperationsMask) allowedOperations);
}

void CefBrowserHostWrapper::DragTargetDragDrop(MouseEvent^ mouseEvent)
{
    ThrowIfDisposed();

    _browserHost->DragTargetDrop(GetCefMouseEvent(mouseEvent));
}

void CefBrowserHostWrapper::DragSourceEndedAt(int x, int y, DragOperationsMask op)
{
    ThrowIfDisposed();

    _browserHost->DragSourceEndedAt(x, y, (CefBrowserHost::DragOperationsMask)op);
}

void CefBrowserHostWrapper::DragTargetDragLeave()
{
    ThrowIfDisposed();

    _browserHost->DragTargetDragLeave();
}

void CefBrowserHostWrapper::DragSourceSystemDragEnded()
{
    ThrowIfDisposed();

    _browserHost->DragSourceSystemDragEnded();
}

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

Task<bool>^ CefBrowserHostWrapper::PrintToPdfAsync(String^ path, PdfPrintSettings^ settings)
{
    ThrowIfDisposed();

    auto printToPdfTask = gcnew TaskPrintToPdfCallback();
    PrintToPdf(path, settings, printToPdfTask);
    return printToPdfTask->Task;
}

void CefBrowserHostWrapper::PrintToPdf(String^ path, PdfPrintSettings^ settings, IPrintToPdfCallback^ callback)
{
    ThrowIfDisposed();

    CefPdfPrintSettings nativeSettings;
    if (settings != nullptr)
    {
        StringUtils::AssignNativeFromClr(nativeSettings.header_footer_title, settings->HeaderFooterTitle);
        StringUtils::AssignNativeFromClr(nativeSettings.header_footer_url, settings->HeaderFooterUrl);
        nativeSettings.backgrounds_enabled = settings->BackgroundsEnabled ? 1 : 0;
        nativeSettings.header_footer_enabled = settings->HeaderFooterEnabled ? 1 : 0;
        nativeSettings.landscape = settings->Landscape ? 1 : 0;
        nativeSettings.selection_only = settings->SelectionOnly ? 1 : 0;
        nativeSettings.margin_bottom = settings->MarginBottom;
        nativeSettings.margin_top = settings->MarginTop;
        nativeSettings.margin_left = settings->MarginLeft;
        nativeSettings.margin_right = settings->MarginRight;
        nativeSettings.page_height = settings->PageHeight;
        nativeSettings.page_width = settings->PageWidth;
        nativeSettings.margin_type = static_cast<cef_pdf_print_margin_type_t>(settings->MarginType);
    }

    _browserHost->PrintToPDF(StringUtils::ToNative(path), nativeSettings, new CefPdfPrintCallbackWrapper(callback));
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
        auto taskSource = gcnew TaskCompletionSource<double>();

        CefSharp::Internals::TaskExtensions::TrySetResultAsync<double>(taskSource, GetZoomLevelOnUI());
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

void CefBrowserHostWrapper::ShowDevTools(IWindowInfo^ windowInfo, int inspectElementAtX, int inspectElementAtY)
{
    ThrowIfDisposed();

    CefBrowserSettings settings;
    CefWindowInfo nativeWindowInfo;

    if(windowInfo == nullptr)
    {
        nativeWindowInfo.SetAsPopup(_browserHost->GetWindowHandle(), "DevTools");
    }
    else
    {
        auto cefWindowInfoWrapper = static_cast<WindowInfo^>(windowInfo);

        nativeWindowInfo = *cefWindowInfoWrapper->GetWindowInfo();
    }

    _browserHost->ShowDevTools(nativeWindowInfo, _browserHost->GetClient(), settings, CefPoint(inspectElementAtX, inspectElementAtY));
}

void CefBrowserHostWrapper::CloseDevTools()
{
    ThrowIfDisposed();

    _browserHost->CloseDevTools();
}

bool CefBrowserHostWrapper::HasDevTools::get()
{
    ThrowIfDisposed();

    return _browserHost->HasDevTools();
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
    ThrowIfDisposed();

    CefKeyEvent nativeKeyEvent;
    nativeKeyEvent.focus_on_editable_field = keyEvent.FocusOnEditableField == 1;
    nativeKeyEvent.is_system_key = keyEvent.IsSystemKey == 1;
    nativeKeyEvent.modifiers = (uint32)keyEvent.Modifiers;
    nativeKeyEvent.type = (cef_key_event_type_t)keyEvent.Type;
    nativeKeyEvent.native_key_code = keyEvent.NativeKeyCode;
    nativeKeyEvent.windows_key_code = keyEvent.WindowsKeyCode;
        
    _browserHost->SendKeyEvent(nativeKeyEvent);
}

void CefBrowserHostWrapper::SendKeyEvent(int message, int wParam, int lParam)
{
    ThrowIfDisposed();

    CefKeyEvent keyEvent;
    keyEvent.windows_key_code = wParam;
    keyEvent.native_key_code = lParam;
    keyEvent.is_system_key = message == WM_SYSCHAR ||
        message == WM_SYSKEYDOWN ||
        message == WM_SYSKEYUP;

    if (message == WM_KEYDOWN || message == WM_SYSKEYDOWN)
    {
        keyEvent.type = KEYEVENT_RAWKEYDOWN;
    }
    else if (message == WM_KEYUP || message == WM_SYSKEYUP)
    {
        keyEvent.type = KEYEVENT_KEYUP;
    }
    else
    {
        keyEvent.type = KEYEVENT_CHAR;
    }
    keyEvent.modifiers = GetCefKeyboardModifiers(wParam, lParam);

    _browserHost->SendKeyEvent(keyEvent);
}

double CefBrowserHostWrapper::GetZoomLevelOnUI()
{
    if (_disposed)
    {
        return 0.0;
    }

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

void CefBrowserHostWrapper::WasResized()
{
    ThrowIfDisposed();

    _browserHost->WasResized();
}

void CefBrowserHostWrapper::WasHidden(bool hidden)
{
    ThrowIfDisposed();

    _browserHost->WasHidden(hidden);
}

void CefBrowserHostWrapper::GetNavigationEntries(INavigationEntryVisitor^ visitor, bool currentOnly)
{
    ThrowIfDisposed();

    auto navEntryVisitor = new CefNavigationEntryVisitorAdapter(visitor);

    _browserHost->GetNavigationEntries(navEntryVisitor, currentOnly);
}

void CefBrowserHostWrapper::NotifyMoveOrResizeStarted()
{
    ThrowIfDisposed();

    _browserHost->NotifyMoveOrResizeStarted();
}

void CefBrowserHostWrapper::NotifyScreenInfoChanged()
{
    ThrowIfDisposed();

    _browserHost->NotifyScreenInfoChanged();
}

int CefBrowserHostWrapper::WindowlessFrameRate::get()
{
    ThrowIfDisposed();

    return _browserHost->GetWindowlessFrameRate();
}

void CefBrowserHostWrapper::WindowlessFrameRate::set(int val)
{
    ThrowIfDisposed();

    _browserHost->SetWindowlessFrameRate(val);
}

bool CefBrowserHostWrapper::MouseCursorChangeDisabled::get()
{
    ThrowIfDisposed();

    return _browserHost->IsMouseCursorChangeDisabled();
}

void CefBrowserHostWrapper::MouseCursorChangeDisabled::set(bool val)
{
    ThrowIfDisposed();

    _browserHost->SetMouseCursorChangeDisabled(val);
}

bool CefBrowserHostWrapper::WindowRenderingDisabled::get()
{
    ThrowIfDisposed();

    return _browserHost->IsWindowRenderingDisabled();
}

IntPtr CefBrowserHostWrapper::GetOpenerWindowHandle()
{
    ThrowIfDisposed();

    return IntPtr(_browserHost->GetOpenerWindowHandle());
}

void CefBrowserHostWrapper::SendCaptureLostEvent()
{
    ThrowIfDisposed();

    _browserHost->SendCaptureLostEvent();
}


IRequestContext^ CefBrowserHostWrapper::RequestContext::get()
{
    ThrowIfDisposed();

    return gcnew CefSharp::RequestContext(_browserHost->GetRequestContext());
}

CefMouseEvent CefBrowserHostWrapper::GetCefMouseEvent(MouseEvent^ mouseEvent)
{
    CefMouseEvent cefMouseEvent;
    cefMouseEvent.x = mouseEvent->X;
    cefMouseEvent.y = mouseEvent->Y;
    cefMouseEvent.modifiers = (uint32)mouseEvent->Modifiers;
    return cefMouseEvent;
}

//Code imported from
//https://bitbucket.org/chromiumembedded/branches-2062-cef3/src/a073e92426b3967f1fc2f1d3fd7711d809eeb03a/tests/cefclient/cefclient_osr_widget_win.cpp?at=master#cl-361
int CefBrowserHostWrapper::GetCefKeyboardModifiers(WPARAM wparam, LPARAM lparam)
{
    int modifiers = 0;
    if (IsKeyDown(VK_SHIFT))
        modifiers |= EVENTFLAG_SHIFT_DOWN;
    if (IsKeyDown(VK_CONTROL))
        modifiers |= EVENTFLAG_CONTROL_DOWN;
    if (IsKeyDown(VK_MENU))
        modifiers |= EVENTFLAG_ALT_DOWN;

    // Low bit set from GetKeyState indicates "toggled".
    if (::GetKeyState(VK_NUMLOCK) & 1)
        modifiers |= EVENTFLAG_NUM_LOCK_ON;
    if (::GetKeyState(VK_CAPITAL) & 1)
        modifiers |= EVENTFLAG_CAPS_LOCK_ON;

    switch (wparam)
    {
    case VK_RETURN:
        if ((lparam >> 16) & KF_EXTENDED)
            modifiers |= EVENTFLAG_IS_KEY_PAD;
        break;
    case VK_INSERT:
    case VK_DELETE:
    case VK_HOME:
    case VK_END:
    case VK_PRIOR:
    case VK_NEXT:
    case VK_UP:
    case VK_DOWN:
    case VK_LEFT:
    case VK_RIGHT:
        if (!((lparam >> 16) & KF_EXTENDED))
            modifiers |= EVENTFLAG_IS_KEY_PAD;
        break;
    case VK_NUMLOCK:
    case VK_NUMPAD0:
    case VK_NUMPAD1:
    case VK_NUMPAD2:
    case VK_NUMPAD3:
    case VK_NUMPAD4:
    case VK_NUMPAD5:
    case VK_NUMPAD6:
    case VK_NUMPAD7:
    case VK_NUMPAD8:
    case VK_NUMPAD9:
    case VK_DIVIDE:
    case VK_MULTIPLY:
    case VK_SUBTRACT:
    case VK_ADD:
    case VK_DECIMAL:
    case VK_CLEAR:
        modifiers |= EVENTFLAG_IS_KEY_PAD;
        break;
    case VK_SHIFT:
        if (IsKeyDown(VK_LSHIFT))
            modifiers |= EVENTFLAG_IS_LEFT;
        else if (IsKeyDown(VK_RSHIFT))
            modifiers |= EVENTFLAG_IS_RIGHT;
        break;
    case VK_CONTROL:
        if (IsKeyDown(VK_LCONTROL))
            modifiers |= EVENTFLAG_IS_LEFT;
        else if (IsKeyDown(VK_RCONTROL))
            modifiers |= EVENTFLAG_IS_RIGHT;
        break;
    case VK_MENU:
        if (IsKeyDown(VK_LMENU))
            modifiers |= EVENTFLAG_IS_LEFT;
        else if (IsKeyDown(VK_RMENU))
            modifiers |= EVENTFLAG_IS_RIGHT;
        break;
    case VK_LWIN:
        modifiers |= EVENTFLAG_IS_LEFT;
        break;
    case VK_RWIN:
        modifiers |= EVENTFLAG_IS_RIGHT;
        break;
    }
    return modifiers;
}