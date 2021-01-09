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
    /// <summary>
    /// Common Base class for DevTools Domain Model classes
    /// </summary>
    [DataContract]
    public abstract class DevToolsDomainEntityBase
    {
#if NETCOREAPP
        internal static string EnumToString(Enum e)
        {
            var memberInfo = e.GetType().GetMember(e.ToString()).FirstOrDefault();

            var enumMemberAttribute = (System.Text.Json.Serialization.JsonPropertyNameAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(System.Text.Json.Serialization.JsonPropertyNameAttribute), false);

            return enumMemberAttribute.Name;
        }

        internal static string EnumToString(Array enumArray)
        {
            var returnValue = "[";

            foreach (var e in enumArray)
            {
                var memberInfo = e.GetType().GetMember(e.ToString()).FirstOrDefault();

                var enumMemberAttribute = (System.Text.Json.Serialization.JsonPropertyNameAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(System.Text.Json.Serialization.JsonPropertyNameAttribute), false);

                returnValue += enumMemberAttribute.Name + ",";
            }

            returnValue += returnValue.Substring(0, returnValue.Length - 1) + "]";

            return returnValue;
        }
#else
        internal static object StringToEnum(Type enumType, string input)
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

        internal static string EnumToString(Enum e)
        {
            var memberInfo = e.GetType().GetMember(e.ToString()).FirstOrDefault();

            var enumMemberAttribute = (EnumMemberAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(EnumMemberAttribute), false);

            return enumMemberAttribute.Value;
        }

        internal static string EnumToString(Array enumArray)
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
#endif



#if NETCOREAPP
        public IDictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>();

            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var propertyAttribute = (System.Text.Json.Serialization.JsonPropertyNameAttribute)Attribute.GetCustomAttribute(prop, typeof(System.Text.Json.Serialization.JsonPropertyNameAttribute), false);

                //Only add members that have JsonPropertyNameAttribute
                if (propertyAttribute == null)
                {
                    continue;
                }

                var propertyName = propertyAttribute.Name;
                var propertyRequired = Attribute.IsDefined(prop, typeof(System.Diagnostics.CodeAnalysis.DisallowNullAttribute));
                
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
                else if (propertyValueType.IsEnum)
                {
                    propertyValue = EnumToString((Enum)propertyValue);
                }
                else if(propertyValueType.IsGenericType)
                {
                    var nullableType = Nullable.GetUnderlyingType(propertyValueType);
                    if(nullableType != null && nullableType.IsEnum)
                    {
                        propertyValue = EnumToString((Enum)propertyValue);
                    }
                }
                else if(propertyValueType.IsArray && propertyValueType.GetElementType().IsEnum)
                {
                    propertyValue = EnumToString((Array)propertyValue);
                }

                dict.Add(propertyName, propertyValue);
            }

            return dict;
        }
#else
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
#endif
    }
}
