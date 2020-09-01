// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Types of reasons why a cookie may not be sent with a request.
    /// </summary>
    public enum CookieBlockedReason
    {
        SecureOnly,
        NotOnPath,
        DomainMismatch,
        SameSiteStrict,
        SameSiteLax,
        SameSiteUnspecifiedTreatedAsLax,
        SameSiteNoneInsecure,
        UserPreferences,
        UnknownError
    }
}