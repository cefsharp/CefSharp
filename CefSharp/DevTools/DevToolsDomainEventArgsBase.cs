// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.Serialization;

namespace CefSharp.DevTools
{
    /// <summary>
    /// DevToolsDomainEventArgsBase
    /// </summary>
    [DataContract]
    public abstract class DevToolsDomainEventArgsBase : EventArgs
    {
#if !NETCOREAPP
        public static object StringToEnum(Type enumType, string input)
        {
            return DevToolsDomainEntityBase.StringToEnum(enumType, input);
        }

        public static string EnumToString(Enum e)
        {
            return DevToolsDomainEntityBase.EnumToString(e);
        }
#endif
    }
}
