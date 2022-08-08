// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Callback interface used for asynchronous continuation of permission prompts.
    /// </summary>
    public interface IPermissionPromptCallback : IDisposable
    {
        /// <summary>
        /// Complete the permissions request with the specified result.
        /// </summary>
        /// <param name="result">Permission request results.</param>
        void Continue(PermissionRequestResult result);
    }
}
