// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public class BrowserProcessServiceHost : ServiceHost
    {
        private const long SixteenMegaBytesInBytes = 16*1024*1024;

        public JavascriptObjectRepository JavascriptObjectRepository { get; private set; }
        private TaskCompletionSource<OperationContext> operationContextTaskCompletionSource = new TaskCompletionSource<OperationContext>();
        
        public BrowserProcessServiceHost(JavascriptObjectRepository javascriptObjectRepository, int parentProcessId, int browserId)
            : base(typeof(BrowserProcessService), new Uri[0])
        {
            JavascriptObjectRepository = javascriptObjectRepository;

            var serviceName = RenderprocessClientFactory.GetServiceName(parentProcessId, browserId);

            Description.ApplyServiceBehavior(() => new ServiceDebugBehavior(), p => p.IncludeExceptionDetailInFaults = true);

            var binding = CreateBinding();

            var endPoint = AddServiceEndpoint(
                typeof(IBrowserProcess),
                binding,
                new Uri(serviceName)
            );

            endPoint.Contract.ProtectionLevel = ProtectionLevel.None;
        }

        public void SetOperationContext(OperationContext operationContext)
        {
            if (operationContextTaskCompletionSource.Task.Status == TaskStatus.RanToCompletion)
            {
                operationContextTaskCompletionSource = new TaskCompletionSource<OperationContext>();
            }
                
            operationContextTaskCompletionSource.SetResult(operationContext);
        }

        public Task<JavascriptResponse> EvaluateScriptAsync(int browserId, long frameId, string script, TimeSpan? timeout)
        {
            var operationContextTask = operationContextTaskCompletionSource.Task;
            return operationContextTask.ContinueWith(t =>
            {
                var context = t.Result;
                var renderProcess = context.GetCallbackChannel<IRenderProcess>();
                var asyncResult = renderProcess.BeginEvaluateScriptAsync(browserId, frameId, script, timeout, null, null);
                return Task.Factory.FromAsync<JavascriptResponse>(asyncResult, renderProcess.EndEvaluateScriptAsync);
            }).Unwrap();
        }

        protected override void OnClose(TimeSpan timeout)
        {
            var task = operationContextTaskCompletionSource.Task;

            CloseChannel(task);

            base.OnClose(timeout);
        }

        private void CloseChannel(Task<OperationContext> task)
        {
            try
            {
                if (task.IsCompleted)
                {
                    var context = task.Result;

                    if (context.Channel != null && context.Channel.State == CommunicationState.Opened)
                    {
                        context.Channel.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            JavascriptObjectRepository = null;
            operationContextTaskCompletionSource = null;
        }

        public static CustomBinding CreateBinding()
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            binding.MaxReceivedMessageSize = SixteenMegaBytesInBytes;
            binding.ReceiveTimeout = TimeSpan.MaxValue;
            binding.SendTimeout = TimeSpan.MaxValue;
            binding.OpenTimeout = TimeSpan.MaxValue;
            binding.CloseTimeout = TimeSpan.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;

            // Ensure binding connection stays open indefinitely until closed
            var customBinding = new CustomBinding(binding);
            var connectionSettings = customBinding.Elements.Find<NamedPipeTransportBindingElement>().ConnectionPoolSettings;
            connectionSettings.IdleTimeout = TimeSpan.MaxValue;
            connectionSettings.MaxOutboundConnectionsPerEndpoint = 0;

            return customBinding;
        }
    }
}