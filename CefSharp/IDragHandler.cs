// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp
    {
        public interface IDragHandler
        {
            bool OnDragEnter(IWebBrowser browser, List<string> dragData, out List<string> result);
        }
    }
