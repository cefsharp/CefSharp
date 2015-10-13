// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "ManagedCefBrowserAdapter.h"
#include "Internals/Messaging/Messages.h"
#include "Internals/CefFrameWrapper.h"
#include "Internals/CefSharpBrowserWrapper.h"

using namespace CefSharp::Internals::Messaging;

bool ManagedCefBrowserAdapter::IsDisposed::get()
{
    return _isDisposed;
}

void ManagedCefBrowserAdapter::CreateOffscreenBrowser(IntPtr windowHandle, BrowserSettings^ browserSettings, RequestContext^ requestContext, String^ address)
{
    //Create the required BitmapInfo classes before the offscreen browser is initialized  
    auto renderClientAdapter = dynamic_cast<RenderClientAdapter*>(_clientAdapter.get());  
    renderClientAdapter->CreateBitmapInfo();

    auto hwnd = static_cast<HWND>(windowHandle.ToPointer());

    CefWindowInfo window;
    auto transparent = browserSettings->OffScreenTransparentBackground.GetValueOrDefault(true);
    window.SetAsWindowless(hwnd, transparent);
    CefString addressNative = StringUtils::ToNative(address);

    if (!CefBrowserHost::CreateBrowser(window, _clientAdapter.get(), addressNative,
        *browserSettings->_browserSettings, requestContext))
    {
        throw gcnew InvalidOperationException("Failed to create offscreen browser. Call Cef.Initialize() first.");
    }
}

void ManagedCefBrowserAdapter::OnAfterBrowserCreated(int browserId)
{
    if (!_isDisposed)
    {
        //browser wrapper instance has to be set up for the BrowserProcessServiceHost
        auto browser = _clientAdapter->GetCefBrowser();
        if (browser != nullptr)
        {
            //the js callback factory needs the browser instance to pass it to the js callback implementations for messaging purposes
            _browserWrapper = gcnew CefSharpBrowserWrapper(browser);
        }

        _javascriptCallbackFactory->BrowserAdapter = gcnew WeakReference(this);

        if (CefSharpSettings::WcfEnabled)
        {
            _browserProcessServiceHost = gcnew BrowserProcessServiceHost(_javaScriptObjectRepository, Process::GetCurrentProcess()->Id, this);
            //NOTE: Attempt to solve timing issue where browser is opened and rapidly disposed. In some cases a call to Open throws
            // an exception about the process already being closed. Two relevant issues are #862 and #804.
            // Considering adding an IsDisposed check and also may have to revert to a try catch block
            if (_browserProcessServiceHost->State == CommunicationState::Created)
            {
                _browserProcessServiceHost->Open();
            }
        }
    
        if (_webBrowserInternal != nullptr)
        {
            _webBrowserInternal->OnAfterBrowserCreated();
        }
    }
}

void ManagedCefBrowserAdapter::WasResized()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->WasResized();
    }
}

void ManagedCefBrowserAdapter::WasHidden(bool hidden)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->WasHidden(hidden);
    }
}

void ManagedCefBrowserAdapter::SendFocusEvent(bool isFocused)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->SendFocusEvent(isFocused);
    }
}

void ManagedCefBrowserAdapter::SetFocus(bool isFocused)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->SetFocus(isFocused);
    }
}

bool ManagedCefBrowserAdapter::SendKeyEvent(int message, int wParam, int lParam)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser->GetHost() == nullptr)
    {
        return false;
    }

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

    browser->GetHost()->SendKeyEvent(keyEvent);

    return true;
}

//Code imported from
//https://bitbucket.org/chromiumembedded/branches-2062-cef3/src/a073e92426b3967f1fc2f1d3fd7711d809eeb03a/tests/cefclient/cefclient_osr_widget_win.cpp?at=master#cl-361
int ManagedCefBrowserAdapter::GetCefKeyboardModifiers(WPARAM wparam, LPARAM lparam)
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

void ManagedCefBrowserAdapter::CreateBrowser(BrowserSettings^ browserSettings, RequestContext^ requestContext, IntPtr sourceHandle, String^ address)
{
    HWND hwnd = static_cast<HWND>(sourceHandle.ToPointer());
    RECT rect;
    GetClientRect(hwnd, &rect);
    CefWindowInfo window;
    window.SetAsChild(hwnd, rect);
    CefString addressNative = StringUtils::ToNative(address);

    CefBrowserHost::CreateBrowser(window, _clientAdapter.get(), addressNative,
        *browserSettings->_browserSettings, requestContext);
}

void ManagedCefBrowserAdapter::Resize(int width, int height)
{
    HWND browserHwnd = _clientAdapter->GetBrowserHwnd();
    if (browserHwnd)
    {
        if (width == 0 && height == 0)
        {
            // For windowed browsers when the frame window is minimized set the
            // browser window size to 0x0 to reduce resource usage.
            SetWindowPos(browserHwnd, NULL, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOMOVE | SWP_NOACTIVATE);
        }
        else
        {
            SetWindowPos(browserHwnd, NULL, 0, 0, width, height, SWP_NOZORDER);
        }
    }
}

void ManagedCefBrowserAdapter::NotifyMoveOrResizeStarted()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->NotifyMoveOrResizeStarted();
    }
}

void ManagedCefBrowserAdapter::NotifyScreenInfoChanged()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->NotifyScreenInfoChanged();
    }
}

void ManagedCefBrowserAdapter::RegisterJsObject(String^ name, Object^ object, bool lowerCaseJavascriptNames)
{
    if (!CefSharpSettings::WcfEnabled)
    {
        throw gcnew InvalidOperationException("To enable synchronous JS bindings set WcfEnabled true in CefSettings during initialization.");
    }

    _javaScriptObjectRepository->Register(name, object, lowerCaseJavascriptNames);
}

void ManagedCefBrowserAdapter::RegisterAsyncJsObject(String^ name, Object^ object, bool lowerCaseJavascriptNames)
{
    _javaScriptObjectRepository->RegisterAsync(name, object, lowerCaseJavascriptNames);
}

CefMouseEvent ManagedCefBrowserAdapter::GetCefMouseEvent(MouseEvent^ mouseEvent)
{
    CefMouseEvent cefMouseEvent;
    cefMouseEvent.x = mouseEvent->X;
    cefMouseEvent.y = mouseEvent->Y;
    cefMouseEvent.modifiers = (uint32)mouseEvent->Modifiers;
    return cefMouseEvent;
}

void ManagedCefBrowserAdapter::OnDragTargetDragEnter(CefDragDataWrapper^ dragData, MouseEvent^ mouseEvent, DragOperationsMask allowedOperations)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        dragData->ResetFileContents(); // Recommended by documentation to reset before calling DragEnter
        browser->GetHost()->DragTargetDragEnter(*dragData->InternalDragData, GetCefMouseEvent(mouseEvent), (CefBrowserHost::DragOperationsMask) allowedOperations);
    }
}

void ManagedCefBrowserAdapter::OnDragTargetDragOver(MouseEvent^ mouseEvent, DragOperationsMask allowedOperations)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->DragTargetDragOver(GetCefMouseEvent(mouseEvent), (CefBrowserHost::DragOperationsMask) allowedOperations);
    }
}

void ManagedCefBrowserAdapter::OnDragTargetDragLeave()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->DragTargetDragLeave();
    }
}

void ManagedCefBrowserAdapter::OnDragTargetDragDrop(MouseEvent^ mouseEvent)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->DragTargetDrop(GetCefMouseEvent(mouseEvent));
    }
}

void ManagedCefBrowserAdapter::OnDragSourceEndedAt(int x, int y, DragOperationsMask op)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->DragSourceEndedAt(x, y, (CefBrowserHost::DragOperationsMask)op);
    }
}

void ManagedCefBrowserAdapter::OnDragSourceSystemDragEnded()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->DragSourceSystemDragEnded();
    }
}

/// <summary>
/// Gets the CefBrowserWrapper instance
/// </summary>
/// <returns>Gets the current instance or null</returns>
IBrowser^ ManagedCefBrowserAdapter::GetBrowser()
{
    return _browserWrapper;
}

IBrowser^ ManagedCefBrowserAdapter::GetBrowser(int browserId)
{
    return _clientAdapter->GetBrowserWrapper(browserId);
}

IJavascriptCallbackFactory^ ManagedCefBrowserAdapter::JavascriptCallbackFactory::get()
{
    return _javascriptCallbackFactory;
}

JavascriptObjectRepository^ ManagedCefBrowserAdapter::JavascriptObjectRepository::get()
{
    return _javaScriptObjectRepository;
}

MethodRunnerQueue^ ManagedCefBrowserAdapter::MethodRunnerQueue::get()
{
    return _methodRunnerQueue;
}

void ManagedCefBrowserAdapter::MethodInvocationComplete(Object^ sender, MethodInvocationCompleteArgs^ e)
{
    auto result = e->Result;
    if (result->CallbackId.HasValue)
    {
        _clientAdapter->MethodInvocationComplete(result);
    }
}

MCefRefPtr<ClientAdapter> ManagedCefBrowserAdapter::GetClientAdapter()
{
    return _clientAdapter;
}
