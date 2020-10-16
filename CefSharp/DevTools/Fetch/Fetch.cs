// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Fetch
{
    using System.Linq;

    /// <summary>
    /// A domain for letting clients substitute browser's network layer with client code.
    /// </summary>
    public partial class Fetch : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public Fetch(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Disables the fetch domain.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.disable", dict);
            return methodResult;
        }

        partial void ValidateEnable(System.Collections.Generic.IList<CefSharp.DevTools.Fetch.RequestPattern> patterns = null, bool? handleAuthRequests = null);
        /// <summary>
        /// Enables issuing of requestPaused events. A request will be paused until client
        /// calls one of failRequest, fulfillRequest or continueRequest/continueWithAuth.
        /// </summary>
        /// <param name = "patterns">If specified, only requests matching any of these patterns will producefetchRequested event and will be paused until clients response. If not set,all requests will be affected.</param>
        /// <param name = "handleAuthRequests">If true, authRequired events will be issued and requests will be pausedexpecting a call to continueWithAuth.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync(System.Collections.Generic.IList<CefSharp.DevTools.Fetch.RequestPattern> patterns = null, bool? handleAuthRequests = null)
        {
            ValidateEnable(patterns, handleAuthRequests);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if ((patterns) != (null))
            {
                dict.Add("patterns", patterns.Select(x => x.ToDictionary()));
            }

            if (handleAuthRequests.HasValue)
            {
                dict.Add("handleAuthRequests", handleAuthRequests.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.enable", dict);
            return methodResult;
        }

        partial void ValidateFailRequest(string requestId, CefSharp.DevTools.Network.ErrorReason errorReason);
        /// <summary>
        /// Causes the request to fail with specified reason.
        /// </summary>
        /// <param name = "requestId">An id the client received in requestPaused event.</param>
        /// <param name = "errorReason">Causes the request to fail with the given reason.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> FailRequestAsync(string requestId, CefSharp.DevTools.Network.ErrorReason errorReason)
        {
            ValidateFailRequest(requestId, errorReason);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            dict.Add("errorReason", this.EnumToString(errorReason));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.failRequest", dict);
            return methodResult;
        }

        partial void ValidateFulfillRequest(string requestId, int responseCode, System.Collections.Generic.IList<CefSharp.DevTools.Fetch.HeaderEntry> responseHeaders = null, byte[] binaryResponseHeaders = null, byte[] body = null, string responsePhrase = null);
        /// <summary>
        /// Provides response to the request.
        /// </summary>
        /// <param name = "requestId">An id the client received in requestPaused event.</param>
        /// <param name = "responseCode">An HTTP response code.</param>
        /// <param name = "responseHeaders">Response headers.</param>
        /// <param name = "binaryResponseHeaders">Alternative way of specifying response headers as a \0-separatedseries of name: value pairs. Prefer the above method unless youneed to represent some non-UTF8 values that can't be transmittedover the protocol as text.</param>
        /// <param name = "body">A response body.</param>
        /// <param name = "responsePhrase">A textual representation of responseCode.If absent, a standard phrase matching responseCode is used.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> FulfillRequestAsync(string requestId, int responseCode, System.Collections.Generic.IList<CefSharp.DevTools.Fetch.HeaderEntry> responseHeaders = null, byte[] binaryResponseHeaders = null, byte[] body = null, string responsePhrase = null)
        {
            ValidateFulfillRequest(requestId, responseCode, responseHeaders, binaryResponseHeaders, body, responsePhrase);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            dict.Add("responseCode", responseCode);
            if ((responseHeaders) != (null))
            {
                dict.Add("responseHeaders", responseHeaders.Select(x => x.ToDictionary()));
            }

            if ((binaryResponseHeaders) != (null))
            {
                dict.Add("binaryResponseHeaders", ToBase64String(binaryResponseHeaders));
            }

            if ((body) != (null))
            {
                dict.Add("body", ToBase64String(body));
            }

            if (!(string.IsNullOrEmpty(responsePhrase)))
            {
                dict.Add("responsePhrase", responsePhrase);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.fulfillRequest", dict);
            return methodResult;
        }

        partial void ValidateContinueRequest(string requestId, string url = null, string method = null, string postData = null, System.Collections.Generic.IList<CefSharp.DevTools.Fetch.HeaderEntry> headers = null);
        /// <summary>
        /// Continues the request, optionally modifying some of its parameters.
        /// </summary>
        /// <param name = "requestId">An id the client received in requestPaused event.</param>
        /// <param name = "url">If set, the request url will be modified in a way that's not observable by page.</param>
        /// <param name = "method">If set, the request method is overridden.</param>
        /// <param name = "postData">If set, overrides the post data in the request.</param>
        /// <param name = "headers">If set, overrides the request headers.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ContinueRequestAsync(string requestId, string url = null, string method = null, string postData = null, System.Collections.Generic.IList<CefSharp.DevTools.Fetch.HeaderEntry> headers = null)
        {
            ValidateContinueRequest(requestId, url, method, postData, headers);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            if (!(string.IsNullOrEmpty(url)))
            {
                dict.Add("url", url);
            }

            if (!(string.IsNullOrEmpty(method)))
            {
                dict.Add("method", method);
            }

            if (!(string.IsNullOrEmpty(postData)))
            {
                dict.Add("postData", postData);
            }

            if ((headers) != (null))
            {
                dict.Add("headers", headers.Select(x => x.ToDictionary()));
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.continueRequest", dict);
            return methodResult;
        }

        partial void ValidateContinueWithAuth(string requestId, CefSharp.DevTools.Fetch.AuthChallengeResponse authChallengeResponse);
        /// <summary>
        /// Continues a request supplying authChallengeResponse following authRequired event.
        /// </summary>
        /// <param name = "requestId">An id the client received in authRequired event.</param>
        /// <param name = "authChallengeResponse">Response to  with an authChallenge.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ContinueWithAuthAsync(string requestId, CefSharp.DevTools.Fetch.AuthChallengeResponse authChallengeResponse)
        {
            ValidateContinueWithAuth(requestId, authChallengeResponse);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            dict.Add("authChallengeResponse", authChallengeResponse.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.continueWithAuth", dict);
            return methodResult;
        }

        partial void ValidateGetResponseBody(string requestId);
        /// <summary>
        /// Causes the body of the response to be received from the server and
        /// returned as a single string. May only be issued for a request that
        /// is paused in the Response stage and is mutually exclusive with
        /// takeResponseBodyForInterceptionAsStream. Calling other methods that
        /// affect the request or disabling fetch domain before body is received
        /// results in an undefined behavior.
        /// </summary>
        /// <param name = "requestId">Identifier for the intercepted request to get body for.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetResponseBodyResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetResponseBodyResponse> GetResponseBodyAsync(string requestId)
        {
            ValidateGetResponseBody(requestId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.getResponseBody", dict);
            return methodResult.DeserializeJson<GetResponseBodyResponse>();
        }

        partial void ValidateTakeResponseBodyAsStream(string requestId);
        /// <summary>
        /// Returns a handle to the stream representing the response body.
        /// The request must be paused in the HeadersReceived stage.
        /// Note that after this command the request can't be continued
        /// as is -- client either needs to cancel it or to provide the
        /// response body.
        /// The stream only supports sequential read, IO.read will fail if the position
        /// is specified.
        /// This method is mutually exclusive with getResponseBody.
        /// Calling other methods that affect the request or disabling fetch
        /// domain before body is received results in an undefined behavior.
        /// </summary>
        /// <param name = "requestId">requestId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;TakeResponseBodyAsStreamResponse&gt;</returns>
        public async System.Threading.Tasks.Task<TakeResponseBodyAsStreamResponse> TakeResponseBodyAsStreamAsync(string requestId)
        {
            ValidateTakeResponseBodyAsStream(requestId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.takeResponseBodyAsStream", dict);
            return methodResult.DeserializeJson<TakeResponseBodyAsStreamResponse>();
        }
    }
}