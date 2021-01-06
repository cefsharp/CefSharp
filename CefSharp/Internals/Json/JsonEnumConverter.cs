// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CefSharp.Internals.Json
{
    /// <summary>
    /// System.Text.Json Enum Converter for the DevToolsClient
    /// Only handles enums as strings and converts from .Net naming
    /// to the naming convention used by DevTools protocol
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    /// <remarks>
    /// There aren't any Type constraints on T currently, we just rely on the
    /// <see cref="JsonEnumConverterFactory"/> to only use this for
    /// Enum and Nullable Enum
    /// </remarks>
    public class JsonEnumConverter<T> : JsonConverter<T>
    {
        /// <inheritdoc/>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var nullableType = Nullable.GetUnderlyingType(typeToConvert);
            var val = reader.GetString();

            if (nullableType == null)
            {
                return (T)JsonEnumConverterFactory.ConvertStringToEnum(val, typeToConvert);
            }

            if(string.IsNullOrEmpty(val))
            {
                return default;
            }

            return (T)JsonEnumConverterFactory.ConvertStringToEnum(val, nullableType);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            //Not called when the Nullable enum value is null so we don't need to perform any sort of null checking
            var str = JsonEnumConverterFactory.ConvertEnumToString(value);

            writer.WriteStringValue(str);
        }        
    }
}
