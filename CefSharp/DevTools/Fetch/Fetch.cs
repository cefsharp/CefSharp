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
        public Fetch(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Disables the fetch domain.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables issuing of requestPaused events. A request will be paused until client
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync(System.Collections.Generic.IList<CefSharp.DevTools.Fetch.RequestPattern> patterns = null, bool? handleAuthRequests = null)
        {
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

        /// <summary>
        /// Causes the request to fail with specified reason.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> FailRequestAsync(string requestId, CefSharp.DevTools.Network.ErrorReason errorReason)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            dict.Add("errorReason", this.EnumToString(errorReason));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.failRequest", dict);
            return methodResult;
        }

        /// <summary>
        /// Provides response to the request.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> FulfillRequestAsync(string requestId, int responseCode, System.Collections.Generic.IList<CefSharp.DevTools.Fetch.HeaderEntry> responseHeaders = null, byte[] binaryResponseHeaders = null, byte[] body = null, string responsePhrase = null)
        {
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

        /// <summary>
        /// Continues the request, optionally modifying some of its parameters.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ContinueRequestAsync(string requestId, string url = null, string method = null, string postData = null, System.Collections.Generic.IList<CefSharp.DevTools.Fetch.HeaderEntry> headers = null)
        {
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

        /// <summary>
        /// Continues a request supplying authChallengeResponse following authRequired event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ContinueWithAuthAsync(string requestId, CefSharp.DevTools.Fetch.AuthChallengeResponse authChallengeResponse)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            dict.Add("authChallengeResponse", authChallengeResponse.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.continueWithAuth", dict);
            return methodResult;
        }

        /// <summary>
        /// Causes the body of the response to be received from the server and
        public async System.Threading.Tasks.Task<GetResponseBodyResponse> GetResponseBodyAsync(string requestId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.getResponseBody", dict);
            return methodResult.DeserializeJson<GetResponseBodyResponse>();
        }

        /// <summary>
        /// Returns a handle to the stream representing the response body.
        public async System.Threading.Tasks.Task<TakeResponseBodyAsStreamResponse> TakeResponseBodyAsStreamAsync(string requestId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Fetch.takeResponseBodyAsStream", dict);
            return methodResult.DeserializeJson<TakeResponseBodyAsStreamResponse>();
        }
    }
}