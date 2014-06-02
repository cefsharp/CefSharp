using System.Runtime.Serialization;

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
