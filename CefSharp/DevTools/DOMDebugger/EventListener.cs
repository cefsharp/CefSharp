// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMDebugger
{
    /// <summary>
    /// Object event listener.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class EventListener : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// `EventListener`'s type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// `EventListener`'s useCapture.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("useCapture"), IsRequired = (true))]
        public bool UseCapture
        {
            get;
            set;
        }

        /// <summary>
        /// `EventListener`'s passive flag.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("passive"), IsRequired = (true))]
        public bool Passive
        {
            get;
            set;
        }

        /// <summary>
        /// `EventListener`'s once flag.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("once"), IsRequired = (true))]
        public bool Once
        {
            get;
            set;
        }

        /// <summary>
        /// Script id of the handler code.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scriptId"), IsRequired = (true))]
        public string ScriptId
        {
            get;
            set;
        }

        /// <summary>
        /// Line number in the script (0-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lineNumber"), IsRequired = (true))]
        public int LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Column number in the script (0-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("columnNumber"), IsRequired = (true))]
        public int ColumnNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Event handler function value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("handler"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.RemoteObject Handler
        {
            get;
            set;
        }

        /// <summary>
        /// Event original handler function value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("originalHandler"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.RemoteObject OriginalHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Node the listener is added to (if any).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("backendNodeId"), IsRequired = (false))]
        public int? BackendNodeId
        {
            get;
            set;
        }
    }
}