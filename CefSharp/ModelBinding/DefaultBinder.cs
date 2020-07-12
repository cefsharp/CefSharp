// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Default binder - used as a fallback when a specific modelbinder
    /// is not available.
    /// </summary>
    public class DefaultBinder : IBinder
    {
        private static readonly MethodInfo ToArrayMethodInfo = typeof(Enumerable).GetMethod("ToArray", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// Bind to the given model type
        /// </summary>
        /// <param name="obj">object to be converted into a model</param>
        /// <param name="targetType">the target param type</param>
        /// <returns>Bound model</returns>
        public virtual object Bind(object obj, Type targetType)
        {
            if (obj == null)
            {
                if (targetType.IsValueType)
                {
                    //For value types (int, double, etc) we cannot return null,
                    //we need to return the default value for that type.
                    return Activator.CreateInstance(targetType);
                }
                return null;
            }

            var objType = obj.GetType();

            // If the object can be directly assigned to the modelType then return immediately. 
            if (targetType.IsAssignableFrom(objType))
            {
                return obj;
            }

            if (targetType.IsEnum && targetType.IsEnumDefined(obj))
            {
                return Enum.ToObject(targetType, obj);
            }

            var typeConverter = TypeDescriptor.GetConverter(objType);

            // If the object can be converted to the modelType (eg: double to int)
            if (typeConverter.CanConvertTo(targetType))
            {
                return typeConverter.ConvertTo(obj, targetType);
            }

            if (targetType.IsCollection() || targetType.IsArray() || targetType.IsEnumerable())
            {
                return BindCollection(targetType, objType, obj);
            }

            return BindObject(targetType, objType, obj);
        }

        /// <summary>
        /// Bind collection.
        /// </summary>
        /// <param name="targetType">the target param type.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="obj">object to be converted into a model.</param>
        /// <returns>
        /// An object.
        /// </returns>
        protected virtual object BindCollection(Type targetType, Type objType, object obj)
        {
            var collection = obj as ICollection;

            if (collection == null)
            {
                return null;
            }

            Type genericType = null;

            // Make sure it has a generic type
            if (targetType.GetTypeInfo().IsGenericType)
            {
                genericType = targetType.GetGenericArguments().FirstOrDefault();
            }
            else
            {
                var ienumerable = targetType.GetInterfaces().Where(i => i.GetTypeInfo().IsGenericType).FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                genericType = ienumerable == null ? null : ienumerable.GetGenericArguments().FirstOrDefault();
            }

            if (genericType == null)
            {
                // If we don't have a generic type then just use object
                genericType = typeof(object);
            }

            var modelType = typeof(List<>).MakeGenericType(genericType);
            var model = (IList)Activator.CreateInstance(modelType);
            var list = (IList<object>)obj;

            for (var i = 0; i < collection.Count; i++)
            {
                var val = list.ElementAtOrDefault(i);

                //Previously we only called bind for IDictionary<string, object>
                // and IList<object>, we now bind for all values to allow for
                // type conversion like int -> double where javascript gives
                // us a mixed array of types, some int, some double (Issue #3129)
                var result = Bind(val, genericType);
                model.Add(result);
            }

            if (targetType.IsArray())
            {
                var genericToArrayMethod = ToArrayMethodInfo.MakeGenericMethod(new[] { genericType });
                return genericToArrayMethod.Invoke(null, new[] { model });
            }

            return model;
        }

        /// <summary>
        /// Bind object.
        /// </summary>
        /// <param name="targetType">the target param type.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="obj">object to be converted into a model.</param>
        /// <returns>
        /// An object.
        /// </returns>
        protected virtual object BindObject(Type targetType, Type objType, object obj)
        {
            var model = Activator.CreateInstance(targetType, true);

            // If the object type is a dictionary (we're using ExpandoObject instead of Dictionary now)
            // Then attempt to bind all the members
            if (typeof(IDictionary<string, object>).IsAssignableFrom(objType))
            {
                var dictionary = (IDictionary<string, object>)obj;
                var members = BindingMemberInfo.Collect(targetType).ToList();

                foreach (var modelProperty in members)
                {
                    object val;

                    if (dictionary.TryGetValue(modelProperty.Name, out val))
                    {
                        var propertyVal = Bind(val, modelProperty.Type);

                        modelProperty.SetValue(model, propertyVal);
                    }
                }
            }

            return model;
        }
    }
}
