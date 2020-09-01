// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Indicates whether a frame has been identified as an ad.
    /// </summary>
    public enum AdFrameType
    {
        None,
        Child,
        Root
    }
}