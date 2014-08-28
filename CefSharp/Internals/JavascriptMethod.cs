using System;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptMethod
    {
        /// <summary>
        /// Gets or sets a delegate which is used to invoke the method if the member is a method. 
        /// </summary>
        public Func<object, object[], object> Function { get; set; }

        /// <summary>
        /// Identifies the <see cref="JavascriptMethod" /> for BrowserProcess to RenderProcess communication
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the managed property.
        /// </summary>
        [DataMember]
        public string ManagedName { get; set; }

        /// <summary>
        /// Gets or sets the name of the property in the JavaScript runtime.
        /// </summary>
        [DataMember]
        public string JavascriptName { get; set; }

        public override string ToString()
        {
            return ManagedName ?? base.ToString();
        }
    }
}
