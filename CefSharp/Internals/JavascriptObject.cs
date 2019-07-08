// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Runtime.Serialization;
using CefSharp.ModelBinding;

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
        /// Identifies the <see cref="JavascriptObject" /> for BrowserProcess to RenderProcess communication
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string JavascriptName { get; set; }

        /// <summary>
        /// Indicate if this object bound as async
        /// </summary>
        [DataMember]
        public bool IsAsync { get; set; }

        /// <summary>
        /// Indicate if JavascriptName is camel case or not
        /// </summary>
        public bool CamelCaseJavascriptNames { get; set; }

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
        /// A javascript object is created for every object, even those that are sub objects
        /// it's important we only transmit the Root Objects (top level/parent)
        /// </summary>
        public bool RootObject { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value { get; set; }

        public IBinder Binder { get; set; }

        public IMethodInterceptor MethodInterceptor { get; set; }

        public JavascriptObject()
        {
            Methods = new List<JavascriptMethod>();
            Properties = new List<JavascriptProperty>();
        }

        public override string ToString()
        {
            return Value != null ? Value.ToString() : base.ToString();
        }
    }
}
