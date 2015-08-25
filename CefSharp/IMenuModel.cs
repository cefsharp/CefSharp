// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IMenuModel
    {
        int Count { get; }
        bool Clear();
        string GetLabelAt(int index);
        int GetCommandIdAt(int index);
        bool Remove(int index);
        bool AddItem(int commandId, string label);
    }
}
