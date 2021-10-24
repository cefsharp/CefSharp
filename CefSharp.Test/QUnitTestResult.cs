// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Test
{
    public class QUnitTestResult
    {
        public bool Success
        {
            get { return Passed == Total; }
        }

        public int Passed { get; set; }
        public int Total { get; set; }
    }
}
