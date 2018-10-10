// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public class JavascriptStackFrame
    {
        public string FunctionName { get; set; }

        public int LineNumber { get; set; }

        public int ColumnNumber { get; set; }

        public string SourceName { get; set; }
    }
}
