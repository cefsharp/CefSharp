// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Callback interface used for asynchronous continuation of media access
    /// permission requests.
    /// </summary>
    public interface IMediaAccessCallback : IDisposable
    {
        /// <summary>
        /// Call to allow or deny media access. If this callback was initiated in
        /// response to a getUserMedia (indicated by
        /// DeviceAudioCapture and/or DeviceVideoCapture being set) then
        /// <paramref name="allowedPermissions"/> must match requestedPermissions param passed to
        /// <see cref="IPermissionHandler.OnRequestMediaAccessPermission"/>
        /// </summary>
        /// <param name="allowedPermissions">Allowed Permissions</param>
        void Continue(MediaAccessPermissionType allowedPermissions);

        /// <summary>
        /// Cancel the media access request.
        /// </summary>
        void Cancel();
    }
}
