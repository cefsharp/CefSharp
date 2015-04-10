﻿// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle dialog events. The methods of this class will be called on the browser process UI thread.
    /// </summary>
    public interface IDialogHandler
    {
        /// <summary>
        /// Runs a file chooser dialog. 
        /// </summary>
        /// <example>
        /// To test assign something like TempFileDialogHandler (from CefSharp.Example) to DialogHandler e.g.
        /// <code>
        /// browser.DialogHandler = new TempFileDialogHandler();
        /// </code>
        /// Example URL to use for file browsing http://www.cs.tut.fi/~jkorpela/forms/file.html#example
        /// Simply click browse, the space next to the browse button should be populated with a randomly generated filename.
        /// </example>
        /// <param name="browser">the browser object</param>
        /// <param name="mode">represents the type of dialog to display</param>
        /// <param name="title">the title to be used for the dialog. It may be empty to show the default title ("Open" or "Save" 
        /// depending on the mode).</param>
        /// <param name="defaultFilePath">is the path with optional directory and/or file name component that
        /// should be initially selected in the dialog.</param>
        /// <param name="acceptFilters">are used to restrict the selectable file types and may any combination of
        /// (a) valid lower-cased MIME types (e.g. "text/*" or "image/*"),
        /// (b) individual file extensions (e.g. ".txt" or ".png"),
        /// (c) combined description and file extension delimited using "|" and ";" (e.g. "Image Types|.png;.gif;.jpg").</param>
        /// <param name="selectedAcceptFilter">is the 0-based index of the filter that should be selected by default.</param>
        /// <param name="result">the filename(s) the dialog returns</param>
        /// <returns>To display a custom dialog return true. To display the default dialog return false.</returns>
        bool OnFileDialog(IWebBrowser browser, CefFileDialogMode mode, string title, string defaultFilePath, List<string> acceptFilters, out int selectedAcceptFilter, out List<string> result);
    }
}
