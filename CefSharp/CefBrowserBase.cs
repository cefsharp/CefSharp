// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public abstract class CefBrowserBase : ObjectBase
    {
        public int BrowserId { get; set; }

        public abstract object EvaluateScript(int frameId, string script, double timeout);
    }
}
