// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Implement this interface to have control of how the names
    /// are converted when binding/executing javascript.
    /// </summary>
    public interface IJavascriptNameConverter
    {
        string ConvertToJavascript(string name);
    }
}
