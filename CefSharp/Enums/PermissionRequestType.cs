// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Permission types used with <see cref="IPermissionHandler.OnShowPermissionPrompt(IWebBrowser, IBrowser, ulong, string, PermissionRequestType, IPermissionPromptCallback)"/>.
    /// Some types are platform-specific or only supported with the Chrome runtime. Should be kept
    /// in sync with Chromium's permissions::RequestType type.
    /// </summary>
    [Flags]
    public enum PermissionRequestType : uint
    {
        None = 0,
        AccessibilityEvents = 1 << 0,
        ArSession = 1 << 1,
        CameraPanTiltZoom = 1 << 2,
        CameraStream = 1 << 3,
        Clipboard = 1 << 4,
        DiskQuota = 1 << 5,
        LocalFonts = 1 << 6,
        Geolocation = 1 << 7,
        IdleDetection = 1 << 8,
        MicStream = 1 << 9,
        MidiSysex = 1 << 10,
        MultipleDownloads = 1 << 11,
        Notifications = 1 << 12,
        ProtectedMediaIdentifier = 1 << 13,
        RegisterProtocolHandler = 1 << 14,
        SecurityAttestation = 1 << 15,
        StorageAccess = 1 << 16,
        U2FApiRequest = 1 << 17,
        VrSession = 1 << 18,
        WindowPlacement = 1 << 19
    }
}
