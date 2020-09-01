// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// CustomPreview
    /// </summary>
    public class CustomPreview
    {
        /// <summary>
        /// The JSON-stringified result of formatter.header(object, config) call.
        public string Header
        {
            get;
            set;
        }

        /// <summary>
        /// If formatter returns true as a result of formatter.hasBody call then bodyGetterId will
        public string BodyGetterId
        {
            get;
            set;
        }
    }
}