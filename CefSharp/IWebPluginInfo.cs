// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Wrapper for the CEF3 CefWebPluginInfo
    /// </summary>
    public interface IWebPluginInfo
    {
        string Name { get; }

        string Description { get; }

        string Path { get; }

        string Version { get; }
    }
}
