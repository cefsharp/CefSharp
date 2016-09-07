// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading;

namespace CefSharp.Example
{
    public class UniqueBoundObject
    {
        private static int lastId = 0;
        private int id;

        public UniqueBoundObject()
        {
            id = Interlocked.Increment(ref lastId);
        }

        public int Id()
        {
            return id;
        }
    }
}
