// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Diagnostics;
using CefSharp.RenderProcess;

namespace CefSharp.BrowserSubprocess
{
    /// <summary>
    /// When implementing your own BrowserSubprocess
    /// - For .Net Core use <see cref="BrowserSubprocessExecutable"/> (No WCF Support)
    /// - Include an app.manifest with the dpi/compatability sections, this is required (this project contains the relevant).
    /// - If you are targeting x86/Win32 then you should set /LargeAddressAware (https://docs.microsoft.com/en-us/cpp/build/reference/largeaddressaware?view=vs-2017)
    /// </summary>
    public class Program
    {
        public static int Main(string[] args)
        {
            Debug.WriteLine("BrowserSubprocess starting up with command line: " + string.Join("\n", args));

            SubProcess.EnableHighDPISupport();

            //Add your own custom implementation of IRenderProcessHandler here
            IRenderProcessHandler handler = null;

            //The BrowserSubprocessExecutable provides BrowserSubProcess functionality
            //specific to CefSharp there is no WCF support used for the sync JSB feature.
            var browserProcessExe = new BrowserSubprocessExecutable();
            var result = browserProcessExe.Main(args, handler);

            Debug.WriteLine("BrowserSubprocess shutting down.");

            return result;
        }
    }
}
