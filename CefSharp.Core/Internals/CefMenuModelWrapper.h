// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_menu_model.h"

namespace CefSharp
{
    public ref class CefMenuModelWrapper : public IMenuModel
    {
    private:
        MCefRefPtr<CefMenuModel> _menu;

    public:
        CefMenuModelWrapper(CefRefPtr<CefMenuModel> &menu) : _menu(menu)
        {
            
        }

        !CefMenuModelWrapper()
        {
            _menu = NULL;
        }

        ~CefMenuModelWrapper()
        {
            this->!CefMenuModelWrapper();
        }

        virtual bool Clear()
        {
            return _menu->Clear();
        }

		virtual int GetCount() {
			return _menu->GetCount();
		}

		virtual String^ GetLabelAt(int index) {
			return StringUtils::ToClr(_menu->GetLabelAt(index));
		}

		virtual int GetCommandIdAt(int index) {
			return _menu->GetCommandIdAt(index);
		}

		virtual bool Remove(int index) {
			return _menu->Remove(index);
		}

		virtual bool AddSeparator() {
			return _menu->AddSeparator();
		}

		virtual bool AddItem(int command_id, String^ label) {
			return _menu->AddItem(command_id, StringUtils::ToNative(label));
		}
    };
}