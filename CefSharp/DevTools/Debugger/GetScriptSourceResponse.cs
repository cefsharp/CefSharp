// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// GetScriptSourceResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetScriptSourceResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string scriptSource
        {
            get;
            set;
        }

        /// <summary>
        /// scriptSource
        /// </summary>
        public string ScriptSource
        {
            get
            {
                return scriptSource;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string bytecode
        {
            get;
            set;
        }

        /// <summary>
        /// bytecode
        /// </summary>
        public byte[] Bytecode
        {
            get
            {
                return Convert(bytecode);
            }
        }
    }
}