// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CefSharp.Example
{
    public class ExceptionTestBoundObject
    {
        [DebuggerStepThrough]
        private double DivisionByZero(int zero)
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
            } catch (Exception e)
            {
                throw new OperationCanceledException("Nested Exception Canceled", e);
            }
        }

        [DebuggerStepThrough]
        public int TriggerParameterException(int parameter)
        {
            return parameter;
        }

        public void TestCallbackException(IJavascriptCallback errorCallback, IJavascriptCallback errorCallbackResult)
        {
            const int taskDelay = 500;

            Task.Run(async () =>
            {
                await Task.Delay(taskDelay);

                using (errorCallback)
                {
                    JavascriptResponse result = await errorCallback.ExecuteAsync("This callback from C# was delayed " + taskDelay + "ms");
                    string resultMessage;
                    if (result.Success)
                    {
                        resultMessage = "Fatal: No Exception thrown in error callback";
                    }
                    else
                    {
                        resultMessage = "Exception Thrown: " + result.Message;
                    }
                    await errorCallbackResult.ExecuteAsync(resultMessage);
                }
            });
        }
    }
}
