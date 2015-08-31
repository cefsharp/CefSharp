// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IMenuModel
    {
        /// <summary>
        /// Get count of menu items.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Remove all menu items. Can be used to disable the context menu. Returns true on success.
        /// </summary>
        /// <returns></returns>
        bool Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string GetLabelAt(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int GetCommandIdAt(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool Remove(int index);

        /// <summary>
        /// Add an item to the menu. Returns true on success.
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        bool AddItem(int commandId, string label);

        /// <summary>
        /// Add a separator to the menu. Returns true on success.
        /// </summary>
        /// <returns></returns>
        bool AddSeparator();
    }
}
