// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Enums;
using CefSharp.Structs;
using System;
using System.Collections.Generic;

using Size = CefSharp.Structs.Size;

namespace CefSharp.Handler
{
    /// <summary>
    /// Handle events related to browser display state.
    /// </summary>
    public class DisplayHandler : IDisplayHandler
    {
        /// <inheritdoc/>
        void IDisplayHandler.OnAddressChanged(IWebBrowser chromiumWebBrowser, AddressChangedEventArgs addressChangedArgs)
        {
            OnAddressChanged(chromiumWebBrowser, addressChangedArgs);
        }

        /// <summary>
        /// Called when a frame's address has changed. 
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="addressChangedArgs">args</param>
        protected virtual void OnAddressChanged(IWebBrowser chromiumWebBrowser, AddressChangedEventArgs addressChangedArgs)
        {

        }

        /// <inheritdoc/>
        bool IDisplayHandler.OnAutoResize(IWebBrowser chromiumWebBrowser, IBrowser browser, Size newSize)
        {
            return OnAutoResize(chromiumWebBrowser, browser, newSize);
        }

        /// <summary>
        /// Called when auto-resize is enabled via IBrowserHost.SetAutoResizeEnabled and the contents have auto-resized.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="newSize">will be the desired size in view coordinates</param>
        /// <returns>Return true if the resize was handled or false for default handling. </returns>
        protected virtual bool OnAutoResize(IWebBrowser chromiumWebBrowser, IBrowser browser, Size newSize)
        {
            return false;
        }

        /// <inheritdoc/>
        bool IDisplayHandler.OnCursorChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IntPtr cursor, CursorType type, CursorInfo customCursorInfo)
        {
            return OnCursorChange(chromiumWebBrowser, browser, cursor, type, customCursorInfo);
        }

        /// <summary>
        /// Called when the browser's cursor has changed.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="cursor">If type is Custom then customCursorInfo will be populated with the custom cursor information</param>
        /// <param name="type">cursor type</param>
        /// <param name="customCursorInfo">custom cursor Information</param>
        /// <returns>Return true if the cursor change was handled or false for default handling.</returns>
        protected virtual bool OnCursorChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IntPtr cursor, CursorType type, CursorInfo customCursorInfo)
        {
            return false;
        }

        /// <inheritdoc/>
        void IDisplayHandler.OnTitleChanged(IWebBrowser chromiumWebBrowser, TitleChangedEventArgs titleChangedArgs)
        {
            OnTitleChanged(chromiumWebBrowser, titleChangedArgs);
        }

        /// <summary>
        /// Called when the page title changes.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="titleChangedArgs">args</param>
        protected virtual void OnTitleChanged(IWebBrowser chromiumWebBrowser, TitleChangedEventArgs titleChangedArgs)
        {

        }

        /// <inheritdoc/>
        void IDisplayHandler.OnFaviconUrlChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IList<string> urls)
        {
            OnFaviconUrlChange(chromiumWebBrowser, browser, urls);
        }

        /// <summary>
        /// Called when the page icon changes.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="urls">list of urls where the favicons can be downloaded</param>
        protected virtual void OnFaviconUrlChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IList<string> urls)
        {

        }

        /// <inheritdoc/>
        void IDisplayHandler.OnFullscreenModeChange(IWebBrowser chromiumWebBrowser, IBrowser browser, bool fullscreen)
        {
            OnFullscreenModeChange(chromiumWebBrowser, browser, fullscreen);
        }

        /// <summary>
        /// Called when web content in the page has toggled fullscreen mode. The client is
        /// responsible for resizing the browser if desired.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="fullscreen">If true the content will automatically be sized to fill the browser content area.
        /// If false the content will automatically return to its original size and position.</param>
        protected virtual void OnFullscreenModeChange(IWebBrowser chromiumWebBrowser, IBrowser browser, bool fullscreen)
        {

        }

        /// <inheritdoc/>
        void IDisplayHandler.OnLoadingProgressChange(IWebBrowser chromiumWebBrowser, IBrowser browser, double progress)
        {
            OnLoadingProgressChange(chromiumWebBrowser, browser, progress);
        }

        /// <summary>
        /// Called when the overall page loading progress has changed
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="progress">ranges from 0.0 to 1.0.</param>
        protected virtual void OnLoadingProgressChange(IWebBrowser chromiumWebBrowser, IBrowser browser, double progress)
        {

        }

        /// <inheritdoc/>
        bool IDisplayHandler.OnTooltipChanged(IWebBrowser chromiumWebBrowser, ref string text)
        {
            return OnTooltipChanged(chromiumWebBrowser, ref text);
        }

        /// <summary>
        /// Called when the browser is about to display a tooltip. text contains the
        /// text that will be displayed in the tooltip. You can optionally modify text
        /// and then return false to allow the browser to display the tooltip.
        /// When window rendering is disabled the application is responsible for
        /// drawing tooltips and the return value is ignored.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="text">the text that will be displayed in the tooltip</param>
        /// <returns>To handle the display of the tooltip yourself return true otherwise return false
        /// to allow the browser to display the tooltip.</returns>
        /// <remarks>Only called when using Off-screen rendering (WPF and OffScreen)</remarks>
        protected virtual bool OnTooltipChanged(IWebBrowser chromiumWebBrowser, ref string text)
        {
            return false;
        }

        /// <inheritdoc/>
        void IDisplayHandler.OnStatusMessage(IWebBrowser chromiumWebBrowser, StatusMessageEventArgs statusMessageArgs)
        {

        }

        /// <summary>
        /// Called when the browser receives a status message.
        /// </summary>
        /// <param name="chromiumWebBrowser">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="statusMessageArgs">args</param>
        protected virtual void OnStatusMessage(IWebBrowser chromiumWebBrowser, StatusMessageEventArgs statusMessageArgs)
        {

        }

        /// <inheritdoc/>
        bool IDisplayHandler.OnConsoleMessage(IWebBrowser chromiumWebBrowser, ConsoleMessageEventArgs consoleMessageArgs)
        {
            return OnConsoleMessage(chromiumWebBrowser, consoleMessageArgs);
        }

        /// <summary>
        /// Called to display a console message. 
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="consoleMessageArgs">args</param>
        /// <returns>Return true to stop the message from being output to the console.</returns>
        protected virtual bool OnConsoleMessage(IWebBrowser chromiumWebBrowser, ConsoleMessageEventArgs consoleMessageArgs)
        {
            return false;
        }
    }
}
