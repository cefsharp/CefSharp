using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptObject //: DynamicObject maybe later
    {
        private static long nextId = 0;
        private static readonly object Lock = new object();

        /// <summary>
        /// Identifies the <see cref="JavascriptObject" /> for BrowserProcess to RenderProcess communication
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public long Id { get; private set; }

        /// <summary>
        /// Gets the members of the <see cref="JavascriptObject" />.
        /// </summary>
        /// <value>
        /// The members.
        /// </value>
        [DataMember]
        public List<JavascriptMember> Members { get; private set; }
        
        [DataMember]
        private object serializeableValue;
        private object _value;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value 
        {
            get { return _value; }
            set 
            {
                _value = value;
                if ( value != null )
                {
                    var type = value.GetType();
                    if (type.IsValueType || type == typeof (string))
                    {
                        serializeableValue = value;
                    }
                }
            }
        }

        public JavascriptObject()
        {
            lock (Lock)
            {
                Id = ++nextId;
            }

            Members = new List<JavascriptMember>();
        }

        public void Analyse()
        {
            foreach (var methodInfo in _value.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where( p => !p.IsSpecialName ) )
            {
                var jsMethod = new JavascriptMethod();
                jsMethod.Description.Analyse(methodInfo);
                Members.Add(jsMethod);
            }

            foreach (var propertyInfo in _value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => !p.IsSpecialName))
            {
                var jsProperty = new JavascriptProperty();
                jsProperty.Description.Analyse(propertyInfo);
                jsProperty.Value.Value = jsProperty.Description.GetValue(_value);
                jsProperty.Value.Analyse();
                Members.Add(jsProperty);
            }
        }
    }
}
