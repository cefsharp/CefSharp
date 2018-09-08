// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// JavascriptIgnoreAttribute - Methods and Properties marked with this attribute
    /// will be excluded from Javascript Binding
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = true)]
    public class JavascriptIgnoreAttribute : Attribute
    {
    }
}
