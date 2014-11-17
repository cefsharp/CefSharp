using System;
using System.Collections.Specialized;
using System.IO;

namespace CefSharp
{
    public class Response : IResponse
    {
        public Stream ResponseStream { get;private set; }
        public String MimeType { get; private set; } 
        public String StatusText { get; private set; }
        public int StatusCode { get; private set; }
        public NameValueCollection ResponseHeaders { get; private set; }
        public String RedirectUrl { get; private set; }
        public ResponseAction Action { get; private set; }

        public Response()
        {
            Action = ResponseAction.Continue;
        }

        public void Cancel()
        {
            Action = ResponseAction.Cancel;
        }

        public void Redirect(String url)
        {
            RedirectUrl = url;
            Action = ResponseAction.Redirect;
        }

        public void RespondWith(Stream stream, String mimeType)
        {
            RespondWith(stream, mimeType, "OK", 200, null);
        }

        public void RespondWith(Stream stream, String mimeType, String statusText, int statusCode, NameValueCollection responseHeaders)
        {
            if (string.IsNullOrEmpty(mimeType))
            {
                throw new ArgumentException("must provide a mime type", "mimeType");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            ResponseStream = stream;
            MimeType = mimeType;
            StatusText = statusText;
            StatusCode = statusCode;
            ResponseHeaders = responseHeaders;

            Action = ResponseAction.Respond;
        }
    }
}
