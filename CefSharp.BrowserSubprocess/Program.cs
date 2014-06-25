// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Internals;

namespace CefSharp.BrowserSubprocess
{
    public class Program
    {
        static int Main(string[] args)
        {
            Kernel32.OutputDebugString("BrowserSubprocess starting up with command line: " + String.Join("\n", args));

            //MessageBox.Show("Please attach debugger now", null, MessageBoxButtons.OK, MessageBoxIcon.Information);

            int result = 0;

            using (var subprocess = new CefSubprocess())
            {
                var wrapper = new CefAppWrapper(subprocess);

                result = wrapper.Run(args);
            }

            Kernel32.OutputDebugString("BrowserSubprocess shutting down.");
            return result;
        }
    }
}
