// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.WinForms
{
    // Should rightfully live in the CefSharp.WinForms project, but the problem is that it's being used from CefSharp.Core
    // so the dependency would go the wrong way... Has to be here for the time being.
    public interface IWinFormsWebBrowser : IWebBrowser
    {
        event EventHandler<TitleChangedEventArgs> TitleChanged;
        event EventHandler<AddressChangedEventArgs> AddressChanged;
    }
}
