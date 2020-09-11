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
        AddressBar,
        /// <summary>
        /// auto_bookmark
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("auto_bookmark"))]
        AutoBookmark,
        /// <summary>
        /// auto_subframe
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("auto_subframe"))]
        AutoSubframe,
        /// <summary>
        /// manual_subframe
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("manual_subframe"))]
        ManualSubframe,
        /// <summary>
        /// generated
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("generated"))]
        Generated,
        /// <summary>
        /// auto_toplevel
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("auto_toplevel"))]
        AutoToplevel,
        /// <summary>
        /// form_submit
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("form_submit"))]
        FormSubmit,
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
        KeywordGenerated,
        /// <summary>
        /// other
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("other"))]
        Other
    }
}