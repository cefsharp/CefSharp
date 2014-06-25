// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public class CefCustomScheme
    {
        public string SchemeName { get; set; }
        public string DomainName { get; set; }
        public bool IsStandard { get; set; }
        public ISchemeHandlerFactory SchemeHandlerFactory { get; set; }

        public CefCustomScheme()
        {
            IsStandard = true;
        }
    }
}
