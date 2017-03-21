// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    public interface IWebPluginInfoVisitor : IDisposable
    {
        /// <summary>
        /// Method that will be called once for each plugin. 
        /// This method may never be called if no plugins are found.
        /// </summary>
        /// <param name="plugin">plugin information</param>
        /// <param name="count">is the 0-based index for the current plugin</param>
        /// <param name="total">total is the total number of plugins.</param>
        /// <returns>Return false to stop visiting plugins otherwise true</returns>
        bool Visit(WebPluginInfo plugin, int count, int total);
    }
}
