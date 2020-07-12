// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.ModelBinding
{
    public class CamelCaseJavascriptNameConverter : IJavascriptNameConverter
    {
        string IJavascriptNameConverter.ConvertToJavascript(string name)
        {
            return ConvertStringToCamelCase(name);
        }

        private static string ConvertStringToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }
    }
}
