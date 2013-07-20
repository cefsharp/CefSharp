// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "DownloadAdapter.h"

using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    namespace Internals
    {
        bool DownloadAdapter::ReceivedData(void* data, int data_size)
        {
            array<Byte>^ bytes = gcnew array<Byte>(data_size);
            Marshal::Copy(IntPtr(data), bytes, 0, data_size);

            return _handler->ReceivedData(bytes);
        }

        void DownloadAdapter::Complete()
        {
            _handler->Complete();
        }
    }
}
