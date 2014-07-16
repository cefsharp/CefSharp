using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [KnownType(typeof(JavascriptMethodDescription))]
    [KnownType(typeof(JavascriptPropertyDescription))] 
    [DataContract]
    public abstract class JavascriptMemberDescription
    {
        /// <summary>
        /// Identifies the <see cref="JavascriptMemberDescription" /> for BrowserProcess to RenderProcess communication
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the managed property.
        /// </summary>
        [DataMember]
        public string ManagedName { get; set; }

        /// <summary>
        /// Gets or sets the name of the property in the JavaScript runtime.
        /// </summary>
        [DataMember]
        public string JavascriptName { get; set; }

        [DataMember]
        public bool IsComplexType { get; set; }

        public static string LowercaseFirst(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return char.ToLower(str[0]) + str.Substring(1);
        }

        public override string ToString()
        {
            return ManagedName;
        }
    }
}