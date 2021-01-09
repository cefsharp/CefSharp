// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CefSharp.Internals.Json
{
    /// <summary>
    /// DevTools Json Enum Converter Factory
    /// </summary>
    public class JsonEnumConverterFactory : JsonConverterFactory
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            if(typeToConvert.IsEnum)
            {
                return true;
            }

            var nullType = Nullable.GetUnderlyingType(typeToConvert);

            return nullType?.IsEnum ?? false;
        }

        /// <inheritdoc/>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(JsonEnumConverter<>).MakeGenericType(typeToConvert),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null);

            return converter;
        }

        public static object ConvertStringToEnum(string val, Type typeToConvert)
        {
            foreach (var name in Enum.GetNames(typeToConvert))
            {
                var attribute = typeToConvert.GetField(name)
                    .GetCustomAttributes(false)
                    .OfType<JsonPropertyNameAttribute>()
                    .Single();

                if (attribute.Name == val)
                {
                    return Enum.Parse(typeToConvert, name);
                }
            }

            throw new JsonException("Unable to convert Enum");
        }

        public static string ConvertEnumToString(object value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            var attribute = type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<JsonPropertyNameAttribute>()
                .Single();

            return attribute.Name;
        }
    }
}
