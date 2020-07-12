// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp.Test
{
    public static class WebBrowserTestExtensions
    {
        public static Task LoadPageAsync(this IWebBrowser browser, string address = null)
        {
            //If using .Net 4.6 then use TaskCreationOptions.RunContinuationsAsynchronously
            //and switch to tcs.TrySetResult below - no need for the custom extension method
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            EventHandler<LoadingStateChangedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                //Wait for while page to finish loading not just the first frame
                if (!args.IsLoading)
                {
                    browser.LoadingStateChanged -= handler;
                    //This is required when using a standard TaskCompletionSource
                    //Extension method found in the CefSharp.Internals namespace
                    tcs.TrySetResult(true);
                }
            };

            browser.LoadingStateChanged += handler;

            if (!string.IsNullOrEmpty(address))
            {
                browser.Load(address);
            }
            return tcs.Task;
        }

        public static Task<bool> WaitForQUnitTestExeuctionToComplete(this IWebBrowser browser)
        {
            //If using .Net 4.6 then use TaskCreationOptions.RunContinuationsAsynchronously
            //and switch to tcs.TrySetResult below - no need for the custom extension method
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            EventHandler<JavascriptMessageReceivedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                browser.JavascriptMessageReceived -= handler;

                dynamic msg = args.Message;
                //Wait for while page to finish loading not just the first frame
                if (msg.Type == "QUnitExecutionComplete")
                {
                    var details = msg.Details;
                    var total = (int)details.total;
                    var passed = (int)details.passed;

                    tcs.TrySetResult(total == passed);
                }
                else
                {
                    tcs.TrySetException(new Exception("WaitForQUnitTestExeuctionToComplete - Incorrect Message Type"));
                }
            };

            browser.JavascriptMessageReceived += handler;

            return tcs.Task;
        }
    }
}
