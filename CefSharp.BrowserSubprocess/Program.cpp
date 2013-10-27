// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "CefRenderProcess.h"

void LogCommandLine(LPTSTR cmdLine);

int APIENTRY wWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPTSTR lpCmdLine, int nCmdShow)
{
	CefMainArgs mainArgs(hInstance);
	LogCommandLine(lpCmdLine);

	return ExecuteCefRenderProcess(mainArgs);
}

void LogCommandLine(LPTSTR cmdLine)
{
	std::wstring message = L"BrowserSubprocess starting up with command line: ";
	message = message + cmdLine;
	OutputDebugString(message.c_str());
}