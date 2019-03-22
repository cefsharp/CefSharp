// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.RenderProcess
{
    /// <summary>
    /// Class used to implement render process callbacks.
    /// The methods of this class will be called on the render process main thread (TID_RENDERER) unless otherwise indicated.
    /// </summary>
    public interface IRenderProcessHandler
    {
        /// <summary>
        /// Called immediately after the V8 context for a frame has been created.
        /// V8 handles can only be accessed from the thread on which they are created.
        /// </summary>
        /// <param name="browser">the browser</param>
        /// <param name="frame">the frame</param>
        /// <param name="context">the V8Context</param>
        void OnContextCreated(IBrowser browser, IFrame frame, IV8Context context);

        /// <summary>
        /// Called immediately before the V8 context for a frame is released.
        /// No references to the context should be kept after this method is called.
        /// </summary>
        /// <param name="browser">the browser</param>
        /// <param name="frame">the frame</param>
        /// <param name="context">the V8Context</param>
        void OnContextReleased(IBrowser browser, IFrame frame, IV8Context context);
    }
}
