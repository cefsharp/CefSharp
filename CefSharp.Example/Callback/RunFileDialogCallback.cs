// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using CefSharp.Callback;

namespace CefSharp.Example.Callback
{
    public class RunFileDialogCallback : IRunFileDialogCallback
    {
        void IRunFileDialogCallback.OnFileDialogDismissed(int selectedAcceptFilter, IList<string> filePaths)
        {
            
        }
    }
}
