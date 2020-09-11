// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.BackgroundService
{
    /// <summary>
    /// The Background Service that will be associated with the commands/events.
    /// Every Background Service operates independently, but they share the same
    /// API.
    /// </summary>
    public enum ServiceName
    {
        /// <summary>
        /// backgroundFetch
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("backgroundFetch"))]
        BackgroundFetch,
        /// <summary>
        /// backgroundSync
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("backgroundSync"))]
        BackgroundSync,
        /// <summary>
        /// pushMessaging
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("pushMessaging"))]
        PushMessaging,
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
        PeriodicBackgroundSync
    }
}