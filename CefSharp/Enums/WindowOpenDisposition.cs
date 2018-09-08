// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// The manner in which a link click should be opened.
    /// </summary>
    public enum WindowOpenDisposition
    {
        Unknown,
        CurrentTab,
        SingletonTab,
        NewForegroundTab,
        NewBackgroundTab,
        NewPopup,
        NewWindow,
        SaveToDisk,
        OffTheRecord,
        IgnoreAction
    }
}
