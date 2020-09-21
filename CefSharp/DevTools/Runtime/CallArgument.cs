// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Represents function call argument. Either remote object id `objectId`, primitive `value`,
    /// unserializable primitive value or neither of (for undefined) them should be specified.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CallArgument : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Primitive value or serializable javascript object.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (false))]
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Primitive value which can not be JSON-stringified.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("unserializableValue"), IsRequired = (false))]
        public string UnserializableValue
        {
            get;
            set;
        }

        /// <summary>
        /// Remote object handle.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("objectId"), IsRequired = (false))]
        public string ObjectId
        {
            get;
            set;
        }
    }
}