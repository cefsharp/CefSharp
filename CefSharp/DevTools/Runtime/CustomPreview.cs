// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// CustomPreview
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CustomPreview : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The JSON-stringified result of formatter.header(object, config) call.
        /// It contains json ML array that represents RemoteObject.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("header"), IsRequired = (true))]
        public string Header
        {
            get;
            set;
        }

        /// <summary>
        /// If formatter returns true as a result of formatter.hasBody call then bodyGetterId will
        /// contain RemoteObjectId for the function that returns result of formatter.body(object, config) call.
        /// The result value is json ML array.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("bodyGetterId"), IsRequired = (false))]
        public string BodyGetterId
        {
            get;
            set;
        }
    }
}