// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IO
{
    /// <summary>
    /// Input/Output operations for streams produced by DevTools.
    /// </summary>
    public partial class IO
    {
        public IO(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Close the stream, discard any temporary backing storage.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Close(string handle)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"handle", handle}, };
            var result = await _client.ExecuteDevToolsMethodAsync("IO.Close", dict);
            return result;
        }

        /// <summary>
        /// Read a chunk of the stream
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Read(string handle, int offset, int size)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"handle", handle}, {"offset", offset}, {"size", size}, };
            var result = await _client.ExecuteDevToolsMethodAsync("IO.Read", dict);
            return result;
        }

        /// <summary>
        /// Return UUID of Blob object specified by a remote object id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ResolveBlob(string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"objectId", objectId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("IO.ResolveBlob", dict);
            return result;
        }
    }
}