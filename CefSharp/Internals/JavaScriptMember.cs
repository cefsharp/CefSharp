using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavaScriptMember
    {
        public JavaScriptMemberDescription Description { get; set; }

        [DataMember]
        public long DescriptionId { get; set; }

        [DataMember]
        public JavaScriptObject Value { get; set; }
    }
}
