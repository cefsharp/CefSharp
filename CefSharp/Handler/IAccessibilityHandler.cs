// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to receive accessibility notification when accessibility events have been registered. 
    /// The methods of this class will be called on the UI thread.
    /// </summary>
    public interface IAccessibilityHandler
    {
        /// <summary>
        /// Called after renderer process sends accessibility location changes to the browser process.
        /// </summary>
        /// <param name="value">Updated location info.</param>
        void OnAccessibilityLocationChange(object value);

        /// <summary>
        /// Called after renderer process sends accessibility tree changes to the browser process.
        /// </summary>
        /// <param name="value">Updated tree info.</param>
        void OnAccessibilityTreeChange(object value);
    }
}