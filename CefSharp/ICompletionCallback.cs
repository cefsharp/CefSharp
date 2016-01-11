// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Generic callback interface used for asynchronous completion. 
    /// </summary>
    public interface ICompletionCallback
    {
        /// <summary>
        /// Method that will be called once the task is complete. 
        /// </summary>
        void OnComplete();
    }
}
