// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "Internals/CefWindowInfoWrapper.h"

using namespace System;

namespace CefSharp
{
	public ref class CefObjectFactory sealed
	{
	public:
		static IWindowInfo^ CreateWindowInfo()
		{
			return gcnew CefWindowInfoWrapper();
		}
	};
}