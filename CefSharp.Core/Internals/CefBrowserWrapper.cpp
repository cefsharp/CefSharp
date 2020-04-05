// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals\CefFrameWrapper.h"
#include "Internals\CefBrowserWrapper.h"
#include "Internals\CefBrowserHostWrapper.h"

///
// Returns the browser host object. This method can only be called in the
// browser process.
///
/*--cef()--*/
IBrowserHost^ CefBrowserWrapper::GetHost()
{
    ThrowIfDisposed();

    if (_browserHost == nullptr)
    {
        _browserHost = gcnew CefBrowserHostWrapper(_browser->GetHost());
    }

    return _browserHost;
}

///
// Returns true if the browser can navigate backwards.
///
/*--cef()--*/
bool CefBrowserWrapper::CanGoBack::get()
{
    ThrowIfDisposed();
    return _browser->CanGoBack();
}

///
// Navigate backwards.
///
/*--cef()--*/
void CefBrowserWrapper::GoBack()
{
    ThrowIfDisposed();
    _browser->GoBack();
}

///
// Returns true if the browser can navigate forwards.
///
/*--cef()--*/
bool CefBrowserWrapper::CanGoForward::get()
{
    ThrowIfDisposed();
    return _browser->CanGoForward();
}

///
// Navigate forwards.
///
/*--cef()--*/
void CefBrowserWrapper::GoForward()
{
    ThrowIfDisposed();
    _browser->GoForward();
}

///
// Returns true if the browser is currently loading.
///
/*--cef()--*/
bool CefBrowserWrapper::IsLoading::get()
{
    ThrowIfDisposed();
    return _browser->IsLoading();
}

void CefBrowserWrapper::CloseBrowser(bool forceClose)
{
    ThrowIfDisposed();
    _browser->GetHost()->CloseBrowser(forceClose);
}

///
// Reload the current page.
///
/*--cef()--*/
void CefBrowserWrapper::Reload(bool ignoreCache)
{
    ThrowIfDisposed();

    if (ignoreCache)
    {
        _browser->ReloadIgnoreCache();
    }
    else
    {
        _browser->Reload();
    }
}

///
// Stop loading the page.
///
/*--cef()--*/
void CefBrowserWrapper::StopLoad()
{
    ThrowIfDisposed();
    _browser->StopLoad();
}

///
// Returns the globally unique identifier for this browser.
///
/*--cef()--*/
int CefBrowserWrapper::Identifier::get()
{
    ThrowIfDisposed();
    return _browser->GetIdentifier();
}

///
// Returns true if this object is pointing to the same handle as |that|
// object.
///
/*--cef()--*/
bool CefBrowserWrapper::IsSame(IBrowser^ that)
{
    ThrowIfDisposed();
    return _browser->IsSame(dynamic_cast<CefBrowserWrapper^>(that)->_browser.get());
}

///
// Returns true if the window is a popup window.
///
/*--cef()--*/
bool CefBrowserWrapper::IsPopup::get()
{
    ThrowIfDisposed();
    return _browser->IsPopup();
}

///
// Returns true if a document has been loaded in the browser.
///
/*--cef()--*/
bool CefBrowserWrapper::HasDocument::get()
{
    ThrowIfDisposed();
    return _browser->HasDocument();
}

IFrame^ CefBrowserWrapper::MainFrame::get()
{
    ThrowIfDisposed();
    auto frame = _browser->GetMainFrame();
    return gcnew CefFrameWrapper(frame);
}

///
// Returns the focused frame for the browser window.
///
/*--cef()--*/
IFrame^ CefBrowserWrapper::FocusedFrame::get()
{
    ThrowIfDisposed();
    return gcnew CefFrameWrapper(_browser->GetFocusedFrame());
}

///
// Returns the frame with the specified identifier, or NULL if not found.
///
/*--cef(capi_name=get_frame_byident)--*/
IFrame^ CefBrowserWrapper::GetFrame(Int64 identifier)
{
    ThrowIfDisposed();

    auto frame = _browser->GetFrame(identifier);

    if (frame.get())
    {
        return gcnew CefFrameWrapper(frame);
    }

    return nullptr;
}

///
// Returns the frame with the specified name, or NULL if not found.
///
/*--cef(optional_param=name)--*/
IFrame^ CefBrowserWrapper::GetFrame(String^ name)
{
    ThrowIfDisposed();

    auto frame = _browser->GetFrame(StringUtils::ToNative(name));

    if (frame.get())
    {
        return gcnew CefFrameWrapper(frame);
    }

    return nullptr;
}

///
// Returns the number of frames that currently exist.
///
/*--cef()--*/
int CefBrowserWrapper::GetFrameCount()
{
    ThrowIfDisposed();
    return _browser->GetFrameCount();
}

///
// Returns the identifiers of all existing frames.
///
/*--cef(count_func=identifiers:GetFrameCount)--*/
List<Int64>^ CefBrowserWrapper::GetFrameIdentifiers()
{
    ThrowIfDisposed();

    std::vector<Int64> identifiers;
    _browser->GetFrameIdentifiers(identifiers);
    List<Int64>^ results = gcnew List<Int64>(identifiers.size());
    for (UINT i = 0; i < identifiers.size(); i++)
    {
        results->Add(identifiers[i]);
    }
    return results;
}

///
// Returns the names of all existing frames.
///
/*--cef()--*/
List<String^>^ CefBrowserWrapper::GetFrameNames()
{
    ThrowIfDisposed();

    std::vector<CefString> names;

    _browser->GetFrameNames(names);
    return StringUtils::ToClr(names);
}

MCefRefPtr<CefBrowser> CefBrowserWrapper::Browser::get()
{
    return _browser;
}
