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
    public abstract class DevToolsDomainBase
    {
#if NETCOREAPP
        protected string EnumToString(Enum val)
        {
            return Internals.Json.JsonEnumConverterFactory.ConvertEnumToString(val);
        }

        protected IEnumerable<string> EnumToString(CefSharp.DevTools.Network.ContentEncoding[] values)
        {
            foreach (var val in values)
            {
                yield return Internals.Json.JsonEnumConverterFactory.ConvertEnumToString(val);
            }
        }

        protected IEnumerable<string> EnumToString(CefSharp.DevTools.Emulation.DisabledImageType[] values)
        {
            foreach (var val in values)
            {
                yield return Internals.Json.JsonEnumConverterFactory.ConvertEnumToString(val);
            }
        }

        protected IEnumerable<string> EnumToString(PermissionType[] values)
        {
            foreach (var val in values)
            {
                yield return Internals.Json.JsonEnumConverterFactory.ConvertEnumToString(val);
            }
        }

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

        protected IEnumerable<string> EnumToString(PermissionType[] values)
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
