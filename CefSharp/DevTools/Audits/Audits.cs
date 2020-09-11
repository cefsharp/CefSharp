// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    using System.Linq;

    /// <summary>
    /// Audits domain allows investigation of page violations and possible improvements.
    /// </summary>
    public partial class Audits : DevToolsDomainBase
    {
        public Audits(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Returns the response body and size if it were re-encoded with the specified settings. Only
        /// applies to images.
        /// </summary>
        public async System.Threading.Tasks.Task<GetEncodedResponseResponse> GetEncodedResponseAsync(string requestId, string encoding, long? quality = null, bool? sizeOnly = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            dict.Add("encoding", encoding);
            if (quality.HasValue)
            {
                dict.Add("quality", quality.Value);
            }

            if (sizeOnly.HasValue)
            {
                dict.Add("sizeOnly", sizeOnly.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Audits.getEncodedResponse", dict);
            return methodResult.DeserializeJson<GetEncodedResponseResponse>();
        }

        /// <summary>
        /// Disables issues domain, prevents further issues from being reported to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Audits.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables issues domain, sends the issues collected so far to the client by means of the
        /// `issueAdded` event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Audits.enable", dict);
            return methodResult;
        }
    }
}