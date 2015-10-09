// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "JavascriptCallbackWrapper.h"

using namespace System::Collections::Concurrent;

namespace CefSharp
{
    namespace Internals
    {
        generic <typename T>
        public ref class ConcurrentObjectRegistry
        {
        private:
            bool _deleteEntriesOnDestroy;
            Int64 _lastId;
            ConcurrentDictionary<Int64, T>^ _entries;

        public:
            ConcurrentObjectRegistry(bool deleteEntriesOnDestroy) :
                _deleteEntriesOnDestroy(deleteEntriesOnDestroy),
                _entries(gcnew ConcurrentDictionary<Int64, T>())
            {
            }

            ~ConcurrentObjectRegistry()
            {
                if (_entries != nullptr)
                {
                    if (_deleteEntriesOnDestroy)
                    {
                        for each (T entry in _entries->Values)
                        {
                            delete entry;
                        }
                    }
                    _entries->Clear();
                    delete _entries;
                    _entries = nullptr;
                }
            }

            int64 RegisterObject(T entry);
            bool TryGetObject(int64 id, T% entry);
            bool TryRemoveObject(int64 id, T% entry);
        };
    }
}

