// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.ServiceWorker
{
    /// <summary>
    /// ServiceWorker error message.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ServiceWorkerErrorMessage : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// ErrorMessage
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("errorMessage"), IsRequired = (true))]
        public string ErrorMessage
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
        /// VersionId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("versionId"), IsRequired = (true))]
        public string VersionId
        {
            get;
            set;
        }

        /// <summary>
        /// SourceURL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sourceURL"), IsRequired = (true))]
        public string SourceURL
        {
            get;
            set;
        }

        /// <summary>
        /// LineNumber
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lineNumber"), IsRequired = (true))]
        public int LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// ColumnNumber
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("columnNumber"), IsRequired = (true))]
        public int ColumnNumber
        {
            get;
            set;
        }
    }
}