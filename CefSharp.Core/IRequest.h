// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace CefSharp
{
    public interface class IRequest
    {
        property String^ Url { String^ get(); void set(String^ url); }
        property String^ Method { String^ get(); }
        property String^ Body { String^ get(); }
        IDictionary<String^, String^>^ GetHeaders();
        void SetHeaders(IDictionary<String^, String^>^ headers);
    };
}
