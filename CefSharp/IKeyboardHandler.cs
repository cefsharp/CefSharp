// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IKeyboardHandler
    {
        bool OnKeyEvent(IWebBrowser browser, KeyType type, int code, CefEventFlags modifiers, bool isSystemKey);
        bool OnPreKeyEvent(IWebBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, bool isKeyboardShortcut);
    }
}
