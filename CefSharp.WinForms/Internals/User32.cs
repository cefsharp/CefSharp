// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;

namespace CefSharp.WinForms.Internals
{
    public static class User32
    {
        [DllImport("User32.dll")]
        public extern static IntPtr GetFocus();

        [DllImport("user32.dll")]
        public extern static bool IsChild(IntPtr parent, IntPtr child);
    }
}