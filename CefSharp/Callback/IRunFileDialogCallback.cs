// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp.Callback
{
    /// <summary>
    /// Callback interface for IBrowserHost.RunFileDialog.
    /// The methods of this class will be called on the CEF UI thread.
    /// </summary>
    public interface IRunFileDialogCallback
    {
        /// <summary>
        /// Called asynchronously after the file dialog is dismissed.
        /// </summary>
        /// <param name="selectedAcceptFilter">is the 0-based index of the value selected from the accept filters array passed to IBrowserHost.RunFileDialog</param>
        /// <param name="filePaths">will be a single value or a list of values depending on the dialog mode. If the selection was cancelled filePaths will be empty</param>
        void OnFileDialogDismissed(int selectedAcceptFilter, IList<string> filePaths);
    }
}
