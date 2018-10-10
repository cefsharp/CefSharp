// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Supports creation and modification of menus. See <see cref="CefMenuCommand"/> for the command ids that have default implementations.
    /// All user-defined command ids should be between <see cref="CefMenuCommand.UserFirst"/> and <see cref="CefMenuCommand.UserFirst"/>.
    /// The methods of this class can only be accessed on the CEF UI thread, which by default is not the same as your application UI thread.
    /// </summary>
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

        /// <summary>
        /// Add a check item to the menu.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <param name="label">the label of the item</param>
        /// <returns>Returns true on success.</returns>
        bool AddCheckItem(CefMenuCommand commandId, string label);

        /// <summary>
        /// Add a radio item to the menu. Only a single item with the specified groupId can be checked at a time.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <param name="label">the label of the item</param>
        /// <param name="groupId">the group id</param>
        /// <returns>Returns true on success.</returns>
        bool AddRadioItem(CefMenuCommand commandId, string label, int groupId);

        /// <summary>
        /// Add a sub-menu to the menu. The new sub-menu is returned.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <param name="label">the label of the item</param>
        /// <returns>Returns the newly created <see cref="IMenuModel"/>.</returns>
        IMenuModel AddSubMenu(CefMenuCommand commandId, string label);

        /// <summary>
        /// Insert a separator in the menu at the specified index. 
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Returns true on success.</returns>
        bool InsertSeparatorAt(int index);

        /// <summary>
        /// Insert an item in the menu at the specified index.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="commandId">the command Id</param>
        /// <param name="label">the label of the item</param>
        /// <returns>Returns true on success.</returns>        
        bool InsertItemAt(int index, CefMenuCommand commandId, string label);

        /// <summary>
        /// Insert a check item in the menu at the specified index.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="commandId">the command Id</param>
        /// <param name="label">the label of the item</param>
        /// <returns>Returns true on success.</returns>
        bool InsertCheckItemAt(int index, CefMenuCommand commandId, string label);

        /// <summary>
        /// Insert a radio item in the menu at the specified index.
        /// Only a single item with the specified groupId can be checked at a time.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="commandId">the command Id</param>
        /// <param name="label">the label of the item</param>
        /// <param name="groupId">the group id</param>
        /// <returns>Returns true on success.</returns>        
        bool InsertRadioItemAt(int index, CefMenuCommand commandId, string label, int groupId);

        /// <summary>
        /// Insert a sub-menu in the menu at the specified index.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="commandId">the command Id</param>
        /// <param name="label">the label of the item</param>
        /// <returns>Returns the newly created <see cref="IMenuModel"/>.</returns>
        IMenuModel InsertSubMenuAt(int index, CefMenuCommand commandId, string label);

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Returns true on success.</returns>
        bool RemoveAt(int index);

        /// <summary>
        /// Returns the index associated with the specified commandId or -1 if not found due to the command id not existing in the menu.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <returns>Returns the index associated with the specified commandId or -1 if not found due to the command id not existing in the menu.</returns>
        int GetIndexOf(CefMenuCommand commandId);

        /// <summary>
        /// Sets the command id at the specified index.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="commandId">the command Id</param>
        /// <returns>Returns true on success.</returns>        
        bool SetCommandIdAt(int index, CefMenuCommand commandId);

        /// <summary>
        /// Returns the label for the specified commandId or empty if not found.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <returns>Returns the label for the specified commandId or empty if not found.</returns>
        string GetLabel(CefMenuCommand commandId);

        /// <summary>
        /// Sets the label for the specified commandId. 
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <param name="label">the label</param>
        /// <returns>Returns true on success.</returns>
        bool SetLabel(CefMenuCommand commandId, string label);

        /// <summary>
        /// Set the label at the specified index.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="label">the label</param>
        /// <returns>Returns true on success.</returns>
        bool SetLabelAt(int index, string label);

        /// <summary>
        /// Returns the item type for the specified commandId.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <returns>Returns the item type for the specified commandId.</returns>
        MenuItemType GetType(CefMenuCommand commandId);

        /// <summary>
        /// Returns the item type at the specified index.
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Returns the item type at the specified index.</returns>
        MenuItemType GetTypeAt(int index);

        /// <summary>
        /// Returns the group id for the specified commandId or -1 if invalid.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <returns>Returns the group id for the specified commandId or -1 if invalid.</returns>        
        int GetGroupId(CefMenuCommand commandId);

        /// <summary>
        /// Returns the group id at the specified index or -1 if invalid.
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Returns the group id at the specified index or -1 if invalid.</returns>        
        int GetGroupIdAt(int index);

        /// <summary>
        /// Sets the group id for the specified commandId.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <param name="groupId">the group id</param>
        /// <returns>Returns true on success.</returns>
        bool SetGroupId(CefMenuCommand commandId, int groupId);

        /// <summary>
        /// Sets the group id at the specified index.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="groupId">the group id</param>
        /// <returns>Returns true on success.</returns>        
        bool SetGroupIdAt(int index, int groupId);

        /// <summary>
        /// Returns the <see cref="IMenuModel"/> for the specified commandId or null if invalid.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <returns>Returns the <see cref="IMenuModel"/> for the specified commandId or null if invalid.</returns>        
        IMenuModel GetSubMenu(CefMenuCommand commandId);

        /// <summary>
        /// Returns the <see cref="IMenuModel"/> at the specified index or empty if invalid.
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Returns the <see cref="IMenuModel"/> for the specified commandId or null if invalid.</returns>
        IMenuModel GetSubMenuAt(int index);

        /// <summary>
        /// Returns true if the specified commandId is visible.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <returns>Returns true if the specified commandId is visible.</returns>        
        bool IsVisible(CefMenuCommand commandId);

        /// <summary>
        /// Returns true if the specified index is visible.
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Returns true if the specified index is visible.</returns>
        bool IsVisibleAt(int index);

        /// <summary>
        /// Change the visibility of the specified commandId.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <param name="visible">visible</param>
        /// <returns>Returns true on success.</returns>
        bool SetVisible(CefMenuCommand commandId, bool visible);

        /// <summary>
        /// Change the visibility at the specified index.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="visible">visible</param>
        /// <returns>Returns true on success.</returns>
        bool SetVisibleAt(int index, bool visible);

        /// <summary>
        /// Returns true if the specified commandId is enabled.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <returns>Returns true if the specified commandId is enabled.</returns>
        bool IsEnabled(CefMenuCommand commandId);

        /// <summary>
        /// Returns true if the specified index is enabled.
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Returns true if the specified index is enabled.</returns>
        bool IsEnabledAt(int index);

        /// <summary>
        /// Change the enabled status of the specified commandId.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <param name="enabled">is enabled</param>
        /// <returns>Returns true on success.</returns>
        bool SetEnabled(CefMenuCommand commandId, bool enabled);

        /// <summary>
        /// Change the enabled status at the specified index.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="enabled">is enabled</param>
        /// <returns>Returns true on success.</returns>
        bool SetEnabledAt(int index, bool enabled);

        /// <summary>
        /// Returns true if the specified commandId is checked. Only applies to check and radio items.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <returns>Returns true if the specified commandId is checked. Only applies to check and radio items.</returns>
        bool IsChecked(CefMenuCommand commandId);

        /// <summary>
        /// Returns true if the specified index is checked. Only applies to check and radio items.
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Returns true if the specified index is checked. Only applies to check and radio items.</returns>
        bool IsCheckedAt(int index);

        /// <summary>
        /// Check the specified commandId. Only applies to check and radio items.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <param name="isChecked">set checked</param>
        /// <returns>Returns true on success.</returns>
        bool SetChecked(CefMenuCommand commandId, bool isChecked);

        /// <summary>
        /// Check the specified index. Only applies to check and radio items.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="isChecked">set checked</param>
        /// <returns>Returns true on success.</returns>
        bool SetCheckedAt(int index, bool isChecked);

        /// <summary>
        /// Returns true if the specified commandId has a keyboard accelerator assigned.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <returns>Returns true if the specified commandId has a keyboard accelerator assigned.</returns>
        bool HasAccelerator(CefMenuCommand commandId);

        /// <summary>
        /// Returns true if the specified index has a keyboard accelerator assigned.
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Returns true if the specified index has a keyboard accelerator assigned.</returns>
        bool HasAcceleratorAt(int index);

        /// <summary>
        /// Set the keyboard accelerator for the specified commandId. 
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <param name="keyCode">keyCode can be any key or character value. </param>
        /// <param name="shiftPressed">shift key pressed</param>
        /// <param name="ctrlPressed">ctrl key pressed</param>
        /// <param name="altPressed">alt key pressed</param>
        /// <returns>Returns true on success.</returns>
        bool SetAccelerator(CefMenuCommand commandId, int keyCode, bool shiftPressed, bool ctrlPressed, bool altPressed);

        /// <summary>
        /// Set the keyboard accelerator at the specified index. keyCode can be any key or character value.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="keyCode">keyCode can be any key or character value. </param>
        /// <param name="shiftPressed">shift key pressed</param>
        /// <param name="ctrlPressed">ctrl key pressed</param>
        /// <param name="altPressed">alt key pressed</param>
        /// <returns>Returns true on success.</returns>
        bool SetAcceleratorAt(int index, int keyCode, bool shiftPressed, bool ctrlPressed, bool altPressed);

        /// <summary>
        /// Remove the keyboard accelerator for the specified commandId.
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <returns>Returns true on success.</returns>
        bool RemoveAccelerator(CefMenuCommand commandId);

        /// <summary>
        /// Remove the keyboard accelerator at the specified index. 
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Returns true on success.</returns>
        bool RemoveAcceleratorAt(int index);

        /// <summary>
        /// Retrieves the keyboard accelerator for the specified commandId. 
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <param name="keyCode">keyCode can be any key or character value. </param>
        /// <param name="shiftPressed">shift key pressed</param>
        /// <param name="ctrlPressed">ctrl key pressed</param>
        /// <param name="altPressed">alt key pressed</param>
        /// <returns>Returns true on success.</returns>
        bool GetAccelerator(CefMenuCommand commandId, out int keyCode, out bool shiftPressed, out bool ctrlPressed, out bool altPressed);

        /// <summary>
        /// Retrieves the keyboard accelerator for the specified index.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="keyCode">keyCode can be any key or character value. </param>
        /// <param name="shiftPressed">shift key pressed</param>
        /// <param name="ctrlPressed">ctrl key pressed</param>
        /// <param name="altPressed">alt key pressed</param>
        /// <returns>Returns true on success.</returns>
        bool GetAcceleratorAt(int index, out int keyCode, out bool shiftPressed, out bool ctrlPressed, out bool altPressed);
    }
}
