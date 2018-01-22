// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

using CefSharp.Event;
using CefSharp.ModelBinding;

namespace CefSharp
{
    public interface IJavascriptObjectRepository : IDisposable
    {
        void Register(string name, object value, bool isAsync, BindingOptions options);
        bool HasBoundObjects { get; }
        bool IsBound(string name);
        event EventHandler<JavascriptBindingEventArgs> ResolveObject;
    }
}
