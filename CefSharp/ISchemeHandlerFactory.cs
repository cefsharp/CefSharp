﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface ISchemeHandlerFactory
    {
        ISchemeHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request);
    }
}
