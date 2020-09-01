// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// The underlying connection technology that the browser is supposedly using.
    /// </summary>
    public enum ConnectionType
    {
        None,
        Cellular2g,
        Cellular3g,
        Cellular4g,
        Bluetooth,
        Ethernet,
        Wifi,
        Wimax,
        Other
    }
}