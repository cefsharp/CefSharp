// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Preferences
{
    /// <summary>
    /// Response when <see cref="IRequestContext.SetPreference(string, object, out string)"/>
    /// is called in an async fashion
    /// </summary>
    public class SetPreferenceResponse
    {
        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="success">success</param>
        /// <param name="errorMessage">error message</param>
        public SetPreferenceResponse(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
    }
}
