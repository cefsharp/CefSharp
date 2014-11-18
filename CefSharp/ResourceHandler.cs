// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace CefSharp
{
    public class ResourceHandler
    {
        public string MimeType { get; set; }
        public Stream Stream { get; set; }
        public int StatusCode { get; set; }
        public string StatusText { get; set; }
        public NameValueCollection Headers { get; set; }

        public ResourceHandler()
        {
            StatusCode = 200;
            StatusText = "OK";
            MimeType = "text/html";
        }

        public static ResourceHandler FromFileName(string fileName)
        {
            return new ResourceHandler { Stream = File.OpenRead(fileName) };
        }

        public static ResourceHandler FromString(string text)
        {
            return new ResourceHandler { Stream = new MemoryStream(Encoding.UTF8.GetBytes(text)) };
        }
    }
}
