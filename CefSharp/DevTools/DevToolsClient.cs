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
    public partial class DevToolsClient : IDevToolsMessageObserver, IDevToolsClient
    {
        //TODO: Message Id is now global, limits the number of messages to int.MaxValue
        //Needs to be unique and incrementing per browser with the option to have multiple
        //DevToolsClient instances per browser.
        private static int lastMessageId = 0;

        private readonly ConcurrentDictionary<int, MethodResultContext> queuedCommandResults = new ConcurrentDictionary<int, MethodResultContext>();
        private IBrowser browser;
        private IRegistration devToolsRegistration;
        private bool devToolsAttached;
        private SynchronizationContext syncContext;

        /// <inheritdoc/>
        public event EventHandler<DevToolsEventArgs> DevToolsEvent;

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
        public Task<DevToolsMethodResponse> ExecuteDevToolsMethodAsync(string method, IDictionary<string, object> parameters = null)
        {
            return ExecuteDevToolsMethodAsync<DevToolsMethodResponse>(method, parameters);
        }

        /// <summary>
        /// Execute a method call over the DevTools protocol. This method can be called on any thread.
        /// See the DevTools protocol documentation at https://chromedevtools.github.io/devtools-protocol/ for details
        /// of supported methods and the expected <paramref name="parameters"/> dictionary contents.
        /// </summary>
        /// <typeparam name="T">The type into which the result will be deserialzed.</typeparam>
        /// <param name="method">is the method name</param>
        /// <param name="parameters">are the method parameters represented as a dictionary,
        /// which may be empty.</param>
        /// <returns>return a Task that can be awaited to obtain the method result</returns>
        public Task<T> ExecuteDevToolsMethodAsync<T>(string method, IDictionary<string, object> parameters = null) where T : DevToolsDomainResponseBase
        {
            if (browser == null || browser.IsDisposed)
            {
                //TODO: Queue up commands where possible
                throw new ObjectDisposedException(nameof(DevToolsClient));
            }

            var messageId = Interlocked.Increment(ref lastMessageId);

            var taskCompletionSource = new TaskCompletionSource<T>();

            var methodResultContext = new MethodResultContext(
                type: typeof(T),
                setResult: o => taskCompletionSource.TrySetResult((T)o),
                setException: taskCompletionSource.TrySetException,
                syncContext: CaptureSyncContext ? SynchronizationContext.Current : SyncContext
            );

            if (!queuedCommandResults.TryAdd(messageId, methodResultContext))
            {
                throw new DevToolsClientException(string.Format("Unable to add MessageId {0} to queuedCommandResults ConcurrentDictionary.", messageId));
            }

            var browserHost = browser.GetHost();

            //Currently on CEF UI Thread we can directly execute
            if (CefThread.CurrentlyOnUiThread)
            {
                ExecuteDevToolsMethod(browserHost, messageId, method, parameters, methodResultContext);
            }
            //ExecuteDevToolsMethod can only be called on the CEF UI Thread
            else if (CefThread.CanExecuteOnUiThread)
            {
                CefThread.ExecuteOnUiThread(() =>
                {
                    ExecuteDevToolsMethod(browserHost, messageId, method, parameters, methodResultContext);
                    return (object)null;
                });
            }
            else
            {
                queuedCommandResults.TryRemove(messageId, out methodResultContext);
                throw new DevToolsClientException("Unable to invoke ExecuteDevToolsMethod on CEF UI Thread.");
            }

            return taskCompletionSource.Task;
        }

        private void ExecuteDevToolsMethod(IBrowserHost browserHost, int messageId, string method, IDictionary<string, object> parameters, MethodResultContext methodResultContext)
        {
            try
            {
                var returnedMessageId = browserHost.ExecuteDevToolsMethod(messageId, method, parameters);
                if (returnedMessageId == 0)
                {
                    throw new DevToolsClientException(string.Format("Failed to execute dev tools method {0}.", method));
                }
                else if (returnedMessageId != messageId)
                {
                    //For some reason our message Id's don't match
                    throw new DevToolsClientException(string.Format("Generated MessageId {0} doesn't match returned Message Id {1}", returnedMessageId, messageId));
                }
            }
            catch (Exception ex)
            {
                queuedCommandResults.TryRemove(messageId, out _);
                methodResultContext.SetException(ex);
            }
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            DevToolsEvent = null;
            devToolsRegistration?.Dispose();
            devToolsRegistration = null;
            browser = null;
        }

        /// <inheritdoc/>
        void IDevToolsMessageObserver.OnDevToolsAgentAttached(IBrowser browser)
        {
            devToolsAttached = true;
        }

        /// <inheritdoc/>
        void IDevToolsMessageObserver.OnDevToolsAgentDetached(IBrowser browser)
        {
            devToolsAttached = false;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        bool IDevToolsMessageObserver.OnDevToolsMessage(IBrowser browser, Stream message)
        {
            return false;
        }

        /// <inheritdoc/>
        void IDevToolsMessageObserver.OnDevToolsMethodResult(IBrowser browser, int messageId, bool success, Stream result)
        {
            MethodResultContext context;

            if (queuedCommandResults.TryRemove(messageId, out context))
            {
                if (success)
                {
                    if (context.Type == typeof(DevToolsMethodResponse) || context.Type == typeof(DevToolsDomainResponseBase))
                    {
                        var memoryStream = new MemoryStream((int)result.Length);
                        result.CopyTo(memoryStream);

                        context.SetResult(new DevToolsMethodResponse
                        {
                            Success = success,
                            MessageId = messageId,
                            ResponseAsJsonString = Encoding.UTF8.GetString(memoryStream.ToArray()),
                        });
                    }
                    else
                    {
                        try
                        {
                            context.SetResult(DeserializeJson(context.Type, result));
                        }
                        catch (Exception ex)
                        {
                            context.SetException(ex);
                        }
                    }
                }
                else
                {
                    var errorObj = (DevToolsDomainErrorResponse)DeserializeJson(typeof(DevToolsDomainErrorResponse), result);
                    errorObj.MessageId = messageId;

                    context.SetException(new DevToolsClientException("DevTools Client Error :" + errorObj.Message, errorObj));
                }
            }
        }

        private struct MethodResultContext
        {
            public readonly Type Type;
            private readonly Func<object, bool> setResult;
            private readonly Func<Exception, bool> setException;
            private readonly SynchronizationContext syncContext;

            public MethodResultContext(Type type, Func<object, bool> setResult, Func<Exception, bool> setException, SynchronizationContext syncContext)
            {
                Type = type;
                this.setResult = setResult;
                this.setException = setException;
                this.syncContext = syncContext;
            }

            public void SetResult(object result)
            {
                InvokeOnSyncContext(setResult, result);
            }

            public void SetException(Exception ex)
            {
                InvokeOnSyncContext(setException, ex);
            }

            private void InvokeOnSyncContext<T>(Func<T, bool> fn, T value)
            {
                if (syncContext == null)
                {
                    fn(value);
                }
                else
                {
                    syncContext.Post(new SendOrPostCallback(s =>
                    {
                        var kv = (KeyValuePair<Func<T, bool>, T>)s;
                        kv.Key(kv.Value);
                    }), new KeyValuePair<Func<T, bool>, T>(fn, value));
                }
            }
        }

        private static object DeserializeJson(Type type, Stream stream)
        {
#if NETCOREAPP
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
            };

            options.Converters.Add(new Internals.Json.JsonEnumConverterFactory());

            // TODO: use synchronus Deserialize<T>(Stream) when System.Text.Json gets updated
            var memoryStream = new MemoryStream((int)stream.Length);
            stream.CopyTo(memoryStream);

            return System.Text.Json.JsonSerializer.Deserialize(memoryStream.ToArray(), type, options);
#else
            var settings = new System.Runtime.Serialization.Json.DataContractJsonSerializerSettings();
            settings.UseSimpleDictionaryFormat = true;

            var dcs = new System.Runtime.Serialization.Json.DataContractJsonSerializer(type, settings);
            return dcs.ReadObject(stream);
#endif
        }

        /// <summary>
        /// Deserialize the JSON string into a .Net object.
        /// For .Net Core/.Net 5.0 uses System.Text.Json
        /// for .Net 4.5.2 uses System.Runtime.Serialization.Json
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="json">JSON</param>
        /// <returns>object of type <typeparamref name="T"/></returns>
        public static T DeserializeJson<T>(string json)
        {
#if NETCOREAPP
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
            };

            options.Converters.Add(new CefSharp.Internals.Json.JsonEnumConverterFactory());

            return System.Text.Json.JsonSerializer.Deserialize<T>(json, options);
#else
            var bytes = Encoding.UTF8.GetBytes(json);
            using (var ms = new MemoryStream(bytes))
            {
                var settings = new System.Runtime.Serialization.Json.DataContractJsonSerializerSettings();
                settings.UseSimpleDictionaryFormat = true;

                var dcs = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T), settings);
                return (T)dcs.ReadObject(ms);
            }
#endif
        }
    }
}
