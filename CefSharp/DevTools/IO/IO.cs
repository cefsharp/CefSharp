// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IO
{
    using System.Linq;

    /// <summary>
    /// Input/Output operations for streams produced by DevTools.
    /// </summary>
    public partial class IO : DevToolsDomainBase
    {
        public IO(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Close the stream, discard any temporary backing storage.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CloseAsync(string handle)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("handle", handle);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("IO.close", dict);
            return methodResult;
        }

        /// <summary>
        /// Read a chunk of the stream
        /// </summary>
        public async System.Threading.Tasks.Task<ReadResponse> ReadAsync(string handle, int? offset = null, int? size = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("handle", handle);
            if (offset.HasValue)
            {
                dict.Add("offset", offset.Value);
            }

            if (size.HasValue)
            {
                dict.Add("size", size.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("IO.read", dict);
            return methodResult.DeserializeJson<ReadResponse>();
        }

        /// <summary>
        /// Return UUID of Blob object specified by a remote object id.
        /// </summary>
        public async System.Threading.Tasks.Task<ResolveBlobResponse> ResolveBlobAsync(string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("IO.resolveBlob", dict);
            return methodResult.DeserializeJson<ResolveBlobResponse>();
        }
    }
}