// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Default binder - used as a fallback when a specific modelbinder
    /// is not available.
    /// </summary>
    public class DefaultBinder : IBinder
    {
        private readonly static MethodInfo ToArrayMethodInfo = typeof(Enumerable).GetMethod("ToArray", BindingFlags.Public | BindingFlags.Static);

        private readonly IFieldNameConverter fieldNameConverter;

        /// <summary>
        /// List of property names to be ignored
        /// </summary>
        public IEnumerable<string> BlackListedPropertyNames { get; set; }

        /// <summary>
        /// DefaultBinder constructor
        /// </summary>
        /// <param name="fieldNameConverter">used to convert field names to property names</param>
        public DefaultBinder(IFieldNameConverter fieldNameConverter)
        {
            if (fieldNameConverter == null)
            {
                throw new ArgumentNullException("fieldNameConverter");
            }

            this.fieldNameConverter = fieldNameConverter;
            BlackListedPropertyNames = new List<string>();
        }

        /// <summary>
        /// Bind to the given model type
        /// </summary>
        /// <param name="obj">object to be converted into a model</param>
        /// <param name="modelType">Model type to bind to</param>
        /// <returns>Bound model</returns>
        public virtual object Bind(object obj, Type modelType)
        {
            if(obj == null)
            {
                return null;
            }

            var objType = obj.GetType();

            // If the object can be directly assigned to the modelType then return immediately. 
            if (modelType.IsAssignableFrom(objType))
            {
                return obj;
            }

            if (modelType.IsEnum && modelType.IsEnumDefined(obj)) 
            {
                return Enum.ToObject(modelType, obj);
            }

            var typeConverter = TypeDescriptor.GetConverter(objType);
            
            // If the object can be converted to the modelType (eg: double to int)
            if (typeConverter.CanConvertTo(modelType)) 
            {
                return typeConverter.ConvertTo(obj, modelType);
            }

            Type genericType = null;
            if (modelType.IsCollection() || modelType.IsArray() || modelType.IsEnumerable())
            {
                // Make sure it has a generic type
                if (modelType.GetTypeInfo().IsGenericType)
                {
                    genericType = modelType.GetGenericArguments().FirstOrDefault();
                }
                else
                {
                    var ienumerable = modelType.GetInterfaces().Where(i => i.GetTypeInfo().IsGenericType).FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                    genericType = ienumerable == null ? null : ienumerable.GetGenericArguments().FirstOrDefault();
                }

                if (genericType == null)
                {
                    // If we don't have a generic type then just use object
                    genericType = typeof(object);
                }
            }

            var bindingContext = this.CreateBindingContext(obj, modelType, genericType);
            var destinationType = bindingContext.DestinationType;

            if (destinationType.IsCollection() || destinationType.IsArray() || destinationType.IsEnumerable())
            {
                var model = (IList)bindingContext.Model;
                var collection = obj as ICollection;

                if(collection == null)
                {
                    return null;
                }

                for (var i = 0; i < collection.Count; i++)
                {
                    var val = GetValue(bindingContext, i);

                    //If the value is null then we'll add null to the collection,
                    if (val == null)
                    {
                        //For value types like int we'll create the default value and assign that as we cannot assign null
                        model.Add(genericType.IsValueType ? Activator.CreateInstance(genericType) : null);
                    }
                    else
                    {
                        var valueType = val.GetType();
                        //If the collection item is a list or dictionary then we'll attempt to bind it
                        if (typeof(IDictionary<string, object>).IsAssignableFrom(valueType) ||
                            typeof(IList<object>).IsAssignableFrom(valueType))
                        {
                            var subModel = Bind(val, genericType);
                            model.Add(subModel);
                        }
                        else
                        { 
                            model.Add(val);
                        }
                    }
                }
            }
            else
            {
                // If the object type is a dictionary (we're using ExpandoObject instead of Dictionary now)
                // Then attempt to bind all the members
                if (typeof(IDictionary<string, object>).IsAssignableFrom(bindingContext.Object.GetType()))
                { 
                    foreach (var modelProperty in bindingContext.ValidModelBindingMembers)
                    {
                        var val = GetValue(modelProperty.Name, bindingContext);

                        if (val != null)
                        {
                            BindValue(modelProperty, val, bindingContext);
                        }
                    }
                }
            }

            if (modelType.IsArray())
            {
                var generictoArrayMethod = ToArrayMethodInfo.MakeGenericMethod(new[] { genericType });
                return generictoArrayMethod.Invoke(null, new[] { bindingContext.Model });
            }
            return bindingContext.Model;
        }

        protected virtual BindingContext CreateBindingContext(object obj, Type modelType, Type genericType)
        {
            return new BindingContext
            {
                DestinationType = modelType,
                Model = CreateModel(modelType, genericType),
                ValidModelBindingMembers = GetBindingMembers(modelType, genericType).ToList(),
                Object = obj,
                GenericType = genericType
            };
        }

        protected virtual void BindValue(BindingMemberInfo modelProperty, object obj, BindingContext context)
        {
            if(obj == null)
            {
                return;
            }

            if (modelProperty.PropertyType.IsAssignableFrom(obj.GetType()))
            {
                // Simply set the property
                modelProperty.SetValue(context.Model, obj);
            }
            else
            {
                // Cannot directly set the property attempt to bind
                var model = Bind(obj, modelProperty.PropertyType);

                modelProperty.SetValue(context.Model, model);
            }
        }

        protected virtual IEnumerable<BindingMemberInfo> GetBindingMembers(Type modelType, Type genericType)
        {
            var blackListHash = new HashSet<string>(BlackListedPropertyNames, StringComparer.Ordinal);

            return BindingMemberInfo.Collect(genericType ?? modelType) .Where(member => !blackListHash.Contains(member.Name));
        }

        protected virtual object CreateModel(Type modelType, Type genericType)
        {
            if (modelType.IsCollection() || modelType.IsArray() || modelType.IsEnumerable())
            {
                // else just make a list
                var listType = typeof(List<>).MakeGenericType(genericType);
                return Activator.CreateInstance(listType);
            }

            return Activator.CreateInstance(modelType, true);
        }

        protected virtual object GetValue(string propertyName, BindingContext context)
        {
            var dictionary = (IDictionary<string, object>)context.Object;
            if (dictionary.ContainsKey(propertyName))
            {
                return dictionary[propertyName];
            }

            return null;
        }

        protected virtual object GetValue(BindingContext context, int index)
        {
            var collection = context.Object as IList<object>;

            return collection.ElementAtOrDefault(index);
        }
    }
}
