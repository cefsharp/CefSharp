// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using CefSharp.ModelBinding;

namespace CefSharp.Example.ModelBinding
{
    public class PropertyInterceptorLogger : IPropertyInterceptor
    {
        object IPropertyInterceptor.InterceptGet(Func<object> propertyGetter, string propertyName)
        {
            object result = propertyGetter();
            Debug.WriteLine("InterceptGet " + propertyName);
            return result;
        }

        void IPropertyInterceptor.InterceptSet(Action<object> propertySetter, object parameter, string propertName)
        {
            Debug.WriteLine("InterceptSet " + propertName);
            propertySetter(parameter);
        }
    }
}
