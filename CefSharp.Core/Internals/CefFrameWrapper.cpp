// Copyright � 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals/CefSharpBrowserWrapper.h"
#include "Internals/CefFrameWrapper.h"

///
// True if this object is currently attached to a valid frame.
///
/*--cef()--*/
bool CefFrameWrapper::IsValid::get()
{
    ThrowIfDisposed();
    return _frame->IsValid();
}

///
// Execute undo in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Undo()
{
    ThrowIfDisposed();
    _frame->Undo();
}

///
// Execute redo in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Redo()
{
    ThrowIfDisposed();
    _frame->Redo();
}

///
// Execute cut in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Cut()
{
    ThrowIfDisposed();
    _frame->Cut();
}

///
// Execute copy in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Copy()
{
    ThrowIfDisposed();
    _frame->Copy();
}

///
// Execute paste in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Paste()
{
    ThrowIfDisposed();
    _frame->Paste();
}

///
// Execute delete in this frame.
///
/*--cef(capi_name=del)--*/
void CefFrameWrapper::Delete()
{
    ThrowIfDisposed();
    _frame->Delete();
}

///
// Execute select all in this frame.
///
/*--cef()--*/
void CefFrameWrapper::SelectAll()
{
    ThrowIfDisposed();
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
    ThrowIfDisposed();
    _frame->ViewSource();
}

///
// Retrieve this frame's HTML source as a string sent to the specified
// visitor.
///
/*--cef()--*/
Task<String^>^ CefFrameWrapper::GetSourceAsync()
{
    ThrowIfDisposed();
    auto taskStringVisitor = gcnew TaskStringVisitor();
    _frame->GetSource(new StringVisitor(taskStringVisitor));
    return taskStringVisitor->Task;
}

///
// Retrieve this frame's display text as a string sent to the specified
// visitor.
///
/*--cef()--*/
Task<String^>^ CefFrameWrapper::GetTextAsync()
{
    ThrowIfDisposed();
    auto taskStringVisitor = gcnew TaskStringVisitor();
    _frame->GetText(new StringVisitor(taskStringVisitor));
    return taskStringVisitor->Task;
}

// TODO: Do we need this?
///
// Load the request represented by the |request| object.
///
/*--cef()--*/
//virtual void LoadRequest(CefRequestWrapper^ request)
//{
//    ThrowIfDisposed();
//    _frame->LoadRequest(request->GetCefRequest().get());
//}

///
// Load the specified |url|.
///
/*--cef()--*/
void CefFrameWrapper::LoadUrl(String^ url)
{
    ThrowIfDisposed();
    _frame->LoadURL(StringUtils::ToNative(url));
}

///
// Load the contents of |html| with the specified dummy |url|. |url|
// should have a standard scheme (for example, http scheme) or behaviors like
// link clicks and web security restrictions may not behave as expected.
///
/*--cef()--*/
void CefFrameWrapper::LoadStringForUrl(String^ html, String^ url)
{
    ThrowIfDisposed();
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
void CefFrameWrapper::ExecuteJavaScriptAsync(String^ code, String^ scriptUrl, int startLine)
{
    ThrowIfDisposed();
    _frame->ExecuteJavaScript(StringUtils::ToNative(code), StringUtils::ToNative(scriptUrl), startLine);
}

Task<JavascriptResponse^>^ CefFrameWrapper::EvaluateScriptAsync(String^ script, Nullable<TimeSpan> timeout)
{
    ThrowIfDisposed();
    if (_browserAdapter == nullptr)
    {
        throw gcnew InvalidOperationException(gcnew String(L"IBrowserAdapter is NULL, EvaluateScriptAsync MUST NOT be called from the render process!"));
    }
    return _browserAdapter->EvaluateScriptAsync(_frame->GetBrowser()->GetIdentifier(), _frame->GetIdentifier(), script, timeout);
}

///
// Returns true if this is the main (top-level) frame.
///
/*--cef()--*/
bool CefFrameWrapper::IsMain::get()
{
    ThrowIfDisposed();
    return _frame->IsMain();
}

///
// Returns true if this is the focused frame.
///
/*--cef()--*/
bool CefFrameWrapper::IsFocused::get()
{
    ThrowIfDisposed();
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
String^ CefFrameWrapper::Name::get()
{
    ThrowIfDisposed();
    return StringUtils::ToClr(_frame->GetName());
}

///
// Returns the globally unique identifier for this frame.
///
/*--cef()--*/
Int64 CefFrameWrapper::Identifier::get()
{
    ThrowIfDisposed();
    return _frame->GetIdentifier();
}

///
// Returns the parent of this frame or NULL if this is the main (top-level)
// frame.
///
/*--cef()--*/
IFrame^ CefFrameWrapper::Parent::get()
{
    ThrowIfDisposed();
    if (_parentFrame != nullptr)
    {
        // Be paranoid about creating the cached IBrowser.
        lock sync(_syncRoot);

        auto parent = _frame->GetParent();
        if (parent != nullptr && _parentFrame != nullptr)
        {
            _parentFrame = gcnew CefFrameWrapper(parent, _browserAdapter);
        }
        else if (parent == nullptr)
        {
            return nullptr;
        }
    }
    else
    {
        return _parentFrame;
    }
    return nullptr;
}

///
// Returns the URL currently loaded in this frame.
///
/*--cef()--*/
String^ CefFrameWrapper::Url::get()
{
    ThrowIfDisposed();
    return StringUtils::ToClr(_frame->GetURL());
}

///
// Returns the browser that this frame belongs to.
///
/*--cef()--*/
IBrowser^ CefFrameWrapper::Browser::get()
{
    ThrowIfDisposed();
    if (_owningBrowser != nullptr)
    {
        return _owningBrowser;
    }

    // Be paranoid about creating the cached IBrowser.
    lock sync(_syncRoot);

    if (_owningBrowser != nullptr)
    {
        return _owningBrowser;
    }

    _owningBrowser = gcnew CefSharpBrowserWrapper(_frame->GetBrowser(), _browserAdapter);
    return _owningBrowser;
}

void CefFrameWrapper::ThrowIfDisposed()
{
    if (_disposed)
    {
        throw gcnew ObjectDisposedException(gcnew String(L"This CefSharp IFrame instance has been disposed!"));
    }
}
