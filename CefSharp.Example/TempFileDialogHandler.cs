// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.IO;

namespace CefSharp.Example
{
    public class TempFileDialogHandler : IDialogHandler
    {
        public bool OnFileDialog(IWebBrowser browser, CefFileDialogMode mode, string title, string defaultFilePath, List<string> acceptFilters, out int selectedAcceptFilter, out List<string> result)
        {
            selectedAcceptFilter = 0;
            result = new List<string> { Path.GetRandomFileName() };
            return true;
        }
    }
}
