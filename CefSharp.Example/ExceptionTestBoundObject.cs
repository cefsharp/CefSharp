// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;

namespace CefSharp.Example
{
    public class ExceptionTestBoundObject
    {
        [DebuggerStepThrough]
        private static double DivisionByZero(int zero)
        {
            return 10 / zero;
        }

        [DebuggerStepThrough]
        public double TriggerNestedExceptions()
        {
            try
            {
                try
                {
                    return DivisionByZero(0);
                }
                catch (Exception innerException)
                {
                    throw new InvalidOperationException("Nested Exception Invalid", innerException);
                }
            }
            catch (Exception e)
            {
                throw new OperationCanceledException("Nested Exception Canceled", e);
            }
        }

        [DebuggerStepThrough]
        public int TriggerParameterException(int parameter)
        {
            return parameter;
        }
    }
}
