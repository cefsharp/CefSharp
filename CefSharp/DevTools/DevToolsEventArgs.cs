// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System;
#if !NETCOREAPP
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
#endif

namespace CefSharp.DevTools
{
    /// <summary>
    /// DevTools Event EventAargs
    /// </summary>
    public class DevToolsEventArgs : EventArgs
    {
        /// <summary>
        /// Event Name
        /// </summary>
        public string EventName { get; private set; }

        /// <summary>
        /// Event paramaters as Json string
        /// </summary>
        public string ParametersAsJsonString { get; private set; }

        public DevToolsEventArgs(string eventName, string paramsAsJsonString)
        {
            EventName = eventName;
            ParametersAsJsonString = paramsAsJsonString;
        }

        internal T DeserializeJson<T>()
        {
#if NETCOREAPP
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
            };

            options.Converters.Add(new Internals.Json.JsonEnumConverterFactory());

            return System.Text.Json.JsonSerializer.Deserialize<T>(ParametersAsJsonString, options);
#else
            var bytes = Encoding.UTF8.GetBytes(ParametersAsJsonString);
            using (var ms = new MemoryStream(bytes))
            {
                var settings = new DataContractJsonSerializerSettings();
                settings.UseSimpleDictionaryFormat = true;

                var dcs = new DataContractJsonSerializer(typeof(T), settings);
                return (T)dcs.ReadObject(ms);
            }
#endif
        }
    }
}
