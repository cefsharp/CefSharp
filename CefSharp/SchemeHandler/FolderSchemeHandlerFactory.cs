// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Net;

namespace CefSharp.SchemeHandler
{
    /// <summary>
    /// FolderSchemeHandlerFactory is a very simple scheme handler that allows you
    /// to map requests for urls to a folder on your file system. For example
    /// creating a setting the rootFolder to c:\projects\CefSharp\CefSharp.Example\Resources
    /// registering the scheme handler
    /// </summary>
    public class FolderSchemeHandlerFactory : ISchemeHandlerFactory
    {
        private readonly string rootFolder;
        private readonly string defaultPage;
        private readonly string schemeName;
        private readonly string hostName;
        private readonly FileShare resourceFileShare;

        /// <summary>
        /// Initialize a new instance of FolderSchemeHandlerFactory
        /// </summary>
        /// <param name="rootFolder">Root Folder where all your files exist, requests cannot be made outside of this folder</param>
        /// <param name="schemeName">if not null then schemeName checking will be implemented</param>
        /// <param name="hostName">if not null then hostName checking will be implemented</param>
        /// <param name="defaultPage">default page if no page specified, defaults to index.html</param>
        /// <param name="resourceFileShare">file share mode used to open resources, defaults to FileShare.Read</param>
        public FolderSchemeHandlerFactory(string rootFolder, string schemeName = null, string hostName = null, string defaultPage = "index.html", FileShare resourceFileShare = FileShare.Read)
        {
            this.rootFolder = Path.GetFullPath(rootFolder);
            this.defaultPage = defaultPage;
            this.schemeName = schemeName;
            this.hostName = hostName;
            this.resourceFileShare = resourceFileShare;

            if (!Directory.Exists(this.rootFolder))
            {
                throw new DirectoryNotFoundException(this.rootFolder);
            }
        }

        /// <summary>
        /// If the file requested is within the rootFolder then a IResourceHandler reference to the file requested will be returned
        /// otherwise a 404 ResourceHandler will be returned.
        /// </summary>
        /// <param name="browser">the browser window that originated the
        /// request or null if the request did not originate from a browser window
        /// (for example, if the request came from CefURLRequest).</param>
        /// <param name="frame">frame that originated the request
        /// or null if the request did not originate from a browser window
        /// (for example, if the request came from CefURLRequest).</param>
        /// <param name="schemeName">the scheme name</param>
        /// <param name="request">The request. (will not contain cookie data)</param>
        /// <returns>
        /// A IResourceHandler
        /// </returns>
        IResourceHandler ISchemeHandlerFactory.Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            if (this.schemeName != null && !schemeName.Equals(this.schemeName, StringComparison.OrdinalIgnoreCase))
            {
                return ResourceHandler.ForErrorMessage(string.Format("SchemeName {0} does not match the expected SchemeName of {1}.", schemeName, this.schemeName), HttpStatusCode.NotFound);

            }

            var uri = new Uri(request.Url);

            if (this.hostName != null && !uri.Host.Equals(this.hostName, StringComparison.OrdinalIgnoreCase))
            {
                return ResourceHandler.ForErrorMessage(string.Format("HostName {0} does not match the expected HostName of {1}.", uri.Host, this.hostName), HttpStatusCode.NotFound);
            }

            //Get the absolute path and remove the leading slash
            var asbolutePath = uri.AbsolutePath.Substring(1);

            if (string.IsNullOrEmpty(asbolutePath))
            {
                asbolutePath = defaultPage;
            }

            var filePath = WebUtility.UrlDecode(Path.GetFullPath(Path.Combine(rootFolder, asbolutePath)));

            //Check the file requested is within the specified path and that the file exists
            if (filePath.StartsWith(rootFolder, StringComparison.OrdinalIgnoreCase) && File.Exists(filePath))
            {
                var fileExtension = Path.GetExtension(filePath);
                var mimeType = ResourceHandler.GetMimeType(fileExtension);
                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, resourceFileShare); 
                return ResourceHandler.FromStream(stream, mimeType);
            }

            return ResourceHandler.ForErrorMessage("File Not Found - " + filePath, HttpStatusCode.NotFound);
        }
    }
}
