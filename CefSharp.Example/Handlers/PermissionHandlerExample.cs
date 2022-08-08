// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Handler;

namespace CefSharp.Example.Handlers
{
    public class PermissionHandlerExample : PermissionHandler
    {
        protected override bool OnShowPermissionPrompt(IWebBrowser chromiumWebBrowser, IBrowser browser, ulong promptId, string requestingOrigin, PermissionRequestType requestedPermissions, IPermissionPromptCallback callback)
        {
            using (callback)
            {
                System.Diagnostics.Debug.WriteLine($"{promptId}|{requestedPermissions} {requestingOrigin}");
                callback.Continue(PermissionRequestResult.Accept);
                return true;
            }
        }

        protected override void OnDismissPermissionPrompt(IWebBrowser chromiumWebBrowser, IBrowser browser, ulong promptId, PermissionRequestResult result)
        {
            base.OnDismissPermissionPrompt(chromiumWebBrowser, browser, promptId, result);
        }

        protected override bool OnRequestMediaAccessPermission(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string requestingOrigin, MediaAccessPermissionType requestedPermissions, IMediaAccessCallback callback)
        {
            using (callback)
            {
                callback.Continue(MediaAccessPermissionType.AudioCapture |
                                  MediaAccessPermissionType.VideoCapture |
                                  MediaAccessPermissionType.DesktopVideoCapture |
                                  MediaAccessPermissionType.DesktopAudioCapture);
                return true;
            }
        }
    }
}
