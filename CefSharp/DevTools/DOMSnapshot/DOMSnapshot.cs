// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMSnapshot
{
    using System.Linq;

    /// <summary>
    /// This domain facilitates obtaining document snapshots with DOM, layout, and style information.
    /// </summary>
    public partial class DOMSnapshot : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public DOMSnapshot(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Disables DOM snapshot agent for the given page.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMSnapshot.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables DOM snapshot agent for the given page.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMSnapshot.enable", dict);
            return methodResult;
        }

        partial void ValidateCaptureSnapshot(string[] computedStyles, bool? includePaintOrder = null, bool? includeDOMRects = null);
        /// <summary>
        /// Returns a document snapshot, including the full DOM tree of the root node (including iframes,
        /// template contents, and imported documents) in a flattened array, as well as layout and
        /// white-listed computed style information for the nodes. Shadow DOM in the returned DOM tree is
        /// flattened.
        /// </summary>
        /// <param name = "computedStyles">Whitelist of computed styles to return.</param>
        /// <param name = "includePaintOrder">Whether to include layout object paint orders into the snapshot.</param>
        /// <param name = "includeDOMRects">Whether to include DOM rectangles (offsetRects, clientRects, scrollRects) into the snapshot</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;CaptureSnapshotResponse&gt;</returns>
        public async System.Threading.Tasks.Task<CaptureSnapshotResponse> CaptureSnapshotAsync(string[] computedStyles, bool? includePaintOrder = null, bool? includeDOMRects = null)
        {
            ValidateCaptureSnapshot(computedStyles, includePaintOrder, includeDOMRects);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("computedStyles", computedStyles);
            if (includePaintOrder.HasValue)
            {
                dict.Add("includePaintOrder", includePaintOrder.Value);
            }

            if (includeDOMRects.HasValue)
            {
                dict.Add("includeDOMRects", includeDOMRects.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMSnapshot.captureSnapshot", dict);
            return methodResult.DeserializeJson<CaptureSnapshotResponse>();
        }
    }
}