// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using ValueType = CefSharp.Enums.ValueType;

namespace CefSharp
{
    /// <summary>
    /// Interface representing CefValue.
    /// </summary>
    public interface IValue : IDisposable
    {
        /// <summary>
        /// Returns the underlying value type.
        /// </summary>
        /// <returns>
        /// Returns the underlying value type.
        /// </returns>
        ValueType Type { get; }

        /// <summary>
        /// Returns the underlying value as type bool.
        /// </summary>
        /// <returns>
        /// Returns the underlying value as type bool.
        /// </returns>
        bool GetBool();

        /// <summary>
        /// Returns the underlying value as type double.
        /// </summary>
        /// <returns>
        /// Returns the underlying value as type double.
        /// </returns>
        double GetDouble();

        /// <summary>
        /// Returns the underlying value as type int.
        /// </summary>
        /// <returns>
        /// Returns the underlying value as type int.
        /// </returns>
        int GetInt();

        /// <summary>
        /// Returns the underlying value as type string.
        /// </summary>
        /// <returns>
        /// Returns the underlying value as type string.
        /// </returns>
        string GetString();

        /// <summary>
        /// Returns the underlying value as type dictionary.
        /// </summary>
        /// <returns>
        /// Returns the underlying value as type dictionary.
        /// </returns>
        IDictionary<string, IValue> GetDictionary();

        /// <summary>
        /// Returns the underlying value as type list.
        /// </summary>
        /// <returns>
        /// Returns the underlying value as type list.
        /// </returns>
        IList<IValue> GetList();

        /// <summary>
        /// Returns the underlying value converted to a managed object.
        /// </summary>
        /// <returns>
        /// Returns the underlying value converted to a managed object.
        /// </returns>
        object GetObject();
    }
}
