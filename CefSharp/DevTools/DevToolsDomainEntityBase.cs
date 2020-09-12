// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace CefSharp.DevTools
{
    [DataContract]
    public abstract class DevToolsDomainEntityBase
    {
        public static object StringToEnum(Type enumType, string input)
        {
            if (enumType.IsArray)
            {
                if (string.IsNullOrEmpty(input) || input == "[]" || input == "[ ]")
                {
                    return null;
                    //return Array.CreateInstance(enumType.GetElementType(), 0);
                }

                var values = input.Substring(1, input.Length - 2).Split(',');

                var returnValues = Array.CreateInstance(enumType.GetElementType(), values.Length);

                for (int i = 0; i < values.Length; i++)
                {
                    var str = values[i].Trim('\r', '\n', '"', ' ');

                    var enumVal = StringToEnumInternal(enumType.GetElementType(), str);

                    returnValues.SetValue(enumVal, i);
                }

                return returnValues;
            }

            if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (string.IsNullOrEmpty(input))
                {
                    return null;
                }

                enumType = Nullable.GetUnderlyingType(enumType);
            }

            return StringToEnumInternal(enumType, input);
        }

        private static object StringToEnumInternal(Type enumType, string input)
        {
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == input)
                {
                    return Enum.Parse(enumType, name);
                }
            }

            return (Enum.GetValues(enumType).GetValue(0));
        }

        public static string EnumToString(Enum e)
        {
            var memberInfo = e.GetType().GetMember(e.ToString()).FirstOrDefault();

            var enumMemberAttribute = (EnumMemberAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(EnumMemberAttribute), false);

            return enumMemberAttribute.Value;
        }

        public static string EnumToString(Array enumArray)
        {
            var returnValue = "[";

            foreach (var e in enumArray)
            {
                var memberInfo = e.GetType().GetMember(e.ToString()).FirstOrDefault();

                var enumMemberAttribute = (EnumMemberAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(EnumMemberAttribute), false);

                returnValue += enumMemberAttribute.Value + ",";
            }

            returnValue += returnValue.Substring(0, returnValue.Length - 1) + "]";

            return returnValue;
        }

        public IDictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>();

            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var dataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(prop, typeof(DataMemberAttribute), false);

                //Only add members that have DataMemberAttribute
                if (dataMemberAttribute == null)
                {
                    continue;
                }

                var propertyName = dataMemberAttribute.Name;
                var propertyRequired = dataMemberAttribute.IsRequired;
                var propertyValue = prop.GetValue(this);

                if (propertyRequired && propertyValue == null)
                {
                    throw new DevToolsClientException(prop.Name + " is required");
                }

                //Not required and value null, don't add to dictionary
                if (propertyValue == null)
                {
                    continue;
                }

                var propertyValueType = propertyValue.GetType();

                if (typeof(DevToolsDomainEntityBase).IsAssignableFrom(propertyValueType))
                {
                    propertyValue = ((DevToolsDomainEntityBase)(propertyValue)).ToDictionary();
                }

                dict.Add(propertyName, propertyValue);
            }

            return dict;
        }
    }
}
