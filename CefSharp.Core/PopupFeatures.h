// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    /// <summary>
    /// Class representing popup window features.
    /// </summary>
    public ref class PopupFeatures : IPopupFeatures
    {
    private:
        const CefPopupFeatures* _popupFeatures;

    public:
        PopupFeatures(const CefPopupFeatures* popupFeatures)
        {
            _popupFeatures = popupFeatures;
        }

        !PopupFeatures()
        {
            _popupFeatures = NULL;
        }

        ~PopupFeatures()
        {
            this->!PopupFeatures();
        }

        virtual property int X
        {
            int get() { return _popupFeatures->x; }
        }

        virtual property int XSet
        {
            int get() { return _popupFeatures->xSet; }
        }

        virtual property int Y
        {
            int get() { return _popupFeatures->y; }
        }

        virtual property int YSet
        {
            int get() { return _popupFeatures->ySet; }
        }

        virtual property int Width
        {
            int get() { return _popupFeatures->width; }
        }

        virtual property int WidthSet
        {
            int get() { return _popupFeatures->width; }
        }

        virtual property int Height
        {
            int get() { return _popupFeatures->height; }
        }

        virtual property int HeightSet
        {
            int get() { return _popupFeatures->heightSet; }
        }

        virtual property bool MenuBarVisible
        {
            bool get() { return _popupFeatures->menuBarVisible == 1; }
        }

        virtual property bool StatusBarVisible
        {
            bool get() { return _popupFeatures->statusBarVisible == 1; }
        }

        virtual property bool ToolBarVisible
        {
            bool get() { return _popupFeatures->toolBarVisible == 1; }
        }

        virtual property bool ScrollbarsVisible
        {
            bool get() { return _popupFeatures->scrollbarsVisible == 1; }
        }

        /*property List<String^>^ AdditionalFeatures
        {
            List<String^>^ get() { return StringUtils::ToClr(_popupFeatures->additionalFeatures); }
            void set(List<String^>^ value) { _popupFeatures->dialog = value; }
        }*/
    };
}