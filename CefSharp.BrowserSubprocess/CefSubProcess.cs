// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp.BrowserSubprocess
{
    public class CefSubProcess : CefAppWrapper
    {
        public CefSubProcess(IEnumerable<string> args) : base(args)
        {
            
        }

        public override void OnBrowserCreated(CefBrowserWrapper cefBrowserWrapper)
        {
            
        }

        public override void OnBrowserDestroyed(CefBrowserWrapper cefBrowserWrapper)
        {
            
        }
    }
}