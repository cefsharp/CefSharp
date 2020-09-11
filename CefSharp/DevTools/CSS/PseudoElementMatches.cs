// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// CSS rule collection for a single pseudo style.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class PseudoElementMatches : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        public CefSharp.DevTools.DOM.PseudoType PseudoType
        {
            get
            {
                return (CefSharp.DevTools.DOM.PseudoType)(StringToEnum(typeof(CefSharp.DevTools.DOM.PseudoType), pseudoType));
            }

            set
            {
                pseudoType = (EnumToString(value));
            }
        }

        /// <summary>
        /// Pseudo element type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pseudoType"), IsRequired = (true))]
        internal string pseudoType
        {
            get;
            set;
        }

        /// <summary>
        /// Matches of CSS rules applicable to the pseudo style.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("matches"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.RuleMatch> Matches
        {
            get;
            set;
        }
    }
}