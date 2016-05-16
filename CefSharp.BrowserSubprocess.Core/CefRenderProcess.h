// Copyright © 2010-2016 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "CefSubProcess.h"
#include "CefBrowserWrapper.h"

using namespace System;

namespace CefSharp
{
	namespace BrowserSubprocess
	{
		public ref class CefRenderProcess : CefSubProcess
		{
		private:
			Nullable<int> parentBrowserId;

			/// <summary>
			/// The PID for the parent (browser) process
			/// </summary>
			Nullable<int> parentProcessId;

		public:
			CefRenderProcess(IEnumerable<String^>^ args) : CefSubProcess(args)
			{
				parentProcessId = CommandLineArgsParser::LocateParentProcessId(args);
			}

			void OnBrowserCreated(CefBrowserWrapper^ browser) override;
			void OnBrowserDestroyed(CefBrowserWrapper^ browser) override;

		};
	}
}