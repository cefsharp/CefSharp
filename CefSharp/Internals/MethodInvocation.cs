// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp.Internals
{
    public sealed class MethodInvocation
    {
        private readonly List<object> parameters = new List<object>();

        public long? CallbackId { get; private set; }

        public long ObjectId { get; private set; }

        public string MethodName { get; private set; }

        public List<object> Parameters
        {
            get { return parameters; }
        }

        public MethodInvocation(long objectId, string methodName, long? callbackId)
        {
            CallbackId = callbackId;
            ObjectId = objectId;
            MethodName = methodName;
        }
    }
}
