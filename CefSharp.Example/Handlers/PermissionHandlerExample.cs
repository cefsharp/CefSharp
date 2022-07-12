// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Handler;

namespace CefSharp.Example.Handlers
{
    public class PermissionHandlerExample : PermissionHandler
    {
        public override bool OnShowPermissionPrompt(IBrowser browser, ulong promptId, string requestingOrigin, CefPermissionType requestedPermissions, IPermissionPromptCallback callback)
        {
            using (callback)
            {
                System.Diagnostics.Debug.WriteLine($"{promptId}|{requestedPermissions} {requestingOrigin}");
                callback.Continue(CefPermissionResult.Accept);
                return true;
            }
        }

        public override void OnDismissPermissionPrompt(IBrowser browser, ulong promptId, CefPermissionResult result)
        {
            base.OnDismissPermissionPrompt(browser, promptId, result);
        }

        public override bool OnRequestMediaAccessPermission(IBrowser browser, IFrame frame, string requestingOrigin, CefMediaAccessPermissionType requestedPermissions, IMediaAccessCallback callback)
        {
            using (callback)
            {
                callback.Continue(CefMediaAccessPermissionType.AudioCapture |
                                  CefMediaAccessPermissionType.VideoCapture |
                                  CefMediaAccessPermissionType.DesktopVideoCapture |
                                  CefMediaAccessPermissionType.DesktopAudioCapture);
                return true;
            }
        }
    }
}
