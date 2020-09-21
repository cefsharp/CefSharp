// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Scope description.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
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
        /// object; for the rest of the scopes, it is artificial transient object enumerating scope
        /// variables as its properties.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("object"), IsRequired = (true))]
        public CefSharp.DevTools.Runtime.RemoteObject Object
        {
            get;
            set;
        }

        /// <summary>
        /// Name
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
        public CefSharp.DevTools.Debugger.Location StartLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Location in the source code where scope ends
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("endLocation"), IsRequired = (false))]
        public CefSharp.DevTools.Debugger.Location EndLocation
        {
            get;
            set;
        }
    }
}