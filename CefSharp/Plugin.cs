// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public struct Plugin
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
    }
}
