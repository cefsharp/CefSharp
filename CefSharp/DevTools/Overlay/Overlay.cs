// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Overlay
{
    using System.Linq;

    /// <summary>
    /// This domain provides various functionality related to drawing atop the inspected page.
    /// </summary>
    public partial class Overlay : DevToolsDomainBase
    {
        public Overlay(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Disables domain notifications.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables domain notifications.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// For testing.
        /// </summary>
        public async System.Threading.Tasks.Task<GetHighlightObjectForTestResponse> GetHighlightObjectForTestAsync(int nodeId, bool? includeDistance = null, bool? includeStyle = null, CefSharp.DevTools.Overlay.ColorFormat? colorFormat = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            if (includeDistance.HasValue)
            {
                dict.Add("includeDistance", includeDistance.Value);
            }

            if (includeStyle.HasValue)
            {
                dict.Add("includeStyle", includeStyle.Value);
            }

            if (colorFormat.HasValue)
            {
                dict.Add("colorFormat", this.EnumToString(colorFormat));
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.getHighlightObjectForTest", dict);
            return methodResult.DeserializeJson<GetHighlightObjectForTestResponse>();
        }

        /// <summary>
        /// Hides any highlight.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HideHighlightAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.hideHighlight", dict);
            return methodResult;
        }

        /// <summary>
        /// Highlights owner element of the frame with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HighlightFrameAsync(string frameId, CefSharp.DevTools.DOM.RGBA contentColor = null, CefSharp.DevTools.DOM.RGBA contentOutlineColor = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            if ((contentColor) != (null))
            {
                dict.Add("contentColor", contentColor.ToDictionary());
            }

            if ((contentOutlineColor) != (null))
            {
                dict.Add("contentOutlineColor", contentOutlineColor.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.highlightFrame", dict);
            return methodResult;
        }

        /// <summary>
        /// Highlights DOM node with given id or with the given JavaScript object wrapper. Either nodeId or
        /// objectId must be specified.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HighlightNodeAsync(CefSharp.DevTools.Overlay.HighlightConfig highlightConfig, int? nodeId = null, int? backendNodeId = null, string objectId = null, string selector = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("highlightConfig", highlightConfig.ToDictionary());
            if (nodeId.HasValue)
            {
                dict.Add("nodeId", nodeId.Value);
            }

            if (backendNodeId.HasValue)
            {
                dict.Add("backendNodeId", backendNodeId.Value);
            }

            if (!(string.IsNullOrEmpty(objectId)))
            {
                dict.Add("objectId", objectId);
            }

            if (!(string.IsNullOrEmpty(selector)))
            {
                dict.Add("selector", selector);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.highlightNode", dict);
            return methodResult;
        }

        /// <summary>
        /// Highlights given quad. Coordinates are absolute with respect to the main frame viewport.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HighlightQuadAsync(long[] quad, CefSharp.DevTools.DOM.RGBA color = null, CefSharp.DevTools.DOM.RGBA outlineColor = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("quad", quad);
            if ((color) != (null))
            {
                dict.Add("color", color.ToDictionary());
            }

            if ((outlineColor) != (null))
            {
                dict.Add("outlineColor", outlineColor.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.highlightQuad", dict);
            return methodResult;
        }

        /// <summary>
        /// Highlights given rectangle. Coordinates are absolute with respect to the main frame viewport.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HighlightRectAsync(int x, int y, int width, int height, CefSharp.DevTools.DOM.RGBA color = null, CefSharp.DevTools.DOM.RGBA outlineColor = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("x", x);
            dict.Add("y", y);
            dict.Add("width", width);
            dict.Add("height", height);
            if ((color) != (null))
            {
                dict.Add("color", color.ToDictionary());
            }

            if ((outlineColor) != (null))
            {
                dict.Add("outlineColor", outlineColor.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.highlightRect", dict);
            return methodResult;
        }

        /// <summary>
        /// Enters the 'inspect' mode. In this mode, elements that user is hovering over are highlighted.
        /// Backend then generates 'inspectNodeRequested' event upon element selection.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetInspectModeAsync(CefSharp.DevTools.Overlay.InspectMode mode, CefSharp.DevTools.Overlay.HighlightConfig highlightConfig = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("mode", this.EnumToString(mode));
            if ((highlightConfig) != (null))
            {
                dict.Add("highlightConfig", highlightConfig.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setInspectMode", dict);
            return methodResult;
        }

        /// <summary>
        /// Highlights owner element of all frames detected to be ads.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowAdHighlightsAsync(bool show)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("show", show);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowAdHighlights", dict);
            return methodResult;
        }

        /// <summary>
        /// SetPausedInDebuggerMessage
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPausedInDebuggerMessageAsync(string message = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(message)))
            {
                dict.Add("message", message);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setPausedInDebuggerMessage", dict);
            return methodResult;
        }

        /// <summary>
        /// Requests that backend shows debug borders on layers
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowDebugBordersAsync(bool show)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("show", show);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowDebugBorders", dict);
            return methodResult;
        }

        /// <summary>
        /// Requests that backend shows the FPS counter
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowFPSCounterAsync(bool show)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("show", show);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowFPSCounter", dict);
            return methodResult;
        }

        /// <summary>
        /// Requests that backend shows paint rectangles
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowPaintRectsAsync(bool result)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("result", result);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowPaintRects", dict);
            return methodResult;
        }

        /// <summary>
        /// Requests that backend shows layout shift regions
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowLayoutShiftRegionsAsync(bool result)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("result", result);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowLayoutShiftRegions", dict);
            return methodResult;
        }

        /// <summary>
        /// Requests that backend shows scroll bottleneck rects
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowScrollBottleneckRectsAsync(bool show)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("show", show);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowScrollBottleneckRects", dict);
            return methodResult;
        }

        /// <summary>
        /// Requests that backend shows hit-test borders on layers
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowHitTestBordersAsync(bool show)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("show", show);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowHitTestBorders", dict);
            return methodResult;
        }

        /// <summary>
        /// Paints viewport size upon main frame resize.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowViewportSizeOnResizeAsync(bool show)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("show", show);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowViewportSizeOnResize", dict);
            return methodResult;
        }

        /// <summary>
        /// Add a dual screen device hinge
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowHingeAsync(CefSharp.DevTools.Overlay.HingeConfig hingeConfig = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if ((hingeConfig) != (null))
            {
                dict.Add("hingeConfig", hingeConfig.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowHinge", dict);
            return methodResult;
        }
    }
}