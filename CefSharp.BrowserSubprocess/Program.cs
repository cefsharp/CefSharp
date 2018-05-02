// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using CefSharp.Internals;

namespace CefSharp.BrowserSubprocess
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Debug.WriteLine("BrowserSubprocess starting up with command line: " + String.Join("\n", args));

            SubProcess.EnableHighDPISupport();

            int result;
            var type = args.GetArgumentValue(CefSharpArguments.SubProcessTypeArgument);
            var parentProcessId = int.Parse(args.GetArgumentValue(CefSharpArguments.HostProcessIdArgument));

            //Use our custom subProcess provides features like EvaluateJavascript
            if (type == "renderer")
            {
                var wcfEnabled = args.HasArgument(CefSharpArguments.WcfEnabledArgument);
                var subProcess = wcfEnabled ? new WcfEnabledSubProcess(parentProcessId, args) : new SubProcess(args);

                using (subProcess)
                {
                    result = subProcess.Run();
                }
            }
            else
            {
                result = SubProcess.ExecuteProcess();
            }

            Debug.WriteLine("BrowserSubprocess shutting down.");

            return result;
        }
    }
}
