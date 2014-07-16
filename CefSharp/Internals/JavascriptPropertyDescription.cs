using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptPropertyDescription : JavascriptMemberDescription
    {
        // The following members are used to cache reflection code to improve performance

        /// <summary>
        /// Gets or sets a delegate which is used to set the property / field value in the managed object.
        /// </summary>
        public Action<object, object> SetValue { get; set; }

        /// <summary>
        /// Gets or sets a delegate which is used to get the property / field value from the managed object.
        /// </summary>
        public Func<object, object> GetValue { get; set; }

        public void Analyse(PropertyInfo propertyInfo)
        {
            ManagedName = propertyInfo.Name;
            JavascriptName = LowercaseFirst(propertyInfo.Name);
            SetValue = (o, v) => propertyInfo.SetValue(o, v, null);
            GetValue = (o) => propertyInfo.GetValue(o, null);

            IsComplexType = !propertyInfo.PropertyType.IsPrimitive;
        }
    }
}