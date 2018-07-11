// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
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

            var parentProcessId = -1;

            // The Crashpad Handler doesn't have any HostProcessIdArgument, so we must not try to
            // parse it lest we want an ArgumentNullException.
            if (type != "crashpad-handler")
            {
                parentProcessId = int.Parse(args.GetArgumentValue(CefSharpArguments.HostProcessIdArgument));
                if (args.HasArgument(CefSharpArguments.ExitIfParentProcessClosed))
                {
                    Task.Factory.StartNew(() => AwaitParentProcessExit(parentProcessId), TaskCreationOptions.LongRunning);
                }
            }

            // Use our custom subProcess provides features like EvaluateJavascript
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

        private static async void AwaitParentProcessExit(int parentProcessId) 
        {
            try 
            {
                var parentProcess = Process.GetProcessById(parentProcessId);
                parentProcess.WaitForExit();
            }
            catch (Exception e) 
            {
                //main process probably died already
                Debug.WriteLine(e);
            }

            await Task.Delay(1000); //wait a bit before exiting

            Debug.WriteLine("BrowserSubprocess shutting down forcibly.");

            Environment.Exit(0);
        }
    }
}
