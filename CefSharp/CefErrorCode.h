// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    /// <summary>
    /// Lists some of the error codes that can be reported by CEF.
    /// </summary>
    public enum class CefErrorCode
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        None = ERR_NONE,

        /// <summary>
        /// A request was aborted, possibly by the user.
        /// </summary>
        Aborted = ERR_ABORTED,
    };
}
