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
        private CefSharp.DevTools.IDevToolsClient _client;
        public ApplicationCache(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Enables application cache domain notifications.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ApplicationCache.enable", dict);
            return methodResult;
        }

        partial void ValidateGetApplicationCacheForFrame(string frameId);
        /// <summary>
        /// Returns relevant application cache data for the document in given frame.
        /// </summary>
        /// <param name = "frameId">Identifier of the frame containing document whose application cache is retrieved.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetApplicationCacheForFrameResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetApplicationCacheForFrameResponse> GetApplicationCacheForFrameAsync(string frameId)
        {
            ValidateGetApplicationCacheForFrame(frameId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ApplicationCache.getApplicationCacheForFrame", dict);
            return methodResult.DeserializeJson<GetApplicationCacheForFrameResponse>();
        }

        /// <summary>
        /// Returns array of frame identifiers with manifest urls for each frame containing a document
        /// associated with some application cache.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetFramesWithManifestsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetFramesWithManifestsResponse> GetFramesWithManifestsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ApplicationCache.getFramesWithManifests", dict);
            return methodResult.DeserializeJson<GetFramesWithManifestsResponse>();
        }

        partial void ValidateGetManifestForFrame(string frameId);
        /// <summary>
        /// Returns manifest URL for document in the given frame.
        /// </summary>
        /// <param name = "frameId">Identifier of the frame containing document whose manifest is retrieved.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetManifestForFrameResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetManifestForFrameResponse> GetManifestForFrameAsync(string frameId)
        {
            ValidateGetManifestForFrame(frameId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ApplicationCache.getManifestForFrame", dict);
            return methodResult.DeserializeJson<GetManifestForFrameResponse>();
        }
    }
}