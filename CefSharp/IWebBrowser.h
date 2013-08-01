// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "ConsoleMessageEventArgs.h"
#include "LoadCompletedEventArgs.h"

namespace CefSharp
{
    public interface class IWebBrowser
    {
        /// <summary>
        /// Event handler for receiving Javascript console messages being sent from web pages.
        /// </summary>
        event ConsoleMessageEventHandler^ ConsoleMessage;

        /// <summary>
        /// Event handler that will get called whenever page loading is complete.
        /// </summary>        
        event LoadCompletedEventHandler^ LoadCompleted;
    };
}