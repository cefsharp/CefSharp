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
        private readonly ConcurrentDictionary<int, DevToolsMethodResponseContext> queuedCommandResults = new ConcurrentDictionary<int, DevToolsMethodResponseContext>();
        private readonly ConcurrentDictionary<string, IEventProxy> eventHandlers = new ConcurrentDictionary<string, IEventProxy>();
        private IBrowser browser;
        private IRegistration devToolsRegistration;
        private bool devToolsAttached;
        private SynchronizationContext syncContext;
        private int disposeCount;

        /// <inheritdoc/>
        public event EventHandler<DevToolsEventArgs> DevToolsEvent;

        /// <inheritdoc/>
        public event EventHandler<DevToolsErrorEventArgs> DevToolsEventError;

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

        /// <inheritdoc/>
        public void AddEventHandler<T>(string eventName, EventHandler<T> eventHandler) where T : EventArgs
        {
            var eventProxy = eventHandlers.GetOrAdd(eventName, _ => new EventProxy<T>(DeserializeJsonEvent<T>));

            var p = (EventProxy<T>)eventProxy;

            p.AddHandler(eventHandler);
        }

        /// <inheritdoc/>
        public bool RemoveEventHandler<T>(string eventName, EventHandler<T> eventHandler) where T : EventArgs
        {
            if (eventHandlers.TryGetValue(eventName, out IEventProxy eventProxy))
            {
                var p = ((EventProxy<T>)eventProxy);

                if(p.RemoveHandler(eventHandler))
                {
                    return !eventHandlers.TryRemove(eventName, out _);
                }
            }

            return true;
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
                throw new ObjectDisposedException(nameof(IBrowser));
            }

            var taskCompletionSource = new TaskCompletionSource<T>();

            var methodResultContext = new DevToolsMethodResponseContext(
                type: typeof(T),
                setResult: o => taskCompletionSource.TrySetResult((T)o),
                setException: taskCompletionSource.TrySetException,
                syncContext: CaptureSyncContext ? SynchronizationContext.Current : SyncContext
            );

            var browserHost = browser.GetHost();

            var messageId = browserHost.GetNextDevToolsMessageId();

            if (!queuedCommandResults.TryAdd(messageId, methodResultContext))
            {
                throw new DevToolsClientException(string.Format("Unable to add MessageId {0} to queuedCommandResults ConcurrentDictionary.", messageId));
            }

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
                });
            }
            else
            {
                queuedCommandResults.TryRemove(messageId, out methodResultContext);
                throw new DevToolsClientException("Unable to invoke ExecuteDevToolsMethod on CEF UI Thread.");
            }

            return taskCompletionSource.Task;
        }

        private void ExecuteDevToolsMethod(IBrowserHost browserHost, int messageId, string method, IDictionary<string, object> parameters, DevToolsMethodResponseContext methodResultContext)
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
        public void Dispose()
        {
            //Dispose can be called from different Threads
            //CEF maintains a reference and the user
            //maintains a reference, we in a rare case
            //we end up disposing of #3725 twice from different
            //threads. This will ensure our dispose only runs once.
            if (Interlocked.Increment(ref disposeCount) == 1)
            {
                DevToolsEvent = null;
                devToolsRegistration?.Dispose();
                devToolsRegistration = null;
                browser = null;

                var events = eventHandlers.Values;
                eventHandlers.Clear();

                foreach(var evt in events)
                {
                    evt.Dispose();
                }
            }
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
            try
            {
                var evt = DevToolsEvent;

                //Only parse the data if we have an event handler
                if (evt != null)
                {
                    var paramsAsJsonString = StreamToString(parameters, leaveOpen: true);

                    evt(this, new DevToolsEventArgs(method, paramsAsJsonString));
                }

                if (eventHandlers.TryGetValue(method, out IEventProxy eventProxy))
                {
                    eventProxy.Raise(this, method, parameters, SyncContext);
                }
            }
            catch(Exception ex)
            {
                var errorEvent = DevToolsEventError;

                var json = "";

                if(parameters.Length > 0)
                {
                    parameters.Position = 0;

                    try
                    {
                        json = StreamToString(parameters, leaveOpen: false);
                    }
                    catch(Exception)
                    {
                        //TODO: do we somehow pass this exception to the user?
                    }
                }

                var args = new DevToolsErrorEventArgs(method, json, ex);

                errorEvent?.Invoke(this, args);
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
            DevToolsMethodResponseContext context;
            if (queuedCommandResults.TryRemove(messageId, out context))
            {
                if (success)
                {
                    if (context.Type == typeof(DevToolsMethodResponse) || context.Type == typeof(DevToolsDomainResponseBase))
                    {
                        context.SetResult(new DevToolsMethodResponse
                        {
                            Success = success,
                            MessageId = messageId,
                            ResponseAsJsonString = StreamToString(result),
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
                    var errorObj = DeserializeJson<DevToolsDomainErrorResponse>(result);
                    errorObj.MessageId = messageId;

                    context.SetException(new DevToolsClientException("DevTools Client Error :" + errorObj.Message, errorObj));
                }
            }
        }


        /// <summary>
        /// Deserialize the JSON stream into a .Net object.
        /// For .Net Core/.Net 5.0 uses System.Text.Json
        /// for .Net 4.5.2 uses System.Runtime.Serialization.Json
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="eventName">event Name</param>
        /// <param name="stream">JSON stream</param>
        /// <returns>object of type <typeparamref name="T"/></returns>
        private static T DeserializeJsonEvent<T>(string eventName, Stream stream)  where T : EventArgs
        {
            if (typeof(T) == typeof(EventArgs))
            {
                return (T)EventArgs.Empty;
            }

            if (typeof(T) == typeof(DevToolsEventArgs))
            {
                var paramsAsJsonString = StreamToString(stream, leaveOpen: true);
                var args = new DevToolsEventArgs(eventName, paramsAsJsonString);

                return (T)(object)args;
            }

            return (T)DeserializeJson(typeof(T), stream);
        }

        /// <summary>
        /// Deserialize the JSON stream into a .Net object.
        /// For .Net Core/.Net 5.0 uses System.Text.Json
        /// for .Net 4.5.2 uses System.Runtime.Serialization.Json
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="stream">JSON stream</param>
        /// <returns>object of type <typeparamref name="T"/></returns>
        private static T DeserializeJson<T>(Stream stream)
        {
            return (T)DeserializeJson(typeof(T), stream);
        }

        /// <summary>
        /// Deserialize the JSON stream into a .Net object.
        /// For .Net Core/.Net 5.0 uses System.Text.Json
        /// for .Net 4.5.2 uses System.Runtime.Serialization.Json
        /// </summary>
        /// <param name="type">Object type</param>
        /// <param name="stream">JSON stream</param>
        /// <returns>object of type <paramref name="type"/></returns>
        private static object DeserializeJson(Type type, Stream stream)
        {
#if NETCOREAPP
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
            };

            options.Converters.Add(new CefSharp.Internals.Json.JsonEnumConverterFactory());

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

        private static string StreamToString(Stream stream, bool leaveOpen = false)
        {
            using (var streamReader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: leaveOpen))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
