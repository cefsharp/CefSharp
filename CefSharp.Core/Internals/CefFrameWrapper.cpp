// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals/CefSharpBrowserWrapper.h"
#include "Internals/CefFrameWrapper.h"

///
// True if this object is currently attached to a valid frame.
///
/*--cef()--*/
bool CefFrameWrapper::IsValid()
{
    return _frame->IsValid();
}

///
// Execute undo in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Undo()
{
    _frame->Undo();
}

///
// Execute redo in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Redo()
{
    _frame->Redo();
}

///
// Execute cut in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Cut()
{
    _frame->Cut();
}

///
// Execute copy in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Copy()
{
    _frame->Copy();
}

///
// Execute paste in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Paste()
{
    _frame->Paste();
}

///
// Execute delete in this frame.
///
/*--cef(capi_name=del)--*/
void CefFrameWrapper::Delete()
{
    _frame->Delete();
}

///
// Execute select all in this frame.
///
/*--cef()--*/
void CefFrameWrapper::SelectAll()
{
    _frame->SelectAll();
}

///
// Save this frame's HTML source to a temporary file and open it in the
// default text viewing application. This method can only be called from the
// browser process.
///
/*--cef()--*/
void CefFrameWrapper::ViewSource()
{
    _frame->ViewSource();
}

///
// Retrieve this frame's HTML source as a string sent to the specified
// visitor.
///
/*--cef()--*/
void CefFrameWrapper::GetSource(IStringVisitor^ visitor)
{
    auto stringVisitor = new StringVisitor(visitor);
    _frame->GetSource(stringVisitor);
}

///
// Retrieve this frame's display text as a string sent to the specified
// visitor.
///
/*--cef()--*/
void CefFrameWrapper::GetText(IStringVisitor^ visitor)
{
    auto stringVisitor = new StringVisitor(visitor);
    _frame->GetText(stringVisitor);
}

// TODO: Do we need this?
///
// Load the request represented by the |request| object.
///
/*--cef()--*/
//virtual void LoadRequest(CefRequestWrapper^ request)
//{
//    _frame->LoadRequest(request->GetCefRequest().get());
//}

///
// Load the specified |url|.
///
/*--cef()--*/
void CefFrameWrapper::LoadUrl(String^ url)
{
    _frame->LoadURL(StringUtils::ToNative(url));
}

///
// Load the contents of |html| with the specified dummy |url|. |url|
// should have a standard scheme (for example, http scheme) or behaviors like
// link clicks and web security restrictions may not behave as expected.
///
/*--cef()--*/
void CefFrameWrapper::LoadHtml(String^ html, String^ url)
{
    _frame->LoadString(StringUtils::ToNative(html), StringUtils::ToNative(url));
}

///
// Execute a string of JavaScript code in this frame. The |script_url|
// parameter is the URL where the script in question can be found, if any.
// The renderer may request this URL to show the developer the source of the
// error.  The |start_line| parameter is the base line number to use for error
// reporting.
///
/*--cef(optional_param=script_url)--*/
void CefFrameWrapper::ExecuteJavaScriptAsync(String ^code, String^ scriptUrl, int startLine)
{
    _frame->ExecuteJavaScript(StringUtils::ToNative(code), StringUtils::ToNative(scriptUrl), startLine);
}


Task<JavascriptResponse^>^ CefFrameWrapper::EvaluateScriptAsync(String^ script, Nullable<TimeSpan> timeout)
{
    return _browserAdapter->EvaluateScriptAsync(_frame->GetBrowser()->GetIdentifier(), _frame->GetIdentifier(), script, timeout);
}

///
// Returns true if this is the main (top-level) frame.
///
/*--cef()--*/
bool CefFrameWrapper::IsMain()
{
    return _frame->IsMain();
}

///
// Returns true if this is the focused frame.
///
/*--cef()--*/
bool CefFrameWrapper::IsFocused()
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
String^ CefFrameWrapper::GetName()
{
    return StringUtils::ToClr(_frame->GetName());
}

///
// Returns the globally unique identifier for this frame.
///
/*--cef()--*/
Int64 CefFrameWrapper::GetIdentifier()
{
    return _frame->GetIdentifier();
}

///
// Returns the parent of this frame or NULL if this is the main (top-level)
// frame.
///
/*--cef()--*/
IFrame^ CefFrameWrapper::GetParent()
{
    auto parent = _frame->GetParent();
    if (parent != nullptr)
    {
        return gcnew CefFrameWrapper(parent, _browserAdapter);
    }
    return nullptr;
}

///
// Returns the URL currently loaded in this frame.
///
/*--cef()--*/
String^ CefFrameWrapper::GetUrl()
{
    return StringUtils::ToClr(_frame->GetURL());
}

///
// Returns the browser that this frame belongs to.
///
/*--cef()--*/
IBrowser^ CefFrameWrapper::GetBrowser()
{
    return dynamic_cast<IBrowser^>(gcnew CefSharpBrowserWrapper(_frame->GetBrowser(), _browserAdapter));
}
