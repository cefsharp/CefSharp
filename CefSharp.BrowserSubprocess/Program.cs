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

            CefAppWrapper.EnableHighDPISupport();

            int result;

            using (var subprocess = Create(args))
            {
                result = subprocess.Run();
            }

            Kernel32.OutputDebugString("BrowserSubprocess shutting down.");
            return result;
        }

        private static CefSubProcess Create(IEnumerable<string> args)
        {
            const string typePrefix = "--type=";
            var typeArgument = args.SingleOrDefault(arg => arg.StartsWith(typePrefix));
            var wcfEnabled = args.HasArgument(CefSharpArguments.WcfEnabledArgument);

            var type = typeArgument.Substring(typePrefix.Length);

            switch (type)
            {
                case "renderer":
                {
                    return wcfEnabled ? new CefRenderProcess(args) : new CefSubProcess(args);
                }
                case "gpu-process":
                default:
                {
                    return new CefSubProcess(args);
                }
            }
        }
    }
}
