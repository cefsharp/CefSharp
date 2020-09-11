// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.ApplicationCache
{
    using System.Linq;

    /// <summary>
    /// ApplicationCache
    /// </summary>
    public partial class ApplicationCache : DevToolsDomainBase
    {
        public ApplicationCache(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Enables application cache domain notifications.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ApplicationCache.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns relevant application cache data for the document in given frame.
        /// </summary>
        public async System.Threading.Tasks.Task<GetApplicationCacheForFrameResponse> GetApplicationCacheForFrameAsync(string frameId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ApplicationCache.getApplicationCacheForFrame", dict);
            return methodResult.DeserializeJson<GetApplicationCacheForFrameResponse>();
        }

        /// <summary>
        /// Returns array of frame identifiers with manifest urls for each frame containing a document
        public async System.Threading.Tasks.Task<GetFramesWithManifestsResponse> GetFramesWithManifestsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ApplicationCache.getFramesWithManifests", dict);
            return methodResult.DeserializeJson<GetFramesWithManifestsResponse>();
        }

        /// <summary>
        /// Returns manifest URL for document in the given frame.
        /// </summary>
        public async System.Threading.Tasks.Task<GetManifestForFrameResponse> GetManifestForFrameAsync(string frameId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ApplicationCache.getManifestForFrame", dict);
            return methodResult.DeserializeJson<GetManifestForFrameResponse>();
        }
    }
}