// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// CefFileDialogMode (Based on cef_file_dialog_mode_t)
    /// </summary>
    public enum CefFileDialogMode
    {
        /// <summary>
        /// Requires that the file exists before allowing the user to pick it.
        /// </summary>
        Open,
        /// <summary>
        /// Like Open, but allows picking multiple files to open.
        /// </summary>
        OpenMultiple,
        /// <summary>
        /// Like Open, but selects a folder to open.
        /// </summary>
        OpenFolder,
        /// <summary>
        /// Allows picking a nonexistent file, and prompts to overwrite if the file already exists.
        /// </summary>
        Save
    }
}
