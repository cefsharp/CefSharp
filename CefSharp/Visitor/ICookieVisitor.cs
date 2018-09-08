// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Interface to implement for visiting cookie values. 
    /// The methods of this class will always be called on the IO thread.
    /// If there are no cookies then Visit will never be called, you must implement
    /// Dispose to handle this scenario.
    /// </summary>
    public interface ICookieVisitor : IDisposable
    {
        /// <summary>
        /// Method that will be called once for each cookie. This method may never be called if no cookies are found. 
        /// </summary>
        /// <param name="cookie">cookie</param>
        /// <param name="count">is the 0-based index for the current cookie.</param>
        /// <param name="total">is the total number of cookies.</param>
        /// <param name="deleteCookie">Set to true to delete the cookie currently being visited.</param>
        /// <returns>Return false to stop visiting cookies otherwise true</returns>
        bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie);
    }
}
