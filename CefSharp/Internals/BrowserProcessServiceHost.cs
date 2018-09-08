// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace CefSharp.Internals
{
    public class BrowserProcessServiceHost : ServiceHost
    {
        private const long OneHundredAndTwentyEightMegaBytesInBytes = 128 * 1024 * 1024;

        public JavascriptObjectRepository JavascriptObjectRepository { get; private set; }

        public BrowserProcessServiceHost(JavascriptObjectRepository javascriptObjectRepository, int parentProcessId, int browserId, IJavascriptCallbackFactory callbackFactory)
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
            endPoint.Behaviors.Add(new JavascriptCallbackEndpointBehavior(callbackFactory));
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            JavascriptObjectRepository = null;
        }

        public static CustomBinding CreateBinding()
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            binding.MaxReceivedMessageSize = OneHundredAndTwentyEightMegaBytesInBytes;
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