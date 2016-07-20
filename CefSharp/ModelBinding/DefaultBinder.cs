// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections;
using System.Collections.Generic;
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

        protected bool IgnoreErrors { get; private set; }

        public IEnumerable<string> BlackListedPropertyNames { get; set; }

        public DefaultBinder(IFieldNameConverter fieldNameConverter)
        {
            if (fieldNameConverter == null)
            {
                throw new ArgumentNullException("fieldNameConverter");
            }

            this.fieldNameConverter = fieldNameConverter;
            IgnoreErrors = true;
            BlackListedPropertyNames = new List<string>();
        }

        /// <summary>
        /// Bind to the given model type
        /// </summary>
        /// <param name="obj">object to be converted into a model</param>
        /// <param name="modelType">Model type to bind to</param>
        /// <param name="blackList">Blacklisted binding property names</param>
        /// <returns>Bound model</returns>
        public virtual object Bind(object obj, Type modelType)
        {
            if(obj == null)
            {
                return null;
            }

            Type genericType = null;
            if (modelType.IsCollectionOrArray())
            {
                //make sure it has a generic type
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
                    throw new ArgumentException("When modelType is an enumerable it must specify the type.", "modelType");
                }
            }

            var bindingContext = this.CreateBindingContext(obj, modelType, genericType);

            if (bindingContext.DestinationType.IsCollectionOrArray())
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

                    if (val != null)
                    {
                        if (val.GetType() == typeof(Dictionary<string, object>))
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
                foreach (var modelProperty in bindingContext.ValidModelBindingMembers)
                {
                    var val = GetValue(modelProperty.Name, bindingContext);

                    if (val != null)
                    {
                        BindValue(modelProperty, val, bindingContext);
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

            Type dictionaryType = typeof(Dictionary<string, object>);

            //If the type is a dictionary and the PropertyType isn't then we'll bind.
            if (obj.GetType() == dictionaryType && modelProperty.PropertyType != dictionaryType)
            {
                //We have a sub dictionary, attempt to bind it to the class
                var model = Bind(obj, modelProperty.PropertyType);

                modelProperty.SetValue(context.Model, model);
            }
            //If both types are collections then we'll bind
            else if (obj.GetType().IsCollectionOrArray() && modelProperty.PropertyType.IsCollectionOrArray())
            {
                //We have a sub dictionary, attempt to bind it to the class
                var model = Bind(obj, modelProperty.PropertyType);

                modelProperty.SetValue(context.Model, model);
            }
            else
            { 
                //Simply set the property
                modelProperty.SetValue(context.Model, obj);
            }
        }

        protected virtual IEnumerable<BindingMemberInfo> GetBindingMembers(Type modelType, Type genericType)
        {
            var blackListHash = new HashSet<string>(BlackListedPropertyNames, StringComparer.Ordinal);

            return BindingMemberInfo.Collect(genericType ?? modelType) .Where(member => !blackListHash.Contains(member.Name));
        }

        protected virtual object CreateModel(Type modelType, Type genericType)
        {
            if (modelType.IsCollectionOrArray())
            {
                //else just make a list
                var listType = typeof(List<>).MakeGenericType(genericType);
                return Activator.CreateInstance(listType);
            }

            return modelType.CreateInstance(true);
        }

        protected virtual object GetValue(string propertyName, BindingContext context)
        {
            if (context.Object.GetType() == typeof(Dictionary<string, object>))
            {
                var dictionary = (Dictionary<string, object>)context.Object;
                if (dictionary.ContainsKey(propertyName))
                {
                    return dictionary[propertyName];
                }
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
