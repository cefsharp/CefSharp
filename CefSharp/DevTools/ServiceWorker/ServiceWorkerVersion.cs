// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.ServiceWorker
{
    /// <summary>
    /// ServiceWorker version.
    /// </summary>
    public class ServiceWorkerVersion : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// VersionId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("versionId"), IsRequired = (true))]
        public string VersionId
        {
            get;
            set;
        }

        /// <summary>
        /// RegistrationId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("registrationId"), IsRequired = (true))]
        public string RegistrationId
        {
            get;
            set;
        }

        /// <summary>
        /// ScriptURL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scriptURL"), IsRequired = (true))]
        public string ScriptURL
        {
            get;
            set;
        }

        /// <summary>
        /// RunningStatus
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("runningStatus"), IsRequired = (true))]
        public CefSharp.DevTools.ServiceWorker.ServiceWorkerVersionRunningStatus RunningStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Status
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("status"), IsRequired = (true))]
        public CefSharp.DevTools.ServiceWorker.ServiceWorkerVersionStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// The Last-Modified header value of the main script.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scriptLastModified"), IsRequired = (false))]
        public long? ScriptLastModified
        {
            get;
            set;
        }

        /// <summary>
        /// The time at which the response headers of the main script were received from the server.
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scriptResponseTime"), IsRequired = (false))]
        public long? ScriptResponseTime
        {
            get;
            set;
        }

        /// <summary>
        /// ControlledClients
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("controlledClients"), IsRequired = (false))]
        public string[] ControlledClients
        {
            get;
            set;
        }

        /// <summary>
        /// TargetId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("targetId"), IsRequired = (false))]
        public string TargetId
        {
            get;
            set;
        }
    }
}