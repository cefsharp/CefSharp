// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle events related to focus.
    /// The methods of this class will be called on the CEF UI thread. 
    /// </summary>
    public interface IFocusHandler
    {
        /// <summary>
        /// Called when the browser component has received focus.
        /// </summary>
        void OnGotFocus();

        /// <summary>
        /// Called when the browser component is requesting focus.
        /// </summary>
        /// <param name="source">Indicates where the focus request is originating from.</param>
        /// <returns>Return false to allow the focus to be set or true to cancel setting the focus.</returns>
        bool OnSetFocus(CefFocusSource source);

        /// <summary>
        /// Called when the browser component is about to lose focus.
        /// For instance, if focus was on the last HTML element and the user pressed the TAB key.
        /// </summary>
        /// <param name="next">Will be true if the browser is giving focus to the next component
        /// and false if the browser is giving focus to the previous component.</param>
        void OnTakeFocus(bool next);
    }
}
