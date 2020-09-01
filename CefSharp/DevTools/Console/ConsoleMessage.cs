// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Console
{
    /// <summary>
    /// Console message.
    /// </summary>
    public class ConsoleMessage
    {
        /// <summary>
        /// Message source.
        /// </summary>
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Message severity.
        /// </summary>
        public string Level
        {
            get;
            set;
        }

        /// <summary>
        /// Message text.
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// URL of the message origin.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Line number in the resource that generated this message (1-based).
        /// </summary>
        public int Line
        {
            get;
            set;
        }

        /// <summary>
        /// Column number in the resource that generated this message (1-based).
        /// </summary>
        public int Column
        {
            get;
            set;
        }
    }
}