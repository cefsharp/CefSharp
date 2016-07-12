// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Binds incoming request data to a model type
    /// </summary>
    public interface IBinder
    {
        /// <summary>
        /// Bind to the given model type
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="modelType">Model type to bind to</param>
        /// <param name="configuration">The <see cref="BindingConfig"/> that should be applied during binding.</param>
        /// <param name="blackList">Blacklisted property names</param>
        /// <returns>Bound model</returns>
        object Bind(IDictionary<string, object> objectDictionary, Type modelType, BindingConfig configuration, params string[] blackList);
    }
}