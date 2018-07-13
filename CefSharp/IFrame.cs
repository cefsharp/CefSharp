// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp
{
    /// <summary>
    /// This interface represents a CefFrame object (i.e. a HTML frame)
    /// </summary>
    public interface IFrame : IDisposable
    {
        /// <summary>
        /// True if this object is currently attached to a valid frame.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Execute undo in this frame.
        /// </summary>
        void Undo();

        /// <summary>
        /// Execute redo in this frame.
        /// </summary>
        void Redo();

        /// <summary>
        /// Execute cut in this frame.
        /// </summary>
        void Cut();

        /// <summary>
        /// Execute copy in this frame.
        /// </summary>
        void Copy();

        /// <summary>
        /// Execute paste in this frame.
        /// </summary>
        void Paste();

        /// <summary>
        /// Execute delete in this frame.
        /// </summary>
        void Delete();

        /// <summary>
        /// Execute select all in this frame.
        /// </summary>
        void SelectAll();

        /// <summary>
        /// Save this frame's HTML source to a temporary file and open it in the
        /// default text viewing application. This method can only be called from the
        /// browser process.
        /// </summary>
        void ViewSource();

        /// <summary>
        /// Retrieve this frame's HTML source as a string sent to the specified visitor.
        /// </summary>
        /// <returns>
        /// a <see cref="Task{String}"/> that when executed returns this frame's HTML source as a string.
        /// </returns>
        Task<string> GetSourceAsync();

        /// <summary>
        /// Retrieve this frame's HTML source as a string sent to the specified visitor. 
        /// Use the <see cref="GetSourceAsync"/> method for a Task based async wrapper
        /// </summary>
        /// <param name="visitor">visitor will recieve string values asynchronously</param>
        void GetSource(IStringVisitor visitor);

        /// <summary>
        /// Retrieve this frame's display text as a string sent to the specified visitor.
        /// </summary>
        /// <returns>
        /// a <see cref="Task{String}"/> that when executed returns the frame's display text as a string.
        /// </returns>
        Task<string> GetTextAsync();

        /// <summary>
        /// Retrieve this frame's display text as a string sent to the specified visitor. 
        /// Use the <see cref="GetTextAsync"/> method for a Task based async wrapper
        /// </summary>
        /// <param name="visitor">visitor will recieve string values asynchronously</param>
        void GetText(IStringVisitor visitor);

        /// <summary>
        /// Load the custom request. LoadRequest can only be used if a renderer process already exists.
        /// In newer versions initially loading about:blank no longer creates a renderer process. You
        /// can load a Data Uri initially then call this method.
        /// https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/Data_URIs
        /// </summary>
        /// <param name="request">request to be loaded in the frame</param>
        void LoadRequest(IRequest request);

        /// <summary>
        /// Load the specified url.
        /// </summary>
        /// <param name="url">url to be loaded in the frame</param>
        void LoadUrl(string url);

        /// <summary>
        /// Load the contents of html with the specified dummy url.
        /// </summary>
        /// <param name="html">html to be loaded</param>
        /// <param name="url"> should have a standard scheme (for example, http scheme) or behaviors like 
        /// link clicks and web security restrictions may not behave as expected.</param>
        void LoadStringForUrl(string html, string url);

        /// <summary>
        /// Execute a string of JavaScript code in this frame.
        /// </summary>
        /// <param name="code">Javascript to execute</param>
        /// <param name="scriptUrl">is the URL where the script in question can be found, if any.
        /// The renderer may request this URL to show the developer the source of the error.</param>
        /// <param name="startLine">is the base line number to use for error reporting.</param>
        void ExecuteJavaScriptAsync(string code, string scriptUrl = "about:blank", int startLine = 1);

        /// <summary>
        /// Execute some Javascript code in the context of this WebBrowser, and return the result of the evaluation
        /// in an Async fashion
        /// </summary>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="scriptUrl">is the URL where the script in question can be found, if any.</param>
        /// <param name="startLine">is the base line number to use for error reporting.</param>
        /// <param name="timeout">The timeout after which the Javascript code execution should be aborted.</param>
        /// <returns>A Task that can be awaited to perform the script execution</returns>
        Task<JavascriptResponse> EvaluateScriptAsync(string script, string scriptUrl = "about:blank", int startLine = 1, TimeSpan? timeout = null);

        /// <summary>
        /// Returns true if this is the main (top-level) frame.
        /// </summary>
        bool IsMain { get;  }

        /// <summary>
        /// Returns true if this is the focused frame.
        /// </summary>
        bool IsFocused { get; }

        /// <summary>
        /// Returns the name for this frame. If the frame has an assigned name (for
        /// example, set via the iframe "name" attribute) then that value will be
        /// returned. Otherwise a unique name will be constructed based on the frame
        /// parent hierarchy. The main (top-level) frame will always have an empty name
        /// value.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the globally unique identifier for this frame or &lt; 0 if the underlying frame does not yet exist.
        /// </summary>
        Int64 Identifier { get;  }

        /// <summary>
        /// Returns the parent of this frame or NULL if this is the main (top-level) frame.
        /// </summary>
        IFrame Parent { get; }

        /// <summary>
        /// Returns the URL currently loaded in this frame.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Returns the browser that this frame belongs to.
        /// </summary>
        IBrowser Browser { get; }

        /// <summary>
        /// Gets a value indicating whether the frame has been disposed of.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Create a custom request for use with <see cref="LoadRequest"/>
        /// </summary>
        /// <param name="initializePostData">Initialize the PostData object when creating this request</param>
        /// <returns>A new instance of the request</returns>
        IRequest CreateRequest(bool initializePostData = true);
    }
}
