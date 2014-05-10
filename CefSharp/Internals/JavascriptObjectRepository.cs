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
        }

        public void Register(string name, object value)
        {
            var member = new JavascriptProperty();
            member.Description.ManagedName = name;
            member.Description.JavascriptName = name;
            member.Value.Value = value;

            rootObject.Members.Add(member);
        }
    }
}
