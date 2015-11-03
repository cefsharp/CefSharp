// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IMenuModel
    {
        /// <summary>
        /// Returns the number of items in this menu.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Remove all menu items. Can be used to disable the context menu. Returns true on success.
        /// </summary>
        /// <returns>Returns true on success</returns>
        bool Clear();

        /// <summary>
        /// Returns the label at the specified index or empty if not found due to
        /// invalid range or the index being a separator.
        /// </summary>
        /// <param name="index">specified index</param>
        /// <returns>Label or empty if not found due to invalid range or the index being a separator.</returns>
        string GetLabelAt(int index);

        /// <summary>
        /// Returns the command id at the specified index or -1 if not found due to invalid range or the index being a separator.
        /// </summary>
        /// <param name="index">the index</param>
        /// <returns>Command or -1 if not found due to invalid range or the index being a separator.</returns>
        CefMenuCommand GetCommandIdAt(int index);

        /// <summary>
        /// Removes the item with the specified commandId.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <returns>Returns true on success</returns>
        bool Remove(CefMenuCommand commandId);

        /// <summary>
        /// Add an item to the menu. 
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <param name="label">the label of the item</param>
        /// <returns>Returns true on success.</returns>
        bool AddItem(CefMenuCommand commandId, string label);

        /// <summary>
        /// Add a separator to the menu. 
        /// </summary>
        /// <returns>Returns true on success.</returns>
        bool AddSeparator();
    }
}
