using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptObject //: DynamicObject maybe later
    {
        private static long nextId = 0;
        private static readonly object Lock = new object();

        /// <summary>
        /// Identifies the <see cref="JavascriptObject" /> for BrowserProcess to RenderProcess communication
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public long Id { get; private set; }

        /// <summary>
        /// Gets the members of the <see cref="JavascriptObject" />.
        /// </summary>
        /// <value>
        /// The members.
        /// </value>
        [DataMember]
        public List<JavascriptMember> Members { get; private set; }

        public JavascriptObject()
        {
            lock (Lock)
            {
                Id = ++nextId;
            }

            Members = new List<JavascriptMember>();
        }
    }
}
