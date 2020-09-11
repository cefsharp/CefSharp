// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Description of an isolated world.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ExecutionContextDescription : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Unique id of the execution context. It can be used to specify in which execution context
        /// script evaluation should be performed.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("id"), IsRequired = (true))]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Execution context origin.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("origin"), IsRequired = (true))]
        public string Origin
        {
            get;
            set;
        }

        /// <summary>
        /// Human readable name describing given context.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Embedder-specific auxiliary data.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("auxData"), IsRequired = (false))]
        public object AuxData
        {
            get;
            set;
        }
    }
}