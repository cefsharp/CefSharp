// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.IO;
using CefSharp.Handler;

namespace CefSharp.Example.Handlers
{
    public class TempFileDialogHandler : DialogHandler
    {
        protected override bool OnFileDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, CefFileDialogMode mode, string title, string defaultFilePath, IReadOnlyCollection<string> acceptFilters, IReadOnlyCollection<string> acceptExtensions, IReadOnlyCollection<string> acceptDescriptions, IFileDialogCallback callback)
        {
            callback.Continue(new List<string> { Path.GetRandomFileName() });

            return true;
        }
    }
}
