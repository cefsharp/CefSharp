// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using CefSharp.ModelBinding;

namespace CefSharp.Example.ModelBinding
{
    public class PropertyInterceptorLogger : IPropertyInterceptor
    {
        object IPropertyInterceptor.GetIntercept(Func<object> property, string propertyName)
        {
            object result = property();
            Debug.WriteLine("GetIntercept " + propertyName);
            return result;
        }

        public void SetIntercept(Action<object> property, object parameter, string propertName)
        {
            property(parameter);
            Debug.WriteLine("SetIntercept " + propertName);
        }
    }
}
