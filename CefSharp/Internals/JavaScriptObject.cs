using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavaScriptObject //: DynamicObject maybe later
    {
        private static long nextId = 0;
        private static readonly object Lock = new object();

        /// <summary>
        /// Identifies the <see cref="JavaScriptObject" /> for BrowserProcess to RenderProcess communication
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public long Id { get; private set; }

        /// <summary>
        /// Gets the members of the <see cref="JavaScriptObject" />.
        /// </summary>
        /// <value>
        /// The members.
        /// </value>
        [DataMember]
        public List<JavaScriptMember> Members { get; private set; }

        public JavaScriptObject()
        {
            lock (Lock)
            {
                Id = ++nextId;
            }

            Members = new List<JavaScriptMember>();
        }
    }
}
