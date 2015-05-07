// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IJsDialogCallback
    {
        /// <summary>
        /// Continue the Javascript dialog request.
        /// </summary>
        /// <param name="success">Set to true if the OK button was pressed.</param>
        /// <param name="userInput">value should be specified for prompt dialogs.</param>
        void Continue(bool success, string userInput);
    }
}
