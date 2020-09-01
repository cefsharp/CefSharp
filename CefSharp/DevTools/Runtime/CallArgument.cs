// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Represents function call argument. Either remote object id `objectId`, primitive `value`,
    public class CallArgument
    {
        /// <summary>
        /// Primitive value or serializable javascript object.
        /// </summary>
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Primitive value which can not be JSON-stringified.
        /// </summary>
        public string UnserializableValue
        {
            get;
            set;
        }

        /// <summary>
        /// Remote object handle.
        /// </summary>
        public string ObjectId
        {
            get;
            set;
        }
    }
}