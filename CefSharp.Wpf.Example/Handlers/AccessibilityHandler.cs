// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Enums;

namespace CefSharp.Wpf.Example.Handlers
{
    public class AccessibilityHandler : IAccessibilityHandler
    {
        public void OnAccessibilityLocationChange(ICefValue value)
        {
            var cefValueType = value.GetCefValueType();

            if (cefValueType == CefValueType.List)
            {
                var cefValues = value.GetList();

                foreach (var listValue in cefValues)
                {
                    var type = listValue.GetCefValueType();

                    if (type == CefValueType.Dictionary)
                    {
                        var dictionary = listValue.GetDictionary();
                    }
                }
            }
        }

        public void OnAccessibilityTreeChange(ICefValue value)
        {
            var cefValueType = value.GetCefValueType();
        }
    }
}
