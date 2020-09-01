// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Indicates whether the frame is cross-origin isolated and why it is the case.
    /// </summary>
    public enum CrossOriginIsolatedContextType
    {
        Isolated,
        NotIsolated,
        NotIsolatedFeatureDisabled
    }
}