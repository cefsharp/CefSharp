// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Accessibility
{
    /// <summary>
    /// Values of AXProperty name:
    /// - from 'busy' to 'roledescription': states which apply to every AX node
    /// - from 'live' to 'root': attributes which apply to nodes in live regions
    /// - from 'autocomplete' to 'valuetext': attributes which apply to widgets
    /// - from 'checked' to 'selected': states which apply to widgets
    /// - from 'activedescendant' to 'owns' - relationships between elements other than parent/child/sibling.
    /// </summary>
    public enum AXPropertyName
    {
        /// <summary>
        /// busy
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("busy"))]
        Busy,
        /// <summary>
        /// disabled
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("disabled"))]
        Disabled,
        /// <summary>
        /// editable
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("editable"))]
        Editable,
        /// <summary>
        /// focusable
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("focusable"))]
        Focusable,
        /// <summary>
        /// focused
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("focused"))]
        Focused,
        /// <summary>
        /// hidden
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("hidden"))]
        Hidden,
        /// <summary>
        /// hiddenRoot
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("hiddenRoot"))]
        HiddenRoot,
        /// <summary>
        /// invalid
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("invalid"))]
        Invalid,
        /// <summary>
        /// keyshortcuts
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("keyshortcuts"))]
        Keyshortcuts,
        /// <summary>
        /// settable
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("settable"))]
        Settable,
        /// <summary>
        /// roledescription
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("roledescription"))]
        Roledescription,
        /// <summary>
        /// live
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("live"))]
        Live,
        /// <summary>
        /// atomic
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("atomic"))]
        Atomic,
        /// <summary>
        /// relevant
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("relevant"))]
        Relevant,
        /// <summary>
        /// root
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("root"))]
        Root,
        /// <summary>
        /// autocomplete
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("autocomplete"))]
        Autocomplete,
        /// <summary>
        /// hasPopup
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("hasPopup"))]
        HasPopup,
        /// <summary>
        /// level
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("level"))]
        Level,
        /// <summary>
        /// multiselectable
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("multiselectable"))]
        Multiselectable,
        /// <summary>
        /// orientation
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("orientation"))]
        Orientation,
        /// <summary>
        /// multiline
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("multiline"))]
        Multiline,
        /// <summary>
        /// readonly
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("readonly"))]
        Readonly,
        /// <summary>
        /// required
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("required"))]
        Required,
        /// <summary>
        /// valuemin
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("valuemin"))]
        Valuemin,
        /// <summary>
        /// valuemax
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("valuemax"))]
        Valuemax,
        /// <summary>
        /// valuetext
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("valuetext"))]
        Valuetext,
        /// <summary>
        /// checked
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("checked"))]
        Checked,
        /// <summary>
        /// expanded
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("expanded"))]
        Expanded,
        /// <summary>
        /// modal
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("modal"))]
        Modal,
        /// <summary>
        /// pressed
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("pressed"))]
        Pressed,
        /// <summary>
        /// selected
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("selected"))]
        Selected,
        /// <summary>
        /// activedescendant
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("activedescendant"))]
        Activedescendant,
        /// <summary>
        /// controls
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("controls"))]
        Controls,
        /// <summary>
        /// describedby
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("describedby"))]
        Describedby,
        /// <summary>
        /// details
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("details"))]
        Details,
        /// <summary>
        /// errormessage
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("errormessage"))]
        Errormessage,
        /// <summary>
        /// flowto
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("flowto"))]
        Flowto,
        /// <summary>
        /// labelledby
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("labelledby"))]
        Labelledby,
        /// <summary>
        /// owns
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("owns"))]
        Owns
    }
}