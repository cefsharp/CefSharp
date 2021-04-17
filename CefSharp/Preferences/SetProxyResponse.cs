// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Preferences
{
    /// <summary>
    /// Response when <see cref="RequestContextExtensions.SetProxyAsync(IRequestContext, string, string, int?, out string)"/>
    /// is called.
    /// </summary>
    public class SetProxyResponse : SetPreferenceResponse
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="success">success</param>
        /// <param name="errorMessage">error message</param>
        public SetProxyResponse(bool success, string errorMessage) : base(success, errorMessage)
        {
        }
    }
}
