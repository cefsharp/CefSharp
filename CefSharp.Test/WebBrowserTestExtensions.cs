// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Reflection;
using System.Threading.Tasks;
using CefSharp.OffScreen;
using Xunit;

namespace CefSharp.Test
{
    public static class WebBrowserTestExtensions
    {
        public static int PaintEventHandlerCount(this CefSharp.Wpf.ChromiumWebBrowser browser)
        {
            var field = typeof(CefSharp.Wpf.ChromiumWebBrowser).GetField("Paint", BindingFlags.NonPublic | BindingFlags.Instance);

            if(field == null)
            {
                throw new Exception("Unable to obtain Paint event handler");
            }

            var handler = field.GetValue(browser) as Delegate;

            if (handler == null)
            {
                return 0;
            }

            var subscribers = handler.GetInvocationList();

            return subscribers.Length;
        }

        public static int PaintEventHandlerCount(this ChromiumWebBrowser browser)
        {
            var field = typeof(ChromiumWebBrowser).GetField("Paint", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
            {
                throw new Exception("Unable to obtain Paint event handler");
            }

            var handler = field.GetValue(browser) as Delegate;

            if (handler == null)
            {
                return 0;
            }

            var subscribers = handler.GetInvocationList();

            return subscribers.Length;
        }

        public static Task<LoadUrlAsyncResponse> LoadRequestAsync(this IWebBrowser browser, IRequest request)
        {
            if(request == null)
            {
                throw new ArgumentNullException("request");
            }

            //If using .Net 4.6 then use TaskCreationOptions.RunContinuationsAsynchronously
            //and switch to tcs.TrySetResult below - no need for the custom extension method
            var tcs = new TaskCompletionSource<LoadUrlAsyncResponse>(TaskCreationOptions.RunContinuationsAsynchronously);

            EventHandler<LoadErrorEventArgs> loadErrorHandler = null;
            EventHandler<LoadingStateChangedEventArgs> loadingStateChangeHandler = null;

            loadErrorHandler = (sender, args) =>
            {
                //Ignore Aborted
                //Currently invalid SSL certificates which aren't explicitly allowed
                //end up with CefErrorCode.Aborted, I've created the following PR
                //in the hopes of getting this fixed.
                //https://bitbucket.org/chromiumembedded/cef/pull-requests/373
                if (args.ErrorCode == CefErrorCode.Aborted)
                {
                    return;
                }

                //If LoadError was called then we'll remove both our handlers
                //as we won't need to capture LoadingStateChanged, we know there
                //was an error
                browser.LoadError -= loadErrorHandler;
                browser.LoadingStateChanged -= loadingStateChangeHandler;

                tcs.TrySetResult(new LoadUrlAsyncResponse(args.ErrorCode, -1));
            };

            loadingStateChangeHandler = (sender, args) =>
            {
                //Wait for while page to finish loading not just the first frame
                if (!args.IsLoading)
                {
                    var host = args.Browser.GetHost();

                    var navEntry = host?.GetVisibleNavigationEntry();

                    int statusCode = navEntry?.HttpStatusCode ?? -1;

                    //By default 0 is some sort of error, we map that to -1
                    //so that it's clearer that something failed.
                    if (statusCode == 0)
                    {
                        statusCode = -1;
                    }

                    browser.LoadingStateChanged -= loadingStateChangeHandler;
                    //This is required when using a standard TaskCompletionSource
                    //Extension method found in the CefSharp.Internals namespace
                    tcs.TrySetResult(new LoadUrlAsyncResponse(CefErrorCode.None, statusCode));
                }
            };

            browser.LoadingStateChanged += loadingStateChangeHandler;

            browser.GetMainFrame().LoadRequest(request);

            return tcs.Task;
        }

        public static Task<QUnitTestResult> CreateBrowserAndWaitForQUnitTestExeuctionToComplete(this ChromiumWebBrowser browser)
        {
            //If using .Net 4.6 then use TaskCreationOptions.RunContinuationsAsynchronously
            //and switch to tcs.TrySetResult below - no need for the custom extension method
            var tcs = new TaskCompletionSource<QUnitTestResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            EventHandler<JavascriptMessageReceivedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                dynamic msg = args.Message;
                //Wait for while page to finish loading not just the first frame
                if (msg.Type == "QUnitExecutionComplete")
                {
                    browser.JavascriptMessageReceived -= handler;

                    var details = msg.Details;
                    var total = (int)details.total;
                    var passed = (int)details.passed;

                    tcs.TrySetResult(new QUnitTestResult { Passed = passed, Total = total });
                }
            };

            browser.JavascriptMessageReceived += handler;

            browser.CreateBrowser();

            return tcs.Task;
        }
    }
}
