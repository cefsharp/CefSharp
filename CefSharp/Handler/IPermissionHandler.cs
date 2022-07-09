// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle events related to permission requests.
    /// It's important to note that the methods of this interface are called on a CEF UI thread,
    /// which by default is not the same as your application UI thread.
    /// </summary>
    public interface IPermissionHandler
    {
        /// <summary>
        /// Called when a page requests permission to access media.
        /// </summary>
        /// <param name="browser">browser</param>
        /// <param name="frame">frame></param>
        /// <param name="requestingOrigin">is the URL origin requesting permission.</param>
        /// <param name="requestedPermissions">is a combination of values that represent the requested permissions</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of media access.</param>
        /// <returns>Return true and call CefMediaAccessCallback methods either in this method or at a later time to continue or cancel the request.
        /// Return false to proceed with default handling.
        /// With the Chrome runtime, default handling will display the
        /// permission request UI.With the Alloy runtime, default handling will deny
        /// the request.This method will not be called if the "--enable-media-stream"
        /// command-line switch is used to grant all permissions.
        /// </returns>
        bool OnRequestMediaAccessPermission(IBrowser browser, IFrame frame, string requestingOrigin, CefMediaAccessPermissionType requestedPermissions, IMediaAccessCallback callback);

        /// <summary>
        /// Called when a page should show a permission prompt.
        /// </summary>
        /// <param name="browser">browser</param>
        /// <param name="promptId">uniquely identifies the prompt.</param>
        /// <param name="requestingOrigin">is the URL origin requesting permission.</param>
        /// <param name="requestedPermissions">is a combination of values from CefPermissionType that represent the requested permissions.</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of permission prompts.</param>
        /// <returns>Return true and call <see cref="IPermissionPromptCallback.Continue"/> either in this method or at a later time to continue or cancel the request.
        /// Return false to proceed with default handling.
        /// With the Chrome runtime, default handling
        /// will display the permission prompt UI. With the Alloy runtime, default
        /// handling is <see cref="CefPermissionResult.Ignore"/>.</returns>
        bool OnShowPermissionPrompt(IBrowser browser, ulong promptId, string requestingOrigin, CefPermissionType requestedPermissions, IPermissionPromptCallback callback);

        /// <summary>
        /// Called when a permission prompt handled via <see cref="OnShowPermissionPrompt"/> is dismissed.
        /// <paramref name="result"/> will be the value passed to
        /// <see cref="IPermissionPromptCallback.Continue"/> or <see cref="CefPermissionResult.Ignore"/> if
        /// the dialog was dismissed for other reasons such as navigation, browser
        /// closure, etc. This method will not be called if <see cref="OnShowPermissionPrompt"/>
        /// returned false for <paramref name="promptId"/>.
        /// </summary>
        /// <param name="browser">browser</param>
        /// <param name="promptId">will match the value that was passed to <see cref="OnShowPermissionPrompt"/>.</param>
        /// <param name="result">will be the value passed to <see cref="IPermissionPromptCallback.Continue"/> or <see cref="CefPermissionResult.Ignore"/> if the dialog was dismissed for other reasons such as navigation, browser closure, etc. This method will not be called if <see cref="OnShowPermissionPrompt"/> returned false for <paramref name="promptId"/>.</param>
        void OnDismissPermissionPrompt(IBrowser browser, ulong promptId, CefPermissionResult result);
    }
}
