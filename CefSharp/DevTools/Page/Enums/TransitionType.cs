// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Transition type.
    /// </summary>
    public enum TransitionType
    {
        /// <summary>
        /// link
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("link"))]
        Link,
        /// <summary>
        /// typed
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("typed"))]
        Typed,
        /// <summary>
        /// address_bar
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("address_bar"))]
        Address_bar,
        /// <summary>
        /// auto_bookmark
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("auto_bookmark"))]
        Auto_bookmark,
        /// <summary>
        /// auto_subframe
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("auto_subframe"))]
        Auto_subframe,
        /// <summary>
        /// manual_subframe
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("manual_subframe"))]
        Manual_subframe,
        /// <summary>
        /// generated
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("generated"))]
        Generated,
        /// <summary>
        /// auto_toplevel
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("auto_toplevel"))]
        Auto_toplevel,
        /// <summary>
        /// form_submit
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("form_submit"))]
        Form_submit,
        /// <summary>
        /// reload
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("reload"))]
        Reload,
        /// <summary>
        /// keyword
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("keyword"))]
        Keyword,
        /// <summary>
        /// keyword_generated
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("keyword_generated"))]
        Keyword_generated,
        /// <summary>
        /// other
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("other"))]
        Other
    }
}