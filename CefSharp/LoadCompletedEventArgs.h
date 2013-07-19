// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System;

namespace CefSharp
{
    public ref class LoadCompletedEventArgs : EventArgs
    {
        String^ _url;

    public:
        LoadCompletedEventArgs(String^ url)
            : _url(url) {}

        property String^ Url { String^ get() { return _url; } }
    };

    public delegate void LoadCompletedEventHandler(Object^ sender, LoadCompletedEventArgs^ url);
}
