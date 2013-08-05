// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    public interface class IRequestResponse
    {
        /// cancel the request, return nothing
        void Cancel();

        /// the current request
        property IRequest^ Request { IRequest^ get(); };

        /// respond with redirection to the provided URL
        void Redirect(String^ url);

        /// respond with data from Stream
        void RespondWith(Stream^ stream, String^ mimeType);
    };
}
