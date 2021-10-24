// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Text.RegularExpressions;

namespace CefSharp.Internals
{
    /// <summary>
    /// String validation
    /// </summary>
    public static class StringCheck
    {
        /// <summary>
        /// Regex check to ensure string contains only letters, numbers and underscores.
        /// </summary>
        /// <param name="stringToCheck"></param>
        /// <returns>false if string is invalid</returns>
        public static bool IsLettersAndNumbers(string stringToCheck)
        {
            if (string.IsNullOrWhiteSpace(stringToCheck))
            {
                return false;
            }

            return Regex.IsMatch(stringToCheck, @"^\w+$");
        }

        /// <summary>
        /// Return true if the first chracter of the specified string is lowercase
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>returns false if empty or null, otherwise true if first character is lowercase</returns>
        public static bool IsFirstCharacterLowercase(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (char.IsLetter(str[0]))
                {
                    return char.IsLower(str[0]);
                }
            }

            return false;
        }
    }
}
