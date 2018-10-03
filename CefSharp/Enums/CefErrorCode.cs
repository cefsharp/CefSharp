// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Lists some of the error codes that can be reported by CEF.
    /// 
    /// For a complete up-to-date list, see the CEF source code
    /// (cef_errorcode_t in include/internal/cef_types.h)
    /// and the Chromium source code (net/base/net_error_list.h).
    /// </summary>
    public enum CefErrorCode
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        None = 0,

        /// <summary>
        /// An asynchronous IO operation is not yet complete.  This usually does not
        /// indicate a fatal error.  Typically this error will be generated as a
        /// notification to wait for some external notification that the IO operation
        /// finally completed.
        /// </summary>
        IoPending = -1,

        /// <summary>
        /// A generic failure occurred.
        /// </summary>
        Failed = -2,

        /// <summary>
        /// A request was aborted, possibly by the user.
        /// </summary>
        Aborted = -3,

        /// <summary>
        /// An argument to the function is incorrect.
        /// </summary>
        InvalidArgument = -4,

        /// <summary>
        /// The handle or file descriptor is invalid.
        /// </summary>
        InvalidHandle = -5,

        /// <summary>
        /// The file or directory cannot be found.
        /// </summary>
        FileNotFound = -6,

        /// <summary>
        /// An operation timed out.
        /// </summary>
        TimedOut = -7,

        /// <summary>
        /// The file is too large.
        /// </summary>
        FileTooBig = -8,

        /// <summary>
        /// An unexpected error. This may be caused by a programming mistake or an invalid assumption
        /// </summary>
        Unexpected = -9,

        /// <summary>
        /// Permission to access a resource, other than the network, was denied.
        /// </summary>
        AccessDenied = -10,

        /// <summary>
        /// The operation failed because of unimplemented functionality.
        /// </summary>
        NotImplemented = -11,

        /// <summary>
        /// There were not enough resources to complete the operation.
        /// </summary>
        InsufficientResources = -12,

        /// <summary>
        /// Memory allocation failed.
        /// </summary>
        OutOfMemory = -13,

        /// <summary>
        /// The file upload failed because the file's modification time was different
        /// from the expectation.
        /// </summary>
        UploadFileChanged = -14,

        /// <summary>
        /// The socket is not connected.
        /// </summary>
        SocketNotConnected = -15,

        /// <summary>
        /// The file already exists.
        /// </summary>
        FileExists = -16,

        /// <summary>
        /// The path or file name is too long.
        /// </summary>
        FilePathTooLong = -17,

        /// <summary>
        /// Not enough room left on the disk.
        /// </summary>
        FileNoSpace = -18,

        /// <summary>
        /// The file has a virus.
        /// </summary>
        FileVirusInfected = -19,

        /// <summary>
        /// The client chose to block the request.
        /// </summary>
        BlockedByClient = -20,

        /// <summary>
        /// The network changed.
        /// </summary>
        NetworkChanged = -21,

        /// <summary>
        /// The request was blocked by the URL blacklist configured by the domain
        /// administrator.
        /// </summary>
        BlockedByAdministrator = -22,

        /// <summary>
        /// The socket is already connected.
        /// </summary>
        SocketIsConnected = -23,

        /// <summary>
        /// The request was blocked because the forced reenrollment check is still
        /// pending. This error can only occur on ChromeOS.
        /// The error can be emitted by code in chrome/browser/policy/policy_helpers.cc.
        /// </summary>
        BlockedEnrollmentCheckPending = -24,

        /// <summary>
        /// A connection was closed (corresponding to a TCP FIN).
        /// </summary>
        ConnectionClosed = -100,

        /// <summary>
        /// A connection was reset (corresponding to a TCP RST).
        /// </summary>
        ConnectionReset = -101,

        /// <summary>
        /// A connection attempt was refused.
        /// </summary>
        ConnectionRefused = -102,

        /// <summary>
        /// A connection timed out as a result of not receiving an ACK for data sent.
        /// This can include a FIN packet that did not get ACK'd.
        /// </summary>
        ConnectionAborted = -103,

        /// <summary>
        /// A connection attempt failed.
        /// </summary>
        ConnectionFailed = -104,

        /// <summary>
        /// The host name could not be resolved.
        /// </summary>
        NameNotResolved = -105,

        /// <summary>
        /// The Internet connection has been lost.
        /// </summary>
        InternetDisconnected = -106,

        /// <summary>
        /// An SSL protocol error occurred.
        /// </summary>
        SslProtocolError = -107,

        /// <summary>
        /// The IP address or port number is invalid (e.g., cannot connect to the IP address 0 or the port 0).
        /// </summary>
        AddressInvalid = -108,

        /// <summary>
        /// The IP address is unreachable.  This usually means that there is no route to the specified host or network.
        /// </summary>
        AddressUnreachable = -109,

        /// <summary>
        /// The server requested a client certificate for SSL client authentication.
        /// </summary>
        SslClientAuthCertNeeded = -110,

        /// <summary>
        /// A tunnel connection through the proxy could not be established.
        /// </summary>
        TunnelConnectionFailed = -111,

        /// <summary>
        /// No SSL protocol versions are enabled.
        /// </summary>
        NoSslVersionsEnabled = -112,

        /// <summary>
        /// The client and server don't support a common SSL protocol version or cipher suite.
        /// </summary>
        SslVersionOrCipherMismatch = -113,

        /// <summary>
        /// The server requested a renegotiation (rehandshake).
        /// </summary>
        SslRenegotiationRequested = -114,

        /// <summary>
        /// The proxy requested authentication (for tunnel establishment) with an
        /// unsupported method.
        /// </summary>
        ProxyAuthUnsupported = -115,

        /// <summary>
        /// During SSL renegotiation (rehandshake), the server sent a certificate with
        /// an error.
        ///
        /// Note: this error is not in the -2xx range so that it won't be handled as a
        /// certificate error.
        /// </summary>
        CertErrorInSslRenegotiation = -116,

        /// <summary>
        /// The SSL handshake failed because of a bad or missing client certificate.
        /// </summary>
        BadSslClientAuthCert = -117,

        /// <summary>
        /// A connection attempt timed out.
        /// </summary>
        ConnectionTimedOut = -118,

        /// <summary>
        /// There are too many pending DNS resolves, so a request in the queue was
        /// aborted.
        /// </summary>
        HostResolverQueueTooLarge = -119,

        /// <summary>
        /// Failed establishing a connection to the SOCKS proxy server for a target host.
        /// </summary>
        SocksConnectionFailed = -120,

        /// <summary>
        /// The SOCKS proxy server failed establishing connection to the target host
        /// because that host is unreachable.
        /// </summary>
        SocksConnectionHostUnreachable = -121,

        /// <summary>
        /// The request to negotiate an alternate protocol failed.
        /// </summary>
        NpnNegotiationFailed = -122,

        /// <summary>
        /// The peer sent an SSL no_renegotiation alert message.
        /// </summary>
        SslNoRenegotiation = -123,

        /// <summary>
        /// Winsock sometimes reports more data written than passed.  This is probably
        /// due to a broken LSP.
        /// </summary>
        WinsockUnexpectedWrittenBytes = -124,

        /// <summary>
        /// An SSL peer sent us a fatal decompression_failure alert. This typically
        /// occurs when a peer selects DEFLATE compression in the mistaken belief that
        /// it supports it.
        /// </summary>
        SslDecompressionFailureAlert = -125,

        /// <summary>
        /// An SSL peer sent us a fatal bad_record_mac alert. This has been observed
        /// from servers with buggy DEFLATE support.
        /// </summary>
        SslBadRecordMacAlert = -126,

        /// <summary>
        /// The proxy requested authentication (for tunnel establishment).
        /// </summary>
        ProxyAuthRequested = -127,

        /// <summary>
        /// A known TLS strict server didn't offer the renegotiation extension.
        /// </summary>
        SslUnsafeNegotiation = -128,

        /// <summary>
        /// The SSL server attempted to use a weak ephemeral Diffie-Hellman key.
        /// </summary>
        SslWeakServerEphemeralDhKey = -129,

        /// <summary>
        /// Could not create a connection to the proxy server. An error occurred
        /// either in resolving its name, or in connecting a socket to it.
        /// Note that this does NOT include failures during the actual "CONNECT" method
        /// of an HTTP proxy.
        /// </summary>
        ProxyConnectionFailed = -130,

        /// <summary>
        /// A mandatory proxy configuration could not be used. Currently this means
        /// that a mandatory PAC script could not be fetched, parsed or executed.
        /// </summary>
        MandatoryProxyConfigurationFailed = -131,

        /// <summary>
        /// We've hit the max socket limit for the socket pool while preconnecting.  We
        /// don't bother trying to preconnect more sockets.
        /// </summary>
        PreconnectMaxSocketLimit = -133,

        /// <summary>
        /// The permission to use the SSL client certificate's private key was denied.
        /// </summary>
        SslClientAuthPrivateKeyAccessDenied = -134,

        /// <summary>
        /// The SSL client certificate has no private key.
        /// </summary>
        SslClientAuthCertNoPrivateKey = -135,

        /// <summary>
        /// The certificate presented by the HTTPS Proxy was invalid.
        /// </summary>
        ProxyCertificateInvalid = -136,

        /// <summary>
        /// An error occurred when trying to do a name resolution (DNS).
        /// </summary>
        NameResolutionFailed = -137,

        /// <summary>
        /// Permission to access the network was denied. This is used to distinguish
        /// errors that were most likely caused by a firewall from other access denied
        /// errors. See also ERR_ACCESS_DENIED.
        /// </summary>
        NetworkAccessDenied = -138,

        /// <summary>
        /// The request throttler module cancelled this request to avoid DDOS.
        /// </summary>
        TemporarilyThrottled = -139,

        /// <summary>
        /// A request to create an SSL tunnel connection through the HTTPS proxy
        /// received a non-200 (OK) and non-407 (Proxy Auth) response.  The response
        /// body might include a description of why the request failed.
        /// </summary>
        HttpsProxyTunnelResponse = -140,

        /// <summary>
        /// We were unable to sign the CertificateVerify data of an SSL client auth
        /// handshake with the client certificate's private key.
        ///
        /// Possible causes for this include the user implicitly or explicitly
        /// denying access to the private key, the private key may not be valid for
        /// signing, the key may be relying on a cached handle which is no longer
        /// valid, or the CSP won't allow arbitrary data to be signed.
        /// </summary>
        SslClientAuthSignatureFailed = -141,

        /// <summary>
        /// The message was too large for the transport.  (for example a UDP message
        /// which exceeds size threshold).
        /// </summary>
        MsgTooBig = -142,

        /// <summary>
        /// A SPDY session already exists, and should be used instead of this connection.
        /// </summary>
        SpdySessionAlreadyExists = -143,

        /// <summary>
        /// Websocket protocol error. Indicates that we are terminating the connection
        /// due to a malformed frame or other protocol violation.
        /// </summary>
        WsProtocolError = -145,

        /// <summary>
        /// Connection was aborted for switching to another ptotocol.
        /// WebSocket abort SocketStream connection when alternate protocol is found.
        /// </summary>
        ProtocolSwitched = -146,

        /// <summary>
        /// Returned when attempting to bind an address that is already in use.
        /// </summary>
        AddressInUse = -147,

        /// <summary>
        /// An operation failed because the SSL handshake has not completed.
        /// </summary>
        SslHandshakeNotCompleted = -148,

        /// <summary>
        /// SSL peer's public key is invalid.
        /// </summary>
        SslBadPeerPublicKey = -149,

        /// <summary>
        /// The certificate didn't match the built-in public key pins for the host name.
        /// The pins are set in net/http/transport_security_state.cc and require that
        /// one of a set of public keys exist on the path from the leaf to the root.
        /// </summary>
        SslPinnedKeyNotInCertChain = -150,

        /// <summary>
        /// Server request for client certificate did not contain any types we support.
        /// </summary>
        ClientAuthCertTypeUnsupported = -151,

        /// <summary>
        /// Server requested one type of cert, then requested a different type while the
        /// first was still being generated.
        /// </summary>
        OriginBoundCertGenerationTypeMismatch = -152,

        /// <summary>
        /// An SSL peer sent us a fatal decrypt_error alert. This typically occurs when
        /// a peer could not correctly verify a signature (in CertificateVerify or
        /// ServerKeyExchange) or validate a Finished message.
        /// </summary>
        SslDecryptErrorAlert = -153,

        /// <summary>
        /// There are too many pending WebSocketJob instances, so the new job was not
        /// pushed to the queue.
        /// </summary>
        WsThrottleQueueTooLarge = -154,

        /// <summary>
        /// There are too many active SocketStream instances, so the new connect request
        /// was rejected.
        /// </summary>
        TooManySocketStreams = -155,

        /// <summary>
        /// The SSL server certificate changed in a renegotiation.
        /// </summary>
        SslServerCertChanged = -156,

        /// <summary>
        /// The SSL server indicated that an unnecessary TLS version fallback was
        /// performed.
        /// </summary>
        SslInappropriateFallback = -157,

        /// <summary>
        /// Certificate Transparency: All Signed Certificate Timestamps failed to verify.
        /// </summary>
        CtNoSctsVerifiedOk = -158,

        /// <summary>
        /// The SSL server sent us a fatal unrecognized_name alert.
        /// </summary>
        SslUnrecognizedNameAlert = -159,

        /// <summary>
        /// Failed to set the socket's receive buffer size as requested.
        /// </summary>
        SocketSetReceiveBufferSizeError = -160,

        /// <summary>
        /// Failed to set the socket's send buffer size as requested.
        /// </summary>
        SocketSetSendBufferSizeError = -161,

        /// <summary>
        /// Failed to set the socket's receive buffer size as requested, despite success
        /// return code from setsockopt.
        /// </summary>
        SocketReceiveBufferSizeUnchangeable = -162,

        /// <summary>
        /// Failed to set the socket's send buffer size as requested, despite success
        /// return code from setsockopt.
        /// </summary>
        SocketSendBufferSizeUnchangeable = -163,

        /// <summary>
        /// Failed to import a client certificate from the platform store into the SSL
        /// library.
        /// </summary>
        SslClientAuthCertBadFormat = -164,

        /// <summary>
        /// The server responded with a certificate whose common name did not match the host name.
        /// This could mean:
        /// 1. An attacker has redirected our traffic to his server and is presenting a certificate
        /// for which he knows the private key.
        /// 2. The server is misconfigured and responding with the wrong cert.
        /// 3. The user is on a wireless network and is being redirected to the network's login page.
        /// 4. The OS has used a DNS search suffix and the server doesn't have a certificate for the
        /// abbreviated name in the address bar.
        /// </summary>
        CertCommonNameInvalid = -200,

        /// <summary>
        /// The server responded with a certificate that, by our clock, appears to either not yet be valid or to have expired.
        /// This could mean:
        /// 1. An attacker is presenting an old certificate for which he has managed to obtain the private key
        /// 2. The server is misconfigured and is not presenting a valid cert.
        /// 3. Our clock is wrong.
        /// </summary>
        CertDateInvalid = -201,

        /// <summary>
        /// The server responded with a certificate that is signed by an authority we don't trust.
        /// The could mean:
        /// 1. An attacker has substituted the real certificate for a cert that
        /// contains his public key and is signed by his cousin.
        /// 2. The server operator has a legitimate certificate from a CA we don't know about, but should trust.
        /// 3. The server is presenting a self-signed certificate, providing no defense against active attackers (but foiling passive attackers).
        /// </summary>
        CertAuthorityInvalid = -202,

        /// <summary>
        /// The server responded with a certificate that contains errors. This error is not recoverable.
        /// MSDN describes this error as follows:
        /// "The SSL certificate contains errors."
        /// NOTE: It's unclear how this differs from ERR_CERT_INVALID. For consistency,
        /// use that code instead of this one from now on.
        /// </summary>
        CertContainsErrors = -203,

        /// <summary>
        /// The certificate has no mechanism for determining if it is revoked.  In effect, this certificate cannot be revoked.
        /// </summary>
        CertNoRevocationMechanism = -204,

        /// <summary>
        /// Revocation information for the security certificate for this site is not available.
        /// This could mean:
        /// 1. An attacker has compromised the private key in the certificate and is blocking our attempt to
        /// find out that the cert was revoked.
        /// 2. The certificate is unrevoked, but the revocation server is busy or unavailable.
        /// </summary>
        CertUnableToCheckRevocation = -205,

        /// <summary>
        /// The server responded with a certificate has been revoked.
        /// We have the capability to ignore this error, but it is probably not the thing to do.
        /// </summary>
        CertRevoked = -206,

        /// <summary>
        /// The server responded with a certificate that is invalid. This error is not recoverable.
        /// </summary>
        CertInvalid = -207,

        /// <summary>
        /// The server responded with a certificate that is signed using a weak
        /// signature algorithm.
        /// </summary>
        CertWeakSignatureAlgorithm = -208,

        /// <summary>
        /// The host name specified in the certificate is not unique.
        /// </summary>
        CertNonUniqueName = -210,

        /// <summary>
        /// The server responded with a certificate that contains a weak key (e.g.
        /// a too-small RSA key).
        /// </summary>
        CertWeakKey = -211,

        /// <summary>
        /// The certificate claimed DNS names that are in violation of name constraints.
        /// </summary>
        CertNameConstraintViolation = -212,

        /// <summary>
        /// Add new certificate error codes here.
        ///
        /// Update the value of CERT_END whenever you add a new certificate error
        /// code.
        ///
        /// The value immediately past the last certificate error code.
        /// </summary>
        CertEnd = -213,

        /// <summary>
        /// The URL is invalid.
        /// </summary>
        InvalidUrl = -300,

        /// <summary>
        /// The scheme of the URL is disallowed.
        /// </summary>
        DisallowedUrlScheme = -301,

        /// <summary>
        /// The scheme of the URL is unknown.
        /// </summary>
        UnknownUrlScheme = -302,

        /// <summary>
        /// Attempting to load an URL resulted in too many redirects.
        /// </summary>
        TooManyRedirects = -310,

        /// <summary>
        /// Attempting to load an URL resulted in an unsafe redirect (e.g., a redirect to file:// is considered unsafe).
        /// </summary>
        UnsafeRedirect = -311,

        /// <summary>
        /// Attempting to load an URL with an unsafe port number.  These are port
        /// numbers that correspond to services, which are not robust to spurious input
        /// that may be constructed as a result of an allowed web construct (e.g., HTTP
        /// looks a lot like SMTP, so form submission to port 25 is denied).
        /// </summary>
        UnsafePort = -312,

        /// <summary>
        /// The server's response was invalid.
        /// </summary>
        InvalidResponse = -320,

        /// <summary>
        /// Error in chunked transfer encoding.
        /// </summary>
        InvalidChunkedEncoding = -321,

        /// <summary>
        /// The server did not support the request method.
        /// </summary>
        MethodNotSupported = -322,

        /// <summary>
        /// The response was 407 (Proxy Authentication Required), yet we did not send the request to a proxy.
        /// </summary>
        UnexpectedProxyAuth = -323,

        /// <summary>
        /// The server closed the connection without sending any data.
        /// </summary>
        EmptyResponse = -324,

        /// <summary>
        /// The headers section of the response is too large.
        /// </summary>
        ResponseHeadersTooBig = -325,

        /// <summary>
        /// The PAC requested by HTTP did not have a valid status code (non-200).
        /// </summary>
        PacStatusNotOk = -326,

        /// <summary>
        /// The evaluation of the PAC script failed.
        /// </summary>
        PacScriptFailed = -327,

        /// <summary>
        /// The response was 416 (Requested range not satisfiable) and the server cannot
        /// satisfy the range requested.
        /// </summary>
        RequestRangeNotSatisfiable = -328,

        /// <summary>
        /// The identity used for authentication is invalid.
        /// </summary>
        MalformedIdentity = -329,

        /// <summary>
        /// Content decoding of the response body failed.
        /// </summary>
        ContentDecodingFailed = -330,

        /// <summary>
        /// An operation could not be completed because all network IO
        /// is suspended.
        /// </summary>
        NetworkIoSuspended = -331,

        /// <summary>
        /// FLIP data received without receiving a SYN_REPLY on the stream.
        /// </summary>
        SynReplyNotReceived = -332,

        /// <summary>
        /// Converting the response to target encoding failed.
        /// </summary>
        EncodingConversionFailed = -333,

        /// <summary>
        /// The server sent an FTP directory listing in a format we do not understand.
        /// </summary>
        UnrecognizedFtpDirectoryListingFormat = -334,

        /// <summary>
        /// Attempted use of an unknown SPDY stream id.
        /// </summary>
        InvalidSpdyStream = -335,

        /// <summary>
        /// There are no supported proxies in the provided list.
        /// </summary>
        NoSupportedProxies = -336,

        /// <summary>
        /// There is a SPDY protocol error.
        /// </summary>
        SpdyProtocolError = -337,

        /// <summary>
        /// Credentials could not be established during HTTP Authentication.
        /// </summary>
        InvalidAuthCredentials = -338,

        /// <summary>
        /// An HTTP Authentication scheme was tried which is not supported on this
        /// machine.
        /// </summary>
        UnsupportedAuthScheme = -339,

        /// <summary>
        /// Detecting the encoding of the response failed.
        /// </summary>
        EncodingDetectionFailed = -340,

        /// <summary>
        /// (GSSAPI) No Kerberos credentials were available during HTTP Authentication.
        /// </summary>
        MissingAuthCredentials = -341,

        /// <summary>
        /// An unexpected, but documented, SSPI or GSSAPI status code was returned.
        /// </summary>
        UnexpectedSecurityLibraryStatus = -342,

        /// <summary>
        /// The environment was not set up correctly for authentication (for
        /// example, no KDC could be found or the principal is unknown.
        /// </summary>
        MisconfiguredAuthEnvironment = -343,

        /// <summary>
        /// An undocumented SSPI or GSSAPI status code was returned.
        /// </summary>
        UndocumentedSecurityLibraryStatus = -344,

        /// <summary>
        /// The HTTP response was too big to drain.
        /// </summary>
        ResponseBodyTooBigToDrain = -345,

        /// <summary>
        /// The HTTP response contained multiple distinct Content-Length headers.
        /// </summary>
        ResponseHeadersMultipleContentLength = -346,

        /// <summary>
        /// SPDY Headers have been received, but not all of them - status or version
        /// headers are missing, so we're expecting additional frames to complete them.
        /// </summary>
        IncompleteSpdyHeaders = -347,

        /// <summary>
        /// No PAC URL configuration could be retrieved from DHCP. This can indicate
        /// either a failure to retrieve the DHCP configuration, or that there was no
        /// PAC URL configured in DHCP.
        /// </summary>
        PacNotInDhcp = -348,

        /// <summary>
        /// The HTTP response contained multiple Content-Disposition headers.
        /// </summary>
        ResponseHeadersMultipleContentDisposition = -349,

        /// <summary>
        /// The HTTP response contained multiple Location headers.
        /// </summary>
        ResponseHeadersMultipleLocation = -350,

        /// <summary>
        /// SPDY server refused the stream. Client should retry. This should never be a
        /// user-visible error.
        /// </summary>
        SpdyServerRefusedStream = -351,

        /// <summary>
        /// SPDY server didn't respond to the PING message.
        /// </summary>
        SpdyPingFailed = -352,

        /// <summary>
        /// The HTTP response body transferred fewer bytes than were advertised by the
        /// Content-Length header when the connection is closed.
        /// </summary>
        ContentLengthMismatch = -354,

        /// <summary>
        /// The HTTP response body is transferred with Chunked-Encoding, but the
        /// terminating zero-length chunk was never sent when the connection is closed.
        /// </summary>
        IncompleteChunkedEncoding = -355,

        /// <summary>
        /// There is a QUIC protocol error.
        /// </summary>
        QuicProtocolError = -356,

        /// <summary>
        /// The HTTP headers were truncated by an EOF.
        /// </summary>
        ResponseHeadersTruncated = -357,

        /// <summary>
        /// The QUIC crytpo handshake failed.  This means that the server was unable
        /// to read any requests sent, so they may be resent.
        /// </summary>
        QuicHandshakeFailed = -358,

        /// <summary>
        /// An https resource was requested over an insecure QUIC connection.
        /// </summary>
        RequestForSecureResourceOverInsecureQuic = -359,

        /// <summary>
        /// Transport security is inadequate for the SPDY version.
        /// </summary>
        SpdyInadequateTransportSecurity = -360,

        /// <summary>
        /// The peer violated SPDY flow control.
        /// </summary>
        SpdyFlowControlError = -361,

        /// <summary>
        /// The peer sent an improperly sized SPDY frame.
        /// </summary>
        SpdyFrameSizeError = -362,

        /// <summary>
        /// Decoding or encoding of compressed SPDY headers failed.
        /// </summary>
        SpdyCompressionError = -363,

        /// <summary>
        /// Proxy Auth Requested without a valid Client Socket Handle.
        /// </summary>
        ProxyAuthRequestedWithNoConnection = -364,

        /// <summary>
        /// The cache does not have the requested entry.
        /// </summary>
        CacheMiss = -400,

        /// <summary>
        /// Unable to read from the disk cache.
        /// </summary>
        CacheReadFailure = -401,

        /// <summary>
        /// Unable to write to the disk cache.
        /// </summary>
        CacheWriteFailure = -402,

        /// <summary>
        /// The operation is not supported for this entry.
        /// </summary>
        CacheOperationNotSupported = -403,

        /// <summary>
        /// The disk cache is unable to open this entry.
        /// </summary>
        CacheOpenFailure = -404,

        /// <summary>
        /// The disk cache is unable to create this entry.
        /// </summary>
        CacheCreateFailure = -405,

        /// <summary>
        /// Multiple transactions are racing to create disk cache entries. This is an
        /// internal error returned from the HttpCache to the HttpCacheTransaction that
        /// tells the transaction to restart the entry-creation logic because the state
        /// of the cache has changed.
        /// </summary>
        CacheRace = -406,

        /// <summary>
        /// The cache was unable to read a checksum record on an entry. This can be
        /// returned from attempts to read from the cache. It is an internal error,
        /// returned by the SimpleCache backend, but not by any URLRequest methods
        /// or members.
        /// </summary>
        CacheChecksumReadFailure = -407,

        /// <summary>
        /// The cache found an entry with an invalid checksum. This can be returned from
        /// attempts to read from the cache. It is an internal error, returned by the
        /// SimpleCache backend, but not by any URLRequest methods or members.
        /// </summary>
        CacheChecksumMismatch = -408,

        /// <summary>
        /// Internal error code for the HTTP cache. The cache lock timeout has fired.
        /// </summary>
        CacheLockTimeout = -409,

        /// <summary>
        /// The server's response was insecure (e.g. there was a cert error).
        /// </summary>
        InsecureResponse = -501,

        /// <summary>
        /// The server responded to a &lt;keygen&gt; with a generated client cert that we
        /// don't have the matching private key for.
        /// </summary>
        NoPrivateKeyForCert = -502,

        /// <summary>
        /// An error adding to the OS certificate database (e.g. OS X Keychain).
        /// </summary>
        AddUserCertFailed = -503,

        /// <summary>
        /// A generic error for failed FTP control connection command.
        /// If possible, please use or add a more specific error code.
        /// </summary>
        FtpFailed = -601,

        /// <summary>
        /// The server cannot fulfill the request at this point. This is a temporary
        /// error.
        /// FTP response code 421.
        /// </summary>
        FtpServiceUnavailable = -602,

        /// <summary>
        /// The server has aborted the transfer.
        /// FTP response code 426.
        /// </summary>
        FtpTransferAborted = -603,

        /// <summary>
        /// The file is busy, or some other temporary error condition on opening
        /// the file.
        /// FTP response code 450.
        /// </summary>
        FtpFileBusy = -604,

        /// <summary>
        /// Server rejected our command because of syntax errors.
        /// FTP response codes 500, 501.
        /// </summary>
        FtpSyntaxError = -605,

        /// <summary>
        /// Server does not support the command we issued.
        /// FTP response codes 502, 504.
        /// </summary>
        FtpCommandNotSupported = -606,

        /// <summary>
        /// Server rejected our command because we didn't issue the commands in right
        /// order.
        /// FTP response code 503.
        /// </summary>
        FtpBadCommandSequence = -607,

        /// <summary>
        /// PKCS #12 import failed due to incorrect password.
        /// </summary>
        Pkcs12ImportBadPassword = -701,

        /// <summary>
        /// PKCS #12 import failed due to other error.
        /// </summary>
        Pkcs12ImportFailed = -702,

        /// <summary>
        /// CA import failed - not a CA cert.
        /// </summary>
        ImportCaCertNotCa = -703,

        /// <summary>
        /// Import failed - certificate already exists in database.
        /// Note it's a little weird this is an error but reimporting a PKCS12 is ok
        /// (no-op).  That's how Mozilla does it, though.
        /// </summary>
        ImportCertAlreadyExists = -704,

        /// <summary>
        /// CA import failed due to some other error.
        /// </summary>
        ImportCaCertFailed = -705,

        /// <summary>
        /// Server certificate import failed due to some internal error.
        /// </summary>
        ImportServerCertFailed = -706,

        /// <summary>
        /// PKCS #12 import failed due to invalid MAC.
        /// </summary>
        Pkcs12ImportInvalidMac = -707,

        /// <summary>
        /// PKCS #12 import failed due to invalid/corrupt file.
        /// </summary>
        Pkcs12ImportInvalidFile = -708,

        /// <summary>
        /// PKCS #12 import failed due to unsupported features.
        /// </summary>
        Pkcs12ImportUnsupported = -709,

        /// <summary>
        /// Key generation failed.
        /// </summary>
        KeyGenerationFailed = -710,

        /// <summary>
        /// Server-bound certificate generation failed.
        /// </summary>
        OriginBoundCertGenerationFailed = -711,

        /// <summary>
        /// Failure to export private key.
        /// </summary>
        PrivateKeyExportFailed = -712,

        /// <summary>
        /// Self-signed certificate generation failed.
        /// </summary>
        SelfSignedCertGenerationFailed = -713,

        /// <summary>
        /// The certificate database changed in some way.
        /// </summary>
        CertDatabaseChanged = -714,

        /// <summary>
        /// Failure to import Channel ID.
        /// </summary>
        ChannelIdImportFailed = -715,

        /// <summary>
        /// DNS resolver received a malformed response.
        /// </summary>
        DnsMalformedResponse = -800,

        /// <summary>
        /// DNS server requires TCP
        /// </summary>
        DnsServerRequiresTcp = -801,

        /// <summary>
        /// DNS server failed.  This error is returned for all of the following
        /// error conditions:
        /// 1 - Format error - The name server was unable to interpret the query.
        /// 2 - Server failure - The name server was unable to process this query
        ///     due to a problem with the name server.
        /// 4 - Not Implemented - The name server does not support the requested
        ///     kind of query.
        /// 5 - Refused - The name server refuses to perform the specified
        ///     operation for policy reasons.
        /// </summary>
        DnsServerFailed = -802,

        /// <summary>
        /// DNS transaction timed out.
        /// </summary>
        DnsTimedOut = -803,

        /// <summary>
        /// The entry was not found in cache, for cache-only lookups.
        /// </summary>
        DnsCacheMiss = -804,

        /// <summary>
        /// Suffix search list rules prevent resolution of the given host name.
        /// </summary>
        DnsSearchEmpty = -805,

        /// <summary>
        /// Failed to sort addresses according to RFC3484.
        /// </summary>
        DnsSortError = -806
    };
}
