// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using CefSharp.ModelBinding;

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
    public class JavascriptObjectRepository
    {
        private static long lastId;

        /// <summary>
        /// A hash from assigned object ids to the objects,
        /// this is done to speed up finding the object in O(1) time
        /// instead of traversing the JavaScriptRootObject tree.
        /// </summary>
        private readonly Dictionary<long, JavascriptObject> objects = new Dictionary<long, JavascriptObject>();

        /// <summary>
        /// This is the root of the objects that get serialized to the child process.
        /// </summary>
        public JavascriptRootObject RootObject { get; private set; }
        
        /// <summary>
        /// This is the root of the objects that get serialized to the child
        /// process with cef ipc serialization (wcf not required).
        /// </summary>
        public JavascriptRootObject AsyncRootObject { get; private set; }

        public JavascriptObjectRepository()
        {
            RootObject = new JavascriptRootObject();
            AsyncRootObject = new JavascriptRootObject();
        }

        public bool HasBoundObjects
        {
            get { return RootObject.MemberObjects.Count > 0 || AsyncRootObject.MemberObjects.Count > 0; }
        }

        private JavascriptObject CreateJavascriptObject(bool camelCaseJavascriptNames)
        {
            var id = Interlocked.Increment(ref lastId);

            var result = new JavascriptObject { Id = id, CamelCaseJavascriptNames = camelCaseJavascriptNames };
            objects[id] = result;

            return result;
        }

        public void RegisterAsync(string name, object value, BindingOptions options)
        {
            AsyncRootObject.MemberObjects.Add(CreateInternal(name, value, analyseProperties: false, options: options));
        }

        public void Register(string name, object value, BindingOptions options)
        {
            RootObject.MemberObjects.Add(CreateInternal(name, value, analyseProperties: true, options: options));
        }

        private JavascriptObject CreateInternal(string name, object value, bool analyseProperties, BindingOptions options)
        {
            var camelCaseJavascriptNames = options == null ? true : options.CamelCaseJavascriptNames;
            var jsObject = CreateJavascriptObject(camelCaseJavascriptNames);
            jsObject.Value = value;
            jsObject.Name = name;
            jsObject.JavascriptName = name;
            jsObject.Binder = options == null ? null : options.Binder;

            AnalyseObjectForBinding(jsObject, analyseMethods: true, analyseProperties: analyseProperties, readPropertyValue: false, camelCaseJavascriptNames: camelCaseJavascriptNames);

            return jsObject;
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
                //Check if the bound object method contains a ParamArray as the last parameter on the method signature.
                //NOTE: No additional parameters are permitted after the params keyword in a method declaration,
                //and only one params keyword is permitted in a method declaration.
                //https://msdn.microsoft.com/en-AU/library/w5zay9db.aspx
                if (method.HasParamArray)
                {
                    var paramList = new List<object>(method.Parameters.Count);

                    //Loop through all of the method parameters on the bound object.
                    for (var i = 0; i < method.Parameters.Count; i++)
                    {
                        //If the method parameter is a paramArray IE: (params string[] args)
                        //grab the parameters from the javascript function starting at the current bound object parameter index
                        //and add create an array that will be passed in as the last bound object method parameter.
                        if (method.Parameters[i].IsParamArray)
                        {
                            var convertedParams = new List<object>();
                            for (var s = i; s < parameters.Length; s++)
                            {
                                convertedParams.Add(parameters[s]);
                            }
                            paramList.Add(convertedParams.ToArray());
                        }
                        else
                        {
                            var jsParam = parameters.ElementAtOrDefault(i);
                            paramList.Add(jsParam);
                        }
                    }

                    parameters = paramList.ToArray();
                }
                
                //Check for parameter count missmatch between the parameters on the javascript function and the
                //number of parameters on the bound object method. (This is relevant for methods that have default values)
                //NOTE it's possible to have default params and a paramArray, so check missing params last
                var missingParams = method.ParameterCount - parameters.Length;

                if(missingParams > 0)
                {
                    var paramList = new List<object>(parameters);

                    for (var i = 0; i < missingParams; i++)
                    {
                        paramList.Add(Type.Missing);
                    }

                    parameters = paramList.ToArray();
                }

                try
                {
                    if(obj.Binder != null)
                    { 
                        for (var i = 0; i < parameters.Length; i++)
                        { 
                            if(parameters[i] != null)
                            { 
                                var paramType = method.Parameters[i].Type;

                                if(parameters[i].GetType() == typeof(Dictionary<string, object>))
                                {
                                    var dictionary = (Dictionary<string, object>)parameters[i];
                                    parameters[i] = obj.Binder.Bind(dictionary, paramType);
                                }
                                else if (parameters[i].GetType() == typeof(List<object>))
                                {
                                    var list = (List<object>)parameters[i];
                                    parameters[i] = obj.Binder.Bind(list, paramType);
                                }
                            }
                        }
                    }

                    result = method.Function(obj.Value, parameters);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Could not execute method: " + name + "(" + String.Join(", ", parameters) + ")" + " - Missing Parameters: " + missingParams, e);
                }

                if(result != null && IsComplexType(result.GetType()))
                {
                    var jsObject = CreateJavascriptObject(obj.CamelCaseJavascriptNames);
                    jsObject.Value = result;
                    jsObject.Name = "FunctionResult(" + name + ")";
                    jsObject.JavascriptName = jsObject.Name;

                    AnalyseObjectForBinding(jsObject, analyseMethods: false, analyseProperties:true, readPropertyValue: true, camelCaseJavascriptNames: obj.CamelCaseJavascriptNames);

                    result = jsObject;
                }

                return true;
            }
            catch(TargetInvocationException e)
            {
                var baseException = e.GetBaseException();
                exception = baseException.ToString();
            }
            catch (Exception ex)
            {
                exception = ex.ToString();
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
                result = property.GetValue(obj.Value);

                return true;
            }
            catch (Exception ex)
            {
                exception = ex.ToString();
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
                exception = ex.ToString();
            }

            return false;
        }

        /// <summary>
        /// Analyse the object and generate metadata which will
        /// be used by the browser subprocess to interact with Cef.
        /// Method is called recursively
        /// </summary>
        /// <param name="obj">Javascript object</param>
        /// <param name="analyseMethods">Analyse methods for inclusion in metadata model</param>
        /// <param name="readPropertyValue">When analysis is done on a property, if true then get it's value for transmission over WCF</param>
        /// <param name="camelCaseJavascriptNames">camel case the javascript names of properties/methods</param>
        /// <param name="analyseProperties">Analyse properties for binding</param>
        private void AnalyseObjectForBinding(JavascriptObject obj, bool analyseMethods, bool readPropertyValue, bool camelCaseJavascriptNames, bool analyseProperties)
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

            if (analyseMethods)
            {
                foreach (var methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(p => !p.IsSpecialName))
                {
                    // Type objects can not be serialized.
                    if (methodInfo.ReturnType == typeof (Type) || Attribute.IsDefined(methodInfo, typeof (JavascriptIgnoreAttribute)))
                    {
                        continue;
                    }

                    var jsMethod = CreateJavaScriptMethod(methodInfo, camelCaseJavascriptNames);
                    obj.Methods.Add(jsMethod);
                }
            }

            if (analyseProperties)
            {
                foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => !p.IsSpecialName))
                {
                    if (propertyInfo.PropertyType == typeof (Type) || Attribute.IsDefined(propertyInfo, typeof (JavascriptIgnoreAttribute)))
                    {
                        continue;
                    }

                    var jsProperty = CreateJavaScriptProperty(propertyInfo, camelCaseJavascriptNames);
                    if (jsProperty.IsComplexType)
                    {
                        var jsObject = CreateJavascriptObject(camelCaseJavascriptNames);
                        jsObject.Name = propertyInfo.Name;
                        jsObject.JavascriptName = GetJavascriptName(propertyInfo.Name, camelCaseJavascriptNames);
                        jsObject.Value = jsProperty.GetValue(obj.Value);
                        jsProperty.JsObject = jsObject;

                        AnalyseObjectForBinding(jsProperty.JsObject, analyseMethods, readPropertyValue, camelCaseJavascriptNames, true);
                    }
                    else if (readPropertyValue)
                    {
                        jsProperty.PropertyValue = jsProperty.GetValue(obj.Value);
                    }
                    obj.Properties.Add(jsProperty);
                }
            }
        }

        private static JavascriptMethod CreateJavaScriptMethod(MethodInfo methodInfo, bool camelCaseJavascriptNames)
        {
            var jsMethod = new JavascriptMethod();

            jsMethod.ManagedName = methodInfo.Name;
            jsMethod.JavascriptName = GetJavascriptName(methodInfo.Name, camelCaseJavascriptNames);
            jsMethod.Function = methodInfo.Invoke;
            jsMethod.ParameterCount = methodInfo.GetParameters().Length;
            jsMethod.Parameters = methodInfo.GetParameters()
                .Select(t => new MethodParameter()
                {
                    IsParamArray = t.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0,
                    Type = t.ParameterType
                }).ToList();
            //Pre compute HasParamArray for a very minor performance gain 
            jsMethod.HasParamArray = jsMethod.Parameters.LastOrDefault(t => t.IsParamArray) != null;

            return jsMethod;
        }

        private static JavascriptProperty CreateJavaScriptProperty(PropertyInfo propertyInfo, bool camelCaseJavascriptNames)
        {
            var jsProperty = new JavascriptProperty();

            jsProperty.ManagedName = propertyInfo.Name;
            jsProperty.JavascriptName = GetJavascriptName(propertyInfo.Name, camelCaseJavascriptNames);
            jsProperty.SetValue = (o, v) => propertyInfo.SetValue(o, v, null);
            jsProperty.GetValue = (o) => propertyInfo.GetValue(o, null);

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

            var nullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);

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

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }
    }
}
