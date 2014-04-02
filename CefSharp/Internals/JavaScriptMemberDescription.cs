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
        /// May be null as <see cref="JavaScriptObject" />s may have values attached that are not present in the managed object
        /// </summary>
        [DataMember]
        public string ManagedName { get; set; }

        /// <summary>
        /// Gets or sets the name of the JavaScriptProperty.
        /// </summary>
        [DataMember]
        public string JSName { get; set; }

        // The following members are used to cache reflection code to improve performance

        /// <summary>
        /// Gets or sets the set value Action which is used to set the property / field value in the managed Object.
        /// </summary>
        public Action<object, object> SetValue { get; set; }

        /// <summary>
        /// Gets or sets the get value Function which is used to get the property / field value from the managed Object.
        /// </summary>
        public Func<object, object> GetValue { get; set; }

        /// <summary>
        /// Gets or sets the function that is called if the Member is a Function.
        /// Will be null if the Member is no function.
        /// </summary>
        public Delegate Function { get; set; }
    }
}