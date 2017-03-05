// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptProperty
    {
        [DataMember]
        public JavascriptObject JsObject { get; set; }

        /// <summary>
        /// Gets or sets a delegate which is used to set the property / field value in the managed object.
        /// </summary>
        public Action<object, object> SetValue { get; set; }

        /// <summary>
        /// Gets or sets a delegate which is used to get the property / field value from the managed object.
        /// </summary>
        public Func<object, object> GetValue { get; set; }

        /// <summary>
        /// Identifies the <see cref="JavascriptProperty" /> for BrowserProcess to RenderProcess communication
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

        /// <summary>
        /// Gets or sets if this property represents a complex type
        /// </summary>
        [DataMember]
        public bool IsComplexType { get; set; }

        /// <summary>
        /// Gets or sets if this property is read-only
        /// </summary>
        [DataMember]
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the property value
        /// Only primative types can be stored in this property
        /// </summary>
        [DataMember]
        public object PropertyValue { get; set; }

        public override string ToString()
        {
            return ManagedName ?? base.ToString();
        }
    }
}
