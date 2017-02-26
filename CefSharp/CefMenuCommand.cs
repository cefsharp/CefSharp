// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public enum CefMenuCommand
    {
        NotFound = -1,
        // Navigation.
        Back = 100,
        Forward = 101,
        Reload = 102,
        ReloadNoCache = 103,
        StopLoad = 104,

        // Editing.
        Undo = 110,
        Redo = 111,
        Cut = 112,
        Copy = 113,
        Paste = 114,
        Delete = 115,
        SelectAll = 116,

        // Miscellaneous.
        Find = 130,
        Print = 131,
        ViewSource = 132,

        // Spell checking word correction suggestions.
        SpellCheckSuggestion0 = 200,
        SpellCheckSuggestion1 = 201,
        SpellCheckSuggestion2 = 202,
        SpellCheckSuggestion3 = 203,
        SpellCheckSuggestion4 = 204,
        SpellCheckLastSuggestion = 204,
        SpellCheckNoSuggestions = 205,
        AddToDictionary = 206,

        /// <summary>
        /// Custom menu items originating from the renderer process. For example, plugin placeholder menu items or Flash menu items.
        /// This is the first entry
        /// </summary>
        CustomFirst = 220,
        /// <summary>
        /// Custom menu items originating from the renderer process. For example, plugin placeholder menu items or Flash menu items.
        /// This is the last entry
        /// </summary>
        CustomLast = 250,

        // All user-defined menu IDs should come between MENU_ID_USER_FIRST and
        // MENU_ID_USER_LAST to avoid overlapping the Chromium and CEF ID ranges
        // defined in the tools/gritsettings/resource_ids file.
        UserFirst = 26500,
        UserLast = 28500
    }
}
