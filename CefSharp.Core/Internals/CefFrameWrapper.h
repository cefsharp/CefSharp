// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include <include\cef_frame.h>
#include "Internals\CefRequestWrapper.h"
#include "Internals\IBrowserAdapterNative.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    ///
    // Class used to represent a frame in the browser window. When used in the
    // browser process the methods of this class may be called on any thread unless
    // otherwise indicated in the comments. When used in the render process the
    // methods of this class may only be called on the main thread.
    ///
    /*--cef(source=library)--*/
    public ref class CefFrameWrapper : IFrame
    {
    private:
        MCefRefPtr<CefFrame> _frame;
        IBrowserAdapter^ _browserAdapter;

    internal:
        CefFrameWrapper::CefFrameWrapper(CefRefPtr<CefFrame> &frame, IBrowserAdapter^ browserAdapter)
            : _frame(frame), _browserAdapter(browserAdapter)
        {
        }

    public:
        ///
        // True if this object is currently attached to a valid frame.
        ///
        /*--cef()--*/
        virtual bool IsValid()
        {
            return _frame->IsValid();
        }

        ///
        // Execute undo in this frame.
        ///
        /*--cef()--*/
        virtual void Undo()
        {
            _frame->Undo();
        }

        ///
        // Execute redo in this frame.
        ///
        /*--cef()--*/
        virtual void Redo()
        {
            _frame->Redo();
        }

        ///
        // Execute cut in this frame.
        ///
        /*--cef()--*/
        virtual void Cut()
        {
            _frame->Cut();
        }

        ///
        // Execute copy in this frame.
        ///
        /*--cef()--*/
        virtual void Copy()
        {
            _frame->Copy();
        }

        ///
        // Execute paste in this frame.
        ///
        /*--cef()--*/
        virtual void Paste()
        {
            _frame->Paste();
        }

        ///
        // Execute delete in this frame.
        ///
        /*--cef(capi_name=del)--*/
        virtual void Delete()
        {
            _frame->Delete();
        }

        ///
        // Execute select all in this frame.
        ///
        /*--cef()--*/
        virtual void SelectAll()
        {
            _frame->SelectAll();
        }

        ///
        // Save this frame's HTML source to a temporary file and open it in the
        // default text viewing application. This method can only be called from the
        // browser process.
        ///
        /*--cef()--*/
        virtual void ViewSource()
        {
            _frame->ViewSource();
        }

        ///
        // Retrieve this frame's HTML source as a string sent to the specified
        // visitor.
        ///
        /*--cef()--*/
        virtual void GetSource(IStringVisitor^ visitor)
        {
            auto stringVisitor = new StringVisitor(visitor);
            _frame->GetSource(stringVisitor);
        }

        ///
        // Retrieve this frame's display text as a string sent to the specified
        // visitor.
        ///
        /*--cef()--*/
        virtual void GetText(IStringVisitor^ visitor)
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
        virtual void LoadUrl(String^ url)
        {
            _frame->LoadURL(StringUtils::ToNative(url));
        }

        ///
        // Load the contents of |html| with the specified dummy |url|. |url|
        // should have a standard scheme (for example, http scheme) or behaviors like
        // link clicks and web security restrictions may not behave as expected.
        ///
        /*--cef()--*/
        virtual void LoadHtml(String^ html, String^ url)
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
        virtual void ExecuteJavaScriptAsync(String ^code, String^ scriptUrl, int startLine)
        {
            _frame->ExecuteJavaScript(StringUtils::ToNative(code), StringUtils::ToNative(scriptUrl), startLine);
        }


        virtual Task<JavascriptResponse^>^ EvaluateScriptAsync(String^ script, Nullable<TimeSpan> timeout)
        {
            return dynamic_cast<IBrowserAdapterNative^>(_browserAdapter)->EvaluateScriptAsync(_frame.get(), script, timeout);
        }

        ///
        // Returns true if this is the main (top-level) frame.
        ///
        /*--cef()--*/
        virtual bool IsMain()
        {
            return _frame->IsMain();
        }

        ///
        // Returns true if this is the focused frame.
        ///
        /*--cef()--*/
        virtual bool IsFocused()
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
        virtual String^ GetName()
        {
            return StringUtils::ToClr(_frame->GetName());
        }

        ///
        // Returns the globally unique identifier for this frame.
        ///
        /*--cef()--*/
        virtual System::Int64 GetIdentifier()
        {
            return _frame->GetIdentifier();
        }

        ///
        // Returns the parent of this frame or NULL if this is the main (top-level)
        // frame.
        ///
        /*--cef()--*/
        virtual IFrame^ GetParent() = IFrame::GetParent
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
        virtual String^ GetUrl()
        {
            return StringUtils::ToClr(_frame->GetURL());
        }

        // NOTE: Don't expose this yet, because it 
        // would be a HUGE breaking change.
        ///
        // Returns the browser that this frame belongs to.
        ///
        /*--cef()--*/
        //virtual IBrowser^ GetBrowser()
        //{
        //    return nullptr; // _frame->GetBrowser();
        //}

        ///
        // Get the V8 context associated with the frame. This method can only be
        // called from the render process.
        ///
        /*--cef()--*/
        virtual CefRefPtr<CefV8Context> GetV8Context()
        {
            return _frame->GetV8Context();
        }

        // NOTE: Don't include VisitDOM on purpose
        // due to the depreciated nature of accessing the DOM via CEF.
        //virtual void VisitDOM(CefRefPtr<CefDOMVisitor> visitor)
    };
}
