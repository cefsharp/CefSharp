// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// FileDialog Flags
    /// </summary>
    [Flags]
    public enum CefFileDialogFlags
    {
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
