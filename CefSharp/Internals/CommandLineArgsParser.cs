// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Linq;

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

        public static string GetArgumentValue(this IEnumerable<string> args, string argumentName)
        {
            var arg = args.FirstOrDefault(a => a.StartsWith(argumentName));
            if (arg == null)
            {
                return null;
            }

            return arg.Split('=').Last();
        }
    }
}
