// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// KeyType Enum.
    /// Maps to https://magpcss.org/ceforum/apidocs3/projects/(default)/cef_key_event_type_t.html
    /// </summary>
    public enum KeyType
    {
        /// <summary>
        /// Notification that a key transitioned from"up" to"down".
        /// </summary>
        RawKeyDown = 0,
        /// <summary>
        /// Notification that a key was pressed. This does not necessarily correspond to a character depending on the key and language.
        /// Use <seealso cref="Char"/> for character input.
        /// </summary>
        KeyDown,
        /// <summary>
        /// Notification that a key was released.
        /// </summary>
        KeyUp,
        /// <summary>
        /// Notification that a character was typed. Use this for text input. Key
        /// down events may generate 0, 1, or more than one character event depending
        /// on the key, locale, and operating system.
        /// </summary>
        Char,
    };
}
