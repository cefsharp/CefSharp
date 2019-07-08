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
        Default = 0,
        None,
        Text,
        Tel,
        Url,
        EMail,
        Numeric,
        Decimal,
        Search,
        Max = Search
    }
}
