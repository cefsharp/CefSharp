using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptObject //: DynamicObject maybe later
    {
        private static long lastId;
        private static readonly object Lock = new object();

        /// <summary>
        /// Identifies the <see cref="JavascriptObject" /> for BrowserProcess to RenderProcess communication
        /// </summary>
        [DataMember]
        public long Id { get; protected set; }

        /// <summary>
        /// Gets the members of the <see cref="JavascriptObject" />.
        /// </summary>
        [DataMember]
        public List<JavascriptMember> Members { get; private set; }

        [DataMember]
        private object serializeableValue;
        private object value;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value
        {
            get { return value; }
            set
            {
                this.value = value;
                if (value == null)
                {
                    return;
                }

                var type = value.GetType();
                if (type.IsValueType || type == typeof(string))
                {
                    serializeableValue = value;
                }
            }
        }

        public JavascriptObject()
        {
            lock (Lock)
            {
                Id = lastId++;
            }

            Members = new List<JavascriptMember>();
        }
        
        public void Analyse(JavascriptObjectRepository repository)
        {
            if (value == null)
            {
                return;
            }
            
            var type = value.GetType();
            if (type.IsPrimitive || type == typeof(string))
            {
                return;
            }

            foreach (var methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(p => !p.IsSpecialName))
            {
                // types can not be serialized
                if (methodInfo.ReturnType == typeof (Type))
                {
                    continue;
                }
                var jsMethod = new JavascriptMethod();
                jsMethod.Description.Analyse(methodInfo);
                Members.Add(jsMethod);
            }

            foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => !p.IsSpecialName))
            {
                if (propertyInfo.PropertyType == typeof(Type))
                {
                    continue;
                }
                var jsProperty = new JavascriptProperty();
                jsProperty.Value = repository.CreateJavascriptObject();
                jsProperty.Description.Analyse(propertyInfo);
                jsProperty.Value.Value = jsProperty.Description.GetValue(value);
                jsProperty.Value.Analyse(repository);
                Members.Add(jsProperty);
            }
        }

        public override string ToString()
        {
            if (Value != null)
            {
                return Value.ToString();
            }
            return base.ToString();
        }
    }
}
