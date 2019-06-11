// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals\CefFrameWrapper.h"
#include "Internals\CefSharpBrowserWrapper.h"
#include "Internals\CefBrowserHostWrapper.h"

///
// Returns the browser host object. This method can only be called in the
// browser process.
///
/*--cef()--*/
IBrowserHost^ CefSharpBrowserWrapper::GetHost()
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
bool CefSharpBrowserWrapper::CanGoBack::get()
{
    ThrowIfDisposed();
    return _browser->CanGoBack();
}

///
// Navigate backwards.
///
/*--cef()--*/
void CefSharpBrowserWrapper::GoBack()
{
    ThrowIfDisposed();
    _browser->GoBack();
}

///
// Returns true if the browser can navigate forwards.
///
/*--cef()--*/
bool CefSharpBrowserWrapper::CanGoForward::get()
{
    ThrowIfDisposed();
    return _browser->CanGoForward();
}

///
// Navigate forwards.
///
/*--cef()--*/
void CefSharpBrowserWrapper::GoForward()
{
    ThrowIfDisposed();
    _browser->GoForward();
}

///
// Returns true if the browser is currently loading.
///
/*--cef()--*/
bool CefSharpBrowserWrapper::IsLoading::get()
{
    ThrowIfDisposed();
    return _browser->IsLoading();
}

void CefSharpBrowserWrapper::CloseBrowser(bool forceClose)
{
    ThrowIfDisposed();
    _browser->GetHost()->CloseBrowser(forceClose);
}

///
// Reload the current page.
///
/*--cef()--*/
void CefSharpBrowserWrapper::Reload(bool ignoreCache)
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
void CefSharpBrowserWrapper::StopLoad()
{
    ThrowIfDisposed();
    _browser->StopLoad();
}

///
// Returns the globally unique identifier for this browser.
///
/*--cef()--*/
int CefSharpBrowserWrapper::Identifier::get()
{
    ThrowIfDisposed();
    return _browser->GetIdentifier();
}

///
// Returns true if this object is pointing to the same handle as |that|
// object.
///
/*--cef()--*/
bool CefSharpBrowserWrapper::IsSame(IBrowser^ that)
{
    ThrowIfDisposed();
    return _browser->IsSame(dynamic_cast<CefSharpBrowserWrapper^>(that)->_browser.get());
}

///
// Returns true if the window is a popup window.
///
/*--cef()--*/
bool CefSharpBrowserWrapper::IsPopup::get()
{
    ThrowIfDisposed();
    return _browser->IsPopup();
}

///
// Returns true if a document has been loaded in the browser.
///
/*--cef()--*/
bool CefSharpBrowserWrapper::HasDocument::get()
{
    ThrowIfDisposed();
    return _browser->HasDocument();
}

IFrame^ CefSharpBrowserWrapper::MainFrame::get()
{
    ThrowIfDisposed();
    auto frame = _browser->GetMainFrame();
    return gcnew CefFrameWrapper(frame);
}

///
// Returns the focused frame for the browser window.
///
/*--cef()--*/
IFrame^ CefSharpBrowserWrapper::FocusedFrame::get()
{
    ThrowIfDisposed();
    return gcnew CefFrameWrapper(_browser->GetFocusedFrame());
}

///
// Returns the frame with the specified identifier, or NULL if not found.
///
/*--cef(capi_name=get_frame_byident)--*/
IFrame^ CefSharpBrowserWrapper::GetFrame(Int64 identifier)
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
IFrame^ CefSharpBrowserWrapper::GetFrame(String^ name)
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
int CefSharpBrowserWrapper::GetFrameCount()
{
    ThrowIfDisposed();
    return _browser->GetFrameCount();
}

///
// Returns the identifiers of all existing frames.
///
/*--cef(count_func=identifiers:GetFrameCount)--*/
List<Int64>^ CefSharpBrowserWrapper::GetFrameIdentifiers()
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
List<String^>^ CefSharpBrowserWrapper::GetFrameNames()
{
    ThrowIfDisposed();

    std::vector<CefString> names;

    _browser->GetFrameNames(names);
    return StringUtils::ToClr(names);
}

MCefRefPtr<CefBrowser> CefSharpBrowserWrapper::Browser::get()
{
    return _browser;
}
