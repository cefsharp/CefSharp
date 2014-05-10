using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp.Internals
{
    public class JavascriptObjectRepository : DisposableResource
    {
        private JavascriptObject rootObject;
        private Dictionary<long, JavascriptObject> objects = new Dictionary<long, JavascriptObject>();

        public JavascriptObjectRepository()
        {
            rootObject = new JavascriptObject();
            objects[rootObject.Id] = rootObject;

            //TODO: remove this dummy code
            var bar = new JavascriptMethod();
            bar.Description.JavascriptName = "bar";
            bar.Description.ManagedName = "Bar";

            var foo = new JavascriptProperty();
            foo.Description.JavascriptName = "foo";
            foo.Description.ManagedName = "Foo";
            foo.Value.Members.Add(bar);

            rootObject.Members.Add(foo);
        }

        public void Register(string name, object value)
        {
            var member = new JavascriptProperty();
            member.Description.ManagedName = name;
            member.Description.JavascriptName = name;
            member.Value.Value = value;

            //TODO: analyze object and build a object graph

            rootObject.Members.Add(member);
        }

        public JavascriptObject RootObject 
        {
            get { return rootObject; }
        }

        public bool TryCallMethod(int objectId, string name, object[] parameters, out object result)
        {
            result = null;
            JavascriptObject obj;
            if (!objects.TryGetValue(objectId, out obj))
            {
                return false;
            }

            var method = obj.Members.OfType<JavascriptMethod>().FirstOrDefault(p => p.Description.ManagedName == name);
            if (method == null)
            {
                throw new InvalidOperationException(string.Format("Method {0} not found on Object of Type {1}", name, obj.Value.GetType()));
            }

            result = method.Description.Function.DynamicInvoke(obj.Value, parameters);
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

            var property = obj.Members.OfType<JavascriptProperty>().FirstOrDefault(p => p.Description.ManagedName == name);
            if (property == null)
            {
                throw new InvalidOperationException(string.Format("Property {0} not found on Object of Type {1}", name, obj.Value.GetType()));
            }

            result = property.Description.GetValue(obj.Value);
            return true;
        }

        public bool TrySetProperty(int objectId, string name, object value)
        {
            JavascriptObject obj;
            if (!objects.TryGetValue(objectId, out obj))
            {
                return false;
            }

            var property = obj.Members.OfType<JavascriptProperty>().FirstOrDefault(p => p.Description.ManagedName == name);
            if (property == null)
            {
                throw new InvalidOperationException(string.Format("Property {0} not found on Object of Type {1}", name, obj.Value.GetType()));
            }

            property.Description.SetValue(obj.Value, value);
            return true;
        }
    }
}
