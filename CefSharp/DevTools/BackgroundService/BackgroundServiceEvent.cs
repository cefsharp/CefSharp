// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.BackgroundService
{
    /// <summary>
    /// BackgroundServiceEvent
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class BackgroundServiceEvent : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Timestamp of the event (in seconds).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("timestamp"), IsRequired = (true))]
        public long Timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// The origin this event belongs to.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("origin"), IsRequired = (true))]
        public string Origin
        {
            get;
            set;
        }

        /// <summary>
        /// The Service Worker ID that initiated the event.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("serviceWorkerRegistrationId"), IsRequired = (true))]
        public string ServiceWorkerRegistrationId
        {
            get;
            set;
        }

        /// <summary>
        /// The Background Service this event belongs to.
        /// </summary>
        public CefSharp.DevTools.BackgroundService.ServiceName Service
        {
            get
            {
                return (CefSharp.DevTools.BackgroundService.ServiceName)(StringToEnum(typeof(CefSharp.DevTools.BackgroundService.ServiceName), service));
            }

            set
            {
                service = (EnumToString(value));
            }
        }

        /// <summary>
        /// The Background Service this event belongs to.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("service"), IsRequired = (true))]
        internal string service
        {
            get;
            set;
        }

        /// <summary>
        /// A description of the event.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("eventName"), IsRequired = (true))]
        public string EventName
        {
            get;
            set;
        }

        /// <summary>
        /// An identifier that groups related events together.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("instanceId"), IsRequired = (true))]
        public string InstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// A list of event-specific information.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("eventMetadata"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.BackgroundService.EventMetadata> EventMetadata
        {
            get;
            set;
        }
    }
}