// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
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
        /// <param name="defaultFileName">the default file name to select in the dialog.</param>
        /// <param name="acceptTypes">a list of valid lower-cased MIME types or file extensions specified in an input element and 
        /// is used to restrict selectable files to such types.</param>
        /// <param name="result">the filename(s) the dialog returns</param>
        /// <returns>To display a custom dialog return true. To display the default dialog return false.</returns>
        bool OnFileDialog(IWebBrowser browser, CefFileDialogMode mode, string title, string defaultFileName, List<string> acceptTypes, out List<string> result);
    }
}
