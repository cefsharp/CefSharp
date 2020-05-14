// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;

namespace CefSharp.Internals
{
    public static class StringCheck
    {
        /// <summary>
        /// Regex check to ensure string contains only letters, numbers and underscores.
        /// </summary>
        /// <param name="stringToCheck"></param>
        /// <returns></returns>
        public static bool EnsureLettersAndNumbers(string stringToCheck)
        {
            if (!string.IsNullOrWhiteSpace(stringToCheck))
            {
                return Regex.IsMatch(stringToCheck, @"^\w+$");
            }

            return false;
        }
    }
}
