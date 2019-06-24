// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public class ResourceRequestHandlerFactoryItem
    {
        /// <summary>
        /// Data
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Mime Type
        /// </summary>
        public string MimeType { get; private set; }

        /// <summary>
        /// Whether or not the handler should be used once (true) or until manually unregistered (false)
        /// </summary>
        public bool OneTimeUse { get; private set; }

        /// <summary>
        /// DefaultResourceHandlerFactoryItem constructor
        /// </summary>
        /// <param name="data">The data in byte[] format that will be used for the response</param>
        /// <param name="mimeType">mime type</param>
        /// <param name="oneTimeUse">Whether or not the handler should be used once (true) or until manually unregistered (false)</param>
        public ResourceRequestHandlerFactoryItem(byte[] data, string mimeType, bool oneTimeUse)
        {
            Data = data;
            MimeType = mimeType;
            OneTimeUse = oneTimeUse;
        }
    }
}
