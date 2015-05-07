// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp
{
    /// <summary>
    /// This interface represents a CefFrame object (i.e. a HTML frame)
    /// </summary>
    public interface IFrame
    {
        bool IsValid { get; }

        ///
        // Execute undo in this frame.
        ///
        /*--cef()--*/
        void Undo();

        ///
        // Execute redo in this frame.
        ///
        /*--cef()--*/
        void Redo();

        ///
        // Execute cut in this frame.
        ///
        /*--cef()--*/
        void Cut();

        ///
        // Execute copy in this frame.
        ///
        /*--cef()--*/
        void Copy();

        ///
        // Execute paste in this frame.
        ///
        /*--cef()--*/
        void Paste();

        ///
        // Execute delete in this frame.
        ///
        /*--cef(capi_name=del)--*/
        void Delete();

        ///
        // Execute select all in this frame.
        ///
        /*--cef()--*/
        void SelectAll();

        ///
        // Save this frame's HTML source to a temporary file and open it in the
        // default text viewing application. This method can only be called from the
        // browser process.
        ///
        /*--cef()--*/
        void ViewSource();

        ///
        // Retrieve this frame's HTML source as a string sent to the specified
        // visitor.
        ///
        /*--cef()--*/
        Task<string> GetSourceAsync();

        ///
        // Retrieve this frame's display text as a string sent to the specified
        // visitor.
        ///
        /*--cef()--*/
        Task<string> GetTextAsync();

        // TODO: Expose a public constructor to CefRequestWrapper maybe?
        ///
        // Load the request represented by the |request| object.
        ///
        /*--cef()--*/
        //virtual void LoadRequest(CefRequestWrapper^ request) = 0;

        ///
        // Load the specified |url|.
        ///
        /*--cef()--*/
        void LoadUrl(String url);

        ///
        // Load the contents of |html| with the specified dummy |url|. |url|
        // should have a standard scheme (for example, http scheme) or behaviors like
        // link clicks and web security restrictions may not behave as expected.
        ///
        /*--cef()--*/
        void LoadHtml(String html, String url);

        ///
        // Execute a string of JavaScript code in this frame. The |script_url|
        // parameter is the URL where the script in question can be found, if any.
        // The renderer may request this URL to show the developer the source of the
        // error.  The |start_line| parameter is the base line number to use for error
        // reporting.
        ///
        /*--cef(optional_param=script_url)--*/
        void ExecuteJavaScriptAsync(string code, string scriptUrl, int startLine);

        void ExecuteJavaScriptAsync(string code);

        Task<JavascriptResponse> EvaluateScriptAsync(string script, TimeSpan? timeout);

        ///
        // Returns true if this is the main (top-level) frame.
        ///
        /*--cef()--*/
        bool IsMain { get;  }

        ///
        // Returns true if this is the focused frame.
        ///
        /*--cef()--*/
        bool IsFocused { get; }


        ///
        // Returns the name for this frame. If the frame has an assigned name (for
        // example, set via the iframe "name" attribute) then that value will be
        // returned. Otherwise a unique name will be constructed based on the frame
        // parent hierarchy. The main (top-level) frame will always have an empty name
        // value.
        ///
        /*--cef()--*/
        string Name { get; }

        ///
        // Returns the globally unique identifier for this frame.
        ///
        /*--cef()--*/
        Int64 Identifier { get;  }

        ///
        // Returns the parent of this frame or NULL if this is the main (top-level)
        // frame.
        ///
        /*--cef()--*/
        IFrame Parent { get; }

        ///
        // Returns the URL currently loaded in this frame.
        ///
        /*--cef()--*/
        string Url { get; }

        ///
        // Returns the browser that this frame belongs to.
        ///
        /*--cef()--*/
        IBrowser Browser { get; }
    }
}
