// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    /// <summary>
    /// This maps the registered objects in the browser process
    /// to the reflection data necessary to update the objects,
    /// and mapping information to how the object/method/proprerty
    /// will be exposed to JavaScript.
    /// </summary>
    [DataContract]
    public class JavascriptObject //: DynamicObject maybe later
    {
        /// <summary>
        /// Indicates if this <see cref="JavascriptObject"/> represents an Array
        /// </summary>
        private bool _isArray;

        /// <summary>
        /// Identifies the <see cref="JavascriptObject" /> for BrowserProcess to RenderProcess communication
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string JavascriptName { get; set; }

        /// <summary>
        /// Indicate if JavascriptName is camel case or not
        /// </summary>
        public bool CamelCaseJavascriptNames { get; set; }

        /// <summary>
        /// Developer defined predicate when registering an object to filter out members from the object
        /// </summary>
        public Func<MemberInfo, bool> Predicate { get; set; }

        /// <summary>
        /// Gets the methods of the <see cref="JavascriptObject" />.
        /// </summary>
        [DataMember]
        public List<JavascriptMethod> Methods { get; private set; }

        /// <summary>
        /// Gets the properties of the <see cref="JavascriptObject" />.
        /// </summary>
        [DataMember]
        public List<JavascriptProperty> Properties { get; private set; }

        /// <summary>
        /// Indicate if the <see cref="JavascriptObject" /> is null, so that on browser side we don't need to create an cef object.
        /// </summary>
        [DataMember]
        public bool IsNull { get; private set; }

        /// <summary>
        /// Gets or sets a delegate which is called when binding occurred.  
        /// </summary>
        internal Action LateBinding { private get; set; }

        /// <summary>
        /// Calls <see cref="LateBinding" />.
        /// </summary>
        /// <returns>Value if it's an array, else this.</returns>
        internal Object Bind()
        {
            if (LateBinding != null)
            {
                LateBinding();
            }
            
            return _isArray ? Value : this;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value { get; private set; }

        public void SetValue(object value)
        {
            if (Value == value)
            {
                return;
            }
            Value = value;
            _isArray = value != null && value.GetType().IsArray;
            IsNull = value == null;
            // need to clear methods and properties when new value is set
            Methods.Clear();
            Properties.Clear();
        }

        public JavascriptObject()
        {
            Methods = new List<JavascriptMethod>();
            Properties = new List<JavascriptProperty>();
            IsNull = true; // Value starts as null
        }

        public override string ToString()
        {
            return Value != null ? Value.ToString() : base.ToString();
        }
    }
}
