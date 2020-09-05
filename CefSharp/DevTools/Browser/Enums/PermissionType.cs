// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// PermissionType
    /// </summary>
    public enum PermissionType
    {
        /// <summary>
        /// accessibilityEvents
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("accessibilityEvents"))]
        AccessibilityEvents,
        /// <summary>
        /// audioCapture
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("audioCapture"))]
        AudioCapture,
        /// <summary>
        /// backgroundSync
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("backgroundSync"))]
        BackgroundSync,
        /// <summary>
        /// backgroundFetch
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("backgroundFetch"))]
        BackgroundFetch,
        /// <summary>
        /// clipboardReadWrite
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("clipboardReadWrite"))]
        ClipboardReadWrite,
        /// <summary>
        /// clipboardSanitizedWrite
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("clipboardSanitizedWrite"))]
        ClipboardSanitizedWrite,
        /// <summary>
        /// durableStorage
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("durableStorage"))]
        DurableStorage,
        /// <summary>
        /// flash
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("flash"))]
        Flash,
        /// <summary>
        /// geolocation
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("geolocation"))]
        Geolocation,
        /// <summary>
        /// midi
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("midi"))]
        Midi,
        /// <summary>
        /// midiSysex
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("midiSysex"))]
        MidiSysex,
        /// <summary>
        /// nfc
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("nfc"))]
        Nfc,
        /// <summary>
        /// notifications
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("notifications"))]
        Notifications,
        /// <summary>
        /// paymentHandler
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("paymentHandler"))]
        PaymentHandler,
        /// <summary>
        /// periodicBackgroundSync
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("periodicBackgroundSync"))]
        PeriodicBackgroundSync,
        /// <summary>
        /// protectedMediaIdentifier
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("protectedMediaIdentifier"))]
        ProtectedMediaIdentifier,
        /// <summary>
        /// sensors
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("sensors"))]
        Sensors,
        /// <summary>
        /// videoCapture
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("videoCapture"))]
        VideoCapture,
        /// <summary>
        /// idleDetection
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("idleDetection"))]
        IdleDetection,
        /// <summary>
        /// wakeLockScreen
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("wakeLockScreen"))]
        WakeLockScreen,
        /// <summary>
        /// wakeLockSystem
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("wakeLockSystem"))]
        WakeLockSystem
    }
}