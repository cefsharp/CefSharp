// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Enums
{
    /// <summary>
    /// Input mode of a virtual keyboard. These constants match their equivalents
    /// in Chromium's text_input_mode.h and should not be renumbered.
    /// See https://html.spec.whatwg.org/#input-modalities:-the-inputmode-attribute
    /// </summary>
    public enum TextInputMode
    {
        /// <summary>
        /// An enum constant representing the default option.
        /// </summary>
        Default = 0,
        /// <summary>
        /// An enum constant representing the none option.
        /// </summary>
        None,
        /// <summary>
        /// An enum constant representing the text option.
        /// </summary>
        Text,
        /// <summary>
        /// An enum constant representing the tel option.
        /// </summary>
        Tel,
        /// <summary>
        /// An enum constant representing the URL option.
        /// </summary>
        Url,
        /// <summary>
        /// An enum constant representing the mail option.
        /// </summary>
        EMail,
        /// <summary>
        /// An enum constant representing the numeric option.
        /// </summary>
        Numeric,
        /// <summary>
        /// An enum constant representing the decimal option.
        /// </summary>
        Decimal,
        /// <summary>
        /// An enum constant representing the search option.
        /// </summary>
        Search,
        /// <summary>
        /// An enum constant representing the Maximum option.
        /// </summary>
        Max = Search
    }
}
