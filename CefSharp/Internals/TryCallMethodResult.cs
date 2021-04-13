// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    public class TryCallMethodResult
    {
        public string Exception { get; private set; }
        public object ReturnValue { get; private set; }
        public bool Success { get; private set; }

        public TryCallMethodResult(bool success, object returnValue, string exception)
        {
            Success = success;
            ReturnValue = returnValue;
            Exception = exception;
        }
    }
}
