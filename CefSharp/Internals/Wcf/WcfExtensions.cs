// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ServiceModel.Description;

namespace CefSharp.Internals.Wcf
{
    internal static class WcfExtensions
    {
        public static void ApplyOperationBehavior<T>(this ServiceDescription description,
            Func<OperationDescription, T> behaviorFactory,
            Action<T> behaviorManipulation)
                where T : class, IOperationBehavior
        {
            foreach (ServiceEndpoint ep in description.Endpoints)
            {
                ep.ApplyOperationBehavior(behaviorFactory, behaviorManipulation);
            }
        }

        public static void ApplyOperationBehavior<T>(this ServiceEndpoint endpoint,
            Func<OperationDescription, T> behaviorFactory,
            Action<T> behaviorManipulation)
                where T : class, IOperationBehavior
        {
            foreach (OperationDescription op in endpoint.Contract.Operations)
            {
                T behavior = op.Behaviors.Find<T>();
                if (behavior == null)
                {
                    behavior = behaviorFactory(op);
                    op.Behaviors.Add(behavior);
                }
                behaviorManipulation(behavior);
            }
        }

        public static void ApplyServiceBehavior<T>(this ServiceDescription description,
            Func<T> behaviorFactory,
            Action<T> behaviorManipulation)
                where T : class, IServiceBehavior
        {
            T behavior = description.Behaviors.Find<T>();
            if (behavior == null)
            {
                behavior = behaviorFactory();
                description.Behaviors.Add(behavior);
            }
            behaviorManipulation(behavior);
        }
    }
}
