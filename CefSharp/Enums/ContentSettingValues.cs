// Copyright © 2024 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Enums
{
    /// <summary>
    /// Supported content setting values. Should be kept in sync with Chromium's
    /// ContentSetting type.
    /// </summary>
    public enum ContentSettingValues
    {
        Default = 0,
        Allow,
        Block,
        Ask,
        SessionOnly,
        DetectImportantContent,
        NumValues
    }
}
