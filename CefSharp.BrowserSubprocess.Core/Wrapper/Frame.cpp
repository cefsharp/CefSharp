// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include <msclr/lock.h>

#include "Frame.h"
#include "Browser.h"

using namespace CefSharp::BrowserSubprocess;

///
// True if this object is currently attached to a valid frame.
///
/*--cef()--*/
bool Frame::IsValid::get()
{
    return _frame->IsValid();
}

///
// Execute undo in this frame.
///
/*--cef()--*/
void Frame::Undo()
{
    _frame->Undo();
}

///
// Execute redo in this frame.
///
/*--cef()--*/
void Frame::Redo()
{
    _frame->Redo();
}

///
// Execute cut in this frame.
///
/*--cef()--*/
void Frame::Cut()
{
    _frame->Cut();
}

///
// Execute copy in this frame.
///
/*--cef()--*/
void Frame::Copy()
{
    _frame->Copy();
}

///
// Execute paste in this frame.
///
/*--cef()--*/
void Frame::Paste()
{
    _frame->Paste();
}

///
// Execute delete in this frame.
///
/*--cef(capi_name=del)--*/
void Frame::Delete()
{
    _frame->Delete();
}

///
// Execute select all in this frame.
///
/*--cef()--*/
void Frame::SelectAll()
{
    _frame->SelectAll();
}

///
// Save this frame's HTML source to a temporary file and open it in the
// default text viewing application. This method can only be called from the
// browser process.
///
/*--cef()--*/
void Frame::ViewSource()
{
    throw gcnew NotImplementedException("Browser process only");
}

///
// Retrieve this frame's HTML source as a string sent to the specified
// visitor.
///
/*--cef()--*/
Task<String^>^ Frame::GetSourceAsync()
{
    throw gcnew NotImplementedException();
}

///
// Retrieve this frame's HTML source as a string sent to the specified
// visitor.
///
/*--cef()--*/
void Frame::GetSource(IStringVisitor^ visitor)
{
    throw gcnew NotImplementedException();
}

///
// Retrieve this frame's display text as a string sent to the specified
// visitor.
///
/*--cef()--*/
Task<String^>^ Frame::GetTextAsync()
{
    throw gcnew NotImplementedException();
}

///
// Retrieve this frame's display text as a string sent to the specified
// visitor.
///
/*--cef()--*/
void Frame::GetText(IStringVisitor^ visitor)
{
    throw gcnew NotImplementedException();
}


///
// Load the request represented by the |request| object.
///
/*--cef()--*/
void Frame::LoadRequest(IRequest^ request)
{
    throw gcnew NotImplementedException();
}

///
// Load the specified |url|.
///
/*--cef()--*/
void Frame::LoadUrl(String^ url)
{
    _frame->LoadURL(StringUtils::ToNative(url));
}

///
// Execute a string of JavaScript code in this frame. The |script_url|
// parameter is the URL where the script in question can be found, if any.
// The renderer may request this URL to show the developer the source of the
// error.  The |start_line| parameter is the base line number to use for error
// reporting.
///
/*--cef(optional_param=script_url)--*/
void Frame::ExecuteJavaScriptAsync(String^ code, String^ scriptUrl, int startLine)
{
    _frame->ExecuteJavaScript(StringUtils::ToNative(code), StringUtils::ToNative(scriptUrl), startLine);
}

Task<JavascriptResponse^>^ Frame::EvaluateScriptAsync(String^ script, String^ scriptUrl, int startLine, Nullable<TimeSpan> timeout)
{
    throw gcnew NotImplementedException();
}

///
// Returns true if this is the main (top-level) frame.
///
/*--cef()--*/
bool Frame::IsMain::get()
{
    return _frame->IsMain();
}

///
// Returns true if this is the focused frame.
///
/*--cef()--*/
bool Frame::IsFocused::get()
{
    return _frame->IsFocused();
}

///
// Returns the name for this frame. If the frame has an assigned name (for
// example, set via the iframe "name" attribute) then that value will be
// returned. Otherwise a unique name will be constructed based on the frame
// parent hierarchy. The main (top-level) frame will always have an empty name
// value.
///
/*--cef()--*/
String^ Frame::Name::get()
{
    return StringUtils::ToClr(_frame->GetName());
}

///
// Returns the globally unique identifier for this frame.
///
/*--cef()--*/
Int64 Frame::Identifier::get()
{
    return _frame->GetIdentifier();
}

///
// Returns the parent of this frame or NULL if this is the main (top-level)
// frame.
///
/*--cef()--*/
IFrame^ Frame::Parent::get()
{
    if (_parentFrame != nullptr)
    {
        return _parentFrame;
    }

    // Be paranoid about creating the cached IFrame.
    msclr::lock sync(_syncRoot);

    if (_parentFrame != nullptr)
    {
        return _parentFrame;
    }

    auto parent = _frame->GetParent();

    if (parent == nullptr)
    {
        return nullptr;
    }

    _parentFrame = gcnew Frame(parent);

    return _parentFrame;
}

///
// Returns the URL currently loaded in this frame.
///
/*--cef()--*/
String^ Frame::Url::get()
{
    return StringUtils::ToClr(_frame->GetURL());
}

///
// Returns the browser that this frame belongs to.
///
/*--cef()--*/
IBrowser^ Frame::Browser::get()
{
    if (_owningBrowser != nullptr)
    {
        return _owningBrowser;
    }

    // Be paranoid about creating the cached IBrowser.
    msclr::lock sync(_syncRoot);

    if (_owningBrowser != nullptr)
    {
        return _owningBrowser;
    }

    _owningBrowser = gcnew CefSharp::BrowserSubprocess::Browser(_frame->GetBrowser());
    return _owningBrowser;
}

IRequest^ Frame::CreateRequest(bool initializePostData)
{
    throw gcnew NotImplementedException();
}

IUrlRequest^ Frame::CreateUrlRequest(IRequest^ request, IUrlRequestClient^ client)
{
    throw gcnew NotImplementedException();
}

bool Frame::IsDisposed::get()
{
    return _disposed;
}
