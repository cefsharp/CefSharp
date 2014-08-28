using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptProperty : JavascriptMember
    {
        [DataMember]
        public JavascriptObject Value { get; set; }

        /// <summary>
        /// Gets or sets a delegate which is used to set the property / field value in the managed object.
        /// </summary>
        public Action<object, object> SetValue { get; set; }

        /// <summary>
        /// Gets or sets a delegate which is used to get the property / field value from the managed object.
        /// </summary>
        public Func<object, object> GetValue { get; set; }
    }
}
