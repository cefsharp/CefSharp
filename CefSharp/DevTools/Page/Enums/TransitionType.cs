// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Transition type.
    /// </summary>
    public enum TransitionType
    {
        Link,
        Typed,
        Address_bar,
        Auto_bookmark,
        Auto_subframe,
        Manual_subframe,
        Generated,
        Auto_toplevel,
        Form_submit,
        Reload,
        Keyword,
        Keyword_generated,
        Other
    }
}