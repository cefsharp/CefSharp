// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Handler
{
    /// <summary>
    /// Inherit from this class to receive accessibility notification when accessibility events have been registered. 
    /// It's important to note that the methods of this interface are called on a CEF UI thread,
    /// which by default is not the same as your application UI thread.
    /// </summary>
    public class AccessibilityHandler : IAccessibilityHandler
    {
        /// <inheritdoc/>
        void IAccessibilityHandler.OnAccessibilityLocationChange(IValue value)
        {
            OnAccessibilityLocationChange(value);
        }

        /// <summary>
        /// Called after renderer process sends accessibility location changes to the browser process.
        /// </summary>
        /// <param name="value">Updated location info.</param>
        protected virtual void OnAccessibilityLocationChange(IValue value)
        {

        }

        /// <inheritdoc/>
        void IAccessibilityHandler.OnAccessibilityTreeChange(IValue value)
        {
            OnAccessibilityTreeChange(value);
        }

        /// <summary>
        /// Called after renderer process sends accessibility tree changes to the browser process.
        /// </summary>
        /// <param name="value">Updated tree info.</param>
        protected virtual void OnAccessibilityTreeChange(IValue value)
        {

        }
    }
}
