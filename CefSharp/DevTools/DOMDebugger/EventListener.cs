// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMDebugger
{
    /// <summary>
    /// Object event listener.
    /// </summary>
    public class EventListener
    {
        /// <summary>
        /// `EventListener`'s type.
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// `EventListener`'s useCapture.
        /// </summary>
        public bool UseCapture
        {
            get;
            set;
        }

        /// <summary>
        /// `EventListener`'s passive flag.
        /// </summary>
        public bool Passive
        {
            get;
            set;
        }

        /// <summary>
        /// `EventListener`'s once flag.
        /// </summary>
        public bool Once
        {
            get;
            set;
        }

        /// <summary>
        /// Script id of the handler code.
        /// </summary>
        public string ScriptId
        {
            get;
            set;
        }

        /// <summary>
        /// Line number in the script (0-based).
        /// </summary>
        public int LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Column number in the script (0-based).
        /// </summary>
        public int ColumnNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Event handler function value.
        /// </summary>
        public Runtime.RemoteObject Handler
        {
            get;
            set;
        }

        /// <summary>
        /// Event original handler function value.
        /// </summary>
        public Runtime.RemoteObject OriginalHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Node the listener is added to (if any).
        /// </summary>
        public int BackendNodeId
        {
            get;
            set;
        }
    }
}