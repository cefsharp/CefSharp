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
        CapturedSurfaceControl = 1 << 4,
        Clipboard = 1 << 5,
        TopLevelStorageAccess = 1 << 6,
        DiskQuota = 1 << 7,
        LocalFonts = 1 << 8,
        Geolocation = 1 << 9,
        Identity_Provider = 1 << 10,
        IdleDetection = 1 << 11,
        MicStream = 1 << 12,
        MidiSysex = 1 << 13,
        MultipleDownloads = 1 << 14,
        Notifications = 1 << 15,
        KeyboardLock = 1 << 16,
        PointerLock = 1 << 17,
        ProtectedMediaIdentifier = 1 << 18,
        RegisterProtocolHandler = 1 << 19,
        StorageAccess = 1 << 20,
        VrSession = 1 << 21,
        WindowManagement = 1 << 22,
        FileSystemAccess = 1 << 23
    }
}
