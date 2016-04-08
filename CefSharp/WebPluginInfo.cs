// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Information about a specific web plugin.
    /// </summary>
    public class WebPluginInfo
    {
        /// <summary>
        /// Gets or sets the plugin name (i.e. Flash).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets tge description of the plugin from the version information.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the plugin file path (DLL/bundle/library).
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the version of the plugin (may be OS-specific).
        /// </summary>
        public string Version { get; set; }
    }
}
