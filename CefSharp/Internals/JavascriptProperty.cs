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
        public JavascriptProperty()
        {
            Value = new JavascriptObject();
            Description = new JavascriptPropertyDescription();
        }

        public new JavascriptPropertyDescription Description 
        {
            get { return (JavascriptPropertyDescription)base.Description; }
            set { base.Description = value; }
        }

        [DataMember]
        public JavascriptObject Value { get; set; }
    }
}
