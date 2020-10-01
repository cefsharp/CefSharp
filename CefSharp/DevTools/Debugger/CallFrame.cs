// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// JavaScript call frame. Array of call frames form the call stack.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CallFrame : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Call frame identifier. This identifier is only valid while the virtual machine is paused.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("callFrameId"), IsRequired = (true))]
        public string CallFrameId
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the JavaScript function called on this call frame.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("functionName"), IsRequired = (true))]
        public string FunctionName
        {
            get;
            set;
        }

        /// <summary>
        /// Location in the source code.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("functionLocation"), IsRequired = (false))]
        public CefSharp.DevTools.Debugger.Location FunctionLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Location in the source code.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("location"), IsRequired = (true))]
        public CefSharp.DevTools.Debugger.Location Location
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script name or url.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (true))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Scope chain for this call frame.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scopeChain"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Debugger.Scope> ScopeChain
        {
            get;
            set;
        }

        /// <summary>
        /// `this` object for this call frame.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("this"), IsRequired = (true))]
        public CefSharp.DevTools.Runtime.RemoteObject This
        {
            get;
            set;
        }

        /// <summary>
        /// The value being returned, if the function is at return point.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("returnValue"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.RemoteObject ReturnValue
        {
            get;
            set;
        }
    }
}