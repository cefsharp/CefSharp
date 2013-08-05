// Copyright � 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    public interface class IDownloadHandler
    {
        bool OnBeforeDownload(String^ suggestedName, [Out] String^% downloadPath, [Out] bool% showDialog);
        bool ReceivedData(array<Byte>^ data);
        void Complete();
    };
}