// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Information about a specific web plugin.
    /// </summary>
    public sealed class WebPluginInfo
    {
        /// <summary>
        /// Gets or sets the plugin name (i.e. Flash).
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets tge description of the plugin from the version information.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets or sets the plugin file path (DLL/bundle/library).
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets or sets the version of the plugin (may be OS-specific).
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// WebPluginInfo
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="description">description</param>
        /// <param name="path">path</param>
        /// <param name="version">version</param>
        public WebPluginInfo(string name, string description, string path, string version)
        {
            Name = name;
            Description = description;
            Path = path;
            Version = version;
        }
    }
}
