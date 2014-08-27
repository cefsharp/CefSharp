using System;
using System.Collections.Generic;
using System.Linq;

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
            member.Value.Analyse(this);
            
            RootObject.Members.Add(member);
        }

        public bool TryCallMethod(int objectId, string name, object[] parameters, out object result)
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

        public bool TryGetProperty(int objectId, string name, out object result)
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

        public bool TrySetProperty(int objectId, string name, object value)
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
