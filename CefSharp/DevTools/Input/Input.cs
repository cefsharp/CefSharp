// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Input
{
    /// <summary>
    /// Input
    /// </summary>
    public partial class Input
    {
        public Input(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Dispatches a key event to the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DispatchKeyEvent(string type, int modifiers, long timestamp, string text, string unmodifiedText, string keyIdentifier, string code, string key, int windowsVirtualKeyCode, int nativeVirtualKeyCode, bool autoRepeat, bool isKeypad, bool isSystemKey, int location, string commands)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"type", type}, {"modifiers", modifiers}, {"timestamp", timestamp}, {"text", text}, {"unmodifiedText", unmodifiedText}, {"keyIdentifier", keyIdentifier}, {"code", code}, {"key", key}, {"windowsVirtualKeyCode", windowsVirtualKeyCode}, {"nativeVirtualKeyCode", nativeVirtualKeyCode}, {"autoRepeat", autoRepeat}, {"isKeypad", isKeypad}, {"isSystemKey", isSystemKey}, {"location", location}, {"commands", commands}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Input.DispatchKeyEvent", dict);
            return result;
        }

        /// <summary>
        /// This method emulates inserting text that doesn't come from a key press,
        public async System.Threading.Tasks.Task<DevToolsMethodResult> InsertText(string text)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"text", text}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Input.InsertText", dict);
            return result;
        }

        /// <summary>
        /// Dispatches a mouse event to the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DispatchMouseEvent(string type, long x, long y, int modifiers, long timestamp, string button, int buttons, int clickCount, long deltaX, long deltaY, string pointerType)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"type", type}, {"x", x}, {"y", y}, {"modifiers", modifiers}, {"timestamp", timestamp}, {"button", button}, {"buttons", buttons}, {"clickCount", clickCount}, {"deltaX", deltaX}, {"deltaY", deltaY}, {"pointerType", pointerType}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Input.DispatchMouseEvent", dict);
            return result;
        }

        /// <summary>
        /// Dispatches a touch event to the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DispatchTouchEvent(string type, System.Collections.Generic.IList<TouchPoint> touchPoints, int modifiers, long timestamp)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"type", type}, {"touchPoints", touchPoints}, {"modifiers", modifiers}, {"timestamp", timestamp}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Input.DispatchTouchEvent", dict);
            return result;
        }

        /// <summary>
        /// Emulates touch event from the mouse event parameters.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> EmulateTouchFromMouseEvent(string type, int x, int y, string button, long timestamp, long deltaX, long deltaY, int modifiers, int clickCount)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"type", type}, {"x", x}, {"y", y}, {"button", button}, {"timestamp", timestamp}, {"deltaX", deltaX}, {"deltaY", deltaY}, {"modifiers", modifiers}, {"clickCount", clickCount}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Input.EmulateTouchFromMouseEvent", dict);
            return result;
        }

        /// <summary>
        /// Ignores input events (useful while auditing page).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetIgnoreInputEvents(bool ignore)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"ignore", ignore}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Input.SetIgnoreInputEvents", dict);
            return result;
        }

        /// <summary>
        /// Synthesizes a pinch gesture over a time period by issuing appropriate touch events.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SynthesizePinchGesture(long x, long y, long scaleFactor, int relativeSpeed, string gestureSourceType)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"x", x}, {"y", y}, {"scaleFactor", scaleFactor}, {"relativeSpeed", relativeSpeed}, {"gestureSourceType", gestureSourceType}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Input.SynthesizePinchGesture", dict);
            return result;
        }

        /// <summary>
        /// Synthesizes a scroll gesture over a time period by issuing appropriate touch events.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SynthesizeScrollGesture(long x, long y, long xDistance, long yDistance, long xOverscroll, long yOverscroll, bool preventFling, int speed, string gestureSourceType, int repeatCount, int repeatDelayMs, string interactionMarkerName)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"x", x}, {"y", y}, {"xDistance", xDistance}, {"yDistance", yDistance}, {"xOverscroll", xOverscroll}, {"yOverscroll", yOverscroll}, {"preventFling", preventFling}, {"speed", speed}, {"gestureSourceType", gestureSourceType}, {"repeatCount", repeatCount}, {"repeatDelayMs", repeatDelayMs}, {"interactionMarkerName", interactionMarkerName}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Input.SynthesizeScrollGesture", dict);
            return result;
        }

        /// <summary>
        /// Synthesizes a tap gesture over a time period by issuing appropriate touch events.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SynthesizeTapGesture(long x, long y, int duration, int tapCount, string gestureSourceType)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"x", x}, {"y", y}, {"duration", duration}, {"tapCount", tapCount}, {"gestureSourceType", gestureSourceType}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Input.SynthesizeTapGesture", dict);
            return result;
        }
    }
}