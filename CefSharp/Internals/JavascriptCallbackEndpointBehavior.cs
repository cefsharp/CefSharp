﻿// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace CefSharp.Internals
{
    internal sealed class JavascriptCallbackEndpointBehavior : IEndpointBehavior
    {
        private static readonly List<string> Methods = new List<string>
        {
            ReflectionUtils.GetMethodName<IBrowserProcess>(p => p.CallMethod(0, null, null)),
            ReflectionUtils.GetMethodName<IRenderProcess>(p => p.BeginEvaluateScriptAsync(0, 0, null, null, null, null)).Substring(5),
            ReflectionUtils.GetMethodName<IRenderProcess>(p => p.BeginJavascriptCallbackAsync(0, 0, null, null, null, null)).Substring(5),
        };

        private readonly WeakReference browserProcessWeakReference;

        public JavascriptCallbackEndpointBehavior(BrowserProcessServiceHost browserProcess)
        {
            browserProcessWeakReference = new WeakReference(browserProcess);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            foreach (var operation in endpoint.Contract.Operations.Where(o => Methods.Contains(o.Name)))
            {
                operation.Behaviors.Find<DataContractSerializerOperationBehavior>().DataContractSurrogate = new JavascriptCallbackSurrogate(browserProcessWeakReference);
            }
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }
    }
}
