// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefBrowserHostWrapper.h"

#include "include\cef_client.h"

#include "Cef.h"
#include "CefExtensionWrapper.h"
#include "CefTaskScheduler.h"
#include "CefDragDataWrapper.h"
#include "CefRunFileDialogCallbackAdapter.h"
#include "CefPdfPrintCallbackWrapper.h"
#include "CefNavigationEntryVisitorAdapter.h"
#include "RequestContext.h"
#include "WindowInfo.h"

void CefBrowserHostWrapper::DragTargetDragEnter(IDragData^ dragData, MouseEvent mouseEvent, DragOperationsMask allowedOperations)
{
    ThrowIfDisposed();

    auto dragDataWrapper = static_cast<CefDragDataWrapper^>(dragData);
    dragDataWrapper->ResetFileContents(); // Recommended by documentation to reset before calling DragEnter
    _browserHost->DragTargetDragEnter(static_cast<CefRefPtr<CefDragData>>(dragDataWrapper), GetCefMouseEvent(mouseEvent), (CefBrowserHost::DragOperationsMask) allowedOperations);
}

void CefBrowserHostWrapper::DragTargetDragOver(MouseEvent mouseEvent, DragOperationsMask allowedOperations)
{
    ThrowIfDisposed();

    _browserHost->DragTargetDragOver(GetCefMouseEvent(mouseEvent), (CefBrowserHost::DragOperationsMask) allowedOperations);
}

void CefBrowserHostWrapper::DragTargetDragDrop(MouseEvent mouseEvent)
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
        nativeSettings.scale_factor = settings->ScaleFactor;
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

double CefBrowserHostWrapper::GetZoomLevel()
{
    ThrowIfDisposed();

    if (CefCurrentlyOn(TID_UI))
    {

        return _browserHost->GetZoomLevel();
    }

    throw gcnew InvalidOperationException("This method can only be called directly on the CEF UI Thread. Use GetZoomLevelAsync or use Cef.UIThreadTaskFactory to marshal the call onto the CEF UI Thread.");

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

    if (windowInfo == nullptr)
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

IExtension^ CefBrowserHostWrapper::Extension::get()
{
    ThrowIfDisposed();

    auto extension = _browserHost->GetExtension();

    if (extension.get())
    {
        return gcnew CefExtensionWrapper(_browserHost->GetExtension());
    }

    return nullptr;
}

void CefBrowserHostWrapper::RunFileDialog(CefFileDialogMode mode, String^ title, String^ defaultFilePath, IList<String^>^ acceptFilters, int selectedAcceptFilter, IRunFileDialogCallback^ callback)
{
    ThrowIfDisposed();

    _browserHost->RunFileDialog((CefBrowserHost::FileDialogMode)mode,
        StringUtils::ToNative(title),
        StringUtils::ToNative(defaultFilePath),
        StringUtils::ToNative(acceptFilters),
        selectedAcceptFilter,
        new CefRunFileDialogCallbackAdapter(callback));
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
    if (_browserHost.get())
    {
        return _browserHost->GetZoomLevel();
    }

    return 0.0;
}

void CefBrowserHostWrapper::SendMouseWheelEvent(MouseEvent mouseEvent, int deltaX, int deltaY)
{
    ThrowIfDisposed();

    if (_browserHost.get())
    {
        CefMouseEvent m;
        m.x = mouseEvent.X;
        m.y = mouseEvent.Y;
        m.modifiers = (uint32)mouseEvent.Modifiers;

        _browserHost->SendMouseWheelEvent(m, deltaX, deltaY);
    }
}

void CefBrowserHostWrapper::SendTouchEvent(TouchEvent evt)
{
    ThrowIfDisposed();

    if (_browserHost.get())
    {
        CefTouchEvent e;
        e.id = evt.Id;
        e.modifiers = (uint32)evt.Modifiers;
        e.pointer_type = (cef_pointer_type_t)evt.PointerType;
        e.pressure = evt.Pressure;
        e.radius_x = evt.RadiusX;
        e.radius_y = evt.RadiusY;
        e.rotation_angle = evt.RotationAngle;
        e.type = (cef_touch_event_type_t)evt.Type;
        e.x = evt.X;
        e.y = evt.Y;

        _browserHost->SendTouchEvent(e);
    }
}

void CefBrowserHostWrapper::SetAccessibilityState(CefState accessibilityState)
{
    ThrowIfDisposed();

    _browserHost->SetAccessibilityState((cef_state_t)accessibilityState);
}

void CefBrowserHostWrapper::SetAutoResizeEnabled(bool enabled, Size minSize, Size maxSize)
{
    ThrowIfDisposed();

    _browserHost->SetAutoResizeEnabled(enabled, CefSize(minSize.Width, minSize.Height), CefSize(maxSize.Width, maxSize.Height));
}

void CefBrowserHostWrapper::Invalidate(PaintElementType type)
{
    ThrowIfDisposed();

    _browserHost->Invalidate((CefBrowserHost::PaintElementType)type);
}

bool CefBrowserHostWrapper::IsBackgroundHost::get()
{
    ThrowIfDisposed();

    return _browserHost->IsBackgroundHost();
}

void CefBrowserHostWrapper::ImeSetComposition(String^ text, cli::array<CompositionUnderline>^ underlines, Nullable<Range> replacementRange, Nullable<Range> selectionRange)
{
    ThrowIfDisposed();

    std::vector<CefCompositionUnderline> underlinesVector = std::vector<CefCompositionUnderline>();
    CefRange repRange;
    CefRange selRange;

    if (underlines != nullptr && underlines->Length > 0)
    {
        for each (CompositionUnderline underline in underlines)
        {
            auto c = CefCompositionUnderline();
            c.range = CefRange(underline.Range.From, underline.Range.To);
            c.color = underline.Color;
            c.background_color = underline.BackgroundColor;
            c.thick = (int)underline.Thick;
            underlinesVector.push_back(c);
        }
    }

    if (replacementRange.HasValue)
    {
        repRange = CefRange(replacementRange.Value.From, replacementRange.Value.To);
    }

    if (selectionRange.HasValue)
    {
        selRange = CefRange(selectionRange.Value.From, selectionRange.Value.To);
    }

    _browserHost->ImeSetComposition(StringUtils::ToNative(text), underlinesVector, repRange, selRange);
}

void CefBrowserHostWrapper::ImeCommitText(String^ text, Nullable<Range> replacementRange, int relativeCursorPos)
{
    ThrowIfDisposed();

    CefRange repRange;

    if (replacementRange.HasValue)
    {
        repRange = CefRange(replacementRange.Value.From, replacementRange.Value.To);
    }

    _browserHost->ImeCommitText(StringUtils::ToNative(text), repRange, relativeCursorPos);
}

void CefBrowserHostWrapper::ImeFinishComposingText(bool keepSelection)
{
    ThrowIfDisposed();

    _browserHost->ImeFinishComposingText(keepSelection);
}

void CefBrowserHostWrapper::ImeCancelComposition()
{
    ThrowIfDisposed();

    _browserHost->ImeCancelComposition();
}

void CefBrowserHostWrapper::SendMouseClickEvent(MouseEvent mouseEvent, MouseButtonType mouseButtonType, bool mouseUp, int clickCount)
{
    ThrowIfDisposed();

    CefMouseEvent m;
    m.x = mouseEvent.X;
    m.y = mouseEvent.Y;
    m.modifiers = (uint32)mouseEvent.Modifiers;

    _browserHost->SendMouseClickEvent(m, (CefBrowserHost::MouseButtonType) mouseButtonType, mouseUp, clickCount);
}

void CefBrowserHostWrapper::SendMouseMoveEvent(MouseEvent mouseEvent, bool mouseLeave)
{
    ThrowIfDisposed();

    CefMouseEvent m;
    m.x = mouseEvent.X;
    m.y = mouseEvent.Y;
    m.modifiers = (uint32)mouseEvent.Modifiers;

    _browserHost->SendMouseMoveEvent(m, mouseLeave);
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

NavigationEntry^ CefBrowserHostWrapper::GetVisibleNavigationEntry()
{
    ThrowIfDisposed();

    auto entry = _browserHost->GetVisibleNavigationEntry();

    return TypeConversion::FromNative(entry, true);
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

bool CefBrowserHostWrapper::IsAudioMuted::get()
{
    ThrowIfDisposed();

    ThrowIfExecutedOnNonCefUiThread();

    return _browserHost->IsAudioMuted();
}

void CefBrowserHostWrapper::SetAudioMuted(bool mute)
{
    ThrowIfDisposed();

    _browserHost->SetAudioMuted(mute);
}

IntPtr CefBrowserHostWrapper::GetOpenerWindowHandle()
{
    ThrowIfDisposed();

    return IntPtr(_browserHost->GetOpenerWindowHandle());
}

void CefBrowserHostWrapper::SendExternalBeginFrame()
{
    ThrowIfDisposed();

    _browserHost->SendExternalBeginFrame();
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

CefMouseEvent CefBrowserHostWrapper::GetCefMouseEvent(MouseEvent mouseEvent)
{
    CefMouseEvent cefMouseEvent;
    cefMouseEvent.x = mouseEvent.X;
    cefMouseEvent.y = mouseEvent.Y;
    cefMouseEvent.modifiers = (uint32)mouseEvent.Modifiers;
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
