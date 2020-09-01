// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// CSSComputedStyleProperty
    /// </summary>
    public class CSSComputedStyleProperty
    {
        /// <summary>
        /// Computed style property name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Computed style property value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }
    }
}