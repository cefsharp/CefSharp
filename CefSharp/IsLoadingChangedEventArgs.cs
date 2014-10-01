// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the IsLoadingChanged event handler.
    /// </summary>
    public class IsLoadingChangedEventArgs : EventArgs
    {
        public bool IsLoading { get; private set; }

        public IsLoadingChangedEventArgs(bool isLoading)
        {
            IsLoading = isLoading;
        }
    }
}

