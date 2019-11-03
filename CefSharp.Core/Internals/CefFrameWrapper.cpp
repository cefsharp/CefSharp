// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include <msclr/lock.h>

#include "Internals\CefSharpBrowserWrapper.h"
#include "Internals\CefRequestWrapper.h"
#include "Internals\CefFrameWrapper.h"
#include "Internals\StringVisitor.h"
#include "Internals\ClientAdapter.h"
#include "Internals\Serialization\Primitives.h"
#include "Internals\Messaging\Messages.h"
#include "Internals\CefURLRequestWrapper.h"
#include "Internals\CefURLRequestClientAdapter.h" 

using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;

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
    ThrowIfFrameInvalid();

    _frame->Undo();
}

///
// Execute redo in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Redo()
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    _frame->Redo();
}

///
// Execute cut in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Cut()
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    _frame->Cut();
}

///
// Execute copy in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Copy()
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    _frame->Copy();
}

///
// Execute paste in this frame.
///
/*--cef()--*/
void CefFrameWrapper::Paste()
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    _frame->Paste();
}

///
// Execute delete in this frame.
///
/*--cef(capi_name=del)--*/
void CefFrameWrapper::Delete()
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    _frame->Delete();
}

///
// Execute select all in this frame.
///
/*--cef()--*/
void CefFrameWrapper::SelectAll()
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

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
    ThrowIfFrameInvalid();

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
    ThrowIfFrameInvalid();

    auto taskStringVisitor = gcnew TaskStringVisitor();
    _frame->GetSource(new StringVisitor(taskStringVisitor));
    return taskStringVisitor->Task;
}

///
// Retrieve this frame's HTML source as a string sent to the specified
// visitor.
///
/*--cef()--*/
void CefFrameWrapper::GetSource(IStringVisitor^ visitor)
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    _frame->GetSource(new StringVisitor(visitor));
}

///
// Retrieve this frame's display text as a string sent to the specified
// visitor.
///
/*--cef()--*/
Task<String^>^ CefFrameWrapper::GetTextAsync()
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    auto taskStringVisitor = gcnew TaskStringVisitor();
    _frame->GetText(new StringVisitor(taskStringVisitor));
    return taskStringVisitor->Task;
}

///
// Retrieve this frame's display text as a string sent to the specified
// visitor.
///
/*--cef()--*/
void CefFrameWrapper::GetText(IStringVisitor^ visitor)
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    _frame->GetText(new StringVisitor(visitor));
}


///
// Load the request represented by the |request| object.
///
/*--cef()--*/
void CefFrameWrapper::LoadRequest(IRequest^ request)
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    auto requestWrapper = (CefRequestWrapper^)request;
    _frame->LoadRequest(requestWrapper);
}

///
// Load the specified |url|.
///
/*--cef()--*/
void CefFrameWrapper::LoadUrl(String^ url)
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

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
void CefFrameWrapper::ExecuteJavaScriptAsync(String^ code, String^ scriptUrl, int startLine)
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    _frame->ExecuteJavaScript(StringUtils::ToNative(code), StringUtils::ToNative(scriptUrl), startLine);
}

Task<JavascriptResponse^>^ CefFrameWrapper::EvaluateScriptAsync(String^ script, String^ scriptUrl, int startLine, Nullable<TimeSpan> timeout)
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    auto browser = _frame->GetBrowser();
    auto host = browser->GetHost();

    //If we're unable to get the underlying browser/browserhost then return null
    if (!browser.get() || !host.get())
    {
        return nullptr;
    }

    auto client = static_cast<ClientAdapter*>(host->GetClient().get());

    auto pendingTaskRepository = client->GetPendingTaskRepository();

    //create a new taskcompletionsource
    auto idAndComplectionSource = pendingTaskRepository->CreatePendingTask(timeout);

    auto message = CefProcessMessage::Create(kEvaluateJavascriptRequest);
    auto argList = message->GetArgumentList();
    SetInt64(argList, 0, idAndComplectionSource.Key);
    argList->SetString(1, StringUtils::ToNative(script));
    argList->SetString(2, StringUtils::ToNative(scriptUrl));
    argList->SetInt(3, startLine);

    _frame->SendProcessMessage(CefProcessId::PID_RENDERER, message);

    return idAndComplectionSource.Value->Task;
}

///
// Returns true if this is the main (top-level) frame.
///
/*--cef()--*/
bool CefFrameWrapper::IsMain::get()
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    return _frame->IsMain();
}

///
// Returns true if this is the focused frame.
///
/*--cef()--*/
bool CefFrameWrapper::IsFocused::get()
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

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
    ThrowIfFrameInvalid();

    return StringUtils::ToClr(_frame->GetName());
}

///
// Returns the globally unique identifier for this frame.
///
/*--cef()--*/
Int64 CefFrameWrapper::Identifier::get()
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

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
    ThrowIfFrameInvalid();

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

    _parentFrame = gcnew CefFrameWrapper(parent);

    return _parentFrame;
}

///
// Returns the URL currently loaded in this frame.
///
/*--cef()--*/
String^ CefFrameWrapper::Url::get()
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

    return StringUtils::ToClr(_frame->GetURL());
}

///
// Returns the browser that this frame belongs to.
///
/*--cef()--*/
IBrowser^ CefFrameWrapper::Browser::get()
{
    ThrowIfDisposed();
    ThrowIfFrameInvalid();

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

    _owningBrowser = gcnew CefSharpBrowserWrapper(_frame->GetBrowser());
    return _owningBrowser;
}

IRequest^ CefFrameWrapper::CreateRequest(bool initializePostData)
{
    auto request = CefRequest::Create();

    if (initializePostData)
    {
        request->SetPostData(CefPostData::Create());
    }

    return gcnew CefRequestWrapper(request);
}

IUrlRequest^ CefFrameWrapper::CreateUrlRequest(IRequest^ request, IUrlRequestClient^ client)
{
    ThrowIfDisposed();

    if (request == nullptr)
    {
        throw gcnew ArgumentNullException("request");
    }

    if (client == nullptr)
    {
        throw gcnew ArgumentNullException("client");
    }

    auto urlRequest = _frame->CreateURLRequest(
        (CefRequestWrapper^)request,
        new CefUrlRequestClientAdapter(client));

    return gcnew CefUrlRequestWrapper(urlRequest);
}

void CefFrameWrapper::ThrowIfFrameInvalid()
{
    if (_frame->IsValid() == false)
    {
        throw gcnew Exception(L"The underlying frame is no longer valid - please check the IsValid property before calling!");
    }
}
