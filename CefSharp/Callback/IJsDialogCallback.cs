// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Callback interface used for asynchronous continuation of JavaScript dialog requests.
    /// </summary>
    public interface IJsDialogCallback : IDisposable
    {
        /// <summary>
        /// Continue the Javascript dialog request.
        /// </summary>
        /// <param name="success">Set to true if the OK button was pressed.</param>
        /// <param name="userInput">value should be specified for prompt dialogs.</param>
        void Continue(bool success, string userInput);

        /// <summary>
        /// Continue the Javascript dialog request.
        /// </summary>
        /// <param name="success">Set to true if the OK button was pressed.</param>
        void Continue(bool success);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
