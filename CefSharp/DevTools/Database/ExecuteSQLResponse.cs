// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Database
{
    /// <summary>
    /// ExecuteSQLResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ExecuteSQLResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string[] columnNames
        {
            get;
            set;
        }

        /// <summary>
        /// columnNames
        /// </summary>
        public string[] ColumnNames
        {
            get
            {
                return columnNames;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal object[] values
        {
            get;
            set;
        }

        /// <summary>
        /// values
        /// </summary>
        public object[] Values
        {
            get
            {
                return values;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Database.Error sqlError
        {
            get;
            set;
        }

        /// <summary>
        /// sqlError
        /// </summary>
        public CefSharp.DevTools.Database.Error SqlError
        {
            get
            {
                return sqlError;
            }
        }
    }
}