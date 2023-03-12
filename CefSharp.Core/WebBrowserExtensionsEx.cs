// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using CefSharp.ModelBinding;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CefSharp
{
    /// <summary>
    /// Extended WebBrowserExtensions
    /// </summary>
    public static class WebBrowserExtensionsEx
    {
        /// <summary>
        /// Retrieve the current <see cref="NavigationEntry"/>. Contains information like
        /// <see cref="NavigationEntry.HttpStatusCode"/> and <see cref="NavigationEntry.SslStatus"/>
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <returns>
        /// <see cref="Task{NavigationEntry}"/> that when executed returns the current <see cref="NavigationEntry"/> or null
        /// </returns>
        public static Task<NavigationEntry> GetVisibleNavigationEntryAsync(this IChromiumWebBrowserBase browser)
        {
            var host = browser.GetBrowserHost();

            if (host == null)
            {
                return Task.FromResult<NavigationEntry>(null);
            }

            if(Cef.CurrentlyOnThread(CefThreadIds.TID_UI))
            {
                var entry = host.GetVisibleNavigationEntry();

                return Task.FromResult<NavigationEntry>(entry);
            }

            var tcs = new TaskCompletionSource<NavigationEntry>();

            Cef.UIThreadTaskFactory.StartNew(delegate
            {
                var entry = host.GetVisibleNavigationEntry();

                tcs.TrySetResultAsync(entry);
            });

            return tcs.Task;
        }

        /// <summary>
        /// Downloads the specified <paramref name="url"/> and calls <paramref name="completeHandler"/>
        /// when the download is complete. Makes a GET Request.
        /// </summary>
        /// <param name="frame">valid frame</param>
        /// <param name="url">url to download</param>
        /// <param name="completeHandler">Action to be executed when the download is complete.</param>
        public static void DownloadUrl(this IFrame frame, string url, Action<IUrlRequest, Stream>  completeHandler)
        {
            if (!frame.IsValid)
            {
                throw new Exception("Frame is invalid, unable to continue.");
            }

            //Can be created on any valid CEF Thread, here we'll use the CEF UI Thread
            Cef.UIThreadTaskFactory.StartNew(delegate
            {
                var request = frame.CreateRequest(false);

                request.Method = "GET";
                request.Url = url;

                var memoryStream = new MemoryStream();

                var urlRequestClient = Fluent.UrlRequestClient
                    .Create()
                    .OnDownloadData((req, stream) =>
                    {
                        stream.CopyTo(memoryStream);
                    })
                    .OnRequestComplete((req) =>
                    {
                        memoryStream.Position = 0;

                        completeHandler?.Invoke(req, memoryStream);
                    })
                    .Build();

                var urlRequest = frame.CreateUrlRequest(request, urlRequestClient);
            });
        }

        /// <summary>
        /// Downloads the specified <paramref name="url"/> as a <see cref="T:byte[]"/>.
        /// Makes a GET Request.
        /// </summary>
        /// <param name="frame">valid frame</param>
        /// <param name="url">url to download</param>
        /// <returns>A task that can be awaited to get the <see cref="T:byte[]"/> representing the Url</returns>
        public static Task<byte[]> DownloadUrlAsync(this IFrame frame, string url)
        {
            if (!frame.IsValid)
            {
                throw new Exception("Frame is invalid, unable to continue.");
            }

            var taskCompletionSource = new TaskCompletionSource<byte[]>();

            //Can be created on any valid CEF Thread, here we'll use the CEF UI Thread
            Cef.UIThreadTaskFactory.StartNew(delegate
            {
                var request = frame.CreateRequest(false);

                request.Method = "GET";
                request.Url = url;

                var memoryStream = new MemoryStream();

                var urlRequestClient = Fluent.UrlRequestClient
                    .Create()
                    .OnDownloadData((req, stream) =>
                    {
                        stream.CopyTo(memoryStream);
                    })
                    .OnRequestComplete((req) =>
                    {
                        if (req.RequestStatus == UrlRequestStatus.Success)
                        {
                            taskCompletionSource.TrySetResultAsync(memoryStream.ToArray());
                        }
                        else
                        {
                            taskCompletionSource.TrySetExceptionAsync(new Exception("RequestStatus:" + req.RequestStatus + ";StatusCode:" + req.Response.StatusCode));
                        }
                    })
                    .Build();

                var urlRequest = frame.CreateUrlRequest(request, urlRequestClient);
            });

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Toggles audio mute for the current browser.
        /// If the <paramref name="browser"/> is null or has been disposed
        /// then this command will be a no-op.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void ToggleAudioMute(this IChromiumWebBrowserBase browser)
        {
            if (browser.IsDisposed || Cef.IsShutdown)
            {
                return;
            }

            _ = Cef.UIThreadTaskFactory.StartNew(delegate
            {
                var cefBrowser = browser.BrowserCore;

                if (cefBrowser == null || cefBrowser.IsDisposed)
                {
                    return;
                }

                var host = cefBrowser.GetHost();

                var isAudioMuted = host.IsAudioMuted;

                host.SetAudioMuted(!isAudioMuted);
            });
        }

        /// <summary>
        /// Evaluate javascript code in the context of the <paramref name="frame"/>. The script will be executed
        /// asynchronously and the method returns a Task that can be awaited to obtain the result.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
        /// <exception cref="Exception">Thrown if a Javascript error occurs.</exception>
        /// <param name="frame">The IFrame instance this method extends.</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="timeout">(Optional) The timeout after which the Javascript code execution should be aborted.</param>
        /// <returns>
        /// <see cref="Task{T}"/> that can be awaited to obtain the result of the script execution. The <see cref="ModelBinding.DefaultBinder"/>
        /// is used to convert the result to the desired type. Property names are converted from camelCase.
        /// If the script execution returns an error then an exception is thrown.
        /// </returns>
        public static async Task<T> EvaluateScriptAsync<T>(this IFrame frame, string script, TimeSpan? timeout = null)
        {
            WebBrowserExtensions.ThrowExceptionIfFrameNull(frame);

            if (timeout.HasValue && timeout.Value.TotalMilliseconds > uint.MaxValue)
            {
                throw new ArgumentOutOfRangeException("timeout", "Timeout greater than Maximum allowable value of " + UInt32.MaxValue);
            }           

            var response = await frame.EvaluateScriptAsync(script, timeout: timeout, useImmediatelyInvokedFuncExpression: false).ConfigureAwait(false);

            if (response.Success)
            {
                var binder = DefaultBinder.Instance;

                return (T)binder.Bind(response.Result, typeof(T));
            }

            throw new Exception(response.Message);
        }

        /// <summary>
        /// Evaluate some Javascript code in the context of the MainFrame of the ChromiumWebBrowser. The script will be executed
        /// asynchronously and the method returns a Task encapsulating the response from the Javascript
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        /// <param name="script">The JavaScript code that should be executed.</param>
        /// <param name="timeout">(Optional) The timeout after which the JavaScript code execution should be aborted.</param>
        /// <returns>
        /// <see cref="Task{T}"/> that can be awaited to obtain the result of the JavaScript execution.
        /// </returns>
        public static Task<T> EvaluateScriptAsync<T>(this IBrowser browser, string script, TimeSpan? timeout = null)
        {
            WebBrowserExtensions.ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.MainFrame)
            {
                return frame.EvaluateScriptAsync<T>(script, timeout: timeout);
            }
        }

        /// <summary>
        /// Evaluate Javascript in the context of this Browsers Main Frame. The script will be executed
        /// asynchronously and the method returns a Task encapsulating the response from the Javascript
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="timeout">(Optional) The timeout after which the Javascript code execution should be aborted.</param>
        /// <returns>
        /// <see cref="Task{T}"/> that can be awaited to obtain the result of the script execution.
        /// </returns>
        public static Task<T> EvaluateScriptAsync<T>(this IChromiumWebBrowserBase chromiumWebBrowser, string script, TimeSpan? timeout = null)
        {
            WebBrowserExtensions.ThrowExceptionIfChromiumWebBrowserDisposed(chromiumWebBrowser);

            if (chromiumWebBrowser is IWebBrowser b)
            {
                if (b.CanExecuteJavascriptInMainFrame == false)
                {
                    WebBrowserExtensions.ThrowExceptionIfCanExecuteJavascriptInMainFrameFalse();
                }
            }

            return chromiumWebBrowser.BrowserCore.EvaluateScriptAsync<T>(script, timeout);
        }
    }
}
