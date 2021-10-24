// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Reflection;

namespace CefSharp.JavascriptBinding
{
    /// <summary>
    /// Legacy Naming converter.
    /// Used by default for backwards compatability
    /// Issue #2442
    /// </summary>
    public class LegacyCamelCaseJavascriptNameConverter : IJavascriptNameConverter
    {
        string IJavascriptNameConverter.ConvertReturnedObjectPropertyAndFieldToNameJavascript(MemberInfo memberInfo)
        {
            return memberInfo.Name;
        }

        string IJavascriptNameConverter.ConvertToJavascript(MemberInfo memberInfo)
        {
            var name = memberInfo.Name;

            return char.ToLowerInvariant(name[0]) + name.Substring(1);
        }
    }
}
