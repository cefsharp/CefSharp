// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.IO;
using CefSharp.Structs;

namespace CefSharp
{
    /// <summary>
    /// Used to represent drag data.
    /// </summary>
    public interface IDragData : IDisposable
    {
        /// <summary>
        /// Gets a copy of the current drag data
        /// </summary>
        /// <returns>a clone of the current object</returns>
        IDragData Clone();

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
        /// Returns true if an image representation of drag data is available.
        /// </summary>
        bool HasImage { get; }

        /// <summary>
        /// Get the image representation of drag data.
        /// May return NULL if no image representation is available.
        /// </summary>
        IImage Image { get; }

        /// <summary>
        /// Get the image hotspot (drag start location relative to image dimensions).
        /// </summary>
        Point ImageHotspot { get; }

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
        /// Write the contents of the file being dragged out of the web view into the provided <see cref="Stream"/>
        /// For a suggested filename check the <see cref="FileName"/> property
        /// </summary>
        /// <param name="stream">Stream data is to be written to. If null this method will return the
        /// size of the file contents in bytes.</param>
        /// <returns>Returns the number of bytes written to the stream</returns>
        Int64 GetFileContents(Stream stream);

        /// <summary>
        /// Gets a value indicating whether the object has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
