// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.IO;

namespace CefSharp
{
    /// <summary>
    /// Used to represent drag data.
    /// </summary>
    public interface IDragData : IDisposable
    {
        /// <summary>
        /// Returns true if this object is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Return the name of the file being dragged out of the browser window.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Retrieve the list of file names that are being dragged into the browser window
        /// </summary>
        IList<string> FileNames { get; }

        /// <summary>
        /// Return the base URL that the fragment came from. This value is used for resolving relative URLs and may be empty. 
        /// </summary>
        string FragmentBaseUrl { get; set; }

        /// <summary>
        /// Return the text/html fragment that is being dragged. 
        /// </summary>
        string FragmentHtml { get; set; }

        /// <summary>
        /// Return the plain text fragment that is being dragged.
        /// </summary>
        string FragmentText { get; set; }

        /// <summary>
        /// Return the metadata, if any, associated with the link being dragged. 
        /// </summary>
        string LinkMetaData { set; get; }

        /// <summary>
        /// Return the title associated with the link being dragged.
        /// </summary>
        string LinkTitle { set; get; }

        /// <summary>
        /// Return the link URL that is being dragged. 
        /// </summary>
        string LinkUrl { set; get; }

        /// <summary>
        /// Returns true if the drag data is a file.
        /// </summary>
        bool IsFile { get; set; }

        /// <summary>
        /// Returns true if the drag data is a text or html fragment.
        /// </summary>
        bool IsFragment { get; set; }

        /// <summary>
        /// Returns true if the drag data is a link
        /// </summary>
        bool IsLink { get; set; }

        /// <summary>
        /// Add a file that is being dragged into the webview.
        /// </summary>
        /// <param name="path">File Path</param>
        /// <param name="displayName">Optional Display Name</param>
        void AddFile(string path, string displayName = null);

        /// <summary>
        /// Reset the file contents. You should do this before calling
        /// CefBrowserHost::DragTargetDragEnter as the web view does not allow us to
        /// drag in this kind of data.
        /// </summary>
        void ResetFileContents();

        /// <summary>
        /// Gets the contents of the File as a <see cref="Stream"/>
        /// For a suggested filename check the <see cref="FileName"/> property
        /// </summary>
        /// <returns>the contents of the file</returns>
        Stream GetFileContents();

        /// <summary>
        /// Gets a value indicating whether the object has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}