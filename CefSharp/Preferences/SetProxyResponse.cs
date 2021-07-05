// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Preferences
{
    /// <summary>
    /// Response when either <see cref="RequestContextExtensions.SetProxyAsync(IRequestContext, string, int?)"/>
    /// or <see cref="RequestContextExtensions.SetProxyAsync(IRequestContext, string, string, int?)"/> are called.
    /// </summary>
    public class SetProxyResponse : SetPreferenceResponse
    {
        /// <summary>
        /// Initializes a new instance of the SetProxyResponse class.
        /// </summary>
        /// <param name="success">success</param>
        /// <param name="errorMessage">error message</param>
        public SetProxyResponse(bool success, string errorMessage) : base(success, errorMessage)
        {
        }
    }
}
