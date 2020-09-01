// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Indicates whether the frame is a secure context and why it is the case.
    /// </summary>
    public enum SecureContextType
    {
        Secure,
        SecureLocalhost,
        InsecureScheme,
        InsecureAncestor
    }
}