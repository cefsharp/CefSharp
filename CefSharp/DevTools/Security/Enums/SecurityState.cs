// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// The security level of a page or resource.
    /// </summary>
    public enum SecurityState
    {
        Unknown,
        Neutral,
        Insecure,
        Secure,
        Info,
        InsecureBroken
    }
}