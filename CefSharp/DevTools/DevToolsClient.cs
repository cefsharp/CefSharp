// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.Callback;
using CefSharp.Internals;

namespace CefSharp.DevTools
{
    /// <summary>
    /// DevTool Client 
    /// </summary>
    public partial class DevToolsClient : IDevToolsMessageObserver
    {
        private readonly ConcurrentDictionary<int, TaskCompletionSource<DevToolsMethodResponse>> queuedCommandResults = new ConcurrentDictionary<int, TaskCompletionSource<DevToolsMethodResponse>>();
        private int lastMessageId;
        private IBrowser browser;
        private IRegistration devToolsRegistration;
        private bool devToolsAttached;

        /// <summary>
        /// DevToolsEvent
        /// </summary>
        public EventHandler<DevToolsEventArgs> DevToolsEvent;

        /// <summary>
        /// DevToolsClient
        /// </summary>
        /// <param name="browser">Browser associated with this DevTools client</param>
        public DevToolsClient(IBrowser browser)
        {
            this.browser = browser;

            lastMessageId = browser.Identifier * 100000;
        }

        public void SetDevToolsObserverRegistration(IRegistration devToolsRegistration)
        {
            this.devToolsRegistration = devToolsRegistration;
        }

        /// <summary>
        /// Execute a method call over the DevTools protocol. This method can be called on any thread.
        /// See the DevTools protocol documentation at https://chromedevtools.github.io/devtools-protocol/ for details
        /// of supported methods and the expected <paramref name="parameters"/> dictionary contents.
        /// </summary>
        /// <param name="method">is the method name</param>
        /// <param name="parameters">are the method parameters represented as a dictionary,
        /// which may be empty.</param>
        /// <returns>return a Task that can be awaited to obtain the method result</returns>
        public async Task<DevToolsMethodResponse> ExecuteDevToolsMethodAsync(string method, IDictionary<string, object> parameters = null)
        {
            if (browser == null || browser.IsDisposed)
            {
                //TODO: Queue up commands where possible
                return new DevToolsMethodResponse { Success = false };
            }

            var messageId = Interlocked.Increment(ref lastMessageId);

            var taskCompletionSource = new TaskCompletionSource<DevToolsMethodResponse>();

            if (!queuedCommandResults.TryAdd(messageId, taskCompletionSource))
            {
                return new DevToolsMethodResponse { Success = false };
            }

            var browserHost = browser.GetHost();

            if (CefThread.CurrentlyOnUiThread)
            {
                var returnedMessageId = browserHost.ExecuteDevToolsMethod(messageId, method, parameters);
                if (returnedMessageId == 0)
                {
                    return new DevToolsMethodResponse { Success = false };
                }
            }

            if (CefThread.CanExecuteOnUiThread)
            {
                var returnedMessageId = await CefThread.ExecuteOnUiThread(() =>
                {
                    return browserHost.ExecuteDevToolsMethod(messageId, method, parameters);
                }).ConfigureAwait(false);

                if (returnedMessageId == 0)
                {
                    return new DevToolsMethodResponse { Success = false };
                }
            }

            return await taskCompletionSource.Task;
        }

        void IDisposable.Dispose()
        {
            devToolsRegistration?.Dispose();
            devToolsRegistration = null;
            browser = null;
        }

        void IDevToolsMessageObserver.OnDevToolsAgentAttached(IBrowser browser)
        {
            devToolsAttached = true;
        }

        void IDevToolsMessageObserver.OnDevToolsAgentDetached(IBrowser browser)
        {
            devToolsAttached = false;
        }

        void IDevToolsMessageObserver.OnDevToolsEvent(IBrowser browser, string method, Stream parameters)
        {
            //TODO: Improve this
            var memoryStream = new MemoryStream((int)parameters.Length);
            parameters.CopyTo(memoryStream);

            var paramsAsJsonString = Encoding.UTF8.GetString(memoryStream.ToArray());

            DevToolsEvent?.Invoke(this, new DevToolsEventArgs(method, paramsAsJsonString));
        }

        bool IDevToolsMessageObserver.OnDevToolsMessage(IBrowser browser, Stream message)
        {
            return false;
        }

        void IDevToolsMessageObserver.OnDevToolsMethodResult(IBrowser browser, int messageId, bool success, Stream result)
        {
            TaskCompletionSource<DevToolsMethodResponse> taskCompletionSource = null;

            if (queuedCommandResults.TryRemove(messageId, out taskCompletionSource))
            {
                var methodResult = new DevToolsMethodResponse
                {
                    Success = success
                };

                //TODO: Improve this
                var memoryStream = new MemoryStream((int)result.Length);

                result.CopyTo(memoryStream);

                methodResult.ResponseAsJsonString = Encoding.UTF8.GetString(memoryStream.ToArray());

                if (success)
                {
                    Task.Run(() =>
                    {
                        //Make sure continuation runs on Thread Pool
                        taskCompletionSource.TrySetResult(methodResult);
                    });
                }
                else
                {
                    Task.Run(() =>
                    {
                        //TODO: Improve format error message
                        //Make sure continuation runs on Thread Pool
                        taskCompletionSource.TrySetException(new DevToolsClientException(methodResult.ResponseAsJsonString));
                    });
                }

            }
        }
    }
}
