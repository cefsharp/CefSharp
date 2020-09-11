// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.LayerTree
{
    using System.Linq;

    /// <summary>
    /// LayerTree
    /// </summary>
    public partial class LayerTree : DevToolsDomainBase
    {
        public LayerTree(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Provides the reasons why the given layer was composited.
        /// </summary>
        public async System.Threading.Tasks.Task<CompositingReasonsResponse> CompositingReasonsAsync(string layerId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("layerId", layerId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.compositingReasons", dict);
            return methodResult.DeserializeJson<CompositingReasonsResponse>();
        }

        /// <summary>
        /// Disables compositing tree inspection.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables compositing tree inspection.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns the snapshot identifier.
        /// </summary>
        public async System.Threading.Tasks.Task<LoadSnapshotResponse> LoadSnapshotAsync(System.Collections.Generic.IList<CefSharp.DevTools.LayerTree.PictureTile> tiles)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("tiles", tiles.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.loadSnapshot", dict);
            return methodResult.DeserializeJson<LoadSnapshotResponse>();
        }

        /// <summary>
        /// Returns the layer snapshot identifier.
        /// </summary>
        public async System.Threading.Tasks.Task<MakeSnapshotResponse> MakeSnapshotAsync(string layerId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("layerId", layerId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.makeSnapshot", dict);
            return methodResult.DeserializeJson<MakeSnapshotResponse>();
        }

        /// <summary>
        /// ProfileSnapshot
        /// </summary>
        public async System.Threading.Tasks.Task<ProfileSnapshotResponse> ProfileSnapshotAsync(string snapshotId, int? minRepeatCount = null, long? minDuration = null, CefSharp.DevTools.DOM.Rect clipRect = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("snapshotId", snapshotId);
            if (minRepeatCount.HasValue)
            {
                dict.Add("minRepeatCount", minRepeatCount.Value);
            }

            if (minDuration.HasValue)
            {
                dict.Add("minDuration", minDuration.Value);
            }

            if ((clipRect) != (null))
            {
                dict.Add("clipRect", clipRect.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.profileSnapshot", dict);
            return methodResult.DeserializeJson<ProfileSnapshotResponse>();
        }

        /// <summary>
        /// Releases layer snapshot captured by the back-end.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ReleaseSnapshotAsync(string snapshotId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("snapshotId", snapshotId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.releaseSnapshot", dict);
            return methodResult;
        }

        /// <summary>
        /// Replays the layer snapshot and returns the resulting bitmap.
        /// </summary>
        public async System.Threading.Tasks.Task<ReplaySnapshotResponse> ReplaySnapshotAsync(string snapshotId, int? fromStep = null, int? toStep = null, long? scale = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("snapshotId", snapshotId);
            if (fromStep.HasValue)
            {
                dict.Add("fromStep", fromStep.Value);
            }

            if (toStep.HasValue)
            {
                dict.Add("toStep", toStep.Value);
            }

            if (scale.HasValue)
            {
                dict.Add("scale", scale.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.replaySnapshot", dict);
            return methodResult.DeserializeJson<ReplaySnapshotResponse>();
        }

        /// <summary>
        /// Replays the layer snapshot and returns canvas log.
        /// </summary>
        public async System.Threading.Tasks.Task<SnapshotCommandLogResponse> SnapshotCommandLogAsync(string snapshotId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("snapshotId", snapshotId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.snapshotCommandLog", dict);
            return methodResult.DeserializeJson<SnapshotCommandLogResponse>();
        }
    }
}