// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Location range within one script.
    /// </summary>
    public class LocationRange : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scriptId"), IsRequired = (true))]
        public string ScriptId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("start"), IsRequired = (true))]
        public ScriptPosition Start
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("end"), IsRequired = (true))]
        public ScriptPosition End
        {
            get;
            set;
        }
    }
}