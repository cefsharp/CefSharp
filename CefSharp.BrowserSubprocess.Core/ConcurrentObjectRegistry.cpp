// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "ConcurrentObjectRegistry.h"

using namespace System::Threading;

namespace CefSharp
{
    namespace Internals
    {
        generic <typename T>
        int64 ConcurrentObjectRegistry<T>::RegisterObject(T entry)
        {
            int64 objectId = Interlocked::Increment(_lastId);
            _entries->TryAdd(objectId, entry);
            return objectId;
        }
        generic <typename T>
        bool ConcurrentObjectRegistry<T>::TryGetObject(int64 id, T% entry)
        {
            return _entries->TryGetValue(id, entry) && entry != nullptr;
        }
        generic <typename T>
        bool ConcurrentObjectRegistry<T>::TryRemoveObject(int64 id, T% entry)
        {
            return _entries->TryRemove(id, entry) && entry != nullptr;
        }

    }
}