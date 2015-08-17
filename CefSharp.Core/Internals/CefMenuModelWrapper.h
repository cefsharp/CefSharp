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
    };
}