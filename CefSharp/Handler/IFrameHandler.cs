// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle frame events
    /// All methods will be called on the CEF UI thread
    /// </summary>
    public interface IFrameHandler
    {
        /// <summary>
        /// Called when a new frame is created. This will be the first notification
        /// that references |frame|. Any commands that require transport to the
        /// associated renderer process (LoadRequest, SendProcessMessage, GetSource,
        /// etc.) will be queued until OnFrameAttached is called for |frame|.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">the frame object</param>
        void OnFrameCreated(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame);

        /// <summary>
        /// Called when a frame can begin routing commands to/from the associated
        /// renderer process. Any commands that were queued have now been dispatched.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">the frame object</param>
        void OnFrameAttached(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame);

        /// <summary>
        /// Called when a frame loses its connection to the renderer process and will
        /// be destroyed. Any pending or future commands will be discarded and
        /// CefFrame::IsValid() will now return false for |frame|. If called after
        /// CefLifeSpanHandler::OnBeforeClose() during browser destruction then
        /// CefBrowser::IsValid() will return false for |browser|.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">the frame object</param>
        void OnFrameDetached(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame);

        /// <summary>
        /// Called when the main frame changes due to (a) initial browser creation, (b)
        /// final browser destruction, (c) cross-origin navigation or (d) re-navigation
        /// after renderer process termination (due to crashes, etc). |old_frame| will
        /// be NULL and |new_frame| will be non-NULL when a main frame is assigned to
        /// |browser| for the first time. |old_frame| will be non-NULL and |new_frame|
        /// will be NULL and  when a main frame is removed from |browser| for the last
        /// time. Both |old_frame| and |new_frame| will be non-NULL for cross-origin
        /// navigations or re-navigation after renderer process termination. This
        /// method will be called after OnFrameCreated() for |new_frame| and/or after
        /// OnFrameDetached() for |old_frame|. If called after
        /// CefLifeSpanHandler::OnBeforeClose() during browser destruction then
        /// CefBrowser::IsValid() will return false for |browser|.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="oldFrame">the new frame object</param>
        /// <param name="newFrame">the old frame object</param>
        void OnMainFrameChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame oldFrame, IFrame newFrame);
    }
}
