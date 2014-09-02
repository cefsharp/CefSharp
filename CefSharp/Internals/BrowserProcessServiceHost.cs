// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Net.Security;
using System.ServiceModel;
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
            operationContextTaskCompletionSource.SetResult(operationContext);
        }

        public Task<JavascriptResponse> EvaluateScript(int frameId, string script, TimeSpan timeout)
        {
            var operationContextTask = operationContextTaskCompletionSource.Task;

            return operationContextTask.ContinueWith(t =>
            {
                var context = t.Result;
                var renderProcess = context.GetCallbackChannel<IRenderProcess>();
                return renderProcess.EvaluateScript(frameId, script, timeout);
            }).Unwrap();
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            JavascriptObjectRepository = null;
            operationContextTaskCompletionSource = null;
        }

        public static NetNamedPipeBinding CreateBinding()
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            binding.MaxReceivedMessageSize = SixteenMegaBytesInBytes;
            return binding;
        }
    }
}