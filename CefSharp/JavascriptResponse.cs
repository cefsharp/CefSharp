using System.Collections.Generic;
using System.Runtime.Serialization;
using CefSharp.Internals;

namespace CefSharp
{
    [DataContract]
    [KnownType(typeof (object[]))]
    [KnownType(typeof (JavascriptCallback))]
    [KnownType(typeof (Dictionary<string, object>))]
    public class JavascriptResponse
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public object Result { get; set; }
    }
}
