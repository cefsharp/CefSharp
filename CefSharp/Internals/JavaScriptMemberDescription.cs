using System;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavaScriptMemberDescription
    {
        /// <summary>
        /// Identifies the <see cref="JavaScriptMemberDescription" /> for BrowserProcess to RenderProcess communication
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

        // The following members are used to cache reflection code to improve performance

        /// <summary>
        /// Gets or sets a delegate which is used to set the property / field value in the managed object.
        /// </summary>
        public Action<object, object> SetValue { get; set; }

        /// <summary>
        /// Gets or sets a delegate which is used to get the property / field value from the managed object.
        /// </summary>
        public Func<object, object> GetValue { get; set; }

        /// <summary>
        /// Gets or sets a delegate which is used to invoke the method if the member is a method. 
        /// </summary>
        public Delegate Function { get; set; }
    }
}