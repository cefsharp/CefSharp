// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Converts input params into complex .Net types (can also be used for type conversion).
    /// This feature is similar in concept to ASP.NET MVC Model Binding.
    /// Objects passed from javascript are represented as <see cref="System.Collections.Generic.IDictionary{TKey, TValue}"/>
    /// and arrays/lists as <see cref="System.Collections.Generic.IList{T}"/>
    /// See <see cref="DefaultBinder"/> for the default implementation.
    /// </summary>
    /// <remarks>
    /// A model binder can be specified in <see cref="BindingOptions.Binder"/> and passed into
    /// <see cref="IJavascriptObjectRepository.Register(string, object, bool, BindingOptions)"/>
    /// </remarks>
    public interface IBinder
    {
        /// <summary>
        /// Bind to the given model type, can also be used for type conversion e.g. int to uint
        /// </summary>
        /// <param name="obj">object to be converted into a model</param>
        /// <param name="targetParamType">the target param type</param>
        /// <returns>if the modelType is directly assignable then do so, otherwise perform a conversion
        /// or create a complex object that matches <paramref name="targetParamType"/></returns>
        object Bind(object obj, Type targetParamType);
    }
}
