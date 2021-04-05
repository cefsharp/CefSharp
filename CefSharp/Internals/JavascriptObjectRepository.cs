// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.Event;
using CefSharp.JavascriptBinding;
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
    public class JavascriptObjectRepository : FreezableBase, IJavascriptObjectRepositoryInternal
    {
        public const string AllObjects = "All";
        public const string LegacyObjects = "Legacy";

        private static long lastId;

        public event EventHandler<JavascriptBindingEventArgs> ResolveObject;
        public event EventHandler<JavascriptBindingCompleteEventArgs> ObjectBoundInJavascript;
        public event EventHandler<JavascriptBindingMultipleCompleteEventArgs> ObjectsBoundInJavascript;

        /// <summary>
        /// A hash from assigned object ids to the objects,
        /// this is done to speed up finding the object in O(1) time
        /// instead of traversing the JavaScriptRootObject tree.
        /// </summary>
        protected readonly ConcurrentDictionary<long, JavascriptObject> objects = new ConcurrentDictionary<long, JavascriptObject>();

        /// <summary>
        /// Javascript Name converter
        /// </summary>
        private IJavascriptNameConverter nameConverter;

        /// <summary>
        /// Has the browser this repository is associated with been initilized (set in OnAfterCreated)
        /// </summary>
        public bool IsBrowserInitialized { get; set; }

        public void Dispose()
        {
            ResolveObject = null;
            ObjectBoundInJavascript = null;
            ObjectsBoundInJavascript = null;
        }

        public bool HasBoundObjects
        {
            get { return objects.Count > 0; }
        }

        /// <summary>
        /// Configurable settings for this repository, such as the property names CefSharp injects into the window.
        /// </summary>
        public JavascriptBindingSettings Settings { get; private set; }

        /// <summary>
        /// Converted .Net method/property/field names to the name that
        /// will be used in Javasript. Used for when .Net naming conventions
        /// differ from Javascript naming conventions.
        /// </summary>
        public IJavascriptNameConverter NameConverter
        {
            get { return nameConverter; }
            set
            {
                ThrowIfFrozen();

                nameConverter = value;
            }
        }

        public JavascriptObjectRepository()
        {
            Settings = new JavascriptBindingSettings();
            nameConverter = new LegacyCamelCaseJavascriptNameConverter();
        }

        public bool IsBound(string name)
        {
            return objects.Values.Any(x => x.Name == name);
        }

        List<JavascriptObject> IJavascriptObjectRepositoryInternal.GetLegacyBoundObjects()
        {
            RaiseResolveObjectEvent(LegacyObjects);

            return objects.Values.Where(x => x.RootObject).ToList();
        }

        //Ideally this would internal, unfurtunately it's used in C++
        //and it's hard to expose internals
        List<JavascriptObject> IJavascriptObjectRepositoryInternal.GetObjects(List<string> names)
        {
            //If there are no objects names or the count is 0 then we will raise
            //the resolve event then return all objects that are registered,
            //we'll only perform checking if object(s) of specific name is requested.
            var getAllObjects = names == null || names.Count == 0;
            if (getAllObjects)
            {
                RaiseResolveObjectEvent(AllObjects);

                return objects.Values.Where(x => x.RootObject).ToList();
            }

            foreach (var name in names)
            {
                if (!IsBound(name))
                {
                    RaiseResolveObjectEvent(name);
                }
            }

            var objectsByName = objects.Values.Where(x => names.Contains(x.JavascriptName) && x.RootObject).ToList();

            //TODO: JSB Add another event that signals when no object matching a name
            //in the list was provided.
            return objectsByName;
        }

        void IJavascriptObjectRepositoryInternal.ObjectsBound(List<Tuple<string, bool, bool>> objs)
        {
            var boundObjectHandler = ObjectBoundInJavascript;
            var boundObjectsHandler = ObjectsBoundInJavascript;
            if (boundObjectHandler != null || boundObjectsHandler != null)
            {
                //Execute on Threadpool so we don't unnessicarily block the CEF IO thread
                Task.Run(() =>
                {
                    foreach (var obj in objs)
                    {
                        boundObjectHandler?.Invoke(this, new JavascriptBindingCompleteEventArgs(this, obj.Item1, obj.Item2, obj.Item3));
                    }

                    boundObjectsHandler?.Invoke(this, new JavascriptBindingMultipleCompleteEventArgs(this, objs.Select(x => x.Item1).ToList()));
                });
            }
        }

        private JavascriptObject CreateJavascriptObject(bool rootObject)
        {
            var id = Interlocked.Increment(ref lastId);

            var result = new JavascriptObject
            {
                Id = id,
                RootObject = rootObject
            };

            objects[id] = result;

            return result;
        }

#if NETCOREAPP
        public void Register(string name, object value, BindingOptions options)
#else
        public void Register(string name, object value, bool isAsync, BindingOptions options)
#endif
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Freeze();

            //Enable WCF if not already enabled - can only be done before the browser has been initliazed
            //if done after the subprocess won't be WCF enabled it we'll have to throw an exception
#if NETCOREAPP
            var isAsync = true;
#else
            if (!IsBrowserInitialized && !isAsync)
            {
                CefSharpSettings.WcfEnabled = true;
            }

            if (!CefSharpSettings.WcfEnabled && !isAsync)
            {
                throw new InvalidOperationException(@"To enable synchronous JS bindings set WcfEnabled true in CefSharpSettings before you create
                                                    your ChromiumWebBrowser instances.");
            }            
#endif

            //Validation name is unique
            if (objects.Values.Count(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)) > 0)
            {
                throw new ArgumentException("Object already bound with name:" + name, name);
            }

            //Binding of System types is problematic, so we don't support it
            var type = value.GetType();
            if (type.IsPrimitive || type.BaseType.Namespace.StartsWith("System."))
            {
                throw new ArgumentException("Registering of .Net framework built in types is not supported, " +
                    "create your own Object and proxy the calls if you need to access a Window/Form/Control.", "value");
            }

            var jsObject = CreateJavascriptObject(rootObject: true);
            jsObject.Value = value;
            jsObject.Name = name;
            jsObject.JavascriptName = name;
            jsObject.IsAsync = isAsync;
            jsObject.Binder = options?.Binder;
            jsObject.MethodInterceptor = options?.MethodInterceptor;

            AnalyseObjectForBinding(jsObject, analyseMethods: true, analyseProperties: !isAsync, readPropertyValue: false);
        }

        public void UnRegisterAll()
        {
            objects.Clear();
        }

        public bool UnRegister(string name)
        {
            foreach (var kvp in objects)
            {
                if (string.Equals(kvp.Value.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    JavascriptObject obj;
                    objects.TryRemove(kvp.Key, out obj);

                    return true;
                }
            }

            return false;
        }

        TryCallMethodResult IJavascriptObjectRepositoryInternal.TryCallMethod(long objectId, string name, object[] parameters)
        {
            return TryCallMethod(objectId, name, parameters);
        }

        protected virtual TryCallMethodResult TryCallMethod(long objectId, string name, object[] parameters)
        {
            var exception = "";
            object result = null;
            JavascriptObject obj;

            if (!objects.TryGetValue(objectId, out obj))
            {
                return new TryCallMethodResult(false, result, "Object Not Found Matching Id:" + objectId);
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

                int missingParams = 0;

                try
                {
                    if (obj.Binder != null)
                    {
                        for (var i = 0; i < parameters.Length; i++)
                        {
                            var paramExpectedType = method.Parameters[i].Type;

                            //Previously only IDictionary<string, object> and IList<object> called Binder.Bind
                            //Now every param is bound to allow for type conversion
                            parameters[i] = obj.Binder.Bind(parameters[i], paramExpectedType);
                        }
                    }

                    //Check for parameter count missmatch between the parameters on the javascript function and the
                    //number of parameters on the bound object method. (This is relevant for methods that have default values)
                    //NOTE it's possible to have default params and a paramArray, so check missing params last
                    missingParams = method.ParameterCount - parameters.Length;

                    if (missingParams > 0)
                    {
                        var paramList = new List<object>(parameters);

                        for (var i = 0; i < missingParams; i++)
                        {
                            paramList.Add(Type.Missing);
                        }

                        parameters = paramList.ToArray();
                    }

                    if (obj.MethodInterceptor == null)
                    {
                        result = method.Function(obj.Value, parameters);
                    }
                    else
                    {
                        result = obj.MethodInterceptor.Intercept((p) => method.Function(obj.Value, p), parameters, method.ManagedName);
                    }
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Could not execute method: " + name + "(" + String.Join(", ", parameters) + ") " + (missingParams > 0 ? "- Missing Parameters: " + missingParams : ""), e);
                }

                //For sync binding with methods that return a complex property we create a new JavascriptObject
                //TODO: Fix the memory leak, every call to a method that returns an object will create a new
                //JavascriptObject and they are never released
                if (!obj.IsAsync && result != null && IsComplexType(result.GetType()))
                {
                    var jsObject = CreateJavascriptObject(rootObject: false);
                    jsObject.Value = result;
                    jsObject.Name = "FunctionResult(" + name + ")";
                    jsObject.JavascriptName = jsObject.Name;

                    AnalyseObjectForBinding(jsObject, analyseMethods: false, analyseProperties: true, readPropertyValue: true);

                    result = jsObject;
                }

                return new TryCallMethodResult(true, result, exception); ;
            }
            catch (TargetInvocationException e)
            {
                var baseException = e.GetBaseException();
                exception = baseException.ToString();
            }
            catch (Exception ex)
            {
                exception = ex.ToString();
            }

            return new TryCallMethodResult(false, result, exception); ;
        }

        Task<TryCallMethodResult> IJavascriptObjectRepositoryInternal.TryCallMethodAsync(long objectId, string name, object[] parameters)
        {
            return TryCallMethodAsync(objectId, name, parameters);
        }

        protected virtual async Task<TryCallMethodResult> TryCallMethodAsync(long objectId, string name, object[] parameters)
        {
            var exception = "";
            object result = null;
            JavascriptObject obj;

            if (!objects.TryGetValue(objectId, out obj))
            {
                return new TryCallMethodResult(false, result, exception);
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

                int missingParams = 0;

                try
                {
                    if (obj.Binder != null)
                    {
                        for (var i = 0; i < parameters.Length; i++)
                        {
                            var paramExpectedType = method.Parameters[i].Type;

                            //Previously only IDictionary<string, object> and IList<object> called Binder.Bind
                            //Now every param is bound to allow for type conversion
                            parameters[i] = obj.Binder.Bind(parameters[i], paramExpectedType);
                        }
                    }

                    //Check for parameter count missmatch between the parameters on the javascript function and the
                    //number of parameters on the bound object method. (This is relevant for methods that have default values)
                    //NOTE it's possible to have default params and a paramArray, so check missing params last
                    missingParams = method.ParameterCount - parameters.Length;

                    if (missingParams > 0)
                    {
                        var paramList = new List<object>(parameters);

                        for (var i = 0; i < missingParams; i++)
                        {
                            paramList.Add(Type.Missing);
                        }

                        parameters = paramList.ToArray();
                    }

                    if (obj.MethodInterceptor == null)
                    {
                        result = method.Function(obj.Value, parameters);
                    }
                    else
                    {
                        var asyncInterceptor = obj.MethodInterceptor as IAsyncMethodInterceptor;
                        if (asyncInterceptor == null)
                        {
                            result = obj.MethodInterceptor.Intercept((p) => method.Function(obj.Value, p), parameters, method.ManagedName);
                        }
                        else
                        {
                            //Only call InterceptAsync for methods that return Task or AlwaysInterceptAsynchronously = true
                            //TODO: Add support for ValueTask
                            if (typeof(Task).IsAssignableFrom(method.ReturnType) || Settings.AlwaysInterceptAsynchronously)
                            {
                                result = await asyncInterceptor.InterceptAsync((p) => method.Function(obj.Value, p), parameters, method.ManagedName).ConfigureAwait(false);
                            }
                            else
                            {
                                result = obj.MethodInterceptor.Intercept((p) => method.Function(obj.Value, p), parameters, method.ManagedName);
                            }
                        }
                        
                    }
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Could not execute method: " + name + "(" + String.Join(", ", parameters) + ") " + (missingParams > 0 ? "- Missing Parameters: " + missingParams : ""), e);
                }

                //For sync binding with methods that return a complex property we create a new JavascriptObject
                //TODO: Fix the memory leak, every call to a method that returns an object will create a new
                //JavascriptObject and they are never released
                if (!obj.IsAsync && result != null && IsComplexType(result.GetType()))
                {
                    var jsObject = CreateJavascriptObject(rootObject: false);
                    jsObject.Value = result;
                    jsObject.Name = "FunctionResult(" + name + ")";
                    jsObject.JavascriptName = jsObject.Name;

                    AnalyseObjectForBinding(jsObject, analyseMethods: false, analyseProperties: true, readPropertyValue: true);

                    result = jsObject;
                }

                return new TryCallMethodResult(true, result, exception);
            }
            catch (TargetInvocationException e)
            {
                var baseException = e.GetBaseException();
                exception = baseException.ToString();
            }
            catch (Exception ex)
            {
                exception = ex.ToString();
            }
            return new TryCallMethodResult(false, result, exception);
        }

        bool IJavascriptObjectRepositoryInternal.TryGetProperty(long objectId, string name, out object result, out string exception)
        {
            return TryGetProperty(objectId, name, out result, out exception);
        }

        protected virtual bool TryGetProperty(long objectId, string name, out object result, out string exception)
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

        bool IJavascriptObjectRepositoryInternal.TrySetProperty(long objectId, string name, object value, out string exception)
        {
            return TrySetProperty(objectId, name, value, out exception);
        }

        protected virtual bool TrySetProperty(long objectId, string name, object value, out string exception)
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
        /// <param name="analyseProperties">Analyse properties for inclusion in metadata model</param>
        /// <param name="readPropertyValue">When analysis is done on a property, if true then get it's value for transmission over WCF</param>
        /// <param name="nameConverter">convert names of properties/methods</param>
        private void AnalyseObjectForBinding(JavascriptObject obj, bool analyseMethods, bool analyseProperties, bool readPropertyValue)
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
                    if (methodInfo.ReturnType == typeof(Type) || Attribute.IsDefined(methodInfo, typeof(JavascriptIgnoreAttribute)))
                    {
                        continue;
                    }

                    var jsMethod = CreateJavaScriptMethod(methodInfo, nameConverter);
                    obj.Methods.Add(jsMethod);
                }
            }

            if (analyseProperties)
            {
                foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => !p.IsSpecialName))
                {
                    //https://msdn.microsoft.com/en-us/library/system.reflection.propertyinfo.getindexparameters(v=vs.110).aspx
                    //An array of type ParameterInfo containing the parameters for the indexes. If the property is not indexed, the array has 0 (zero) elements.
                    //According to MSDN array has zero elements when it's not an indexer, so in theory no null check is required
                    var isIndexer = propertyInfo.GetIndexParameters().Length > 0;
                    var hasIgnoreAttribute = Attribute.IsDefined(propertyInfo, typeof(JavascriptIgnoreAttribute));
                    if (propertyInfo.PropertyType == typeof(Type) || isIndexer || hasIgnoreAttribute)
                    {
                        continue;
                    }

                    var jsProperty = CreateJavaScriptProperty(propertyInfo);
                    if (jsProperty.IsComplexType)
                    {
                        var jsObject = CreateJavascriptObject(rootObject: false);
                        jsObject.Name = propertyInfo.Name;
                        jsObject.JavascriptName = nameConverter == null ? propertyInfo.Name : nameConverter.ConvertToJavascript(propertyInfo);
                        jsObject.Value = jsProperty.GetValue(obj.Value);
                        jsProperty.JsObject = jsObject;

                        AnalyseObjectForBinding(jsProperty.JsObject, analyseMethods, analyseProperties: true, readPropertyValue: readPropertyValue);
                    }
                    else if (readPropertyValue)
                    {
                        jsProperty.PropertyValue = jsProperty.GetValue(obj.Value);
                    }
                    obj.Properties.Add(jsProperty);
                }
            }
        }

        private void RaiseResolveObjectEvent(string name)
        {
            ResolveObject?.Invoke(this, new JavascriptBindingEventArgs(this, name));
        }

        private static JavascriptMethod CreateJavaScriptMethod(MethodInfo methodInfo, IJavascriptNameConverter nameConverter)
        {
            var jsMethod = new JavascriptMethod();

            jsMethod.ManagedName = methodInfo.Name;
            jsMethod.JavascriptName = nameConverter == null ? methodInfo.Name : nameConverter.ConvertToJavascript(methodInfo);
            jsMethod.Function = methodInfo.Invoke;
            jsMethod.ReturnType = methodInfo.ReturnType;
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

        private JavascriptProperty CreateJavaScriptProperty(PropertyInfo propertyInfo)
        {
            var jsProperty = new JavascriptProperty();

            jsProperty.ManagedName = propertyInfo.Name;
            jsProperty.JavascriptName = nameConverter == null ? propertyInfo.Name : nameConverter.ConvertToJavascript(propertyInfo);
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

            var nullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (nullable)
            {
                baseType = Nullable.GetUnderlyingType(type);
            }

            if (baseType == null || baseType.IsArray || baseType.Namespace?.StartsWith("System") == true)
            {
                return false;
            }

            if (baseType.IsValueType && !baseType.IsPrimitive && !baseType.IsEnum)
            {
                return false;
            }

            return !baseType.IsPrimitive && baseType != typeof(string);
        }
    }
}
