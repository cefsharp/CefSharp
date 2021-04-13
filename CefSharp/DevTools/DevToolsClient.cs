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
using CefSharp.Internals.Tasks;

namespace CefSharp.DevTools
{
    /// <summary>
    /// DevTool Client 
    /// </summary>
    public partial class DevToolsClient : IDevToolsMessageObserver, IDevToolsClient
    {
        //TODO: Message Id is now global, limits the number of messages to int.MaxValue
        //Needs to be unique and incrementing per browser with the option to have multiple
        //DevToolsClient instances per browser.
        private static int lastMessageId = 0;

        private readonly ConcurrentDictionary<int, SyncContextTaskCompletionSource<DevToolsMethodResponse>> queuedCommandResults = new ConcurrentDictionary<int, SyncContextTaskCompletionSource<DevToolsMethodResponse>>();
        private IBrowser browser;
        private IRegistration devToolsRegistration;
        private bool devToolsAttached;
        private SynchronizationContext syncContext;

        /// <summary>
        /// DevToolsEvent
        /// </summary>
        public EventHandler<DevToolsEventArgs> DevToolsEvent;

        /// <summary>
        /// Capture the current <see cref="SynchronizationContext"/> so
        /// continuation executes on the original calling thread. If
        /// <see cref="SynchronizationContext.Current"/> is null for
        /// <see cref="ExecuteDevToolsMethodAsync(string, IDictionary{string, object})"/>
        /// then the continuation will be run on the CEF UI Thread (by default
        /// this is not the same as the WPF/WinForms UI Thread).
        /// </summary>
        public bool CaptureSyncContext { get; set; }

        /// <summary>
        /// When not null provided <see cref="SynchronizationContext"/>
        /// will be used to run the contination. Defaults to null
        /// Setting this property will change <see cref="CaptureSyncContext"/>
        /// to false.
        /// </summary>
        public SynchronizationContext SyncContext
        {
            get { return syncContext; }
            set
            {
                CaptureSyncContext = false;
                syncContext = value;
            }
        }

        /// <summary>
        /// DevToolsClient
        /// </summary>
        /// <param name="browser">Browser associated with this DevTools client</param>
        public DevToolsClient(IBrowser browser)
        {
            this.browser = browser;

            CaptureSyncContext = true;
        }

        /// <summary>
        /// Store a reference to the IRegistration that's returned when
        /// you register an observer.
        /// </summary>
        /// <param name="devToolsRegistration">registration</param>
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

            var taskCompletionSource = new SyncContextTaskCompletionSource<DevToolsMethodResponse>();

            taskCompletionSource.SyncContext = CaptureSyncContext ? SynchronizationContext.Current : syncContext;

            if (!queuedCommandResults.TryAdd(messageId, taskCompletionSource))
            {
                throw new DevToolsClientException(string.Format("Unable to add MessageId {0} to queuedCommandResults ConcurrentDictionary.", messageId));
            }

            var browserHost = browser.GetHost();

            //Currently on CEF UI Thread we can directly execute
            if (CefThread.CurrentlyOnUiThread)
            {
                var returnedMessageId = browserHost.ExecuteDevToolsMethod(messageId, method, parameters);
                if (returnedMessageId == 0)
                {
                    return new DevToolsMethodResponse { Success = false };
                }
                else if(returnedMessageId != messageId)
                {
                    //For some reason our message Id's don't match
                    throw new DevToolsClientException(string.Format("Generated MessageId {0} doesn't match returned Message Id {1}", returnedMessageId, messageId));
                }
            }
            //ExecuteDevToolsMethod can only be called on the CEF UI Thread
            else if (CefThread.CanExecuteOnUiThread)
            {
                var returnedMessageId = await CefThread.ExecuteOnUiThread(() =>
                {
                    return browserHost.ExecuteDevToolsMethod(messageId, method, parameters);
                }).ConfigureAwait(false);

                if (returnedMessageId == 0)
                {
                    return new DevToolsMethodResponse { Success = false };
                }
                else if (returnedMessageId != messageId)
                {
                    //For some reason our message Id's don't match
                    throw new DevToolsClientException(string.Format("Generated MessageId {0} doesn't match returned Message Id {1}", returnedMessageId, messageId));
                }
            }
            else
            {
                throw new DevToolsClientException("Unable to invoke ExecuteDevToolsMethod on CEF UI Thread.");
            }

            return await taskCompletionSource.Task;
        }

        void IDisposable.Dispose()
        {
            DevToolsEvent = null;
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
            var evt = DevToolsEvent;

            //Only parse the data if we have an event handler
            if (evt != null)
            {
                //TODO: Improve this
                var memoryStream = new MemoryStream((int)parameters.Length);
                parameters.CopyTo(memoryStream);

                var paramsAsJsonString = Encoding.UTF8.GetString(memoryStream.ToArray());

                evt(this, new DevToolsEventArgs(method, paramsAsJsonString));
            }
        }

        bool IDevToolsMessageObserver.OnDevToolsMessage(IBrowser browser, Stream message)
        {
            return false;
        }

        void IDevToolsMessageObserver.OnDevToolsMethodResult(IBrowser browser, int messageId, bool success, Stream result)
        {
            var uiThread = CefThread.CurrentlyOnUiThread;
            SyncContextTaskCompletionSource<DevToolsMethodResponse> taskCompletionSource = null;

            if (queuedCommandResults.TryRemove(messageId, out taskCompletionSource))
            {
                var methodResult = new DevToolsMethodResponse
                {
                    Success = success,
                    MessageId = messageId
                };

                //TODO: Improve this
                var memoryStream = new MemoryStream((int)result.Length);

                result.CopyTo(memoryStream);

                methodResult.ResponseAsJsonString = Encoding.UTF8.GetString(memoryStream.ToArray());

                Action execute = null;

                if (success)
                {
                    execute = () =>
                    {
                        taskCompletionSource.TrySetResult(methodResult);
                    };
                }
                else
                {
                    execute = () =>
                    {
                        var errorObj = methodResult.DeserializeJson<DevToolsDomainErrorResponse>();
                        errorObj.MessageId = messageId;

                        //Make sure continuation runs on Thread Pool
                        taskCompletionSource.TrySetException(new DevToolsClientException("DevTools Client Error :" + errorObj.Message, errorObj));
                    };
                }

                var syncContext = taskCompletionSource.SyncContext;
                if (syncContext == null)
                {
                    execute();
                }
                else
                {
                    syncContext.Post(new SendOrPostCallback((o) =>
                    {
                        execute();
                    }), null);
                }
            }
        }
    }
}
