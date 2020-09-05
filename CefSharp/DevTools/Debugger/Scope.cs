// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Scope description.
    /// </summary>
    public class Scope : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Scope type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Object representing the scope. For `global` and `with` scopes it represents the actual
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("object"), IsRequired = (true))]
        public Runtime.RemoteObject Object
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (false))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Location in the source code where scope starts
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("startLocation"), IsRequired = (false))]
        public Location StartLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Location in the source code where scope ends
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("endLocation"), IsRequired = (false))]
        public Location EndLocation
        {
            get;
            set;
        }
    }
}