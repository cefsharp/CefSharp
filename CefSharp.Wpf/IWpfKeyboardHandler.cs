// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Input;
using System.Windows.Interop;

namespace CefSharp.Wpf 
{
    /// <summary>
    /// Implement this interface to control how keys are forwarded to the browser
    /// </summary>
    public interface IWpfKeyboardHandler : IDisposable 
    {
        void Setup(HwndSource source);
        void HandleKeyPress(KeyEventArgs e);
        void HandleTextInput(TextCompositionEventArgs e);
    }
}