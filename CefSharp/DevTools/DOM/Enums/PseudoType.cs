// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// Pseudo element type.
    /// </summary>
    public enum PseudoType
    {
        /// <summary>
        /// first-line
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("first-line"))]
        FirstLine,
        /// <summary>
        /// first-letter
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("first-letter"))]
        FirstLetter,
        /// <summary>
        /// before
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("before"))]
        Before,
        /// <summary>
        /// after
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("after"))]
        After,
        /// <summary>
        /// marker
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("marker"))]
        Marker,
        /// <summary>
        /// backdrop
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("backdrop"))]
        Backdrop,
        /// <summary>
        /// selection
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("selection"))]
        Selection,
        /// <summary>
        /// first-line-inherited
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("first-line-inherited"))]
        FirstLineInherited,
        /// <summary>
        /// scrollbar
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("scrollbar"))]
        Scrollbar,
        /// <summary>
        /// scrollbar-thumb
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("scrollbar-thumb"))]
        ScrollbarThumb,
        /// <summary>
        /// scrollbar-button
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("scrollbar-button"))]
        ScrollbarButton,
        /// <summary>
        /// scrollbar-track
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("scrollbar-track"))]
        ScrollbarTrack,
        /// <summary>
        /// scrollbar-track-piece
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("scrollbar-track-piece"))]
        ScrollbarTrackPiece,
        /// <summary>
        /// scrollbar-corner
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("scrollbar-corner"))]
        ScrollbarCorner,
        /// <summary>
        /// resizer
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("resizer"))]
        Resizer,
        /// <summary>
        /// input-list-button
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("input-list-button"))]
        InputListButton
    }
}