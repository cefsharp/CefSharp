// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp.Handler
{
    /// <summary>
    /// Inherit from this class to handle frame events
    /// All methods will be called on the CEF UI thread
    /// </summary>
    public class FrameHandler : IFrameHandler
    {
        private Dictionary<long, IFrame> frames = new Dictionary<long, IFrame>();

        /// <inheritdoc/>
        void IFrameHandler.OnFrameAttached(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, bool reattached)
        {
            //If we have a reference to the frame then we'll reuse that for memory management purposes
            //The frame param massed on here is stack allocated so will be disposed when out of scope.
            var storedFrame = GetFrameById(frame.Identifier);

            OnFrameAttached(chromiumWebBrowser, browser, storedFrame ?? frame, reattached);
        }

        /// <summary>
        /// Called when a frame can begin routing commands to/from the associated
        /// renderer process. Any commands that were queued have now been dispatched.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">the frame object</param>
        /// <param name="reattached">will be true if the frame was re-attached after exiting the BackForwardCache.</param>
        protected virtual void OnFrameAttached(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, bool reattached)
        {
            
        }

        /// <inheritdoc/>
        void IFrameHandler.OnFrameCreated(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
            frames.Add(frame.Identifier, frame);

            OnFrameCreated(chromiumWebBrowser, browser, frame);
        }

        /// <summary>
        /// Called when a new frame is created. This will be the first notification
        /// that references <paramref name="frame"/>. Any commands that require transport to the
        /// associated renderer process (LoadRequest, SendProcessMessage, GetSource,
        /// etc.) will be queued until OnFrameAttached is called for <paramref name="frame"/>.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">the frame object</param>
        protected virtual void OnFrameCreated(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
            
        }

        /// <inheritdoc/>
        void IFrameHandler.OnFrameDetached(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
            frames.Remove(frame.Identifier);

            OnFrameDetached(chromiumWebBrowser, browser, frame);
        }

        /// <summary>
        /// Called when a frame loses its connection to the renderer process and will
        /// be destroyed. Any pending or future commands will be discarded and
        /// <see cref="IFrame.IsValid"/> will now return <c>false</c> for <paramref name="frame"/>. If called after
        /// <see cref="ILifeSpanHandler.OnBeforeClose(IWebBrowser, IBrowser)"/> during browser destruction then
        /// <see cref="IBrowser.IsValid"/> will return <c>false</c> for <paramref name="browser"/>.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">the frame object</param>
        protected virtual void OnFrameDetached(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
            
        }

        /// <inheritdoc/>
        void IFrameHandler.OnMainFrameChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame oldFrame, IFrame newFrame)
        {
            //If we have a reference to the frame then we'll reuse that for memory management purposes
            //The frame param massed on here is stack allocated so will be disposed when out of scope.
            var storedFrame = newFrame == null ? null : GetFrameById(newFrame.Identifier);

            OnMainFrameChanged(chromiumWebBrowser, browser, oldFrame, storedFrame ?? newFrame);
        }

        /// <summary>
        /// Called when the main frame changes due to one of the following:
        /// - (a) initial browser creation
        /// - (b) final browser destruction
        /// - (c) cross-origin navigation
        /// - (d) re-navigation after renderer process termination (due to crashes, etc).
        /// 
        /// <paramref name="oldFrame"/> will be <c>null</c> and <paramref name="newFrame"/> will be non-<c>null</c> when a main frame is assigned
        /// to <paramref name="browser"/> for the first time.
        /// <paramref name="oldFrame"/> will be non-<c>null</c> and <paramref name="newFrame"/> will be <c>null</c> when a main frame is
        /// removed from <paramref name="browser"/> for the last time.
        /// Both <paramref name="oldFrame"/> and <paramref name="newFrame"/> will be non-<c>null</c>for cross-origin
        /// navigations or re-navigation after renderer process termination.
        /// This method will be called after <see cref="OnFrameCreated(IWebBrowser, IBrowser, IFrame)"/> for <paramref name="newFrame"/> and/or after
        /// <see cref="OnFrameDetached(IWebBrowser, IBrowser, IFrame)"/> for <paramref name="oldFrame"/>. If called after
        /// <see cref="ILifeSpanHandler.OnBeforeClose(IWebBrowser, IBrowser)"/> during browser destruction then
        /// <see cref="IBrowser.IsValid"/> will return <c>false</c> for <paramref name="browser"/>.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="oldFrame">the old frame object</param>
        /// <param name="newFrame">the new frame object</param>
        protected virtual void OnMainFrameChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame oldFrame, IFrame newFrame)
        {

        }

        private IFrame GetFrameById(long frameId)
        {
            if(frames.TryGetValue(frameId, out var frame))
            {
                return frame;
            }

            return null;
        }
    }
}
