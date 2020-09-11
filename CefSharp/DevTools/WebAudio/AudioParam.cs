// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAudio
{
    /// <summary>
    /// Protocol object for AudioParam
    /// </summary>
    public class AudioParam : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// ParamId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("paramId"), IsRequired = (true))]
        public string ParamId
        {
            get;
            set;
        }

        /// <summary>
        /// NodeId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodeId"), IsRequired = (true))]
        public string NodeId
        {
            get;
            set;
        }

        /// <summary>
        /// ContextId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contextId"), IsRequired = (true))]
        public string ContextId
        {
            get;
            set;
        }

        /// <summary>
        /// ParamType
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("paramType"), IsRequired = (true))]
        public string ParamType
        {
            get;
            set;
        }

        /// <summary>
        /// Rate
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("rate"), IsRequired = (true))]
        public CefSharp.DevTools.WebAudio.AutomationRate Rate
        {
            get;
            set;
        }

        /// <summary>
        /// DefaultValue
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("defaultValue"), IsRequired = (true))]
        public long DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// MinValue
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("minValue"), IsRequired = (true))]
        public long MinValue
        {
            get;
            set;
        }

        /// <summary>
        /// MaxValue
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("maxValue"), IsRequired = (true))]
        public long MaxValue
        {
            get;
            set;
        }
    }
}