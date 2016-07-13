// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Containing extensions for the <see cref="Type"/> object.
    /// </summary>
    internal static class ModelBindingExtensions
    {
        /// <summary>
        /// Creates an instance of <paramref name="type"/> and cast it to <typeparamref name="T"/>.
        /// </summary>
        /// <param name="type">The type to create an instance of.</param>
        /// <param name="nonPublic"><see langword="true"/> if a non-public constructor can be used, otherwise <see langword="false"/>.</param>
        public static T CreateInstance<T>(this Type type, bool nonPublic = false)
        {
            if (!typeof(T).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
            {
                throw new InvalidOperationException("Unable to create instance of " + type.GetTypeInfo().FullName + "since it can't be cast to " + typeof(T).GetTypeInfo().FullName);
            }

            return (T)CreateInstance(type, nonPublic);
        }

        /// <summary>
        /// Creates an instance of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to create an instance of.</param>
        /// <param name="nonPublic"><see langword="true"/> if a non-public constructor can be used, otherwise <see langword="false"/>.</param>
        public static object CreateInstance(this Type type, bool nonPublic = false)
        {
            return CreateInstanceInternal(type, nonPublic);
        }

        /// <summary>
        /// returns the assembly that the type belongs to
        /// </summary>
        /// <param name="source"></param>
        /// <returns> The assembly that contains the type </returns>
        public static Assembly GetAssembly(this Type source)
        {
            return source.GetTypeInfo().Assembly;
        }

        /// <summary>
        /// Checks if a type is an array or not
        /// </summary>
        /// <param name="source">The type to check.</param>
        /// <returns><see langword="true" /> if the type is an array, otherwise <see langword="false" />.</returns>
        public static bool IsArray(this Type source)
        {

            return source.GetTypeInfo().BaseType == typeof(Array);
        }

        /// <summary>
        /// Is the type a collection, array or enumerable
        /// </summary>
        /// <param name="source">source type</param>
        /// <returns>return if collection, array or enumerable</returns>
        public static bool IsCollectionOrArray(this Type source)
        {
            return source.IsCollection() || source.IsArray() || source.IsEnumerable();
        }

        /// <summary>
        /// Determines whether the <paramref name="genericType"/> is assignable from
        /// <paramref name="givenType"/> taking into account generic definitions
        /// </summary>
        /// <remarks>
        /// Borrowed from: http://tmont.com/blargh/2011/3/determining-if-an-open-generic-type-isassignablefrom-a-type
        /// </remarks>
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            if (givenType == null || genericType == null)
            {
                return false;
            }
            return givenType == genericType
                || givenType.MapsToGenericTypeDefinition(genericType)
                || givenType.HasInterfaceThatMapsToGenericTypeDefinition(genericType)
                || givenType.GetTypeInfo().BaseType.IsAssignableToGenericType(genericType);
        }

        /// <summary>
        /// Checks if a type is an collection or not
        /// </summary>
        /// <param name="source">The type to check.</param>
        /// <returns><see langword="true" /> if the type is an collection, otherwise <see langword="false" />.</returns>
        public static bool IsCollection(this Type source)
        {
            var collectionType = typeof(ICollection<>);

            return source.GetTypeInfo().IsGenericType && source
                .GetInterfaces()
                .Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == collectionType);
        }

        /// <summary>
        /// Checks if a type is enumerable or not
        /// </summary>
        /// <param name="source">The type to check.</param>
        /// <returns><see langword="true" /> if the type is an enumerable, otherwise <see langword="false" />.</returns>
        public static bool IsEnumerable(this Type source)
        {
            var enumerableType = typeof(IEnumerable<>);

            return source.GetTypeInfo().IsGenericType && source.GetGenericTypeDefinition() == enumerableType;
        }

        /// <summary>
        /// Determines if a type is numeric.  Nullable numeric types are considered numeric.
        /// </summary>
        /// <remarks>
        /// Boolean is not considered numeric.
        /// </remarks>
        public static bool IsNumeric(this Type source)
        {
            if (source == null)
            {
                return false;
            }

            var underlyingType = Nullable.GetUnderlyingType(source) ?? source;

            if (underlyingType.GetTypeInfo().IsEnum)
            {
                return false;
            }

            switch (underlyingType.GetTypeCode())
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                return true;
                default:
                return false;
            }
        }

        /// <summary>
        /// Filters our all types not assignable to <typeparamref name="TType"/>.
        /// </summary>
        /// <typeparam name="TType">The type that all resulting <see cref="Type"/> should be assignable to.</typeparam>
        /// <param name="types">An <see cref="IEnumerable{T}"/> of <see cref="Type"/> instances that should be filtered.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Type"/> instances.</returns>
        public static IEnumerable<Type> NotOfType<TType>(this IEnumerable<Type> types)
        {
            return types.Where(t => !typeof(TType).IsAssignableFrom(t));
        }

        private static bool HasInterfaceThatMapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return givenType
                .GetInterfaces()
                .Where(it => it.GetTypeInfo().IsGenericType)
                .Any(it => it.GetGenericTypeDefinition() == genericType);
        }

        private static bool MapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return genericType.GetTypeInfo().IsGenericTypeDefinition
                && givenType.GetTypeInfo().IsGenericType
                && givenType.GetGenericTypeDefinition() == genericType;
        }

        public static TypeCode GetTypeCode(this Type type)
        {
            if (type == typeof(bool))
                return TypeCode.Boolean;
            else if (type == typeof(char))
                return TypeCode.Char;
            else if (type == typeof(sbyte))
                return TypeCode.SByte;
            else if (type == typeof(byte))
                return TypeCode.Byte;
            else if (type == typeof(short))
                return TypeCode.Int16;
            else if (type == typeof(ushort))
                return TypeCode.UInt16;
            else if (type == typeof(int))
                return TypeCode.Int32;
            else if (type == typeof(uint))
                return TypeCode.UInt32;
            else if (type == typeof(long))
                return TypeCode.Int64;
            else if (type == typeof(ulong))
                return TypeCode.UInt64;
            else if (type == typeof(float))
                return TypeCode.Single;
            else if (type == typeof(double))
                return TypeCode.Double;
            else if (type == typeof(decimal))
                return TypeCode.Decimal;
            else if (type == typeof(DateTime))
                return TypeCode.DateTime;
            else if (type == typeof(string))
                return TypeCode.String;
            else if (type.GetTypeInfo().IsEnum)
                return GetTypeCode(Enum.GetUnderlyingType(type));
            else
                return TypeCode.Object;
        }

        private static object CreateInstanceInternal(Type type, bool nonPublic = false)
        {
            var constructor = type.GetDefaultConstructor(nonPublic);

            if (constructor == null)
            {
                throw new MissingMethodException("No parameterless constructor defined for this object.");
            }

            return constructor.Invoke(new object[0]);
        }

        private static ConstructorInfo GetDefaultConstructor(this Type type, bool nonPublic = false)
        {
            var typeInfo = type.GetTypeInfo();

            var constructors = typeInfo.DeclaredConstructors;

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                if (parameters.Length > 0)
                {
                    continue;
                }

                if (!constructor.IsPrivate || nonPublic)
                {
                    return constructor;
                }
            }

            return null;
        }
    }
}
