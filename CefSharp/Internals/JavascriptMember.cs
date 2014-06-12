using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [KnownType(typeof(JavascriptMethod))]
    [KnownType(typeof(JavascriptProperty))] 
    [DataContract]
    public class JavascriptMember
    {
        [DataMember]
        public JavascriptMemberDescription Description { get; set; }

        [DataMember]
        public long DescriptionId { get; set; }
        
        public virtual void Bind( JavascriptObject owner )
        {
        }

        public override string ToString()
        {
            return Description != null ? Description.ToString() : base.ToString();
        }
    }
}
