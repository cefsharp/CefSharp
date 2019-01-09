// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Enums
{
    /// <summary>
    /// Value types supported by <see cref="IValue"/>
    /// </summary>
    public enum ValueType
    {
        /// <summary>
        /// Invalid type
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// Null
        /// </summary>
        Null = 1,
        /// <summary>
        /// Boolean
        /// </summary>
        Bool = 2,
        /// <summary>
        /// Integer
        /// </summary>
        Int = 3,
        /// <summary>
        /// Double
        /// </summary>
        Double = 4,
        /// <summary>
        /// String
        /// </summary>
        String = 5,
        /// <summary>
        /// Binary
        /// </summary>
        Binary = 6,
        /// <summary>
        /// Dictionary
        /// </summary>
        Dictionary = 7,
        /// <summary>
        /// List
        /// </summary>
        List = 8
    }
}
