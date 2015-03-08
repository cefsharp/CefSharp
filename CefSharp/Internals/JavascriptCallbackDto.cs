using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptCallbackDto
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public int BrowserId { get; set; }
    }
}
