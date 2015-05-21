// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Cef.h"

void ManagedCefBrowserAdapter::CreateOffscreenBrowser(IntPtr windowHandle, BrowserSettings^ browserSettings, String^ address)
{
    auto hwnd = static_cast<HWND>(windowHandle.ToPointer());

    CefWindowInfo window;
    auto transparent = browserSettings->OffScreenTransparentBackground->GetValueOrDefault(true);
    window.SetAsWindowless(hwnd, transparent);
    CefString addressNative = StringUtils::ToNative(address);

    if (!CefBrowserHost::CreateBrowser(window, _clientAdapter.get(), addressNative,
        *browserSettings->_browserSettings, NULL))
    {
        throw gcnew InvalidOperationException("Failed to create offscreen browser. Call Cef.Initialize() first.");
    }
}

void ManagedCefBrowserAdapter::Close(bool forceClose)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->CloseBrowser(forceClose);
    }
}

void ManagedCefBrowserAdapter::CloseAllPopups(bool forceClose)
{
    _clientAdapter->CloseAllPopups(forceClose);
}

void ManagedCefBrowserAdapter::LoadUrl(String^ address)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetMainFrame()->LoadURL(StringUtils::ToNative(address));
    }
}

void ManagedCefBrowserAdapter::OnAfterBrowserCreated(int browserId)
{
    _browserProcessServiceHost = gcnew BrowserProcessServiceHost(_javaScriptObjectRepository, Process::GetCurrentProcess()->Id, browserId);
    //NOTE: Attempt to solve timing issue where browser is opened and rapidly disposed. In some cases a call to Open throws
    // an exception about the process already being closed. Two relevant issues are #862 and #804.
    // Considering adding an IsDisposed check and also may have to revert to a try catch block
    if (_browserProcessServiceHost->State == CommunicationState::Created)
    {
        _browserProcessServiceHost->Open();
    }

    if (_webBrowserInternal != nullptr)
    {
        _webBrowserInternal->OnInitialized();
    }
}

void ManagedCefBrowserAdapter::LoadHtml(String^ html, String^ url)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetMainFrame()->LoadString(StringUtils::ToNative(html), StringUtils::ToNative(url));
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

void ManagedCefBrowserAdapter::Invalidate(PaintElementType type)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->Invalidate((CefBrowserHost::PaintElementType)type);
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

void ManagedCefBrowserAdapter::OnMouseMove(int x, int y, bool mouseLeave, CefEventFlags modifiers)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        CefMouseEvent mouseEvent;
        mouseEvent.x = x;
        mouseEvent.y = y;

        mouseEvent.modifiers = (uint32)modifiers;

        browser->GetHost()->SendMouseMoveEvent(mouseEvent, mouseLeave);

        if (mouseLeave == true)
        {
            _webBrowserInternal->SetTooltipText(nullptr);
        }
    }
}

void ManagedCefBrowserAdapter::OnMouseButton(int x, int y, int mouseButtonType, bool mouseUp, int clickCount, CefEventFlags modifiers)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        CefMouseEvent mouseEvent;
        mouseEvent.x = x;
        mouseEvent.y = y;
        mouseEvent.modifiers = (uint32)modifiers;

        browser->GetHost()->SendMouseClickEvent(mouseEvent, (CefBrowserHost::MouseButtonType) mouseButtonType, mouseUp, clickCount);
    }
}

void ManagedCefBrowserAdapter::OnMouseWheel(int x, int y, int deltaX, int deltaY)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        CefMouseEvent mouseEvent;
        mouseEvent.x = x;
        mouseEvent.y = y;

        browser->GetHost()->SendMouseWheelEvent(mouseEvent, deltaX, deltaY);
    }
}

void ManagedCefBrowserAdapter::Stop()
{
    auto cefBrowser = _clientAdapter->GetCefBrowser();

    if (cefBrowser != nullptr)
    {
        cefBrowser->StopLoad();
    }
}

void ManagedCefBrowserAdapter::GoBack()
{
    auto cefBrowser = _clientAdapter->GetCefBrowser();

    if (cefBrowser != nullptr)
    {
        cefBrowser->GoBack();
    }
}

void ManagedCefBrowserAdapter::GoForward()
{
    auto cefBrowser = _clientAdapter->GetCefBrowser();

    if (cefBrowser != nullptr)
    {
        cefBrowser->GoForward();
    }
}

void ManagedCefBrowserAdapter::Print()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->Print();
    }
}

void ManagedCefBrowserAdapter::Find(int identifier, String^ searchText, bool forward, bool matchCase, bool findNext)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->Find(identifier, StringUtils::ToNative(searchText), forward, matchCase, findNext);
    }
}

void ManagedCefBrowserAdapter::StopFinding(bool clearSelection)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->StopFinding(clearSelection);
    }
}

void ManagedCefBrowserAdapter::Reload(bool ignoreCache)
{
    auto cefBrowser = _clientAdapter->GetCefBrowser();

    if (cefBrowser != nullptr)
    {
        if (ignoreCache)
        {
            cefBrowser->ReloadIgnoreCache();
        }
        else
        {
            cefBrowser->Reload();
        }
    }
}

void ManagedCefBrowserAdapter::ViewSource()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetMainFrame()->ViewSource();
    }
}

void ManagedCefBrowserAdapter::GetSource(IStringVisitor^ visitor)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        auto stringVisitor = new StringVisitor(visitor);
        browser->GetMainFrame()->GetSource(stringVisitor);
    }
}

void ManagedCefBrowserAdapter::GetText(IStringVisitor^ visitor)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        auto stringVisitor = new StringVisitor(visitor);
        browser->GetMainFrame()->GetText(stringVisitor);
    }
}

void ManagedCefBrowserAdapter::Cut()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetFocusedFrame()->Cut();
    }
}

void ManagedCefBrowserAdapter::Copy()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetFocusedFrame()->Copy();
    }
}

void ManagedCefBrowserAdapter::Paste()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetFocusedFrame()->Paste();
    }
}

void ManagedCefBrowserAdapter::Delete()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetFocusedFrame()->Delete();
    }
}

void ManagedCefBrowserAdapter::SelectAll()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetFocusedFrame()->SelectAll();
    }
}

void ManagedCefBrowserAdapter::Undo()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetFocusedFrame()->Undo();
    }
}

void ManagedCefBrowserAdapter::Redo()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetFocusedFrame()->Redo();
    }
}

void ManagedCefBrowserAdapter::ExecuteScriptAsync(String^ script)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetMainFrame()->ExecuteJavaScript(StringUtils::ToNative(script), "about:blank", 0);
    }
}

Task<JavascriptResponse^>^ ManagedCefBrowserAdapter::EvaluateScriptAsync(int browserId, Int64 frameId, String^ script, Nullable<TimeSpan> timeout)
{
    if (timeout.HasValue && timeout.Value.TotalMilliseconds > UInt32::MaxValue)
    {
        throw gcnew ArgumentOutOfRangeException("timeout", "Timeout greater than Maximum allowable value of " + UInt32::MaxValue);
    }

    return _browserProcessServiceHost->EvaluateScriptAsync(browserId, frameId, script, timeout);
}

Task<JavascriptResponse^>^ ManagedCefBrowserAdapter::EvaluateScriptAsync(String^ script, Nullable<TimeSpan> timeout)
{
    if (timeout.HasValue && timeout.Value.TotalMilliseconds > UInt32::MaxValue)
    {
        throw gcnew ArgumentOutOfRangeException("timeout", "Timeout greater than Maximum allowable value of " + UInt32::MaxValue);
    }

    auto browser = _clientAdapter->GetCefBrowser();

    if (_browserProcessServiceHost == nullptr || browser == nullptr)
    {
        return nullptr;
    }

    auto frame = browser->GetMainFrame();

    if (frame == nullptr)
    {
        return nullptr;
    }

    return _browserProcessServiceHost->EvaluateScriptAsync(browser->GetIdentifier(), frame->GetIdentifier(), script, timeout);
}

double ManagedCefBrowserAdapter::GetZoomLevelOnUI()
{
    CefTaskScheduler::EnsureOn(TID_UI, "ManagedCefBrowserAdapter::GetZoomLevel");

    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        auto host = browser->GetHost();
        return host->GetZoomLevel();
    }
    return 0.0;
}

Task<double>^ ManagedCefBrowserAdapter::GetZoomLevelAsync()
{
    if (CefCurrentlyOn(TID_UI))
    {
        TaskCompletionSource<double>^ taskSource = gcnew TaskCompletionSource<double>();
        taskSource->SetResult(GetZoomLevelOnUI());
        return taskSource->Task;
    }
    return Cef::UIThreadTaskFactory->StartNew(gcnew Func<double>(this, &ManagedCefBrowserAdapter::GetZoomLevelOnUI));
}

void ManagedCefBrowserAdapter::SetZoomLevel(double zoomLevel)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetHost()->SetZoomLevel(zoomLevel);
    }
}

void ManagedCefBrowserAdapter::ShowDevTools()
{
    _clientAdapter->ShowDevTools();
}

void ManagedCefBrowserAdapter::CloseDevTools()
{
    _clientAdapter->CloseDevTools();
}

void ManagedCefBrowserAdapter::CreateBrowser(BrowserSettings^ browserSettings, IntPtr sourceHandle, String^ address)
{
    HWND hwnd = static_cast<HWND>(sourceHandle.ToPointer());
    RECT rect;
    GetClientRect(hwnd, &rect);
    CefWindowInfo window;
    window.SetAsChild(hwnd, rect);
    CefString addressNative = StringUtils::ToNative(address);

    CefBrowserHost::CreateBrowser(window, _clientAdapter.get(), addressNative,
        *browserSettings->_browserSettings, NULL);
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
    _javaScriptObjectRepository->Register(name, object, lowerCaseJavascriptNames);
}

void ManagedCefBrowserAdapter::ReplaceMisspelling(String^ word)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        CefString wordNative = StringUtils::ToNative(word);
        browser->GetHost()->ReplaceMisspelling(wordNative);
    }
}

void ManagedCefBrowserAdapter::AddWordToDictionary(String^ word)
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        CefString wordNative = StringUtils::ToNative(word);
        browser->GetHost()->AddWordToDictionary(wordNative);
    }
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

///
// Returns the number of frames that currently exist.
///
/*--cef()--*/
int ManagedCefBrowserAdapter::GetFrameCount()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        return browser->GetFrameCount();
    }
    return 0;
}

///
// Returns the identifiers of all existing frames.
///
/*--cef(count_func=identifiers:GetFrameCount)--*/
List<Int64>^ ManagedCefBrowserAdapter::GetFrameIdentifiers()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        std::vector<Int64> identifiers;
        browser->GetFrameIdentifiers(identifiers);
        List<Int64>^ results = gcnew List<Int64>(identifiers.size());
        for (UINT i = 0; i < identifiers.size(); i++)
        {
            results->Add(identifiers[i]);
        }
        return results;
    }
    return nullptr;
}

///
// Returns the names of all existing frames.
///
/*--cef()--*/
List<String^>^ ManagedCefBrowserAdapter::GetFrameNames()
{
    std::vector<CefString> names;

    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        browser->GetFrameNames(names);
        return StringUtils::ToClr(names);
    }
    return nullptr;
}

///
// Returns the main (top-level) frame for the browser window.
///
IFrame^ ManagedCefBrowserAdapter::GetMainFrame()
{
    auto browser = _clientAdapter->GetCefBrowser();
    if (browser == nullptr)
    {
        return nullptr;
    }
    auto result = browser->GetMainFrame();
    if (result != nullptr)
    {
        return gcnew CefFrameWrapper(result, this);
    }
    return nullptr;
}

///
// Returns the focused frame for the browser window.
///
/*--cef()--*/
IFrame^ ManagedCefBrowserAdapter::GetFocusedFrame()
{
    auto browser = _clientAdapter->GetCefBrowser();
    if (browser == nullptr)
    {
        return nullptr;
    }
    auto result = browser->GetFocusedFrame();
    if (result != nullptr)
    {
        return gcnew CefFrameWrapper(result, this);
    }
    return nullptr;
}

///
// Returns the frame with the specified identifier, or NULL if not found.
///
/*--cef(capi_name=get_frame_byident)--*/
IFrame^ ManagedCefBrowserAdapter::GetFrame(System::Int64 identifier)
{
    auto browser = _clientAdapter->GetCefBrowser();
    if (browser == nullptr)
    {
        return nullptr;
    }
    auto result = browser->GetFrame(identifier);
    if (result != nullptr)
    {
        return gcnew CefFrameWrapper(result, this);
    }
    return nullptr;
}

///
// Returns the frame with the specified name, or NULL if not found.
///
/*--cef(optional_param=name)--*/
IFrame^ ManagedCefBrowserAdapter::GetFrame(String^ name)
{
    auto browser = _clientAdapter->GetCefBrowser();
    if (browser == nullptr)
    {
        return nullptr;
    }
    auto result = browser->GetFrame(StringUtils::ToNative(name));
    if (result != nullptr)
    {
        return gcnew CefFrameWrapper(result, this);
    }
    return nullptr;
}
