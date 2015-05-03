// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals/CefFrameWrapper.h"
#include "Internals/CefSharpBrowserWrapper.h"

///
// Returns the browser host object. This method can only be called in the
// browser process.
///
/*--cef()--*/
CefRefPtr<CefBrowserHost> CefSharpBrowserWrapper::GetHost()
{
    return _browser->GetHost();
}

///
// Returns true if the browser can navigate backwards.
///
/*--cef()--*/
bool CefSharpBrowserWrapper::CanGoBack()
{
    return _browser->CanGoBack();
}

///
// Navigate backwards.
///
/*--cef()--*/
void CefSharpBrowserWrapper::GoBack()
{
    _browser->GoBack();
}

///
// Returns true if the browser can navigate forwards.
///
/*--cef()--*/
bool CefSharpBrowserWrapper::CanGoForward()
{
    return _browser->CanGoForward();
}

///
// Navigate forwards.
///
/*--cef()--*/
void CefSharpBrowserWrapper::GoForward()
{
    _browser->GoForward();
}

///
// Returns true if the browser is currently loading.
///
/*--cef()--*/
bool CefSharpBrowserWrapper::IsLoading()
{
    return _browser->IsLoading();
}

///
// Reload the current page.
///
/*--cef()--*/
void CefSharpBrowserWrapper::Reload()
{
    _browser->Reload();
}

///
// Reload the current page ignoring any cached data.
///
/*--cef()--*/
void CefSharpBrowserWrapper::ReloadIgnoreCache()
{
    _browser->ReloadIgnoreCache();
}


///
// Stop loading the page.
///
/*--cef()--*/
void CefSharpBrowserWrapper::StopLoad()
{
    _browser->StopLoad();
}

///
// Returns the globally unique identifier for this browser.
///
/*--cef()--*/
int CefSharpBrowserWrapper::GetIdentifier()
{
    return _browser->GetIdentifier();
}

///
// Returns true if this object is pointing to the same handle as |that|
// object.
///
/*--cef()--*/
bool CefSharpBrowserWrapper::IsSame(IBrowser^ that)
{
    return _browser->IsSame(dynamic_cast<CefSharpBrowserWrapper^>(that)->_browser.get());
}

///
// Returns true if the window is a popup window.
///
/*--cef()--*/
bool CefSharpBrowserWrapper::IsPopup()
{
    return _browser->IsPopup();
}

///
// Returns true if a document has been loaded in the browser.
///
/*--cef()--*/
bool CefSharpBrowserWrapper::HasDocument()
{
    return _browser->HasDocument();
}

IFrame^ CefSharpBrowserWrapper::GetMainFrame()
{
    auto frame = _browser->GetMainFrame();
    return gcnew CefFrameWrapper(frame, _browserAdapter);
}

///
// Returns the focused frame for the browser window.
///
/*--cef()--*/
IFrame^ CefSharpBrowserWrapper::GetFocusedFrame()
{
    return gcnew CefFrameWrapper(_browser->GetFocusedFrame(), _browserAdapter);
}

///
// Returns the frame with the specified identifier, or NULL if not found.
///
/*--cef(capi_name=get_frame_byident)--*/
IFrame^ CefSharpBrowserWrapper::GetFrame(Int64 identifier)
{
    return gcnew CefFrameWrapper(_browser->GetFrame(identifier), _browserAdapter);
}

///
// Returns the frame with the specified name, or NULL if not found.
///
/*--cef(optional_param=name)--*/
IFrame^ CefSharpBrowserWrapper::GetFrame(String^ name)
{
    return gcnew CefFrameWrapper(_browser->GetFrame(StringUtils::ToNative(name)), _browserAdapter);
}

///
// Returns the number of frames that currently exist.
///
/*--cef()--*/
int CefSharpBrowserWrapper::GetFrameCount()
{
    return _browser->GetFrameCount();
}

///
// Returns the identifiers of all existing frames.
///
/*--cef(count_func=identifiers:GetFrameCount)--*/
List<Int64>^ CefSharpBrowserWrapper::GetFrameIdentifiers()
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
List<String^>^ CefSharpBrowserWrapper::GetFrameNames()
{
    std::vector<CefString> names;

    _browser->GetFrameNames(names);
    return StringUtils::ToClr(names);
}

//
// Send a message to the specified |target_process|. Returns true if the
// message was sent successfully.
///
/*--cef()--*/
bool CefSharpBrowserWrapper::SendProcessMessage(CefProcessId targetProcess, CefRefPtr<CefProcessMessage> message)
{
    return _browser->SendProcessMessage(targetProcess, message);
}

