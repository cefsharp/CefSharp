// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Callback interface for IBrowserHost.GetNavigationEntries.
    /// The methods of this class will be called on the CEF UI thread. 
    /// </summary>
    public interface INavigationEntryVisitor : IDisposable
    {
        /// <summary>
        /// Method that will be executed.
        /// </summary>
        /// <param name="entry">if the navigationEntry will be invalid then </param>
        /// <param name="current">is true if this entry is the currently loaded navigation entry</param>
        /// <param name="index">is the 0-based index of this entry</param>
        /// <param name="total">is the total number of entries.</param>
        /// <returns>Return true to continue visiting entries or false to stop.</returns>
        bool Visit(NavigationEntry entry, bool current, int index, int total);
    }
}
