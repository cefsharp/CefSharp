// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "WebPluginInfoVisitor.h"
#include "CefWebPluginInfoWrapper.h"
namespace CefSharp
{
    bool WebPluginInfoVisitor::Visit(CefRefPtr<CefWebPluginInfo> info, int count, int totale)
    {
        CefWebPluginInfoWrapper^ pi = gcnew CefWebPluginInfoWrapper(info);
        return this->_visitor->Visit(pi, count, totale);
    }
}