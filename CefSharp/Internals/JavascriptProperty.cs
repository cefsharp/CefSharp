using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptProperty : JavascriptMember
    {
        [DataMember]
        public JavascriptObject Value { get; set; }
    }
}
