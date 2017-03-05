// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Concurrent;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Default field name converter
    /// Converts camel case to pascal case
    /// </summary>
    public class DefaultFieldNameConverter : IFieldNameConverter
    {
        private readonly ConcurrentDictionary<string, string> cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFieldNameConverter"/> class.
        /// </summary>
        public DefaultFieldNameConverter()
        {
            this.cache = new ConcurrentDictionary<string, string>();
        }

        /// <summary>
        /// Converts a field name to a property name
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <returns>Property name</returns>
        public string Convert(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                return fieldName;
            }

            return this.cache.GetOrAdd(fieldName, name =>
            {
                if (name.Length > 1)
                {
                    return char.ToUpperInvariant(name[0]) + name.Substring(1);
                }

                return name.ToUpperInvariant();
            });
        }
    }
}
