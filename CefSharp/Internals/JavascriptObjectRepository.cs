using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CefSharp.Internals
{
    public class JavascriptObjectRepository : DisposableResource
    {
        private readonly Dictionary<long, JavascriptObject> objects = new Dictionary<long, JavascriptObject>();
        public JavascriptObject RootObject { get; private set; }

        public JavascriptObjectRepository()
        {
            RootObject = CreateJavascriptObject();
        }

        internal JavascriptObject CreateJavascriptObject()
        {
            var result = new JavascriptObject();
            objects[result.Id] = result;

            return result;
        }

        public void Register(string name, object value)
        {
            var member = new JavascriptProperty
            {
                Value = CreateJavascriptObject(),
                ManagedName = name,
                JavascriptName = JavascriptMember.LowercaseFirst(name),
                IsComplexType = true,
                GetValue = obj => value
            };
            member.Value.Value = value;
            Analyse(member.Value);
            
            RootObject.Members.Add(member);
        }

        public void Analyse(JavascriptObject obj)
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

            foreach (var methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(p => !p.IsSpecialName))
            {
                // Type objects can not be serialized.
                if (methodInfo.ReturnType == typeof(Type))
                {
                    continue;
                }
                var jsMethod = new JavascriptMethod();
                jsMethod.Analyse(methodInfo);
                obj.Members.Add(jsMethod);
            }

            foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => !p.IsSpecialName))
            {
                if (propertyInfo.PropertyType == typeof(Type))
                {
                    continue;
                }

                var jsProperty = new JavascriptProperty
                {
                    Value = CreateJavascriptObject()
                };

                jsProperty.Analyse(propertyInfo);
                jsProperty.Value.Value = jsProperty.GetValue(obj.Value);
                Analyse(jsProperty.Value);
                obj.Members.Add(jsProperty);
            }
        }

        public bool TryCallMethod(long objectId, string name, object[] parameters, out object result)
        {
            result = null;
            JavascriptObject obj;
            if (!objects.TryGetValue(objectId, out obj))
            {
                return false;
            }

            var method = obj.Members.OfType<JavascriptMethod>().FirstOrDefault(p => p.ManagedName == name);
            if (method == null)
            {
                throw new InvalidOperationException(string.Format("Method {0} not found on Object of Type {1}", name, obj.Value.GetType()));
            }

            result = method.Function(obj.Value, parameters);
            return true;
        }

        public bool TryGetProperty(long objectId, string name, out object result)
        {
            result = null;
            JavascriptObject obj;
            if (!objects.TryGetValue(objectId, out obj))
            {
                return false;
            }

            var property = obj.Members.OfType<JavascriptProperty>().FirstOrDefault(p => p.ManagedName == name);
            if (property == null)
            {
                throw new InvalidOperationException(string.Format("Property {0} not found on Object of Type {1}", name, obj.Value.GetType()));
            }

            result = property.GetValue(obj.Value);
            return true;
        }

        public bool TrySetProperty(long objectId, string name, object value)
        {
            JavascriptObject obj;
            if (!objects.TryGetValue(objectId, out obj))
            {
                return false;
            }

            var property = obj.Members.OfType<JavascriptProperty>().FirstOrDefault(p => p.ManagedName == name);
            if (property == null)
            {
                throw new InvalidOperationException(string.Format("Property {0} not found on Object of Type {1}", name, obj.Value.GetType()));
            }

            property.SetValue(obj.Value, value);
            return true;
        }
    }
}
