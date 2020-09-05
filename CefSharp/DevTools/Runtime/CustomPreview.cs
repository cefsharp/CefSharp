// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// CustomPreview
    /// </summary>
    public class CustomPreview : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The JSON-stringified result of formatter.header(object, config) call.
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("header"), IsRequired = (true))]
        public string Header
        {
            get;
            set;
        }

        /// <summary>
        /// If formatter returns true as a result of formatter.hasBody call then bodyGetterId will
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("bodyGetterId"), IsRequired = (false))]
        public string BodyGetterId
        {
            get;
            set;
        }
    }
}