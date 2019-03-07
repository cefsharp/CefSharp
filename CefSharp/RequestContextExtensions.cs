// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;

namespace CefSharp
{
    public static class RequestContextExtensions
    {
        /// <summary>
        /// Load an extension from the given directory. To load a crx file you must unzip it first.
        /// For further details see <seealso cref="IRequestContext.LoadExtension(string, string, IExtensionHandler)"/>
        /// </summary>
        /// <param name="requestContext">request context</param>
        /// <param name="rootDirectory">absolute path to the directory that contains the extension to be loaded.</param>
        /// <param name="handler">handle events related to browser extensions</param>
        public static void LoadExtensionFromDirectory(this IRequestContext requestContext, string rootDirectory, IExtensionHandler handler)
        {
            requestContext.LoadExtension(Path.GetFullPath(rootDirectory), null, handler);
        }

        /// <summary>
        /// Load extension(s) from the given directory. This methods obtains all the sub directories of <paramref name="rootDirectory"/>
        /// and calls <see cref="IRequestContext.LoadExtension(string, string, IExtensionHandler)"/> if manifest.json
        /// is found in the sub folder. To load crx file(s) you must unzip them first.
        /// For further details see <seealso cref="IRequestContext.LoadExtension(string, string, IExtensionHandler)"/>
        /// </summary>
        /// <param name="requestContext">request context</param>
        /// <param name="rootDirectory">absolute path to the directory that contains the extension(s) to be loaded.</param>
        /// <param name="handler">handle events related to browser extensions</param>
        public static void LoadExtensionsFromDirectory(this IRequestContext requestContext, string rootDirectory, IExtensionHandler handler)
        {
            var fullPath = Path.GetFullPath(rootDirectory);

            foreach (var dir in Directory.GetDirectories(fullPath))
            {
                //Check the directory has a manifest.json, if so call load
                if (File.Exists(Path.Combine(dir, "manifest.json")))
                {
                    requestContext.LoadExtension(dir, null, handler);
                }
            }
        }
    }
}
