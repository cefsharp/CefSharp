// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
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

                    var enumVal = Parse(enumType.GetElementType(), str);

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

            return Parse(enumType, input);

            throw new DevToolsClientException("No Matching Enum Value Found for " + input);

            static object Parse(Type enumType, string input)
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

            var properties = GetType().GetProperties();

            foreach (var prop in properties)
            {
                var dataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(prop, typeof(DataMemberAttribute), false);
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

                if (propertyValueType.IsEnum)
                {
                    var enumMember = propertyValueType.GetMember(propertyValue.ToString()).FirstOrDefault();
                    if (enumMember == null)
                    {
                        throw new NullReferenceException("No matching enum found");
                    }
                    var enumMemberAttribute = (EnumMemberAttribute)Attribute.GetCustomAttribute(enumMember, typeof(EnumMemberAttribute), false);
                    propertyValue = enumMemberAttribute.Value;
                }

                dict.Add(propertyName, propertyValue);
            }

            return dict;
        }
    }
}
