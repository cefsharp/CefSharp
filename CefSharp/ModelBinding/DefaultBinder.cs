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
        private readonly IFieldNameConverter fieldNameConverter;
        
        //private readonly static MethodInfo ToListMethodInfo = typeof(Enumerable).GetMethod("ToList", BindingFlags.Public | BindingFlags.Static);
        private readonly static MethodInfo ToArrayMethodInfo = typeof(Enumerable).GetMethod("ToArray", BindingFlags.Public | BindingFlags.Static);
        private static readonly Regex BracketRegex = new Regex(@"\[(\d+)\]\z", RegexOptions.Compiled);
        private static readonly Regex UnderscoreRegex = new Regex(@"_(\d+)\z", RegexOptions.Compiled);

        public DefaultBinder(IFieldNameConverter fieldNameConverter)
        {
            if (fieldNameConverter == null)
            {
                throw new ArgumentNullException("fieldNameConverter");
            }

            this.fieldNameConverter = fieldNameConverter;
        }

        /// <summary>
        /// Bind to the given model type
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="modelType">Model type to bind to</param>
        /// <param name="configuration">The <see cref="BindingConfig"/> that should be applied during binding.</param>
        /// <param name="blackList">Blacklisted binding property names</param>
        /// <returns>Bound model</returns>
        public object Bind(IDictionary<string, object> objectDictionary, Type modelType, BindingConfig configuration, params string[] blackList)
        {
            Type genericType = null;
            if (modelType.IsArray() || modelType.IsCollection() || modelType.IsEnumerable())
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

            var bindingContext = this.CreateBindingContext(objectDictionary, modelType, configuration, blackList, genericType);

            var bindingExceptions = new List<PropertyBindingException>();

            if (bindingContext.DestinationType.IsCollection() || bindingContext.DestinationType.IsArray() || bindingContext.DestinationType.IsEnumerable())
            {
                var loopCount = objectDictionary.Keys.Select(IsMatch).Where(x => x != -1).Distinct().ToArray().Length;
                var model = (IList)bindingContext.Model;
                for (var i = 0; i < loopCount; i++)
                {
                    object genericinstance;
                    if (model.Count > i)
                    {
                        genericinstance = model[i];
                    }
                    else
                    {
                        genericinstance = bindingContext.GenericType.CreateInstance();
                        model.Add(genericinstance);
                    }

                    foreach (var modelProperty in bindingContext.ValidModelBindingMembers)
                    {
                        var collectionVal = GetValue(modelProperty.Name, bindingContext, i);

                        if (collectionVal != null)
                        {
                            try
                            {
                                BindValue(modelProperty, collectionVal, bindingContext);
                            }
                            catch (PropertyBindingException ex)
                            {
                                bindingExceptions.Add(ex);
                            }
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
                        try
                        {
                            BindValue(modelProperty, val, bindingContext);
                        }
                        catch (PropertyBindingException ex)
                        {
                            bindingExceptions.Add(ex);
                        }
                    }
                }
            }

            if (bindingExceptions.Any() && !bindingContext.Configuration.IgnoreErrors)
            {
                throw new ModelBindingException(modelType, bindingExceptions);
            }

            if (modelType.IsArray())
            {
                var generictoArrayMethod = ToArrayMethodInfo.MakeGenericMethod(new[] { genericType });
                return generictoArrayMethod.Invoke(null, new[] { bindingContext.Model });
            }
            return bindingContext.Model;
        }

        private static int IsMatch(string item)
        {
            var bracketMatch = BracketRegex.Match(item);
            if (bracketMatch.Success)
            {
                return int.Parse(bracketMatch.Groups[1].Value);
            }

            var underscoreMatch = UnderscoreRegex.Match(item);

            if (underscoreMatch.Success)
            {
                return int.Parse(underscoreMatch.Groups[1].Value);
            }

            return -1;
        }

        private BindingContext CreateBindingContext(IDictionary<string, object> objectDictioanry, Type modelType, BindingConfig configuration, IEnumerable<string> blackList, Type genericType)
        {
            return new BindingContext
            {
                Configuration = configuration,
                DestinationType = modelType,
                Model = CreateModel(modelType, genericType),
                ValidModelBindingMembers = GetBindingMembers(modelType, genericType, blackList).ToList(),
                ObjectDictionary = objectDictioanry,
                GenericType = genericType
            };
        }

        private void BindValue(BindingMemberInfo modelProperty, object obj, BindingContext context)
        {
            Type dictionaryType = typeof(Dictionary<string, object>);

            //If the type is a dictionary and the PropertyType isn't then we'll bind.
            if (obj != null && obj.GetType() == dictionaryType && modelProperty.PropertyType != dictionaryType)
            {
                var dictionary = (Dictionary<string, object>)obj;
                var model = Bind(dictionary, modelProperty.PropertyType, context.Configuration);
                //We have a sub dictionary, attempt to bind it to the class

                modelProperty.SetValue(context.Model, model);
            }
            else
            { 
                //Simply set the property
                modelProperty.SetValue(context.Model, obj);
            }
        }

        private static IEnumerable<BindingMemberInfo> GetBindingMembers(Type modelType, Type genericType, IEnumerable<string> blackList)
        {
            var blackListHash = new HashSet<string>(blackList, StringComparer.Ordinal);

            return BindingMemberInfo.Collect(genericType ?? modelType) .Where(member => !blackListHash.Contains(member.Name));
        }

        private static object CreateModel(Type modelType, Type genericType)
        {
            if (modelType.IsArray() || modelType.IsCollection() || modelType.IsEnumerable())
            {
                //else just make a list
                var listType = typeof(List<>).MakeGenericType(genericType);
                return Activator.CreateInstance(listType);
            }

            return modelType.CreateInstance(true);
        }

        private static object GetValue(string propertyName, BindingContext context, int index = -1)
        {
            if (index != -1)
            {

                var indexindexes = context.ObjectDictionary.Keys.Select(IsMatch)
                                           .Where(i => i != -1)
                                           .OrderBy(i => i)
                                           .Distinct()
                                           .Select((k, i) => new KeyValuePair<int, int>(i, k))
                                           .ToDictionary(k => k.Key, v => v.Value);

                if (indexindexes.ContainsKey(index))
                {
                    var propertyValue =
                        context.ObjectDictionary.Where(c =>
                        {
                            var indexId = IsMatch(c.Key);
                            return c.Key.StartsWith(propertyName, StringComparison.OrdinalIgnoreCase) && indexId != -1 && indexId == indexindexes[index];
                        })
                        .Select(k => k.Value)
                        .FirstOrDefault();

                    return propertyValue;
                }

                return string.Empty;
            }
            return context.ObjectDictionary.ContainsKey(propertyName) ? context.ObjectDictionary[propertyName] : string.Empty;
        }
    }
}
