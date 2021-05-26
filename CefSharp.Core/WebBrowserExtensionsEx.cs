// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
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
        public static Task<NavigationEntry> GetVisibleNavigationEntryAsync(this IWebBrowser browser)
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
        /// Downloads the specified <paramref name="url"/> as a <see cref="byte[]"/>.
        /// Makes a GET Request.
        /// </summary>
        /// <param name="frame">valid frame</param>
        /// <param name="url">url to download</param>
        /// <returns>A task that can be awaited to get the <see cref="byte[]"/> representing the Url</returns>
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
    }
}
