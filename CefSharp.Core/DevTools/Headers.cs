// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;

namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Request / response headers as keys / values of JSON object.
    /// </summary>
    /// <remarks>
    /// CDP uses comma seperated values to store multiple header values.
    /// Use <see cref="TryGetValues(string, out string[])"/> or <see cref="GetCommaSeparatedValues(string)"/> to get a string[]
    /// for headers that have multiple values.
    /// </remarks>
    /// Helper methods for dealing with comma separated header values based on https://github.com/dotnet/aspnetcore/blob/52eff90fbcfca39b7eb58baad597df6a99a542b0/src/Http/Http.Abstractions/src/Extensions/HeaderDictionaryExtensions.cs
    public class Headers : Dictionary<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the Headers class.
        /// </summary>
        public Headers() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Returns itself
        /// </summary>
        /// <returns>Dictionary of headers</returns>
        public Dictionary<string, string> ToDictionary()
        {
            return this;
        }

        /// <summary>
        /// Gets an array of values for the specified key. Values are comma seperated and will be split into a string[].
        /// Quoted values will not be split, and the quotes will be removed.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="values">the associated values from the dictionary separated into individual values, or null if the key is not present.</param>
        /// <returns>true if the Dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValues(string key, out string[] values)
        {
            values = null;
            string value;

            if (TryGetValue(key, out value))
            {
                var list = new List<string>();

                var valueStartIndex = -1;
                var valueEndIndex = -1;
                var inQuote = false;
                for (var i = 0; i < value.Length; i++)
                {
                    var c = value[i];

                    if (c == '\"')
                    {
                        inQuote = !inQuote;
                        continue;
                    }

                    if (!inQuote && char.IsWhiteSpace(c))
                    {
                        continue;
                    }

                    if (valueStartIndex == -1)
                    {
                        valueStartIndex = i;
                    }

                    if (!inQuote && c == ',')
                    {
                        if (valueEndIndex == -1)
                        {
                            list.Add(string.Empty);
                        }
                        else
                        {
                            list.Add(value.Substring(valueStartIndex, valueEndIndex + 1 - valueStartIndex));
                        }
                        valueStartIndex = -1;
                        valueEndIndex = -1;
                        continue;
                    }

                    valueEndIndex = i;
                }
                if (valueEndIndex == -1)
                {
                    list.Add(string.Empty);
                }
                else
                {
                    list.Add(value.Substring(valueStartIndex, valueEndIndex + 1 - valueStartIndex));
                }

                values = list.ToArray();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the associated values from the dictionary separated into individual values.
        /// Quoted values will not be split, and the quotes will be removed.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns>the associated values from the dictionary separated into individual values, or null if the key is not present.</returns>
        public string[] GetCommaSeparatedValues(string key)
        {
            string[] values;

            if (TryGetValues(key, out values))
            {
                return values;
            }

            return null;
        }

        /// <summary>
        /// Quotes any values containing commas, and then comma joins all of the values with any existing values.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="values">The header values.</param>
        public void AppendCommaSeparatedValues(string key, params string[] values)
        {
            if (TryGetValue(key, out var existingValue))
            {
                this[key] = existingValue + "," + string.Join(",", values.Select(QuoteIfNeeded));
            }
            else
            {
                SetCommaSeparatedValues(key, values);
            }
        }

        /// <summary>
        /// Quotes any values containing commas, and then comma joins all of the values.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="values">The header values.</param>
        public void SetCommaSeparatedValues(string key, params string[] values)
        {
            this[key] = string.Join(",", values.Select(QuoteIfNeeded));
        }

        private static string QuoteIfNeeded(string value)
        {
            if (!string.IsNullOrEmpty(value) &&
                value.Contains(',') &&
                (value[0] != '"' || value[value.Length - 1] != '"'))
            {
                return "\"" + value + "\"";
            }

            return value;
        }
    }
}
