// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Class representing popup window features. 
    /// </summary>
    public interface IPopupFeatures
    {
        int X { get; }
        int XSet { get; }
        int Y { get; }
        int YSet { get; }
        int Width { get; }
        int WidthSet { get; }
        int Height { get; }
        int HeightSet { get; }
        bool MenuBarVisible { get; }
        bool StatusBarVisible { get; }
        bool ToolBarVisible { get; }
        bool ScrollbarsVisible { get; }
    }
}
