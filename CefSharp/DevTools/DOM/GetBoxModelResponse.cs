// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetBoxModelResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetBoxModelResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.DOM.BoxModel model
        {
            get;
            set;
        }

        /// <summary>
        /// model
        /// </summary>
        public CefSharp.DevTools.DOM.BoxModel Model
        {
            get
            {
                return model;
            }
        }
    }
}