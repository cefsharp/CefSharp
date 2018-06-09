// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Linq;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Used in conjunction with CefSettings.RegisterScheme to register a scheme.
    /// You can register your own custom scheme e.g. custom:// or use an existing
    /// scheme e.g. http://
    /// </summary>
    public sealed class CefCustomScheme
    {
        /// <summary>
        /// Schema Name e.g. custom
        /// </summary>
        public string SchemeName { get; set; }

        /// <summary>
        /// Optional Domain Name. An empty value for a standard scheme
        /// will cause the factory to match all domain names. The |domain_name| value
        /// will be ignored for non-standard schemes.
        /// </summary>
        public string DomainName { get; set; }
        
        /// <summary>
        /// If true the scheme will be treated as a standard scheme.
        /// Standard schemes are subject to URL canonicalization and parsing rules as
        /// defined in the Common Internet Scheme Syntax RFC 1738 Section 3.1 available
        /// at http://www.ietf.org/rfc/rfc1738.txt
        ///
        /// In particular, the syntax for standard scheme URLs must be of the form:
        /// <pre>
        ///  [scheme]://[username]:[password]@[host]:[port]/[url-path]
        /// </pre>
        /// Standard scheme URLs must have a host component that is a fully qualified
        /// domain name as defined in Section 3.5 of RFC 1034 [13] and Section 2.1 of
        /// RFC 1123. These URLs will be canonicalized to "scheme://host/path" in the
        /// simplest case and "scheme://username:password@host:port/path" in the most
        /// explicit case. For example, "scheme:host/path" and "scheme:///host/path"
        /// will both be canonicalized to "scheme://host/path". The origin of a
        /// standard scheme URL is the combination of scheme, host and port (i.e.,
        /// "scheme://host:port" in the most explicit case).
        ///
        /// For non-standard scheme URLs only the "scheme:" component is parsed and
        /// canonicalized. The remainder of the URL will be passed to the handler
        /// as-is. For example, "scheme:///some%20text" will remain the same.
        /// Non-standard scheme URLs cannot be used as a target for form submission.
        /// </summary>
        public bool IsStandard { get; set; }
        
        /// <summary>
        /// If true the scheme will be treated as local (i.e. with the
        /// same security rules as those applied to "file" URLs). Normal pages cannot
        /// link to or access local URLs. Also, by default, local URLs can only perform
        /// XMLHttpRequest calls to the same URL (origin + path) that originated the
        /// request. To allow XMLHttpRequest calls from a local URL to other URLs with
        /// the same origin set the CefSettings.file_access_from_file_urls_allowed
        /// value to true. To allow XMLHttpRequest calls from a local URL to all
        /// origins set the CefSettings.universal_access_from_file_urls_allowed value
        /// to true.
        /// </summary>
        public bool IsLocal { get; set; }
        
        /// <summary>
        /// If true the scheme will be treated as display-isolated.
        /// This means that pages cannot display these URLs unless they are
        /// from the same scheme. For example, pages in another origin cannot create
        /// iframes or hyperlinks to URLs with this scheme.
        /// </summary>
        public bool IsDisplayIsolated { get; set; }

        /// <summary>
        /// If true the scheme will be treated with the same security
        /// rules as those applied to "https" URLs. For example, loading this scheme
        /// from other secure schemes will not trigger mixed content warnings.
        /// </summary>
        public bool IsSecure { get; set; }

        /// <summary>
        /// If true the scheme can be sent CORS requests.
        /// This value should be true in most cases where IsStandard is true.
        /// </summary>
        public bool IsCorsEnabled { get; set; }

        /// <summary>
        /// If true the scheme can bypass Content-Security-Policy(CSP) checks. 
        /// This value should be false in most cases where IsStandard is true.
        /// </summary>
        public bool IsCSPBypassing { get; set; }

        /// <summary>
        /// Factory Class that creates <see cref="IResourceHandler"/> instances
        /// for handling scheme requests.
        /// </summary>
        public ISchemeHandlerFactory SchemeHandlerFactory { get; set; }

        /// <summary>
        /// Creates a new CefCustomScheme.
        /// </summary>
        public CefCustomScheme()
        {
            IsStandard = true;
            IsLocal = false;
            IsDisplayIsolated = false;
            IsSecure = true;
            IsCorsEnabled = true;
            IsCSPBypassing = false;
        }

        /// <summary>
        /// Method used internally
        /// </summary>
        /// <param name="args">command line arguments</param>
        /// <returns>list of scheme objects</returns>
        public static List<CefCustomScheme> ParseCommandLineArguments(IEnumerable<string> args)
        {
            var schemes = args.GetArgumentValue(CefSharpArguments.CustomSchemeArgument);
            var customSchemes = new List<CefCustomScheme>();

            if (!string.IsNullOrEmpty(schemes))
            {
                schemes.Split(';').ToList().ForEach(x =>
                {
                    var tokens = x.Split('|');
                    var customScheme = new CefCustomScheme
                    {
                        SchemeName = tokens[0],
                        IsStandard = tokens[1] == "T",
                        IsLocal = tokens[2] == "T",
                        IsDisplayIsolated = tokens[3] == "T",
                        IsSecure = tokens[4] == "T",
                        IsCorsEnabled = tokens[5] == "T",
                        IsCSPBypassing = tokens[6] == "T"
                    };
                    customSchemes.Add(customScheme);
                });
            }

            return customSchemes;
        }
    }
}
