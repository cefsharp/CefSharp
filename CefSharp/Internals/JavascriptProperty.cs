using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptProperty : JavascriptMember
    {
        public JavascriptProperty()
        {
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
