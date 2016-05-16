// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using CefSharp.Internals;

namespace CefSharp.BrowserSubprocess
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Kernel32.OutputDebugString("BrowserSubprocess starting up with command line: " + String.Join("\n", args));

            SubProcess.EnableHighDPISupport();

            int result;

            const string typePrefix = "--type=";
            var typeArgument = args.SingleOrDefault(arg => arg.StartsWith(typePrefix));
            var type = typeArgument.Substring(typePrefix.Length);

            //Use our custom subProcess provides features like EvaluateJavascript
            if (type == "renderer")
            {
                var wcfEnabled = args.HasArgument(CefSharpArguments.WcfEnabledArgument);
                var subProcess = wcfEnabled ? new WcfEnabledSubProcess(args) : new SubProcess(args);

                using (subProcess)
                {
                    result = subProcess.Run();
                }
            }
            else
            {
                result = SubProcess.ExecuteProcess();
            }            

            Kernel32.OutputDebugString("BrowserSubprocess shutting down.");
            return result;
        }
    }
}
