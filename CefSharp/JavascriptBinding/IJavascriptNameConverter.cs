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
    }
}
