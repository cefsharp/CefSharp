using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CefSharp
{
    public abstract class SchemeHandlerResponseBase : ObjectBase
    {
        protected SchemeHandlerResponseBase()
        {
            StatusCode = HttpStatusCode.OK;
            ContentLength = -1;
            MimeType = string.Empty;
            RedirectUrl = string.Empty;
        }

        protected override void DoDispose(bool isDisposing)
        {
            if ( CloseStream )
            {
                ResponseStream.Close();
            }
            ResponseStream = null;
            ResponseHeaders = null;

            base.DoDispose(isDisposing);
        }

        /// <summary>
        /// A Stream with the response data. If the request didn't return any response, leave this property as null.
        /// </summary>
        public Stream ResponseStream { get; set; }

        public string MimeType { get; set; }

        public IDictionary<string, string> ResponseHeaders { get; set; }

        /// <summary>
        /// The status code of the response. Unless set, the default value used is 200
        /// (corresponding to HTTP status OK).
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// The length of the response contents. Defaults to -1, which means unknown length
        /// and causes CefSharp to read the response stream in pieces. Thus, setting a length
        /// is optional but allows for more optimal response reading.
        /// </summary>
        public int ContentLength { get; set; }

        /// <summary>
        /// URL to redirect to (leave empty to not redirect).
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Set to true to close the response stream once it has been read. The default value
        /// is false in order to preserve the old CefSharp behavior of not closing the stream.
        /// </summary>
        public bool CloseStream { get; set; }

        public int SizeFromStream()
        {
            if ( ResponseStream == null )
            {
                return 0;
            }

            if (ResponseStream.CanSeek)
            {
                ResponseStream.Seek(0, SeekOrigin.End);
                int length = (int)ResponseStream.Position;
                ResponseStream.Seek(0, SeekOrigin.Begin);
                return length;
            }
            return -1;
        }
    }
}
