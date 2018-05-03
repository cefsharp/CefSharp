// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace CefSharp
{
    /// <summary>
    /// Default implementation of <see cref="IResourceHandler"/>. This latest implementation provides some simplification, at
    /// a minimum you only need to override ProcessRequestAsync. See the project source on GitHub for working examples.
    /// used to implement a custom request handler interface. The methods of this class will always be called on the IO thread. 
    /// Static helper methods are included like FromStream and FromString that make dealing with fixed resources easy.
    /// </summary>
    public class ResourceHandler : IResourceHandler
    {
        /// <summary>
        /// MimeType to be used if none provided
        /// </summary>
        public const string DefaultMimeType = "text/html";

        /// <summary>
        /// Gets or sets the Mime Type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the resource stream.
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// Gets or sets the http status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status text.
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// Gets or sets ResponseLength, when you know the size of your
        /// Stream (Response) set this property. This is optional.
        /// If you use a MemoryStream and don't provide a value
        /// here then it will be cast and it's size used
        /// </summary>
        public long? ResponseLength { get; set; }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public NameValueCollection Headers { get; private set; }

        /// <summary>
        /// When true the Stream will be Disposed when
        /// this instance is Disposed. The default value for
        /// this property is false.
        /// </summary>
        public bool AutoDisposeStream { get; set; }

        /// <summary>
        /// If the ErrorCode is set then the response will be ignored and
        /// the errorCode returned.
        /// </summary>
        public CefErrorCode? ErrorCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceHandler"/> class.
        /// </summary>
        /// <param name="mimeType">Optional mimeType defaults to <see cref="ResourceHandler.DefaultMimeType"/></param>
        /// <param name="stream">Optional Stream - must be set at some point to provide a valid response</param>
        public ResourceHandler(string mimeType = DefaultMimeType, Stream stream = null)
        {
            if(string.IsNullOrEmpty(mimeType))
            {
                throw new ArgumentNullException("mimeType", "Please provide a valid mimeType");
            }

            StatusCode = 200;
            StatusText = "OK";
            MimeType = mimeType;
            Headers = new NameValueCollection();
            Stream = stream;
        }

        /// <summary>
        /// Begin processing the request. If you have the data in memory you can execute the callback
        /// immediately and return true. For Async processing you would typically spawn a Task to perform processing,
        /// then return true. When the processing is complete execute callback.Continue(); In your processing Task, simply set
        /// the StatusCode, StatusText, MimeType, ResponseLength and Stream
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="callback">The callback used to Continue or Cancel the request (async).</param>
        /// <returns>To handle the request return true and call
        /// <see cref="ICallback.Continue"/> once the response header information is available
        /// <see cref="ICallback.Continue"/> can also be called from inside this method if
        /// header information is available immediately).
        /// To cancel the request return false.</returns>
        public virtual bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            callback.Continue();

            return true;
        }

        /// <summary>
        /// Populate the response stream, response length. When this method is called
        /// the response should be fully populated with data.
        /// It is possible to redirect to another url at this point in time.
        /// NOTE: It's no longer manditory to implement this method, you can simply populate the
        /// properties of this instance and they will be set by the default implementation. 
        /// </summary>
        /// <param name="response">The response object used to set Headers, StatusCode, etc</param>
        /// <param name="responseLength">length of the response</param>
        /// <param name="redirectUrl">If set the request will be redirect to specified Url</param>
        /// <returns>The response stream</returns>
        public virtual Stream GetResponse(IResponse response, out long responseLength, out string redirectUrl)
        {
            redirectUrl = null;
            responseLength = -1;

            response.MimeType = MimeType;
            response.StatusCode = StatusCode;
            response.StatusText = StatusText;
            response.ResponseHeaders = Headers;

            if(ResponseLength.HasValue)
            {
                responseLength = ResponseLength.Value;
            }
            else
            { 
                //If no ResponseLength provided then attempt to infer the length
                if (Stream != null && Stream.CanSeek)
                {
                    responseLength = Stream.Length;
                }
            }

            return Stream;
        }

        /// <summary>
        /// Called if the request is cancelled
        /// </summary>
        public virtual void Cancel()
        {

        }

        bool IResourceHandler.ProcessRequest(IRequest request, ICallback callback)
        {
            return ProcessRequestAsync(request, callback);
        }

        void IResourceHandler.GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            if (ErrorCode.HasValue)
            {
                responseLength = 0;
                redirectUrl = null;
                response.ErrorCode = ErrorCode.Value;
            }
            else
            { 
                Stream = GetResponse(response, out responseLength, out redirectUrl);
            
                if(Stream != null && Stream.CanSeek)
                {
                    //Reset the stream position to 0
                    Stream.Position = 0;
                }
            }
        }

        bool IResourceHandler.ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
        {
            //We don't need the callback, as it's an unmanaged resource we should dispose it (could wrap it in a using statement).
            callback.Dispose();

            if (Stream == null)
            {
                bytesRead = 0;

                return false;
            }

            //Data out represents an underlying buffer (typically 32kb in size).
            var buffer = new byte[dataOut.Length];
            bytesRead = Stream.Read(buffer, 0, buffer.Length);

            //If bytesRead is 0 then no point attempting a write to dataOut
            if(bytesRead == 0)
            {
                return false;
            }

            dataOut.Write(buffer, 0, buffer.Length);

            return bytesRead > 0;
        }

        bool IResourceHandler.CanGetCookie(Cookie cookie)
        {
            return true;
        }

        bool IResourceHandler.CanSetCookie(Cookie cookie)
        {
            return true;
        }

        void IResourceHandler.Cancel()
        {
            Cancel();

            Stream = null;
        }

        /// <summary>
        /// Gets the resource from the file path specified. Use the <see cref="ResourceHandler.GetMimeType"/>
        /// helper method to lookup the mimeType if required. Uses CefStreamResourceHandler for reading the data
        /// </summary>
        /// <param name="filePath">Location of the file.</param>
        /// <param name="mimeType">The mimeType if null then text/html is used.</param>
        /// <returns>IResourceHandler.</returns>
        public static IResourceHandler FromFilePath(string filePath, string mimeType = null)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            var stream = File.OpenRead(filePath);

            return FromStream(stream, mimeType ?? DefaultMimeType);
        }

        /// <summary>
        /// Creates a IResourceHandler that represents a Byte[], uses CefStreamResourceHandler for reading the data
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="mimeType">mimeType</param>
        /// <returns>IResourceHandler</returns>
        public static IResourceHandler FromByteArray(byte[] data, string mimeType = null)
        {
            if(data == null)
            {
                throw new ArgumentNullException("data", "data cannot be null");
            }

            return new ByteArrayResourceHandler(mimeType ?? DefaultMimeType, data);
        }

        /// <summary>
        /// Gets the resource from the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns>ResourceHandler.</returns>
        public static IResourceHandler FromString(string text, string fileExtension)
        {
            var mimeType = GetMimeType(fileExtension);
            return FromString(text, Encoding.UTF8, false, mimeType);
        }

        /// <summary>
        /// Gets a <see cref="ResourceHandler"/> that represents a string.
        /// Without a Preamble, Cef will use BrowserSettings.DefaultEncoding to load the html.
        /// </summary>
        /// <param name="text">The html string</param>
        /// <param name="encoding">Character Encoding</param>
        /// <param name="includePreamble">Include encoding preamble</param>
        /// <param name="mimeType">Mime Type</param>
        /// <returns>ResourceHandler</returns>
        public static IResourceHandler FromString(string text, Encoding encoding = null, bool includePreamble = true, string mimeType = DefaultMimeType)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("text cannot be an empty string", "text");
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var data = GetByteArray(text, encoding, includePreamble);

            return new ByteArrayResourceHandler(mimeType, data);
        }

        /// <summary>
        /// Generates a ResourceHandler that has it's StatusCode set
        /// </summary>
        /// <param name="errorMessage">Body the response to be displayed</param>
        /// <param name="statusCode">StatusCode</param>
        /// <returns>ResourceHandler</returns>
        public static IResourceHandler ForErrorMessage(string errorMessage, HttpStatusCode statusCode)
        {
            if (errorMessage == null)
            {
                throw new ArgumentNullException("errorMessage");
            }

            var stream = GetMemoryStream(errorMessage, Encoding.UTF8);

            var resourceHandler = FromStream(stream);
            resourceHandler.StatusCode = (int)statusCode;

            return resourceHandler;
        }

        /// <summary>
        /// Gets the resource from a stream.
        /// </summary>
        /// <param name="stream">A stream of the resource.</param>
        /// <param name="mimeType">Type of MIME.</param>
        /// <returns>ResourceHandler.</returns>
        public static ResourceHandler FromStream(Stream stream, string mimeType = DefaultMimeType)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            return new ResourceHandler(mimeType, stream);
        }

        /// <summary>
        /// Gets a MemoryStream from the given string using the provided encoding
        /// </summary>
        /// <param name="text">string to be converted to a stream</param>
        /// <param name="encoding">encoding</param>
        /// <param name="includePreamble">if true a BOM will be written to the beginning of the stream</param>
        /// <returns>A memory stream from the given string</returns>
        public static MemoryStream GetMemoryStream(string text, Encoding encoding, bool includePreamble = true)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (includePreamble)
            {
                var preamble = encoding.GetPreamble();
                var bytes = encoding.GetBytes(text);

                var memoryStream = new MemoryStream(preamble.Length + bytes.Length);

                memoryStream.Write(preamble, 0, preamble.Length);
                memoryStream.Write(bytes, 0, bytes.Length);

                memoryStream.Position = 0;

                return memoryStream;
            }

            return new MemoryStream(encoding.GetBytes(text));
        }

        /// <summary>
        /// Gets a byteArray from the given string using the provided encoding
        /// </summary>
        /// <param name="text">string to be converted to a stream</param>
        /// <param name="encoding">encoding</param>
        /// <param name="includePreamble">if true a BOM will be written to the beginning of the stream</param>
        /// <returns>A memory stream from the given string</returns>
        public static byte[] GetByteArray(string text, Encoding encoding, bool includePreamble = true)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (includePreamble)
            {
                var preamble = encoding.GetPreamble();
                var bytes = encoding.GetBytes(text);

                var memoryStream = new MemoryStream(preamble.Length + bytes.Length);

                memoryStream.Write(preamble, 0, preamble.Length);
                memoryStream.Write(bytes, 0, bytes.Length);

                memoryStream.Position = 0;

                return memoryStream.ToArray();
            }

            return encoding.GetBytes(text);
        }

        //TODO: Replace with call to CefGetMimeType (little difficult at the moment with no access to the CefSharp.Core class from here)
        private static readonly IDictionary<string, string> Mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) 
        {
            // Combination of values from Windows 7 Registry and  C:\Windows\System32\inetsrv\config\applicationHost.config
            {".323", "text/h323"},
            {".3g2", "video/3gpp2"},
            {".3gp", "video/3gpp"},
            {".3gp2", "video/3gpp2"},
            {".3gpp", "video/3gpp"},
            {".7z", "application/x-7z-compressed"},
            {".aa", "audio/audible"},
            {".AAC", "audio/aac"},
            {".aaf", "application/octet-stream"},
            {".aax", "audio/vnd.audible.aax"},
            {".ac3", "audio/ac3"},
            {".aca", "application/octet-stream"},
            {".accda", "application/msaccess.addin"},
            {".accdb", "application/msaccess"},
            {".accdc", "application/msaccess.cab"},
            {".accde", "application/msaccess"},
            {".accdr", "application/msaccess.runtime"},
            {".accdt", "application/msaccess"},
            {".accdw", "application/msaccess.webapplication"},
            {".accft", "application/msaccess.ftemplate"},
            {".acx", "application/internet-property-stream"},
            {".AddIn", "text/xml"},
            {".ade", "application/msaccess"},
            {".adobebridge", "application/x-bridge-url"},
            {".adp", "application/msaccess"},
            {".ADT", "audio/vnd.dlna.adts"},
            {".ADTS", "audio/aac"},
            {".afm", "application/octet-stream"},
            {".ai", "application/postscript"},
            {".aif", "audio/x-aiff"},
            {".aifc", "audio/aiff"},
            {".aiff", "audio/aiff"},
            {".air", "application/vnd.adobe.air-application-installer-package+zip"},
            {".amc", "application/x-mpeg"},
            {".application", "application/x-ms-application"},
            {".art", "image/x-jg"},
            {".asa", "application/xml"},
            {".asax", "application/xml"},
            {".ascx", "application/xml"},
            {".asd", "application/octet-stream"},
            {".asf", "video/x-ms-asf"},
            {".ashx", "application/xml"},
            {".asi", "application/octet-stream"},
            {".asm", "text/plain"},
            {".asmx", "application/xml"},
            {".aspx", "application/xml"},
            {".asr", "video/x-ms-asf"},
            {".asx", "video/x-ms-asf"},
            {".atom", "application/atom+xml"},
            {".au", "audio/basic"},
            {".avi", "video/x-msvideo"},
            {".axs", "application/olescript"},
            {".bas", "text/plain"},
            {".bcpio", "application/x-bcpio"},
            {".bin", "application/octet-stream"},
            {".bmp", "image/bmp"},
            {".c", "text/plain"},
            {".cab", "application/octet-stream"},
            {".caf", "audio/x-caf"},
            {".calx", "application/vnd.ms-office.calx"},
            {".cat", "application/vnd.ms-pki.seccat"},
            {".cc", "text/plain"},
            {".cd", "text/plain"},
            {".cdda", "audio/aiff"},
            {".cdf", "application/x-cdf"},
            {".cer", "application/x-x509-ca-cert"},
            {".chm", "application/octet-stream"},
            {".class", "application/x-java-applet"},
            {".clp", "application/x-msclip"},
            {".cmx", "image/x-cmx"},
            {".cnf", "text/plain"},
            {".cod", "image/cis-cod"},
            {".config", "application/xml"},
            {".contact", "text/x-ms-contact"},
            {".coverage", "application/xml"},
            {".cpio", "application/x-cpio"},
            {".cpp", "text/plain"},
            {".crd", "application/x-mscardfile"},
            {".crl", "application/pkix-crl"},
            {".crt", "application/x-x509-ca-cert"},
            {".cs", "text/plain"},
            {".csdproj", "text/plain"},
            {".csh", "application/x-csh"},
            {".csproj", "text/plain"},
            {".css", "text/css"},
            {".csv", "text/csv"},
            {".cur", "application/octet-stream"},
            {".cxx", "text/plain"},
            {".dat", "application/octet-stream"},
            {".datasource", "application/xml"},
            {".dbproj", "text/plain"},
            {".dcr", "application/x-director"},
            {".def", "text/plain"},
            {".deploy", "application/octet-stream"},
            {".der", "application/x-x509-ca-cert"},
            {".dgml", "application/xml"},
            {".dib", "image/bmp"},
            {".dif", "video/x-dv"},
            {".dir", "application/x-director"},
            {".disco", "text/xml"},
            {".dll", "application/x-msdownload"},
            {".dll.config", "text/xml"},
            {".dlm", "text/dlm"},
            {".doc", "application/msword"},
            {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
            {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {".dot", "application/msword"},
            {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
            {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            {".dsp", "application/octet-stream"},
            {".dsw", "text/plain"},
            {".dtd", "text/xml"},
            {".dtsConfig", "text/xml"},
            {".dv", "video/x-dv"},
            {".dvi", "application/x-dvi"},
            {".dwf", "drawing/x-dwf"},
            {".dwp", "application/octet-stream"},
            {".dxr", "application/x-director"},
            {".eml", "message/rfc822"},
            {".emz", "application/octet-stream"},
            {".eot", "application/octet-stream"},
            {".eps", "application/postscript"},
            {".etl", "application/etl"},
            {".etx", "text/x-setext"},
            {".evy", "application/envoy"},
            {".exe", "application/octet-stream"},
            {".exe.config", "text/xml"},
            {".fdf", "application/vnd.fdf"},
            {".fif", "application/fractals"},
            {".filters", "Application/xml"},
            {".fla", "application/octet-stream"},
            {".flr", "x-world/x-vrml"},
            {".flv", "video/x-flv"},
            {".fsscript", "application/fsharp-script"},
            {".fsx", "application/fsharp-script"},
            {".generictest", "application/xml"},
            {".gif", "image/gif"},
            {".group", "text/x-ms-group"},
            {".gsm", "audio/x-gsm"},
            {".gtar", "application/x-gtar"},
            {".gz", "application/x-gzip"},
            {".h", "text/plain"},
            {".hdf", "application/x-hdf"},
            {".hdml", "text/x-hdml"},
            {".hhc", "application/x-oleobject"},
            {".hhk", "application/octet-stream"},
            {".hhp", "application/octet-stream"},
            {".hlp", "application/winhlp"},
            {".hpp", "text/plain"},
            {".hqx", "application/mac-binhex40"},
            {".hta", "application/hta"},
            {".htc", "text/x-component"},
            {".htm", "text/html"},
            {".html", "text/html"},
            {".htt", "text/webviewhtml"},
            {".hxa", "application/xml"},
            {".hxc", "application/xml"},
            {".hxd", "application/octet-stream"},
            {".hxe", "application/xml"},
            {".hxf", "application/xml"},
            {".hxh", "application/octet-stream"},
            {".hxi", "application/octet-stream"},
            {".hxk", "application/xml"},
            {".hxq", "application/octet-stream"},
            {".hxr", "application/octet-stream"},
            {".hxs", "application/octet-stream"},
            {".hxt", "text/html"},
            {".hxv", "application/xml"},
            {".hxw", "application/octet-stream"},
            {".hxx", "text/plain"},
            {".i", "text/plain"},
            {".ico", "image/x-icon"},
            {".ics", "application/octet-stream"},
            {".idl", "text/plain"},
            {".ief", "image/ief"},
            {".iii", "application/x-iphone"},
            {".inc", "text/plain"},
            {".inf", "application/octet-stream"},
            {".inl", "text/plain"},
            {".ins", "application/x-internet-signup"},
            {".ipa", "application/x-itunes-ipa"},
            {".ipg", "application/x-itunes-ipg"},
            {".ipproj", "text/plain"},
            {".ipsw", "application/x-itunes-ipsw"},
            {".iqy", "text/x-ms-iqy"},
            {".isp", "application/x-internet-signup"},
            {".ite", "application/x-itunes-ite"},
            {".itlp", "application/x-itunes-itlp"},
            {".itms", "application/x-itunes-itms"},
            {".itpc", "application/x-itunes-itpc"},
            {".IVF", "video/x-ivf"},
            {".jar", "application/java-archive"},
            {".java", "application/octet-stream"},
            {".jck", "application/liquidmotion"},
            {".jcz", "application/liquidmotion"},
            {".jfif", "image/pjpeg"},
            {".jnlp", "application/x-java-jnlp-file"},
            {".jpb", "application/octet-stream"},
            {".jpe", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".jpg", "image/jpeg"},
            {".js", "application/x-javascript"},
            {".json", "application/json"},
            {".jsx", "text/jscript"},
            {".jsxbin", "text/plain"},
            {".latex", "application/x-latex"},
            {".library-ms", "application/windows-library+xml"},
            {".lit", "application/x-ms-reader"},
            {".loadtest", "application/xml"},
            {".lpk", "application/octet-stream"},
            {".lsf", "video/x-la-asf"},
            {".lst", "text/plain"},
            {".lsx", "video/x-la-asf"},
            {".lzh", "application/octet-stream"},
            {".m13", "application/x-msmediaview"},
            {".m14", "application/x-msmediaview"},
            {".m1v", "video/mpeg"},
            {".m2t", "video/vnd.dlna.mpeg-tts"},
            {".m2ts", "video/vnd.dlna.mpeg-tts"},
            {".m2v", "video/mpeg"},
            {".m3u", "audio/x-mpegurl"},
            {".m3u8", "audio/x-mpegurl"},
            {".m4a", "audio/m4a"},
            {".m4b", "audio/m4b"},
            {".m4p", "audio/m4p"},
            {".m4r", "audio/x-m4r"},
            {".m4v", "video/x-m4v"},
            {".mac", "image/x-macpaint"},
            {".mak", "text/plain"},
            {".man", "application/x-troff-man"},
            {".manifest", "application/x-ms-manifest"},
            {".map", "text/plain"},
            {".master", "application/xml"},
            {".mda", "application/msaccess"},
            {".mdb", "application/x-msaccess"},
            {".mde", "application/msaccess"},
            {".mdp", "application/octet-stream"},
            {".me", "application/x-troff-me"},
            {".mfp", "application/x-shockwave-flash"},
            {".mht", "message/rfc822"},
            {".mhtml", "message/rfc822"},
            {".mid", "audio/mid"},
            {".midi", "audio/mid"},
            {".mix", "application/octet-stream"},
            {".mk", "text/plain"},
            {".mmf", "application/x-smaf"},
            {".mno", "text/xml"},
            {".mny", "application/x-msmoney"},
            {".mod", "video/mpeg"},
            {".mov", "video/quicktime"},
            {".movie", "video/x-sgi-movie"},
            {".mp2", "video/mpeg"},
            {".mp2v", "video/mpeg"},
            {".mp3", "audio/mpeg"},
            {".mp4", "video/mp4"},
            {".mp4v", "video/mp4"},
            {".mpa", "video/mpeg"},
            {".mpe", "video/mpeg"},
            {".mpeg", "video/mpeg"},
            {".mpf", "application/vnd.ms-mediapackage"},
            {".mpg", "video/mpeg"},
            {".mpp", "application/vnd.ms-project"},
            {".mpv2", "video/mpeg"},
            {".mqv", "video/quicktime"},
            {".ms", "application/x-troff-ms"},
            {".msi", "application/octet-stream"},
            {".mso", "application/octet-stream"},
            {".mts", "video/vnd.dlna.mpeg-tts"},
            {".mtx", "application/xml"},
            {".mvb", "application/x-msmediaview"},
            {".mvc", "application/x-miva-compiled"},
            {".mxp", "application/x-mmxp"},
            {".nc", "application/x-netcdf"},
            {".nsc", "video/x-ms-asf"},
            {".nws", "message/rfc822"},
            {".ocx", "application/octet-stream"},
            {".oda", "application/oda"},
            {".odc", "text/x-ms-odc"},
            {".odh", "text/plain"},
            {".odl", "text/plain"},
            {".odp", "application/vnd.oasis.opendocument.presentation"},
            {".ods", "application/oleobject"},
            {".odt", "application/vnd.oasis.opendocument.text"},
            {".one", "application/onenote"},
            {".onea", "application/onenote"},
            {".onepkg", "application/onenote"},
            {".onetmp", "application/onenote"},
            {".onetoc", "application/onenote"},
            {".onetoc2", "application/onenote"},
            {".orderedtest", "application/xml"},
            {".osdx", "application/opensearchdescription+xml"},
            {".p10", "application/pkcs10"},
            {".p12", "application/x-pkcs12"},
            {".p7b", "application/x-pkcs7-certificates"},
            {".p7c", "application/pkcs7-mime"},
            {".p7m", "application/pkcs7-mime"},
            {".p7r", "application/x-pkcs7-certreqresp"},
            {".p7s", "application/pkcs7-signature"},
            {".pbm", "image/x-portable-bitmap"},
            {".pcast", "application/x-podcast"},
            {".pct", "image/pict"},
            {".pcx", "application/octet-stream"},
            {".pcz", "application/octet-stream"},
            {".pdf", "application/pdf"},
            {".pfb", "application/octet-stream"},
            {".pfm", "application/octet-stream"},
            {".pfx", "application/x-pkcs12"},
            {".pgm", "image/x-portable-graymap"},
            {".pic", "image/pict"},
            {".pict", "image/pict"},
            {".pkgdef", "text/plain"},
            {".pkgundef", "text/plain"},
            {".pko", "application/vnd.ms-pki.pko"},
            {".pls", "audio/scpls"},
            {".pma", "application/x-perfmon"},
            {".pmc", "application/x-perfmon"},
            {".pml", "application/x-perfmon"},
            {".pmr", "application/x-perfmon"},
            {".pmw", "application/x-perfmon"},
            {".png", "image/png"},
            {".pnm", "image/x-portable-anymap"},
            {".pnt", "image/x-macpaint"},
            {".pntg", "image/x-macpaint"},
            {".pnz", "image/png"},
            {".pot", "application/vnd.ms-powerpoint"},
            {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
            {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
            {".ppa", "application/vnd.ms-powerpoint"},
            {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            {".ppm", "image/x-portable-pixmap"},
            {".pps", "application/vnd.ms-powerpoint"},
            {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            {".ppt", "application/vnd.ms-powerpoint"},
            {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {".prf", "application/pics-rules"},
            {".prm", "application/octet-stream"},
            {".prx", "application/octet-stream"},
            {".ps", "application/postscript"},
            {".psc1", "application/PowerShell"},
            {".psd", "application/octet-stream"},
            {".psess", "application/xml"},
            {".psm", "application/octet-stream"},
            {".psp", "application/octet-stream"},
            {".pub", "application/x-mspublisher"},
            {".pwz", "application/vnd.ms-powerpoint"},
            {".qht", "text/x-html-insertion"},
            {".qhtm", "text/x-html-insertion"},
            {".qt", "video/quicktime"},
            {".qti", "image/x-quicktime"},
            {".qtif", "image/x-quicktime"},
            {".qtl", "application/x-quicktimeplayer"},
            {".qxd", "application/octet-stream"},
            {".ra", "audio/x-pn-realaudio"},
            {".ram", "audio/x-pn-realaudio"},
            {".rar", "application/octet-stream"},
            {".ras", "image/x-cmu-raster"},
            {".rat", "application/rat-file"},
            {".rc", "text/plain"},
            {".rc2", "text/plain"},
            {".rct", "text/plain"},
            {".rdlc", "application/xml"},
            {".resx", "application/xml"},
            {".rf", "image/vnd.rn-realflash"},
            {".rgb", "image/x-rgb"},
            {".rgs", "text/plain"},
            {".rm", "application/vnd.rn-realmedia"},
            {".rmi", "audio/mid"},
            {".rmp", "application/vnd.rn-rn_music_package"},
            {".roff", "application/x-troff"},
            {".rpm", "audio/x-pn-realaudio-plugin"},
            {".rqy", "text/x-ms-rqy"},
            {".rtf", "application/rtf"},
            {".rtx", "text/richtext"},
            {".ruleset", "application/xml"},
            {".s", "text/plain"},
            {".safariextz", "application/x-safari-safariextz"},
            {".scd", "application/x-msschedule"},
            {".sct", "text/scriptlet"},
            {".sd2", "audio/x-sd2"},
            {".sdp", "application/sdp"},
            {".sea", "application/octet-stream"},
            {".searchConnector-ms", "application/windows-search-connector+xml"},
            {".setpay", "application/set-payment-initiation"},
            {".setreg", "application/set-registration-initiation"},
            {".settings", "application/xml"},
            {".sgimb", "application/x-sgimb"},
            {".sgml", "text/sgml"},
            {".sh", "application/x-sh"},
            {".shar", "application/x-shar"},
            {".shtml", "text/html"},
            {".sit", "application/x-stuffit"},
            {".sitemap", "application/xml"},
            {".skin", "application/xml"},
            {".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
            {".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
            {".slk", "application/vnd.ms-excel"},
            {".sln", "text/plain"},
            {".slupkg-ms", "application/x-ms-license"},
            {".smd", "audio/x-smd"},
            {".smi", "application/octet-stream"},
            {".smx", "audio/x-smd"},
            {".smz", "audio/x-smd"},
            {".snd", "audio/basic"},
            {".snippet", "application/xml"},
            {".snp", "application/octet-stream"},
            {".sol", "text/plain"},
            {".sor", "text/plain"},
            {".spc", "application/x-pkcs7-certificates"},
            {".spl", "application/futuresplash"},
            {".src", "application/x-wais-source"},
            {".srf", "text/plain"},
            {".SSISDeploymentManifest", "text/xml"},
            {".ssm", "application/streamingmedia"},
            {".sst", "application/vnd.ms-pki.certstore"},
            {".stl", "application/vnd.ms-pki.stl"},
            {".sv4cpio", "application/x-sv4cpio"},
            {".sv4crc", "application/x-sv4crc"},
            {".svc", "application/xml"},
            {".svg", "image/svg+xml"},
            {".swf", "application/x-shockwave-flash"},
            {".t", "application/x-troff"},
            {".tar", "application/x-tar"},
            {".tcl", "application/x-tcl"},
            {".testrunconfig", "application/xml"},
            {".testsettings", "application/xml"},
            {".tex", "application/x-tex"},
            {".texi", "application/x-texinfo"},
            {".texinfo", "application/x-texinfo"},
            {".tgz", "application/x-compressed"},
            {".thmx", "application/vnd.ms-officetheme"},
            {".thn", "application/octet-stream"},
            {".tif", "image/tiff"},
            {".tiff", "image/tiff"},
            {".tlh", "text/plain"},
            {".tli", "text/plain"},
            {".toc", "application/octet-stream"},
            {".tr", "application/x-troff"},
            {".trm", "application/x-msterminal"},
            {".trx", "application/xml"},
            {".ts", "video/vnd.dlna.mpeg-tts"},
            {".tsv", "text/tab-separated-values"},
            {".ttf", "application/octet-stream"},
            {".tts", "video/vnd.dlna.mpeg-tts"},
            {".txt", "text/plain"},
            {".u32", "application/octet-stream"},
            {".uls", "text/iuls"},
            {".user", "text/plain"},
            {".ustar", "application/x-ustar"},
            {".vb", "text/plain"},
            {".vbdproj", "text/plain"},
            {".vbk", "video/mpeg"},
            {".vbproj", "text/plain"},
            {".vbs", "text/vbscript"},
            {".vcf", "text/x-vcard"},
            {".vcproj", "Application/xml"},
            {".vcs", "text/plain"},
            {".vcxproj", "Application/xml"},
            {".vddproj", "text/plain"},
            {".vdp", "text/plain"},
            {".vdproj", "text/plain"},
            {".vdx", "application/vnd.ms-visio.viewer"},
            {".vml", "text/xml"},
            {".vscontent", "application/xml"},
            {".vsct", "text/xml"},
            {".vsd", "application/vnd.visio"},
            {".vsi", "application/ms-vsi"},
            {".vsix", "application/vsix"},
            {".vsixlangpack", "text/xml"},
            {".vsixmanifest", "text/xml"},
            {".vsmdi", "application/xml"},
            {".vspscc", "text/plain"},
            {".vss", "application/vnd.visio"},
            {".vsscc", "text/plain"},
            {".vssettings", "text/xml"},
            {".vssscc", "text/plain"},
            {".vst", "application/vnd.visio"},
            {".vstemplate", "text/xml"},
            {".vsto", "application/x-ms-vsto"},
            {".vsw", "application/vnd.visio"},
            {".vsx", "application/vnd.visio"},
            {".vtx", "application/vnd.visio"},
            {".wav", "audio/wav"},
            {".wave", "audio/wav"},
            {".wax", "audio/x-ms-wax"},
            {".wbk", "application/msword"},
            {".wbmp", "image/vnd.wap.wbmp"},
            {".wcm", "application/vnd.ms-works"},
            {".wdb", "application/vnd.ms-works"},
            {".wdp", "image/vnd.ms-photo"},
            {".webarchive", "application/x-safari-webarchive"},
            {".webtest", "application/xml"},
            {".wiq", "application/xml"},
            {".wiz", "application/msword"},
            {".wks", "application/vnd.ms-works"},
            {".WLMP", "application/wlmoviemaker"},
            {".wlpginstall", "application/x-wlpg-detect"},
            {".wlpginstall3", "application/x-wlpg3-detect"},
            {".wm", "video/x-ms-wm"},
            {".wma", "audio/x-ms-wma"},
            {".wmd", "application/x-ms-wmd"},
            {".wmf", "application/x-msmetafile"},
            {".wml", "text/vnd.wap.wml"},
            {".wmlc", "application/vnd.wap.wmlc"},
            {".wmls", "text/vnd.wap.wmlscript"},
            {".wmlsc", "application/vnd.wap.wmlscriptc"},
            {".wmp", "video/x-ms-wmp"},
            {".wmv", "video/x-ms-wmv"},
            {".wmx", "video/x-ms-wmx"},
            {".wmz", "application/x-ms-wmz"},
            {".wpl", "application/vnd.ms-wpl"},
            {".wps", "application/vnd.ms-works"},
            {".wri", "application/x-mswrite"},
            {".wrl", "x-world/x-vrml"},
            {".wrz", "x-world/x-vrml"},
            {".wsc", "text/scriptlet"},
            {".wsdl", "text/xml"},
            {".wvx", "video/x-ms-wvx"},
            {".x", "application/directx"},
            {".xaf", "x-world/x-vrml"},
            {".xaml", "application/xaml+xml"},
            {".xap", "application/x-silverlight-app"},
            {".xbap", "application/x-ms-xbap"},
            {".xbm", "image/x-xbitmap"},
            {".xdr", "text/plain"},
            {".xht", "application/xhtml+xml"},
            {".xhtml", "application/xhtml+xml"},
            {".xla", "application/vnd.ms-excel"},
            {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
            {".xlc", "application/vnd.ms-excel"},
            {".xld", "application/vnd.ms-excel"},
            {".xlk", "application/vnd.ms-excel"},
            {".xll", "application/vnd.ms-excel"},
            {".xlm", "application/vnd.ms-excel"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
            {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {".xlt", "application/vnd.ms-excel"},
            {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
            {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            {".xlw", "application/vnd.ms-excel"},
            {".xml", "text/xml"},
            {".xmta", "application/xml"},
            {".xof", "x-world/x-vrml"},
            {".XOML", "text/plain"},
            {".xpm", "image/x-xpixmap"},
            {".xps", "application/vnd.ms-xpsdocument"},
            {".xrm-ms", "text/xml"},
            {".xsc", "application/xml"},
            {".xsd", "text/xml"},
            {".xsf", "text/xml"},
            {".xsl", "text/xml"},
            {".xslt", "text/xml"},
            {".xsn", "application/octet-stream"},
            {".xss", "application/xml"},
            {".xtp", "application/octet-stream"},
            {".xwd", "image/x-xwindowdump"},
            {".z", "application/x-compress"},
            {".zip", "application/x-zip-compressed"}
        };

        /// <summary>
        /// Gets the MIME type of the content.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentNullException">extension</exception>
        public static string GetMimeType(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }
            string mime;
            return Mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }

        /// <summary>
        /// Dispose of resources here
        /// </summary>
        public virtual void Dispose()
        {
            if(AutoDisposeStream && Stream != null)
            {
                Stream.Dispose();
                Stream = null;
            }
        }
    }
}
