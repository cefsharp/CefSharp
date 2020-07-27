// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Reflection;

namespace CefSharp.JavascriptBinding
{
    /// <summary>
    /// Implement this interface to have control of how the names
    /// are converted when binding/executing javascript.
    /// </summary>
    public interface IJavascriptNameConverter
    {
        /// <summary>
        /// Get the javascript name for the property/field/method.
        /// Typically this would be based on <see cref="MemberInfo.Name"/>
        /// </summary>
        /// <param name="memberInfo">property/field/method</param>
        /// <returns>javascript name</returns>
        string ConvertToJavascript(MemberInfo memberInfo);

        /// <summary>
        /// This method exists for backwards compatability reasons, historically
        /// only the bound methods/fields/properties were converted. Objects returned
        /// from a method call were not translated. To preserve this functionality
        /// for upgrading users we split this into two methods. Typically thie method
        /// would return the same result as <see cref="ConvertToJavascript(string)"/>
        /// Issue #2442
        /// </summary>
        /// <param name="memberInfo">property/field/method</param>
        /// <returns>javascript name</returns>
        string ConvertReturnedObjectPropertyAndFieldToNameJavascript(MemberInfo memberInfo);
    }
}
