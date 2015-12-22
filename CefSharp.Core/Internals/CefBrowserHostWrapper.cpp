// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "include\cef_client.h"

#include "CefBrowserHostWrapper.h"
#include "CefPdfPrintCallbackWrapper.h"
#include "WindowInfo.h"
#include "CefTaskScheduler.h"
#include "Cef.h"
#include "RequestContext.h"

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

    auto printToPdfTask = gcnew TaskPrintToPdf();
    _browserHost->PrintToPDF(StringUtils::ToNative(path), nativeSettings, new CefPdfPrintCallbackWrapper(printToPdfTask));
    return printToPdfTask->Task;
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
    nativeKeyEvent.type = (cef_key_event_type_t)keyEvent.Type;
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