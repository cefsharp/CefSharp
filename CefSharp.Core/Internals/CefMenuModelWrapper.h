// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_menu_model.h"

#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefMenuModelWrapper : public IMenuModel, public CefWrapper
        {
        private:
            MCefRefPtr<CefMenuModel> _menu;

        public:
            CefMenuModelWrapper(CefRefPtr<CefMenuModel> &menu) :
                _menu(menu)
            {
            
            }

            !CefMenuModelWrapper()
            {
                _menu = NULL;
            }

            ~CefMenuModelWrapper()
            {
                this->!CefMenuModelWrapper();

                _disposed = true;
            }

            virtual property int Count
            {
                int get()
                {
                    ThrowIfDisposed();

                    return _menu->GetCount();
                }
            }

            virtual bool Clear()
            {
                ThrowIfDisposed();

                return _menu->Clear();
            }

            virtual String^ GetLabelAt(int index)
            {
                ThrowIfDisposed();

                return StringUtils::ToClr(_menu->GetLabelAt(index));
            }

            virtual CefMenuCommand GetCommandIdAt(int index) 
            {
                ThrowIfDisposed();

                return (CefMenuCommand)_menu->GetCommandIdAt(index);
            }

            virtual bool Remove(CefMenuCommand commandId)
            {
                ThrowIfDisposed();

                return _menu->Remove((int)commandId);
            }

            virtual bool AddSeparator()
            {
                ThrowIfDisposed();

                return _menu->AddSeparator();
            }

            virtual bool AddItem(CefMenuCommand commandId, String^ label) 
            {
                ThrowIfDisposed();

                return _menu->AddItem((int)commandId, StringUtils::ToNative(label));
            }

            virtual bool AddCheckItem(CefMenuCommand commandId, String^ label)
            {
                ThrowIfDisposed();

                return _menu->AddCheckItem((int)commandId, StringUtils::ToNative(label));
            }

            virtual bool AddRadioItem(CefMenuCommand commandId, String^ label, int groupId)
            {
                ThrowIfDisposed();

                return _menu->AddRadioItem((int)commandId, StringUtils::ToNative(label), groupId);
            }

            virtual IMenuModel^ AddSubMenu(CefMenuCommand commandId, String^ label)
            {
                ThrowIfDisposed();

                auto subMenu =_menu->AddSubMenu((int)commandId, StringUtils::ToNative(label));

                if (subMenu.get())
                {
                    return gcnew CefMenuModelWrapper(subMenu);
                }

                return nullptr;
            }

            virtual bool InsertSeparatorAt(int index)
            {
                ThrowIfDisposed();

                return _menu->InsertSeparatorAt(index);
            }

            virtual bool InsertItemAt(int index, CefMenuCommand commandId, String^ label)
            {
                ThrowIfDisposed();

                return _menu->InsertItemAt(index, (int)commandId, StringUtils::ToNative(label));
            }

            virtual bool InsertCheckItemAt(int index, CefMenuCommand commandId, String^ label)
            {
                ThrowIfDisposed();

                return _menu->InsertCheckItemAt(index, (int)commandId, StringUtils::ToNative(label));
            }

            virtual bool InsertRadioItemAt(int index, CefMenuCommand commandId, String^ label, int groupId)
            {
                ThrowIfDisposed();

                return _menu->InsertRadioItemAt(index, (int)commandId, StringUtils::ToNative(label), groupId);
            }

            virtual IMenuModel^ InsertSubMenuAt(int index, CefMenuCommand commandId, String^ label)
            {
                ThrowIfDisposed();

                auto subMenu = _menu->InsertSubMenuAt(index, (int)commandId, StringUtils::ToNative(label));

                if (subMenu.get())
                {
                    return gcnew CefMenuModelWrapper(subMenu);
                }

                return nullptr;
            }

            virtual bool RemoveAt(int index)
            {
                ThrowIfDisposed();

                return _menu->RemoveAt(index);
            }

            virtual int GetIndexOf(CefMenuCommand commandId)
            {
                ThrowIfDisposed();

                return _menu->GetIndexOf((int)commandId);
            }

            virtual bool SetCommandIdAt(int index, CefMenuCommand commandId)
            {
                ThrowIfDisposed();

                return _menu->SetCommandIdAt(index, (int)commandId);
            }

            virtual String^ GetLabel(CefMenuCommand commandId)
            {
                ThrowIfDisposed();

                return StringUtils::ToClr(_menu->GetLabel((int)commandId));
            }

            virtual bool SetLabel(CefMenuCommand commandId, String^ label)
            {
                ThrowIfDisposed();

                return _menu->SetLabel((int)commandId, StringUtils::ToNative(label));
            }

            virtual bool SetLabelAt(int index, String^ label)
            {
                ThrowIfDisposed();

                return _menu->SetLabelAt(index, StringUtils::ToNative(label));
            }

            virtual MenuItemType GetType(CefMenuCommand commandId)
            {
                ThrowIfDisposed();

                return (CefSharp::MenuItemType)_menu->GetType((int)commandId);
            }

            virtual MenuItemType GetTypeAt(int index)
            {
                ThrowIfDisposed();

                return (MenuItemType)_menu->GetTypeAt(index);
            }

            virtual int GetGroupId(CefMenuCommand commandId)
            {
                ThrowIfDisposed();

                return _menu->GetGroupId((int)commandId);
            }

            virtual int GetGroupIdAt(int index)
            {
                ThrowIfDisposed();

                return _menu->GetGroupIdAt(index);
            }

            virtual bool SetGroupId(CefMenuCommand commandId, int groupId)
            {
                ThrowIfDisposed();

                return _menu->SetGroupId((int)commandId, groupId);
            }

            virtual bool SetGroupIdAt(int index, int groupId)
            {
                ThrowIfDisposed();

                return _menu->SetGroupIdAt(index, groupId);
            }

            virtual IMenuModel^ GetSubMenu(CefMenuCommand commandId)
            {
                ThrowIfDisposed();

                auto subMenu =_menu->GetSubMenu((int)commandId);

                if (subMenu.get())
                {
                    return gcnew CefMenuModelWrapper(subMenu);
                }

                return nullptr;
            }

            virtual IMenuModel^ GetSubMenuAt(int index)
            {
                ThrowIfDisposed();

                auto subMenu = _menu->GetSubMenuAt(index);

                if (subMenu.get())
                {
                    return gcnew CefMenuModelWrapper(subMenu);
                }

                return nullptr;
            }

            virtual bool IsVisible(CefMenuCommand commandId)
            {
                ThrowIfDisposed();

                return _menu->IsVisible((int)commandId);
            }

            virtual bool IsVisibleAt(int index)
            {
                ThrowIfDisposed();

                return _menu->IsVisibleAt(index);
            }

            virtual bool SetVisible(CefMenuCommand commandId, bool visible)
            {
                ThrowIfDisposed();

                return _menu->SetVisible((int)commandId, visible);
            }

            virtual bool SetVisibleAt(int index, bool visible)
            {
                ThrowIfDisposed();

                return _menu->SetVisibleAt(index, visible);
            }

            virtual bool IsEnabled(CefMenuCommand commandId)
            {
                ThrowIfDisposed();

                return _menu->IsEnabled((int)commandId);
            }

            virtual bool IsEnabledAt(int index)
            {
                ThrowIfDisposed();

                return _menu->IsEnabledAt(index);
            }

            virtual bool SetEnabled(CefMenuCommand commandId, bool enabled)
            {
                ThrowIfDisposed();

                return _menu->SetEnabled((int)commandId, enabled);
            }

            virtual bool SetEnabledAt(int index, bool enabled)
            {
                ThrowIfDisposed();

                return _menu->SetEnabledAt(index, enabled);
            }
            
            virtual bool IsChecked(CefMenuCommand commandId)
            {
                ThrowIfDisposed();

                return _menu->IsChecked((int)commandId);
            }

            virtual bool IsCheckedAt(int index)
            {
                ThrowIfDisposed();

                return _menu->IsCheckedAt(index);
            }

            virtual bool SetChecked(CefMenuCommand commandId, bool isChecked)
            {
                ThrowIfDisposed();

                return _menu->SetChecked((int)commandId, isChecked);
            }

            virtual bool SetCheckedAt(int index, bool isChecked)
            {
                ThrowIfDisposed();

                return _menu->SetCheckedAt(index, isChecked);
            }

            virtual bool HasAccelerator(CefMenuCommand commandId)
            {
                ThrowIfDisposed();

                return _menu->HasAccelerator((int)commandId);
            }

            virtual bool HasAcceleratorAt(int index)
            {
                ThrowIfDisposed();

                return _menu->HasAcceleratorAt(index);
            }

            virtual bool SetAccelerator(CefMenuCommand commandId, int keyCode, bool shiftPressed, bool ctrlPressed, bool altPressed)
            {
                ThrowIfDisposed();

                return _menu->SetAccelerator((int)commandId, keyCode, shiftPressed, ctrlPressed, altPressed);
            }

            virtual bool SetAcceleratorAt(int index, int keyCode, bool shiftPressed, bool ctrlPressed, bool altPressed)
            {
                ThrowIfDisposed();

                return _menu->SetAcceleratorAt(index, keyCode, shiftPressed, ctrlPressed, altPressed);
            }

            virtual bool RemoveAccelerator(CefMenuCommand commandId)
            {
                ThrowIfDisposed();

                return _menu->RemoveAccelerator((int)commandId);
            }

            virtual bool RemoveAcceleratorAt(int index)
            {
                ThrowIfDisposed();

                return _menu->RemoveAcceleratorAt(index);
            }

            virtual bool GetAccelerator(CefMenuCommand commandId, int% keyCode, bool% shiftPressed, bool% ctrlPressed, bool% altPressed)
            {
                ThrowIfDisposed();

                int key;
                bool shift;
                bool ctrl;
                bool alt;

                auto result = _menu->GetAccelerator((int)commandId, key, shift, ctrl, alt);

                keyCode = key;
                shiftPressed = shift;
                ctrlPressed = ctrl;
                altPressed = alt;

                return result;
            }

            virtual bool GetAcceleratorAt(int index, int% keyCode, bool% shiftPressed, bool% ctrlPressed, bool% altPressed)
            {
                ThrowIfDisposed();

                int key;
                bool shift;
                bool ctrl;
                bool alt;

                auto result = _menu->GetAcceleratorAt(index, key, shift, ctrl, alt);

                keyCode = key;
                shiftPressed = shift;
                ctrlPressed = ctrl;
                altPressed = alt;

                return result;
            }
        };
    }
}