// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using CefSharp.Enums;

namespace CefSharp
{
    /// <summary>
    /// Interface representing CefValue.
    /// </summary>
    public interface ICefValue : IDisposable
    {
        CefValueType GetCefValueType();

        /// <summary>
        /// Returns the underlying value as type bool.
        /// </summary>
        /// <returns></returns>
        bool GetBool();

        /// <summary>
        /// Returns the underlying value as type double.
        /// </summary>
        /// <returns></returns>
        double GetDouble();

        /// <summary>
        /// Returns the underlying value as type int.
        /// </summary>
        /// <returns></returns>
        int GetInt();

        /// <summary>
        /// Returns the underlying value as type string.
        /// </summary>
        /// <returns></returns>
        string GetString();

        /// <summary>
        /// Returns the underlying value as type dictionary.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, ICefValue> GetDictionary();

        /// <summary>
        /// Returns the underlying value as type list.
        /// </summary>
        /// <returns></returns>
        List<ICefValue> GetList();
    }
}
