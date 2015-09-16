// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
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
    }
}
