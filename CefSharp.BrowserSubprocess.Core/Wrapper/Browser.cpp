// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Frame.h"
#include "Browser.h"

using namespace CefSharp::BrowserSubprocess;

///
// Returns the browser host object. This method can only be called in the
// browser process.
///
/*--cef()--*/
IBrowserHost^ Browser::GetHost()
{
    throw gcnew NotImplementedException("Browser process only");
}

///
// Returns true if the browser can navigate backwards.
///
/*--cef()--*/
bool Browser::CanGoBack::get()
{
    return _browser->CanGoBack();
}

///
// Navigate backwards.
///
/*--cef()--*/
void Browser::GoBack()
{
    _browser->GoBack();
}

///
// Returns true if the browser can navigate forwards.
///
/*--cef()--*/
bool Browser::CanGoForward::get()
{
    return _browser->CanGoForward();
}

///
// Navigate forwards.
///
/*--cef()--*/
void Browser::GoForward()
{
    _browser->GoForward();
}

///
// Returns true if the browser is currently loading.
///
/*--cef()--*/
bool Browser::IsLoading::get()
{
    return _browser->IsLoading();
}

void Browser::CloseBrowser(bool forceClose)
{
    throw gcnew NotImplementedException("Browser process only");
}

///
// Reload the current page.
///
/*--cef()--*/
void Browser::Reload(bool ignoreCache)
{
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
void Browser::StopLoad()
{
    _browser->StopLoad();
}

///
// Returns the globally unique identifier for this browser.
///
/*--cef()--*/
int Browser::Identifier::get()
{
    return _browser->GetIdentifier();
}

///
// Returns true if this object is pointing to the same handle as |that|
// object.
///
/*--cef()--*/
bool Browser::IsSame(IBrowser^ that)
{
    return _browser->IsSame(dynamic_cast<Browser^>(that)->_browser.get());
}

///
// Returns true if the window is a popup window.
///
/*--cef()--*/
bool Browser::IsPopup::get()
{
    return _browser->IsPopup();
}

///
// Returns true if a document has been loaded in the browser.
///
/*--cef()--*/
bool Browser::HasDocument::get()
{
    return _browser->HasDocument();
}

IFrame^ Browser::MainFrame::get()
{
    auto frame = _browser->GetMainFrame();
    return gcnew Frame(frame);
}

///
// Returns the focused frame for the browser window.
///
/*--cef()--*/
IFrame^ Browser::FocusedFrame::get()
{
    return gcnew Frame(_browser->GetFocusedFrame());
}

///
// Returns the frame with the specified identifier, or NULL if not found.
///
/*--cef(capi_name=get_frame_byident)--*/
IFrame^ Browser::GetFrame(Int64 identifier)
{
    auto frame = _browser->GetFrame(identifier);

    if (frame.get())
    {
        return gcnew Frame(frame);
    }

    return nullptr;
}

///
// Returns the frame with the specified name, or NULL if not found.
///
/*--cef(optional_param=name)--*/
IFrame^ Browser::GetFrame(String^ name)
{
    auto frame = _browser->GetFrame(StringUtils::ToNative(name));

    if (frame.get())
    {
        return gcnew Frame(frame);
    }

    return nullptr;
}

///
// Returns the number of frames that currently exist.
///
/*--cef()--*/
int Browser::GetFrameCount()
{
    return _browser->GetFrameCount();
}

///
// Returns the identifiers of all existing frames.
///
/*--cef(count_func=identifiers:GetFrameCount)--*/
List<Int64>^ Browser::GetFrameIdentifiers()
{
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
List<String^>^ Browser::GetFrameNames()
{
    std::vector<CefString> names;

    _browser->GetFrameNames(names);
    return StringUtils::ToClr(names);
}

bool Browser::IsDisposed::get()
{
    return _disposed;
}
