// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Text;

namespace CefSharp
{
    public static class PostDataExtensions
    {
        //size_t count = el->GetBytesCount();
        //                char* bytes = new char[count];

        //                el->GetBytes(count, bytes);

        //                // Attempt to honour the charset specified by the request's Content-Type header.
        //                String^ charset = this->CharSet;
        //                if (charset != nullptr)
        //                {
        //                    Encoding^ encoding;
        //                    try
        //                    {
        //                        encoding = Encoding::GetEncoding(charset);
        //                    }
        //                    catch (ArgumentException^)
        //                    {
        //                        encoding = nullptr;
        //                    }
        //                    if (encoding != nullptr)
        //                    {
        //                        return gcnew String(bytes, 0, count, encoding);
        //                    }
        //                }

        //                // Revert to using the system's default code page.
        //                return gcnew String(bytes, 0, count);

        /// <summary>
        /// Extracts the charset argument from the content-type header.
        /// The charset is optional, so a nullptr may be returned.
        /// For example, given a Content-Type header "application/json; charset=UTF-8",
        /// this function will return "UTF-8".
        /// </summary>
        //String^ CefRequestWrapper::CharSet::get()
        //{
        //    // Extract the Content-Type header value.
        //    auto headers = this->Headers;
            
        //    String^ contentType = nullptr;
        //    for each(String^ key in headers)
        //    {
        //        if (key->Equals("content-type", System::StringComparison::InvariantCultureIgnoreCase))
        //        {
        //            for each(String^ element in headers->GetValues(key))
        //            {
        //                contentType = element;
        //                break;
        //            }
        //            break;
        //        }
        //    }

        //    if (contentType == nullptr)
        //    {
        //        return nullptr;
        //    }

        //    // Look for charset after the mime-type.
        //    const int semiColonIndex = contentType->IndexOf(";");
        //    if (semiColonIndex == -1)
        //    {
        //        return nullptr;
        //    }

        //    String^ charsetArgument = contentType->Substring(semiColonIndex + 1)->Trim();
        //    const int equalsIndex = charsetArgument->IndexOf("=");
        //    if (equalsIndex == -1)
        //    {
        //        return nullptr;
        //    }

        //    String^ argumentName = charsetArgument->Substring(0, equalsIndex)->Trim();
        //    if (!argumentName->Equals("charset", System::StringComparison::InvariantCultureIgnoreCase))
        //    {
        //        return nullptr;
        //    }

        //    String^ charset = charsetArgument->Substring(equalsIndex + 1)->Trim();
        //    // Remove redundant characters (e.g. "UTF-8"; -> UTF-8)
        //    charset = charset->TrimStart(' ', '"');
        //    charset = charset->TrimEnd(' ', '"', ';');

        //    return charset;
        //}

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

        public static string GetBody(this IPostDataElement postDataElement, string charSet)
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
