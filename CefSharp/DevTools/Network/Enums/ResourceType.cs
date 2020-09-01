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
        Document,
        Stylesheet,
        Image,
        Media,
        Font,
        Script,
        TextTrack,
        XHR,
        Fetch,
        EventSource,
        WebSocket,
        Manifest,
        SignedExchange,
        Ping,
        CSPViolationReport,
        Other
    }
}