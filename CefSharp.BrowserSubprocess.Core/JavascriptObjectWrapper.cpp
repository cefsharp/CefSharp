// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptObjectWrapper.h"
#include "CefAppWrapper.h"

using namespace System;

namespace CefSharp
{
	Object^ JavascriptObjectWrapper::GetProperty(String^ memberName)
	{
		auto browserProxy = CefAppWrapper::Instance->CreateBrowserProxy();

		return browserProxy->GetProperty(_object->Id, memberName);
	};
}