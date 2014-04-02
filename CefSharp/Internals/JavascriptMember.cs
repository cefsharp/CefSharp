using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptMember
    {
        public JavascriptMemberDescription Description { get; set; }

        [DataMember]
        public long DescriptionId { get; set; }

        [DataMember]
        public JavascriptObject Value { get; set; }
    }
}
