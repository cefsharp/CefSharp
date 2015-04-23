﻿// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptObject //: DynamicObject maybe later
    {
        private bool bound = false;

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
        internal bool CamelCaseJavascriptNames { get; set; }

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
        /// Gets or sets a delegate which is called when binding occured.  
        /// </summary>
        internal Action LateBinding { private get; set; }

        /// <summary>
        /// Calls <see cref="LateBinding" /> if not already bound.
        /// </summary>
        /// <returns>this, so that calls can be chained.</returns>
        internal JavascriptObject Bind()
        {
            if (!bound && LateBinding != null)
                LateBinding();
            bound = true;
            return this;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value { get; set; }

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
