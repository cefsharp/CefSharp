using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptMethod : JavascriptMember
    {
        /// <summary>
        /// Gets or sets a delegate which is used to invoke the method if the member is a method. 
        /// </summary>
        public Func<object, object[], object> Function { get; set; }

        public void Analyse(MethodInfo method)
        {
            ManagedName = method.Name;
            JavascriptName = LowercaseFirst(method.Name);
            Function = method.Invoke;
        }
    }
}
