// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    public class JavascriptObjectRepositorySettings
    {
        public string WindowPropertyName { get; set; }

        public JavascriptObjectRepositorySettings()
        {
            WindowPropertyName = "CefSharp";
        }

        public bool IsWindowPropertyNameCamelCase()
        {
            return StringCheck.IsFirstCharacterLowercase(WindowPropertyName);
        }
    }
}
