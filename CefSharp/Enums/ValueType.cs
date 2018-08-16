// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Enums
{
    /// <summary>
    /// Value types supported by <see cref="IValue"/>
    /// </summary>
    public enum ValueType
    {
        Invalid = 0,
        Null = 1,
        Bool = 2,
        Int = 3,
        Double = 4,
        String = 5,
        Binary = 6,
        Dictionary = 7,
        List = 8,
    }
}
