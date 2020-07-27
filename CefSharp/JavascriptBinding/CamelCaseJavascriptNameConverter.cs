// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Linq;
using System.Reflection;

namespace CefSharp.JavascriptBinding
{
    /// <summary>
    /// CamelCaseJavascriptNameConverter converts .Net property/method names
    /// to camcel case.
    /// </summary>
    public class CamelCaseJavascriptNameConverter : IJavascriptNameConverter
    {
        string IJavascriptNameConverter.ConvertToJavascript(MemberInfo memberInfo)
        {
            return ConvertToJavascript(memberInfo);
        }

        /// <summary>
        /// Get the javascript name for the property/field/method.
        /// Typically this would be based on <see cref="MemberInfo.Name"/>
        /// </summary>
        /// <param name="memberInfo">property/field/method</param>
        /// <returns>javascript name</returns>
        protected virtual string ConvertToJavascript(MemberInfo memberInfo)
        {
            return ConvertMemberInfoNameToCamelCase(memberInfo);
        }

        string IJavascriptNameConverter.ConvertReturnedObjectPropertyAndFieldToNameJavascript(MemberInfo memberInfo)
        {
            return ConvertReturnedObjectPropertyAndFieldToNameJavascript(memberInfo);
        }

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
        protected virtual string ConvertReturnedObjectPropertyAndFieldToNameJavascript(MemberInfo memberInfo)
        {
            return ConvertMemberInfoNameToCamelCase(memberInfo);
        }

        /// <summary>
        /// Converts the <see cref="MemberInfo.Name"/> to CamelCase
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>camelcased name</returns>
        protected static string ConvertMemberInfoNameToCamelCase(MemberInfo memberInfo)
        {
            var name = memberInfo.Name;

            // camelCase says that if the string is only one character that it is preserved.
            if (name.Length == 1)
            {
                return name;
            }

            // camelCase says that if the entire string is uppercase to preserve it.
            //TODO: We need to cache these values to avoid the cost of validating this
            if (name.All(char.IsUpper))
            {
                return name;
            }

            var firstHalf = name.Substring(0, 1);
            var remainingHalf = name.Substring(1);

            return firstHalf.ToLowerInvariant() + remainingHalf;
        }
    }
}
