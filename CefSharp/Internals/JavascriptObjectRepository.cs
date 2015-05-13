// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    /// <summary>
    /// This class manages the registration of objects in the browser
    /// process to be exposed to JavaScript in the renderer process.
    /// Registration performs method, parameter, property type analysis
    /// of the registered objects into meta-data tied to reflection data 
    /// for later use.
    /// 
    /// This class also is the adaptation layer between the BrowserProcessService
    /// and the registered objects. This means when the renderer wants to call an 
    /// exposed method, get a property of an object, or
    /// set a property of an object in the browser process, that this
    /// class does deals with the previously created meta-data and invokes the correct
    /// behavior via reflection APIs.
    /// 
    /// All of the registered objects are tracked via meta-data for the objects 
    /// expressed starting with the JavaScriptObject type.
    /// </summary>
    public class JavascriptObjectRepository : DisposableResource
    {
        private static long lastId;
        private static readonly object Lock = new object();
        private static readonly BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Public;

        // A hash from assigned object ids to the objects,
        // this is done to speed up finding the object in O(1) time
        // instead of traversing the JavaScriptRootObject tree.
        private readonly Dictionary<long, JavascriptObject> objects = new Dictionary<long, JavascriptObject>();

        // This is the root of the objects that get serialized to the child
        // process.
        public JavascriptRootObject RootObject { get; private set; }

        public JavascriptObjectRepository()
        {
            RootObject = new JavascriptRootObject();
        }

        private JavascriptObject CreateJavascriptObject(bool camelCaseJavascriptNames)
        {
            long id;
            lock (Lock)
            {
                id = lastId++;
            }
            var result = new JavascriptObject { Id = id, CamelCaseJavascriptNames = camelCaseJavascriptNames };
            objects[id] = result;

            return result;
        }

        public void Register(string name, object value, bool camelCaseJavascriptNames)
        {
            var jsObject = CreateJavascriptObject(camelCaseJavascriptNames);
            jsObject.SetValue(value);
            jsObject.Name = name;
            jsObject.JavascriptName = name;

            AnalyseObjectForBinding(jsObject, camelCaseJavascriptNames);

            RootObject.MemberObjects.Add(jsObject);
        }

        public bool TryCallMethod(long objectId, string name, object[] parameters, out object result, out string exception)
        {
            exception = "";
            result = null;
            JavascriptObject obj;
            if (!objects.TryGetValue(objectId, out obj))
            {
                return false;
            }

            var method = obj.Methods.FirstOrDefault(p => p.JavascriptName == name);
            if (method == null)
            {
                throw new InvalidOperationException(string.Format("Method {0} not found on Object of Type {1}", name, obj.Value.GetType()));
            }

            try
            {
                // Do we have enough arguments? Add Type.Missing for any that we don't have incase of optional params
                var missingParams = method.ParameterCount - parameters.Length;
                if (missingParams > 0)
                {
                    var paramList = new List<object>(parameters);

                    for (var i = 0; i < missingParams; i++)
                    {
                        paramList.Add(Type.Missing);
                    }

                    parameters = paramList.ToArray();
                }

                result = method.Function(obj.Value, DefaultBindingFlags, JavascriptTypeBinder.Singleton, parameters, CultureInfo.CurrentCulture);

                if (result != null)
                {
                    var type = result.GetType();
                    if (IsComplexType(type))
                    {
                        var jsObject = CreateJavascriptObject(obj.CamelCaseJavascriptNames);
                        jsObject.Name = "FunctionResult(" + name + ")";
                        jsObject.JavascriptName = jsObject.Name;
                        SetJavascriptObjectValue(jsObject, result, result.GetType(), obj.CamelCaseJavascriptNames);
                        result = jsObject.Bind();
                    }
                    else
                    {
                        result = ConvertIfGenericList(result, type, obj.CamelCaseJavascriptNames);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                exception = ex.Message;
            }

            return false;
        }

        public bool TryGetProperty(long objectId, string name, out object result, out string exception)
        {
            exception = "";
            result = null;
            JavascriptObject obj;
            if (!objects.TryGetValue(objectId, out obj))
            {
                return false;
            }

            var property = obj.Properties.FirstOrDefault(p => p.JavascriptName == name);
            if (property == null)
            {
                throw new InvalidOperationException(string.Format("Property {0} not found on Object of Type {1}", name, obj.Value.GetType()));
            }

            try
            {
                //If the property is of a complex type then perform late binding and return the JavascriptObject
                result = property.JsObject == null ? property.GetValue(obj.Value) : property.JsObject.Bind();
                return true;
            }
            catch (Exception ex)
            {
                exception = ex.Message;
            }

            return false;
        }

        public bool TrySetProperty(long objectId, string name, object value, out string exception)
        {
            exception = "";
            JavascriptObject obj;
            if (!objects.TryGetValue(objectId, out obj))
            {
                return false;
            }

            var property = obj.Properties.FirstOrDefault(p => p.JavascriptName == name);
            if (property == null)
            {
                throw new InvalidOperationException(string.Format("Property {0} not found on Object of Type {1}", name, obj.Value.GetType()));
            }
            try
            {
                property.SetValue(obj.Value, value);

                return true;
            }
            catch (Exception ex)
            {
                exception = ex.Message;
            }

            return false;
        }

        /// <summary>
        /// Analyse the object and generate metadata which will
        /// be used by the browser subprocess to interact with Cef.
        /// </summary>
        /// <param name="obj">Javascript object</param>
        /// <param name="camelCaseJavascriptNames">camel case the javascript names of properties/methods</param>
        private void AnalyseObjectForBinding(JavascriptObject obj, bool camelCaseJavascriptNames)
        {
            if (obj.Value == null)
            {
                return;
            }

            var type = obj.Value.GetType();
            if (type.IsPrimitive || type == typeof(string))
            {
                return;
            }

            foreach (var methodInfo in type.GetMethods(DefaultBindingFlags).Where(p => !p.IsSpecialName))
            {
                // Type objects can not be serialized.
                if (methodInfo.ReturnType == typeof(Type) || Attribute.IsDefined(methodInfo, typeof(JavascriptIgnoreAttribute)))
                {
                    continue;
                }

                var jsMethod = CreateJavaScriptMethod(methodInfo, camelCaseJavascriptNames);
                obj.Methods.Add(jsMethod);
            }

            foreach (var propertyInfo in type.GetProperties(DefaultBindingFlags).Where(p => !p.IsSpecialName))
            {
                if (propertyInfo.PropertyType == typeof(Type) || Attribute.IsDefined(propertyInfo, typeof(JavascriptIgnoreAttribute)))
                {
                    continue;
                }

                var jsProperty = CreateJavaScriptProperty(propertyInfo, camelCaseJavascriptNames);
                if (jsProperty.IsComplexType)
                {
                    var jsObject = CreateJavascriptObject(camelCaseJavascriptNames);
                    jsObject.Name = propertyInfo.Name;
                    jsObject.JavascriptName = GetJavascriptName(propertyInfo.Name, camelCaseJavascriptNames);
                    jsObject.LateBinding = () =>
                    {
                        var newValue = jsProperty.GetValue(obj.Value);
                        if (newValue == jsObject.Value)
                        {
                            return;
                        }
                        SetJavascriptObjectValue(jsObject, newValue, propertyInfo.PropertyType, camelCaseJavascriptNames);
                    };
                    jsProperty.JsObject = jsObject;
                }
                obj.Properties.Add(jsProperty);
            }
        }

        private void SetJavascriptObjectValue(JavascriptObject jsObject, object value, Type type, bool camelCaseJavascriptNames)
        {
            if (type.IsGenericType)
            {
                IList list = value as IList;
                if (list != null)
                {
                    var elemType = type.GetGenericArguments()[0];
                    if (IsComplexType(elemType))
                    {
                        var jsArray = new JavascriptObject[list.Count];
                        for (var i = 0; i < list.Count; i++)
                        {
                            jsArray[i] = CreateJavascriptObject(i.ToString(), list[i], camelCaseJavascriptNames);
                        }
                        jsObject.SetValue(jsArray);
                    }
                    else
                    {
                        var array = Array.CreateInstance(elemType, list.Count);
                        for (int i = 0; i < list.Count; i++)
                        {
                            array.SetValue(list[i], i);
                        }
                        jsObject.SetValue(array);
                    }
                }
                else
                {
                    throw new SerializationException(jsObject.JavascriptName);
                }
            }
            else if (type.IsArray)
            {
                var array = value as Array;
                if (array == null)
                {
                    jsObject.SetValue(null);
                }
                else
                {
                    var jsArray = new JavascriptObject[array.Length];
                    for (var i = 0; i < array.Length; i++)
                    {
                        jsArray[i] = CreateJavascriptObject(i.ToString(), array.GetValue(i), camelCaseJavascriptNames);
                    }
                    jsObject.SetValue(jsArray);
                }
            }
            else
            {
                jsObject.SetValue(value);
                AnalyseObjectForBinding(jsObject, camelCaseJavascriptNames);
            }
        }
        
        private JavascriptObject CreateJavascriptObject(string name, object value, bool camelCaseJavascriptNames)
        {
            var jsObject = CreateJavascriptObject(camelCaseJavascriptNames);
            jsObject.Name = jsObject.JavascriptName = name;
            jsObject.SetValue(value);
            AnalyseObjectForBinding(jsObject, camelCaseJavascriptNames);
            return jsObject;
        }

        private static JavascriptMethod CreateJavaScriptMethod(MethodInfo methodInfo, bool camelCaseJavascriptNames)
        {
            var jsMethod = new JavascriptMethod();

            jsMethod.ManagedName = methodInfo.Name;
            jsMethod.JavascriptName = GetJavascriptName(methodInfo.Name, camelCaseJavascriptNames);
            jsMethod.Function = methodInfo.Invoke;
            jsMethod.ParameterCount = methodInfo.GetParameters().Length;

            return jsMethod;
        }

        private object ConvertIfGenericList(object value, Type type, bool camelCaseJavascriptNames)
        {
            if (!type.IsGenericType) return value;

            IList list = value as IList;
            if (list != null)
            {
                var elemType = type.GetGenericArguments()[0];
                if (IsComplexType(elemType))
                {
                    var jsArray = new JavascriptObject[list.Count];
                    for (var i = 0; i < list.Count; i++)
                    {
                        jsArray[i] = CreateJavascriptObject(i.ToString(), list[i], camelCaseJavascriptNames);
                    }
                    return jsArray;
                }
                else
                {
                    var array = Array.CreateInstance(elemType, list.Count);
                    for (int i = 0; i < list.Count; i++)
                    {
                        array.SetValue(list[i], i);
                    }
                    return array;
                }
            }
            else
            {
                throw new SerializationException("Unable to serialize generic type " + type.ToString());
            }
        }

        private JavascriptProperty CreateJavaScriptProperty(PropertyInfo propertyInfo, bool camelCaseJavascriptNames)
        {
            var jsProperty = new JavascriptProperty();

            jsProperty.ManagedName = propertyInfo.Name;
            jsProperty.JavascriptName = GetJavascriptName(propertyInfo.Name, camelCaseJavascriptNames);
            jsProperty.SetValue = (o, v) => propertyInfo.SetValue(o, v, DefaultBindingFlags, JavascriptTypeBinder.Singleton, null, CultureInfo.CurrentCulture);
            jsProperty.GetValue = (o) => ConvertIfGenericList(propertyInfo.GetValue(o, null), propertyInfo.PropertyType, camelCaseJavascriptNames);
 
            jsProperty.IsComplexType = IsComplexType(propertyInfo.PropertyType);
            jsProperty.IsReadOnly = !propertyInfo.CanWrite;

            return jsProperty;
        }

        private static bool IsComplexType(Type type)
        {
            if (type == typeof(void))
            {
                return false;
            }

            var baseType = type;

            var nullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (nullable)
            {
                baseType = Nullable.GetUnderlyingType(type);
            }

            if (baseType == null || baseType.Namespace.StartsWith("System"))
            {
                return false;
            }

            return !baseType.IsPrimitive && baseType != typeof(string);
        }

        private static string GetJavascriptName(string str, bool camelCaseJavascriptNames)
        {
            if (!camelCaseJavascriptNames)
            {
                return str;
            }

            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return char.ToLower(str[0]) + str.Substring(1);
        }
    }
}
