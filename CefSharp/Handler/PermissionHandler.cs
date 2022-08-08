// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Handler
{
    ///<inheritdoc/>
    public class PermissionHandler : IPermissionHandler
    {
        ///<inheritdoc/>
        bool IPermissionHandler.OnRequestMediaAccessPermission(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string requestingOrigin, MediaAccessPermissionType requestedPermissions, IMediaAccessCallback callback)
        {
            return OnRequestMediaAccessPermission(chromiumWebBrowser, browser, frame, requestingOrigin, requestedPermissions, callback);
        }

        /// <summary>
        /// Called when a page requests permission to access media.
        /// With the Chrome runtime, default handling will display the
        /// permission request UI.With the Alloy runtime, default handling will deny
        /// the request.This method will not be called if the "--enable-media-stream"
        /// command-line switch is used to grant all permissions.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">The browser object</param>
        /// <param name="frame">The frame object</param>
        /// <param name="requestingOrigin">is the URL origin requesting permission.</param>
        /// <param name="requestedPermissions">is a combination of values that represent the requested permissions</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of media access.</param>
        /// <returns>Return true and call CefMediaAccessCallback methods either in this method or at a later time to continue or cancel the request.
        /// Return false to proceed with default handling.
        /// </returns>
        protected virtual bool OnRequestMediaAccessPermission(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string requestingOrigin, MediaAccessPermissionType requestedPermissions, IMediaAccessCallback callback)
        {
            using (callback)
            {
                return false;
            }
        }

        ///<inheritdoc/>
        bool IPermissionHandler.OnShowPermissionPrompt(IWebBrowser chromiumWebBrowser, IBrowser browser, ulong promptId, string requestingOrigin, PermissionRequestType requestedPermissions, IPermissionPromptCallback callback)
        {
            return OnShowPermissionPrompt(chromiumWebBrowser, browser, promptId, requestingOrigin, requestedPermissions, callback);
        }

        /// <summary>
        /// Called when a page should show a permission prompt.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">The browser object</param>
        /// <param name="promptId">Uniquely identifies the prompt.</param>
        /// <param name="requestingOrigin">Is the URL origin requesting permission.</param>
        /// <param name="requestedPermissions">Is a combination of values from <see cref="PermissionRequestType"/> that represent the requested permissions.</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of permission prompts.</param>
        /// <returns>Return true and call <see cref="IPermissionPromptCallback.Continue"/> either in this method or at a later time to continue or cancel the request.
        /// Return false to proceed with default handling.
        /// With the Chrome runtime, default handling
        /// will display the permission prompt UI. With the Alloy runtime, default
        /// handling is <see cref="PermissionRequestResult.Ignore"/>.</returns>
        protected virtual bool OnShowPermissionPrompt(IWebBrowser chromiumWebBrowser, IBrowser browser, ulong promptId, string requestingOrigin, PermissionRequestType requestedPermissions, IPermissionPromptCallback callback)
        {
            using (callback)
            {
                return false;
            }
        }

        ///<inheritdoc/>
        void IPermissionHandler.OnDismissPermissionPrompt(IWebBrowser chromiumWebBrowser, IBrowser browser, ulong promptId, PermissionRequestResult result)
        {
            OnDismissPermissionPrompt(chromiumWebBrowser, browser, promptId, result);
        }

        /// <summary>
        /// Called when a permission prompt handled via <see cref="IPermissionHandler.OnShowPermissionPrompt"/> is dismissed.
        /// <paramref name="result"/> will be the value passed to
        /// <see cref="IPermissionPromptCallback.Continue"/> or <see cref="PermissionRequestResult.Ignore"/> if
        /// the dialog was dismissed for other reasons such as navigation, browser
        /// closure, etc. This method will not be called if <see cref="IPermissionHandler.OnShowPermissionPrompt"/>
        /// returned false for <paramref name="promptId"/>.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">The browser object</param>
        /// <param name="promptId">Will match the value that was passed to <see cref="IPermissionHandler.OnShowPermissionPrompt"/>.</param>
        /// <param name="result">will be the value passed to <see cref="IPermissionPromptCallback.Continue"/> or <see cref="PermissionRequestResult.Ignore"/> if the dialog was dismissed for other reasons such as navigation, browser closure, etc. This method will not be called if <see cref="OnShowPermissionPrompt"/> returned false for <paramref name="promptId"/>.</param>
        protected virtual void OnDismissPermissionPrompt(IWebBrowser chromiumWebBrowser, IBrowser browser, ulong promptId, PermissionRequestResult result)
        {

        }
    }
}
