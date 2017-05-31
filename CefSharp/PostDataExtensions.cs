// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Text;

namespace CefSharp
{
    public static class PostDataExtensions
    {
        /// <summary>
        /// A convenience extension method that extracts the Character set from
        /// the content-type header. Can be used in conjuncation with <see cref="GetBody"/>
        /// </summary>
        /// <param name="request">the request</param>
        /// <returns>character set e.g. UTF-8</returns>
        public static string GetCharSet(this IRequest request)
        {
            //Extract the Content-Type header value.
            var headers = request.Headers;

            string contentType = null;
            foreach(string key in headers)
            {
                if (key.Equals("content-type", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach(var element in headers.GetValues(key))
                    {
                        contentType = element;
                        break;
                    }
                    break;
                }
            }

            if (contentType == null)
            {
                return null;
            }

             //Look for charset after the mime-type.
            var semiColonIndex = contentType.IndexOf(";", StringComparison.InvariantCulture);
            if (semiColonIndex == -1)
            {
                return null;
            }

            var charsetArgument = contentType.Substring(semiColonIndex + 1).Trim();
            var equalsIndex = charsetArgument.IndexOf("=", StringComparison.InvariantCulture);
            if (equalsIndex == -1)
            {
                return null;
            }

            var argumentName = charsetArgument.Substring(0, equalsIndex).Trim();
            if (!argumentName.Equals("charset", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            var charset = charsetArgument.Substring(equalsIndex + 1).Trim();
            // Remove redundant characters (e.g. "UTF-8"; -> UTF-8)
            charset = charset.TrimStart(' ', '"');
            charset = charset.TrimEnd(' ', '"', ';');

            return charset;
        }

        /// <summary>
        /// Converts the <see cref="IPostDataElement.Bytes"/> property into a string
        /// using the specified charset (Encoding) or if unable to parse then uses
        /// the <see cref="Encoding.Default"/>
        /// </summary>
        /// <param name="postDataElement">post data</param>
        /// <param name="charSet">character set</param>
        /// <returns>encoded string</returns>
        public static string GetBody(this IPostDataElement postDataElement, string charSet = null)
        {
            var bytes = postDataElement.Bytes;
            if(bytes.Length == 0)
            {
                return null;
            }

            var encoding = Encoding.Default;

            if (charSet != null)
            {
                try
                {
                    encoding = Encoding.GetEncoding(charSet);
                }
                catch (ArgumentException)
                {
                    
                }
            }

            return encoding.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Add a new <see cref="IPostDataElement"/> that represents the specified file
        /// </summary>
        /// <param name="postData">post data instance</param>
        /// <param name="fileName">file name</param>
        public static void AddFile(this IPostData postData, string fileName)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            var element = postData.CreatePostDataElement();
            element.File = fileName;

            postData.AddElement(element);
        }

        /// <summary>
        /// Add a new <see cref="IPostDataElement"/> that represents the key and value
        /// The data is encoded using
        /// </summary>
        /// <param name="postData">Post Data</param>
        /// <param name="data">Data to be encoded for the post data element</param>
        /// <param name="encoding">Specified Encoding. If null then <see cref="Encoding.Default"/> will be used</param>
        public static void AddData(this IPostData postData, string data, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }

            if(encoding == null)
            {
                encoding = Encoding.Default;
            }

            var element = postData.CreatePostDataElement();

            element.Bytes = encoding.GetBytes(data);

            postData.AddElement(element);
        }

        /// <summary>
        /// Add a new <see cref="IPostDataElement"/> that represents the key and value
        /// </summary>
        /// <param name="postData">Post Data</param>
        /// <param name="bytes">byte array that represents the post data</param>
        public static void AddData(this IPostData postData, byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            var element = postData.CreatePostDataElement();

            element.Bytes = bytes;

            postData.AddElement(element);
        }
    }
}
