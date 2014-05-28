// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows;
using CefSharp.Example;

namespace CefSharp.Wpf.Example
{
    public partial class App : Application
    {
        private App()
        {
            CefExample.Init();
        }
    }
}