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
        private CefSharp.DevTools.IDevToolsClient _client;
        public LayerTree(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateCompositingReasons(string layerId);
        /// <summary>
        /// Provides the reasons why the given layer was composited.
        /// </summary>
        /// <param name = "layerId">The id of the layer for which we want to get the reasons it was composited.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;CompositingReasonsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<CompositingReasonsResponse> CompositingReasonsAsync(string layerId)
        {
            ValidateCompositingReasons(layerId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("layerId", layerId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.compositingReasons", dict);
            return methodResult.DeserializeJson<CompositingReasonsResponse>();
        }

        /// <summary>
        /// Disables compositing tree inspection.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables compositing tree inspection.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.enable", dict);
            return methodResult;
        }

        partial void ValidateLoadSnapshot(System.Collections.Generic.IList<CefSharp.DevTools.LayerTree.PictureTile> tiles);
        /// <summary>
        /// Returns the snapshot identifier.
        /// </summary>
        /// <param name = "tiles">An array of tiles composing the snapshot.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;LoadSnapshotResponse&gt;</returns>
        public async System.Threading.Tasks.Task<LoadSnapshotResponse> LoadSnapshotAsync(System.Collections.Generic.IList<CefSharp.DevTools.LayerTree.PictureTile> tiles)
        {
            ValidateLoadSnapshot(tiles);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("tiles", tiles.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.loadSnapshot", dict);
            return methodResult.DeserializeJson<LoadSnapshotResponse>();
        }

        partial void ValidateMakeSnapshot(string layerId);
        /// <summary>
        /// Returns the layer snapshot identifier.
        /// </summary>
        /// <param name = "layerId">The id of the layer.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;MakeSnapshotResponse&gt;</returns>
        public async System.Threading.Tasks.Task<MakeSnapshotResponse> MakeSnapshotAsync(string layerId)
        {
            ValidateMakeSnapshot(layerId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("layerId", layerId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.makeSnapshot", dict);
            return methodResult.DeserializeJson<MakeSnapshotResponse>();
        }

        partial void ValidateProfileSnapshot(string snapshotId, int? minRepeatCount = null, long? minDuration = null, CefSharp.DevTools.DOM.Rect clipRect = null);
        /// <summary>
        /// ProfileSnapshot
        /// </summary>
        /// <param name = "snapshotId">The id of the layer snapshot.</param>
        /// <param name = "minRepeatCount">The maximum number of times to replay the snapshot (1, if not specified).</param>
        /// <param name = "minDuration">The minimum duration (in seconds) to replay the snapshot.</param>
        /// <param name = "clipRect">The clip rectangle to apply when replaying the snapshot.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;ProfileSnapshotResponse&gt;</returns>
        public async System.Threading.Tasks.Task<ProfileSnapshotResponse> ProfileSnapshotAsync(string snapshotId, int? minRepeatCount = null, long? minDuration = null, CefSharp.DevTools.DOM.Rect clipRect = null)
        {
            ValidateProfileSnapshot(snapshotId, minRepeatCount, minDuration, clipRect);
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

        partial void ValidateReleaseSnapshot(string snapshotId);
        /// <summary>
        /// Releases layer snapshot captured by the back-end.
        /// </summary>
        /// <param name = "snapshotId">The id of the layer snapshot.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ReleaseSnapshotAsync(string snapshotId)
        {
            ValidateReleaseSnapshot(snapshotId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("snapshotId", snapshotId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.releaseSnapshot", dict);
            return methodResult;
        }

        partial void ValidateReplaySnapshot(string snapshotId, int? fromStep = null, int? toStep = null, long? scale = null);
        /// <summary>
        /// Replays the layer snapshot and returns the resulting bitmap.
        /// </summary>
        /// <param name = "snapshotId">The id of the layer snapshot.</param>
        /// <param name = "fromStep">The first step to replay from (replay from the very start if not specified).</param>
        /// <param name = "toStep">The last step to replay to (replay till the end if not specified).</param>
        /// <param name = "scale">The scale to apply while replaying (defaults to 1).</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;ReplaySnapshotResponse&gt;</returns>
        public async System.Threading.Tasks.Task<ReplaySnapshotResponse> ReplaySnapshotAsync(string snapshotId, int? fromStep = null, int? toStep = null, long? scale = null)
        {
            ValidateReplaySnapshot(snapshotId, fromStep, toStep, scale);
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

        partial void ValidateSnapshotCommandLog(string snapshotId);
        /// <summary>
        /// Replays the layer snapshot and returns canvas log.
        /// </summary>
        /// <param name = "snapshotId">The id of the layer snapshot.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SnapshotCommandLogResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SnapshotCommandLogResponse> SnapshotCommandLogAsync(string snapshotId)
        {
            ValidateSnapshotCommandLog(snapshotId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("snapshotId", snapshotId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("LayerTree.snapshotCommandLog", dict);
            return methodResult.DeserializeJson<SnapshotCommandLogResponse>();
        }
    }
}