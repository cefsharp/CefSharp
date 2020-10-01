// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Resource type as it was perceived by the rendering engine.
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// Document
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Document"))]
        Document,
        /// <summary>
        /// Stylesheet
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Stylesheet"))]
        Stylesheet,
        /// <summary>
        /// Image
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Image"))]
        Image,
        /// <summary>
        /// Media
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Media"))]
        Media,
        /// <summary>
        /// Font
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Font"))]
        Font,
        /// <summary>
        /// Script
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Script"))]
        Script,
        /// <summary>
        /// TextTrack
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("TextTrack"))]
        TextTrack,
        /// <summary>
        /// XHR
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("XHR"))]
        XHR,
        /// <summary>
        /// Fetch
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Fetch"))]
        Fetch,
        /// <summary>
        /// EventSource
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("EventSource"))]
        EventSource,
        /// <summary>
        /// WebSocket
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("WebSocket"))]
        WebSocket,
        /// <summary>
        /// Manifest
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Manifest"))]
        Manifest,
        /// <summary>
        /// SignedExchange
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SignedExchange"))]
        SignedExchange,
        /// <summary>
        /// Ping
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Ping"))]
        Ping,
        /// <summary>
        /// CSPViolationReport
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("CSPViolationReport"))]
        CSPViolationReport,
        /// <summary>
        /// Other
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Other"))]
        Other
    }
}