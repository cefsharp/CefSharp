// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Containing extensions for the <see cref="Type"/> object.
    /// </summary>
    internal static class ModelBindingExtensions
    {
        /// <summary>
        /// Checks if a type is an array or not
        /// </summary>
        /// <param name="source">The type to check.</param>
        /// <returns><see langword="true" /> if the type is an array, otherwise <see langword="false" />.</returns>
        public static bool IsArray(this Type source)
        {
            return ReflectionHelpers.GetBaseType(source) == typeof(Array);
        }

        /// <summary>
        /// Checks if a type is an collection or not
        /// </summary>
        /// <param name="source">The type to check.</param>
        /// <returns><see langword="true" /> if the type is a collection, otherwise <see langword="false" />.</returns>
        public static bool IsCollection(this Type source)
        {
            var collectionType = typeof(ICollection<>);

            return ReflectionHelpers.IsGenericType(source) && source
                .GetInterfaces()
                .Any(i => ReflectionHelpers.IsGenericType(i) && i.GetGenericTypeDefinition() == collectionType);
        }

        /// <summary>
        /// Checks if a type is enumerable or not
        /// </summary>
        /// <param name="source">The type to check.</param>
        /// <returns><see langword="true" /> if the type is an enumerable, otherwise <see langword="false" />.</returns>
        public static bool IsEnumerable(this Type source)
        {
            var enumerableType = typeof(IEnumerable<>);

            return ReflectionHelpers.IsGenericType(source) && source.GetGenericTypeDefinition() == enumerableType;
        }
    }
}
