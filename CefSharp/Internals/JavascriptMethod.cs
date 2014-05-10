using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptMethod : JavascriptMember
    {
        public JavascriptMethod()
        {
            Description = new JavascriptMethodDescription();
        }

        public new JavascriptMethodDescription Description 
        {
            get { return (JavascriptMethodDescription)base.Description; }
            set { base.Description = value; }
        }
    }
}
