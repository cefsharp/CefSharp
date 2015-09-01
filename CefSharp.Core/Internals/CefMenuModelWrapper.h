// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
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
        public ref class CefMenuModelWrapper : public IMenuModel, public CefWrapper
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
                    return _menu->GetCount();
                }
            }

            virtual bool Clear()
            {
                return _menu->Clear();
            }

            virtual String^ GetLabelAt(int index)
            {
                return StringUtils::ToClr(_menu->GetLabelAt(index));
            }

            virtual CefMenuCommand GetCommandIdAt(int index) 
            {
                return (CefMenuCommand)_menu->GetCommandIdAt(index);
            }

            virtual bool Remove(CefMenuCommand commandId)
            {
                return _menu->Remove((int)commandId);
            }

            virtual bool AddSeparator()
            {
                return _menu->AddSeparator();
            }

            virtual bool AddItem(CefMenuCommand commandId, String^ label) 
            {
                return _menu->AddItem((int)commandId, StringUtils::ToNative(label));
            }
        };
    }
}