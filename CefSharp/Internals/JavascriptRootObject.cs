using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptRootObject
    {
        [DataMember]
        public List<JavascriptObject> MemberObjects { get; set; }

        public JavascriptRootObject()
        {
            MemberObjects = new List<JavascriptObject>();
        }
    }
}
