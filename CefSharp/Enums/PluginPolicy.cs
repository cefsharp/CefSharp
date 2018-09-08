// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Plugin policies supported by IPluginHandler.OnBeforePluginLoad.
    /// </summary>
    public enum PluginPolicy
    {
        /// <summary>
        /// Allow the content
        /// </summary>
        Allow,

        /// <summary>
        /// Allow important content and block unimportant content based on heuristics. The user can manually load blocked content.
        /// </summary>
        DetectImportant,

        /// <summary>
        /// Block the content. The user can manually load blocked content.
        /// </summary>
        Block,

        /// <summary>
        /// Disable the content. The user cannot load disabled content.
        /// </summary>
        Disable
    }
}
