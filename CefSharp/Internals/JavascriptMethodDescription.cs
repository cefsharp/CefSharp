using System;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptMethodDescription : JavascriptMemberDescription
    {
        // We cache the reflected delegate to improve performance a bit.

        /// <summary>
        /// Gets or sets a delegate which is used to invoke the method if the member is a method. 
        /// </summary>
        public Delegate Function { get; set; }
    }
}