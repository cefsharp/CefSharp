// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CefSharp.DevTools
{
    public abstract class DevToolsDomainEntityBase
    {
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
                    var enumMemberAttribute = (EnumMemberAttribute)Attribute.GetCustomAttribute(prop, typeof(EnumMemberAttribute), false);
                    propertyValue = enumMemberAttribute.Value;
                }

                dict.Add(propertyName, propertyValue);
            }

            return dict;
        }
    }
}
