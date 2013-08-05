// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the LoadCompleted event handler set up in IWebBrowser.
    /// </summary>
    public ref class LoadCompletedEventArgs : EventArgs
    {
    private:
        String^ _url;

    internal:
        LoadCompletedEventArgs(String^ url)
            : _url(url) {}

    public:
        /// <summary>
        /// The URL that was loaded.
        /// </summary>
        property String^ Url { String^ get() { return _url; } }
    };

    /// <summary>
    /// A delegate type used to listen to LoadCompleted events.
    /// </summary>
    public delegate void LoadCompletedEventHandler(Object^ sender, LoadCompletedEventArgs^ url);
}
