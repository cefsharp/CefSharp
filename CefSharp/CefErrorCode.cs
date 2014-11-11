// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Lists some of the error codes that can be reported by CEF.
    /// </summary>
    public enum CefErrorCode
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        None = 0,

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
        /// The value immediately past the last certificate error code.
        /// </summary>
        CertEnd = -208,

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
        /// The cache does not have the requested entry.
        /// </summary>
        CacheMiss = -400,

        /// <summary>
        /// The server's response was insecure (e.g. there was a cert error).
        /// </summary>
        InsecureResponse = -501
    };
}
