// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// CefFileDialogMode (Based on cef_file_dialog_mode_t)
    /// </summary>
    [Flags]
    public enum CefFileDialogMode
    {
        /// <summary>
        /// Requires that the file exists before allowing the user to pick it.
        /// </summary>
        Open = 0,
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
        Save,
        /// <summary>
        /// General mask defining the bits used for the type values.
        /// </summary>
        TypeMask = 0xFF,
        /// <summary>
        /// Prompt to overwrite if the user selects an existing file with the Save dialog.
        /// </summary>
        OverwritePrompt = 0x01000000,
        /// <summary>
        /// Do not display read-only files.
        /// </summary>
        HideReadOnly = 0x02000000,
    }
}
