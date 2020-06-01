// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Reflection;

namespace CefSharp.JavascriptBinding
{
    public class CamelCaseJavascriptNameConverter : IJavascriptNameConverter
    {
        string IJavascriptNameConverter.ConvertToJavascript(MemberInfo memberInfo)
        {
            return ConvertStringToCamelCase(memberInfo.Name);
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
