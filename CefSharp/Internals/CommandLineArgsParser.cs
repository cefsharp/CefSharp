// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Linq;
using System.Collections.Generic;

namespace CefSharp.Internals
{
    /// <summary>
    /// Simple helper class used for checking/parsing command line arguments
    /// </summary>
    public static class CommandLineArgsParser
    {
        public static bool HasArgument(this IEnumerable<string> args, string arg)
        {
            return args.Any(a => a.StartsWith(arg));
        }

        public static int? LocateParentProcessId(this IEnumerable<string> args)
        {
            var hostProcessId = args.SingleOrDefault(arg => arg.StartsWith(CefSharpArguments.WcfHostProcessIdArgument));
            if (hostProcessId == null)
            {
                return null;
            }

            var parentProcessId = hostProcessId
                .Split('=')
                .Last();
            return int.Parse(parentProcessId);
        }
    }
}
