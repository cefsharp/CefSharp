// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CefSharp.Web
{
    /// <summary>
    /// Represents a JsonString that is converted to a V8 Object
    /// Used as a return type of bound methods
    /// </summary>
    public class JsonString
    {
        public string Json { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="json">JSON string</param>
        public JsonString(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException("json");
            }

            Json = json;
        }

        /// <summary>
        /// Create a JsonString from the specfied object using the build in <see cref="DataContractJsonSerializer"/>
        /// </summary>
        /// <param name="obj">object to seriaize</param>
        /// <param name="settings">optional settings</param>
        /// <returns>If <paramref name="obj"/> is null then return nulls otherwise a JsonString.</returns>
        public static JsonString FromObject(object obj, DataContractJsonSerializerSettings settings = null)
        {
            if (obj == null)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                var dataContractJsonSerializer = new DataContractJsonSerializer(obj.GetType(), settings);
                dataContractJsonSerializer.WriteObject(ms, obj);

                var jsonString = Encoding.UTF8.GetString(ms.ToArray());

                if (string.IsNullOrEmpty(jsonString))
                {
                    return null;
                }

                return new JsonString(jsonString);
            }
        }
    }
}
