// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Stack entry for runtime errors and assertions.
    /// </summary>
    public class CallFrame
    {
        /// <summary>
        /// JavaScript function name.
        /// </summary>
        public string FunctionName
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script id.
        /// </summary>
        public string ScriptId
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script name or url.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script line number (0-based).
        /// </summary>
        public int LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script column number (0-based).
        /// </summary>
        public int ColumnNumber
        {
            get;
            set;
        }
    }
}