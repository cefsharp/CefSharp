// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    namespace Core
    {
        /// <summary>
        /// Class representing popup window features.
        /// </summary>
        /// <exclude />
        [System::ComponentModel::EditorBrowsableAttribute(System::ComponentModel::EditorBrowsableState::Never)]
        public ref class PopupFeatures : IPopupFeatures
        {
        private:
            const CefPopupFeatures* _popupFeatures;

        internal:
            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="popupFeatures">The popup features.</param>
            PopupFeatures(const CefPopupFeatures* popupFeatures)
            {
                _popupFeatures = popupFeatures;
            }

        public:
            !PopupFeatures()
            {
                _popupFeatures = NULL;
            }

            ~PopupFeatures()
            {
                this->!PopupFeatures();
            }

            virtual property System::Nullable<int> X
            {
                System::Nullable<int> get() { return _popupFeatures->xSet ? _popupFeatures->x : Nullable<int>(); }
            }

            virtual property System::Nullable<int> Y
            {
                System::Nullable<int> get() { return _popupFeatures->ySet ? _popupFeatures->y : Nullable<int>(); }
            }

            virtual property System::Nullable<int> Width
            {
                System::Nullable<int> get() { return _popupFeatures->widthSet ? _popupFeatures->width : Nullable<int>(); }
            }

            virtual property System::Nullable<int> Height
            {
                System::Nullable<int> get() { return _popupFeatures->heightSet ? _popupFeatures->height : Nullable<int>(); }
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
        };
    }
}
