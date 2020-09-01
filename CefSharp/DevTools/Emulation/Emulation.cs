// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Emulation
{
    /// <summary>
    /// This domain emulates different environments for the page.
    /// </summary>
    public partial class Emulation
    {
        public Emulation(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Tells whether emulation is supported.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CanEmulate()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.CanEmulate", dict);
            return result;
        }

        /// <summary>
        /// Clears the overriden device metrics.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearDeviceMetricsOverride()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.ClearDeviceMetricsOverride", dict);
            return result;
        }

        /// <summary>
        /// Clears the overriden Geolocation Position and Error.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearGeolocationOverride()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.ClearGeolocationOverride", dict);
            return result;
        }

        /// <summary>
        /// Requests that page scale factor is reset to initial values.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ResetPageScaleFactor()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.ResetPageScaleFactor", dict);
            return result;
        }

        /// <summary>
        /// Enables or disables simulating a focused and active page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetFocusEmulationEnabled(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"enabled", enabled}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetFocusEmulationEnabled", dict);
            return result;
        }

        /// <summary>
        /// Enables CPU throttling to emulate slow CPUs.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetCPUThrottlingRate(long rate)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"rate", rate}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetCPUThrottlingRate", dict);
            return result;
        }

        /// <summary>
        /// Sets or clears an override of the default background color of the frame. This override is used
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDefaultBackgroundColorOverride(DOM.RGBA color)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"color", color}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetDefaultBackgroundColorOverride", dict);
            return result;
        }

        /// <summary>
        /// Overrides the values of device screen dimensions (window.screen.width, window.screen.height,
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDeviceMetricsOverride(int width, int height, long deviceScaleFactor, bool mobile, long scale, int screenWidth, int screenHeight, int positionX, int positionY, bool dontSetVisibleSize, ScreenOrientation screenOrientation, Page.Viewport viewport, DisplayFeature displayFeature)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"width", width}, {"height", height}, {"deviceScaleFactor", deviceScaleFactor}, {"mobile", mobile}, {"scale", scale}, {"screenWidth", screenWidth}, {"screenHeight", screenHeight}, {"positionX", positionX}, {"positionY", positionY}, {"dontSetVisibleSize", dontSetVisibleSize}, {"screenOrientation", screenOrientation}, {"viewport", viewport}, {"displayFeature", displayFeature}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetDeviceMetricsOverride", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetScrollbarsHidden(bool hidden)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"hidden", hidden}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetScrollbarsHidden", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDocumentCookieDisabled(bool disabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"disabled", disabled}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetDocumentCookieDisabled", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetEmitTouchEventsForMouse(bool enabled, string configuration)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"enabled", enabled}, {"configuration", configuration}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetEmitTouchEventsForMouse", dict);
            return result;
        }

        /// <summary>
        /// Emulates the given media type or media feature for CSS media queries.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetEmulatedMedia(string media, System.Collections.Generic.IList<MediaFeature> features)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"media", media}, {"features", features}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetEmulatedMedia", dict);
            return result;
        }

        /// <summary>
        /// Emulates the given vision deficiency.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetEmulatedVisionDeficiency(string type)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"type", type}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetEmulatedVisionDeficiency", dict);
            return result;
        }

        /// <summary>
        /// Overrides the Geolocation Position or Error. Omitting any of the parameters emulates position
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetGeolocationOverride(long latitude, long longitude, long accuracy)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"latitude", latitude}, {"longitude", longitude}, {"accuracy", accuracy}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetGeolocationOverride", dict);
            return result;
        }

        /// <summary>
        /// Overrides the Idle state.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetIdleOverride(bool isUserActive, bool isScreenUnlocked)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"isUserActive", isUserActive}, {"isScreenUnlocked", isScreenUnlocked}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetIdleOverride", dict);
            return result;
        }

        /// <summary>
        /// Clears Idle state overrides.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearIdleOverride()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.ClearIdleOverride", dict);
            return result;
        }

        /// <summary>
        /// Overrides value returned by the javascript navigator object.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetNavigatorOverrides(string platform)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"platform", platform}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetNavigatorOverrides", dict);
            return result;
        }

        /// <summary>
        /// Sets a specified page scale factor.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetPageScaleFactor(long pageScaleFactor)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"pageScaleFactor", pageScaleFactor}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetPageScaleFactor", dict);
            return result;
        }

        /// <summary>
        /// Switches script execution in the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetScriptExecutionDisabled(bool value)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"value", value}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetScriptExecutionDisabled", dict);
            return result;
        }

        /// <summary>
        /// Enables touch on platforms which do not support them.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetTouchEmulationEnabled(bool enabled, int maxTouchPoints)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"enabled", enabled}, {"maxTouchPoints", maxTouchPoints}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetTouchEmulationEnabled", dict);
            return result;
        }

        /// <summary>
        /// Turns on virtual time for all frames (replacing real-time with a synthetic time source) and sets
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetVirtualTimePolicy(string policy, long budget, int maxVirtualTimeTaskStarvationCount, bool waitForNavigation, long initialVirtualTime)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"policy", policy}, {"budget", budget}, {"maxVirtualTimeTaskStarvationCount", maxVirtualTimeTaskStarvationCount}, {"waitForNavigation", waitForNavigation}, {"initialVirtualTime", initialVirtualTime}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetVirtualTimePolicy", dict);
            return result;
        }

        /// <summary>
        /// Overrides default host system locale with the specified one.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetLocaleOverride(string locale)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"locale", locale}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetLocaleOverride", dict);
            return result;
        }

        /// <summary>
        /// Overrides default host system timezone with the specified one.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetTimezoneOverride(string timezoneId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"timezoneId", timezoneId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetTimezoneOverride", dict);
            return result;
        }

        /// <summary>
        /// Resizes the frame/viewport of the page. Note that this does not affect the frame's container
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetVisibleSize(int width, int height)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"width", width}, {"height", height}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetVisibleSize", dict);
            return result;
        }

        /// <summary>
        /// Allows overriding user agent with the given string.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetUserAgentOverride(string userAgent, string acceptLanguage, string platform, UserAgentMetadata userAgentMetadata)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"userAgent", userAgent}, {"acceptLanguage", acceptLanguage}, {"platform", platform}, {"userAgentMetadata", userAgentMetadata}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.SetUserAgentOverride", dict);
            return result;
        }
    }
}