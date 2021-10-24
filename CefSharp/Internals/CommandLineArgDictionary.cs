// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp.Internals
{
    /// <summary>
    /// Custom Dictionary that provides an Add(string) method for appending CEF Command line
    /// args that don't have a switch value.
    /// </summary>
    public class CommandLineArgDictionary : Dictionary<string, string>
    {
        /// <summary>
        /// Adds the command line argument
        /// </summary>
        /// <param name="arg">command line argument</param>
        public void Add(string arg)
        {
            Add(arg, string.Empty);
        }
    }
}
