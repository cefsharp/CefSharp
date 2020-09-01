// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Mirror object referencing original JavaScript object.
    /// </summary>
    public class RemoteObject
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Object subtype hint. Specified for `object` or `wasm` type values only.
        /// </summary>
        public string Subtype
        {
            get;
            set;
        }

        /// <summary>
        /// Object class (constructor) name. Specified for `object` type values only.
        /// </summary>
        public string ClassName
        {
            get;
            set;
        }

        /// <summary>
        /// Remote object value in case of primitive values or JSON values (if it was requested).
        /// </summary>
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Primitive value which can not be JSON-stringified does not have `value`, but gets this
        public string UnserializableValue
        {
            get;
            set;
        }

        /// <summary>
        /// String representation of the object.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Unique object identifier (for non-primitive values).
        /// </summary>
        public string ObjectId
        {
            get;
            set;
        }

        /// <summary>
        /// Preview containing abbreviated property values. Specified for `object` type values only.
        /// </summary>
        public ObjectPreview Preview
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CustomPreview CustomPreview
        {
            get;
            set;
        }
    }
}