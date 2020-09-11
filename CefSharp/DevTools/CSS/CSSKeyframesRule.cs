// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// CSS keyframes rule representation.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CSSKeyframesRule : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Animation name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("animationName"), IsRequired = (true))]
        public CefSharp.DevTools.CSS.Value AnimationName
        {
            get;
            set;
        }

        /// <summary>
        /// List of keyframes.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("keyframes"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.CSSKeyframeRule> Keyframes
        {
            get;
            set;
        }
    }
}