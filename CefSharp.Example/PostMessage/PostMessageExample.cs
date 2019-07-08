// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.PostMessage
{
    public class PostMessageExample
    {
        public string Type { get; set; }
        public PostMessageExampleData Data { get; set; }
        public IJavascriptCallback Callback { get; set; }
    }
}
