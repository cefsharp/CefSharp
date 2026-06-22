// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CefSharp.DevTools.Browser;
using CefSharp.DevTools.Network;

namespace CefSharp.DevTools
{
    /// <summary>
    /// DevTools Domain base class
    /// Provides some basic helper methods
    /// </summary>
    public abstract class DevToolsDomainBase
    {
#if NETCOREAPP
        /// <summary>
        /// Convert Enum to String
        /// </summary>
        /// <param name="val">enum</param>
        /// <returns>string</returns>
        protected string EnumToString(Enum val)
        {
            return Internals.Json.JsonEnumConverterFactory.ConvertEnumToString(val);
        }

        /// <summary>
        /// Enum to string
        /// </summary>
        /// <param name="values">array of type <see cref="CefSharp.DevTools.Network.ContentEncoding"/></param>
        /// <returns>enumerable string</returns>
        protected IEnumerable<string> EnumToString(CefSharp.DevTools.Network.ContentEncoding[] values)
        {
            foreach (var val in values)
            {
                yield return Internals.Json.JsonEnumConverterFactory.ConvertEnumToString(val);
            }
        }

        /// <summary>
        /// Enum to string
        /// </summary>
        /// <param name="values">array of type <see cref="CefSharp.DevTools.Emulation.DisabledImageType"/></param>
        /// <returns>enumerable string</returns>
        protected IEnumerable<string> EnumToString(CefSharp.DevTools.Emulation.DisabledImageType[] values)
        {
            foreach (var val in values)
            {
                yield return Internals.Json.JsonEnumConverterFactory.ConvertEnumToString(val);
            }
        }

        /// <summary>
        /// Enum to string
        /// </summary>
        /// <param name="values">array of type <see cref="PermissionType"/></param>
        /// <returns>enumerable string</returns>
        protected IEnumerable<string> EnumToString(PermissionType[] values)
        {
            foreach (var val in values)
            {
                yield return Internals.Json.JsonEnumConverterFactory.ConvertEnumToString(val);
            }
        }

        /// <summary>
        /// Enum to string
        /// </summary>
        /// <param name="values">array of type <see cref="CefSharp.DevTools.DOMDebugger.CSPViolationType"/></param>
        /// <returns>enumerable string</returns>
        protected IEnumerable<string> EnumToString(CefSharp.DevTools.DOMDebugger.CSPViolationType[] values)
        {
            foreach (var val in values)
            {
                yield return Internals.Json.JsonEnumConverterFactory.ConvertEnumToString(val);
            }
        }
#else
        protected string EnumToString(Enum val)
        {
            var memInfo = val.GetType().GetMember(val.ToString());
            var dataMemberAttribute = (EnumMemberAttribute)Attribute.GetCustomAttribute(memInfo[0], typeof(EnumMemberAttribute), false);

            return dataMemberAttribute.Value;
        }

        //TODO: Create a generic function that converts enum array to string
        protected IEnumerable<string> EnumToString(ContentEncoding[] values)
        {
            foreach (var val in values)
            {
                var memInfo = val.GetType().GetMember(val.ToString());
                var dataMemberAttribute = (EnumMemberAttribute)Attribute.GetCustomAttribute(memInfo[0], typeof(EnumMemberAttribute), false);

                yield return dataMemberAttribute.Value;
            }
        }

        protected IEnumerable<string> EnumToString(Browser.PermissionType[] values)
        {
            foreach (var val in values)
            {
                var memInfo = val.GetType().GetMember(val.ToString());
                var dataMemberAttribute = (EnumMemberAttribute)Attribute.GetCustomAttribute(memInfo[0], typeof(EnumMemberAttribute), false);

                yield return dataMemberAttribute.Value;
            }
        }

        protected IEnumerable<string> EnumToString(CefSharp.DevTools.Emulation.DisabledImageType[] values)
        {
            foreach (var val in values)
            {
                var memInfo = val.GetType().GetMember(val.ToString());
                var dataMemberAttribute = (EnumMemberAttribute)Attribute.GetCustomAttribute(memInfo[0], typeof(EnumMemberAttribute), false);

                yield return dataMemberAttribute.Value;
            }
        }

        protected IEnumerable<string> EnumToString(CefSharp.DevTools.DOMDebugger.CSPViolationType[] values)
        {
            foreach (var val in values)
            {
                var memInfo = val.GetType().GetMember(val.ToString());
                var dataMemberAttribute = (EnumMemberAttribute)Attribute.GetCustomAttribute(memInfo[0], typeof(EnumMemberAttribute), false);

                yield return dataMemberAttribute.Value;
            }
        }
#endif

        protected string ToBase64String(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}
