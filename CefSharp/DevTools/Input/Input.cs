// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Input
{
    using System.Linq;

    /// <summary>
    /// Input
    /// </summary>
    public partial class Input : DevToolsDomainBase
    {
        public Input(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Dispatches a key event to the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DispatchKeyEventAsync(string type, int? modifiers = null, long? timestamp = null, string text = null, string unmodifiedText = null, string keyIdentifier = null, string code = null, string key = null, int? windowsVirtualKeyCode = null, int? nativeVirtualKeyCode = null, bool? autoRepeat = null, bool? isKeypad = null, bool? isSystemKey = null, int? location = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("type", type);
            if (modifiers.HasValue)
            {
                dict.Add("modifiers", modifiers.Value);
            }

            if (timestamp.HasValue)
            {
                dict.Add("timestamp", timestamp.Value);
            }

            if (!(string.IsNullOrEmpty(text)))
            {
                dict.Add("text", text);
            }

            if (!(string.IsNullOrEmpty(unmodifiedText)))
            {
                dict.Add("unmodifiedText", unmodifiedText);
            }

            if (!(string.IsNullOrEmpty(keyIdentifier)))
            {
                dict.Add("keyIdentifier", keyIdentifier);
            }

            if (!(string.IsNullOrEmpty(code)))
            {
                dict.Add("code", code);
            }

            if (!(string.IsNullOrEmpty(key)))
            {
                dict.Add("key", key);
            }

            if (windowsVirtualKeyCode.HasValue)
            {
                dict.Add("windowsVirtualKeyCode", windowsVirtualKeyCode.Value);
            }

            if (nativeVirtualKeyCode.HasValue)
            {
                dict.Add("nativeVirtualKeyCode", nativeVirtualKeyCode.Value);
            }

            if (autoRepeat.HasValue)
            {
                dict.Add("autoRepeat", autoRepeat.Value);
            }

            if (isKeypad.HasValue)
            {
                dict.Add("isKeypad", isKeypad.Value);
            }

            if (isSystemKey.HasValue)
            {
                dict.Add("isSystemKey", isSystemKey.Value);
            }

            if (location.HasValue)
            {
                dict.Add("location", location.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Input.dispatchKeyEvent", dict);
            return methodResult;
        }

        /// <summary>
        /// This method emulates inserting text that doesn't come from a key press,
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> InsertTextAsync(string text)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("text", text);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Input.insertText", dict);
            return methodResult;
        }

        /// <summary>
        /// Dispatches a mouse event to the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DispatchMouseEventAsync(string type, long x, long y, int? modifiers = null, long? timestamp = null, CefSharp.DevTools.Input.MouseButton? button = null, int? buttons = null, int? clickCount = null, long? deltaX = null, long? deltaY = null, string pointerType = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("type", type);
            dict.Add("x", x);
            dict.Add("y", y);
            if (modifiers.HasValue)
            {
                dict.Add("modifiers", modifiers.Value);
            }

            if (timestamp.HasValue)
            {
                dict.Add("timestamp", timestamp.Value);
            }

            if (button.HasValue)
            {
                dict.Add("button", this.EnumToString(button));
            }

            if (buttons.HasValue)
            {
                dict.Add("buttons", buttons.Value);
            }

            if (clickCount.HasValue)
            {
                dict.Add("clickCount", clickCount.Value);
            }

            if (deltaX.HasValue)
            {
                dict.Add("deltaX", deltaX.Value);
            }

            if (deltaY.HasValue)
            {
                dict.Add("deltaY", deltaY.Value);
            }

            if (!(string.IsNullOrEmpty(pointerType)))
            {
                dict.Add("pointerType", pointerType);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Input.dispatchMouseEvent", dict);
            return methodResult;
        }

        /// <summary>
        /// Dispatches a touch event to the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DispatchTouchEventAsync(string type, System.Collections.Generic.IList<CefSharp.DevTools.Input.TouchPoint> touchPoints, int? modifiers = null, long? timestamp = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("type", type);
            dict.Add("touchPoints", touchPoints.Select(x => x.ToDictionary()));
            if (modifiers.HasValue)
            {
                dict.Add("modifiers", modifiers.Value);
            }

            if (timestamp.HasValue)
            {
                dict.Add("timestamp", timestamp.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Input.dispatchTouchEvent", dict);
            return methodResult;
        }

        /// <summary>
        /// Emulates touch event from the mouse event parameters.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EmulateTouchFromMouseEventAsync(string type, int x, int y, CefSharp.DevTools.Input.MouseButton button, long? timestamp = null, long? deltaX = null, long? deltaY = null, int? modifiers = null, int? clickCount = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("type", type);
            dict.Add("x", x);
            dict.Add("y", y);
            dict.Add("button", this.EnumToString(button));
            if (timestamp.HasValue)
            {
                dict.Add("timestamp", timestamp.Value);
            }

            if (deltaX.HasValue)
            {
                dict.Add("deltaX", deltaX.Value);
            }

            if (deltaY.HasValue)
            {
                dict.Add("deltaY", deltaY.Value);
            }

            if (modifiers.HasValue)
            {
                dict.Add("modifiers", modifiers.Value);
            }

            if (clickCount.HasValue)
            {
                dict.Add("clickCount", clickCount.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Input.emulateTouchFromMouseEvent", dict);
            return methodResult;
        }

        /// <summary>
        /// Ignores input events (useful while auditing page).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetIgnoreInputEventsAsync(bool ignore)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("ignore", ignore);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Input.setIgnoreInputEvents", dict);
            return methodResult;
        }

        /// <summary>
        /// Synthesizes a pinch gesture over a time period by issuing appropriate touch events.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SynthesizePinchGestureAsync(long x, long y, long scaleFactor, int? relativeSpeed = null, CefSharp.DevTools.Input.GestureSourceType? gestureSourceType = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("x", x);
            dict.Add("y", y);
            dict.Add("scaleFactor", scaleFactor);
            if (relativeSpeed.HasValue)
            {
                dict.Add("relativeSpeed", relativeSpeed.Value);
            }

            if (gestureSourceType.HasValue)
            {
                dict.Add("gestureSourceType", this.EnumToString(gestureSourceType));
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Input.synthesizePinchGesture", dict);
            return methodResult;
        }

        /// <summary>
        /// Synthesizes a scroll gesture over a time period by issuing appropriate touch events.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SynthesizeScrollGestureAsync(long x, long y, long? xDistance = null, long? yDistance = null, long? xOverscroll = null, long? yOverscroll = null, bool? preventFling = null, int? speed = null, CefSharp.DevTools.Input.GestureSourceType? gestureSourceType = null, int? repeatCount = null, int? repeatDelayMs = null, string interactionMarkerName = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("x", x);
            dict.Add("y", y);
            if (xDistance.HasValue)
            {
                dict.Add("xDistance", xDistance.Value);
            }

            if (yDistance.HasValue)
            {
                dict.Add("yDistance", yDistance.Value);
            }

            if (xOverscroll.HasValue)
            {
                dict.Add("xOverscroll", xOverscroll.Value);
            }

            if (yOverscroll.HasValue)
            {
                dict.Add("yOverscroll", yOverscroll.Value);
            }

            if (preventFling.HasValue)
            {
                dict.Add("preventFling", preventFling.Value);
            }

            if (speed.HasValue)
            {
                dict.Add("speed", speed.Value);
            }

            if (gestureSourceType.HasValue)
            {
                dict.Add("gestureSourceType", this.EnumToString(gestureSourceType));
            }

            if (repeatCount.HasValue)
            {
                dict.Add("repeatCount", repeatCount.Value);
            }

            if (repeatDelayMs.HasValue)
            {
                dict.Add("repeatDelayMs", repeatDelayMs.Value);
            }

            if (!(string.IsNullOrEmpty(interactionMarkerName)))
            {
                dict.Add("interactionMarkerName", interactionMarkerName);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Input.synthesizeScrollGesture", dict);
            return methodResult;
        }

        /// <summary>
        /// Synthesizes a tap gesture over a time period by issuing appropriate touch events.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SynthesizeTapGestureAsync(long x, long y, int? duration = null, int? tapCount = null, CefSharp.DevTools.Input.GestureSourceType? gestureSourceType = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("x", x);
            dict.Add("y", y);
            if (duration.HasValue)
            {
                dict.Add("duration", duration.Value);
            }

            if (tapCount.HasValue)
            {
                dict.Add("tapCount", tapCount.Value);
            }

            if (gestureSourceType.HasValue)
            {
                dict.Add("gestureSourceType", this.EnumToString(gestureSourceType));
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Input.synthesizeTapGesture", dict);
            return methodResult;
        }
    }
}