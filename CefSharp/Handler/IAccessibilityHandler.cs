// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to receive accessibility notification when accessibility events have been registered. 
    /// It's important to note that the methods of this interface are called on a CEF UI thread,
    /// which by default is not the same as your application UI thread.
    /// </summary>
    public interface IAccessibilityHandler
    {
        /// <summary>
        /// Called after renderer process sends accessibility location changes to the browser process.
        /// </summary>
        /// <param name="value">Updated location info.</param>
        void OnAccessibilityLocationChange(IValue value);

        /// <summary>
        /// Called after renderer process sends accessibility tree changes to the browser process.
        /// </summary>
        /// <param name="value">Updated tree info.</param>
        void OnAccessibilityTreeChange(IValue value);
    }
}