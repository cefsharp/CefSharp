// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Mirror object referencing original JavaScript object.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RemoteObject : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Object type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Object subtype hint. Specified for `object` or `wasm` type values only.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("subtype"), IsRequired = (false))]
        public string Subtype
        {
            get;
            set;
        }

        /// <summary>
        /// Object class (constructor) name. Specified for `object` type values only.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("className"), IsRequired = (false))]
        public string ClassName
        {
            get;
            set;
        }

        /// <summary>
        /// Remote object value in case of primitive values or JSON values (if it was requested).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (false))]
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Primitive value which can not be JSON-stringified does not have `value`, but gets this
        /// property.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("unserializableValue"), IsRequired = (false))]
        public string UnserializableValue
        {
            get;
            set;
        }

        /// <summary>
        /// String representation of the object.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("description"), IsRequired = (false))]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Unique object identifier (for non-primitive values).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("objectId"), IsRequired = (false))]
        public string ObjectId
        {
            get;
            set;
        }

        /// <summary>
        /// Preview containing abbreviated property values. Specified for `object` type values only.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("preview"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.ObjectPreview Preview
        {
            get;
            set;
        }

        /// <summary>
        /// CustomPreview
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("customPreview"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.CustomPreview CustomPreview
        {
            get;
            set;
        }
    }
}