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
        private CefSharp.DevTools.IDevToolsClient _client;
        public IO(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateClose(string handle);
        /// <summary>
        /// Close the stream, discard any temporary backing storage.
        /// </summary>
        /// <param name = "handle">Handle of the stream to close.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CloseAsync(string handle)
        {
            ValidateClose(handle);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("handle", handle);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("IO.close", dict);
            return methodResult;
        }

        partial void ValidateRead(string handle, int? offset = null, int? size = null);
        /// <summary>
        /// Read a chunk of the stream
        /// </summary>
        /// <param name = "handle">Handle of the stream to read.</param>
        /// <param name = "offset">Seek to the specified offset before reading (if not specificed, proceed with offsetfollowing the last read). Some types of streams may only support sequential reads.</param>
        /// <param name = "size">Maximum number of bytes to read (left upon the agent discretion if not specified).</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;ReadResponse&gt;</returns>
        public async System.Threading.Tasks.Task<ReadResponse> ReadAsync(string handle, int? offset = null, int? size = null)
        {
            ValidateRead(handle, offset, size);
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

        partial void ValidateResolveBlob(string objectId);
        /// <summary>
        /// Return UUID of Blob object specified by a remote object id.
        /// </summary>
        /// <param name = "objectId">Object id of a Blob object wrapper.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;ResolveBlobResponse&gt;</returns>
        public async System.Threading.Tasks.Task<ResolveBlobResponse> ResolveBlobAsync(string objectId)
        {
            ValidateResolveBlob(objectId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("IO.resolveBlob", dict);
            return methodResult.DeserializeJson<ResolveBlobResponse>();
        }
    }
}