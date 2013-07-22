// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "IRenderWebBrowser.h"
#include "Internals/RenderClientAdapterInternal.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class RenderClientAdapter
    {
    private:
        RenderClientAdapterInternal* _renderClientAdapterInternal;

    public:
        RenderClientAdapter(IRenderWebBrowser^ offscreenBrowserControl)
        {
            _renderClientAdapterInternal = new RenderClientAdapterInternal(offscreenBrowserControl);
        }

        ~RenderClientAdapter()
        {
            delete _renderClientAdapterInternal;
        }
    };
}
