// This is the main DLL file.

#include "stdafx.h"

#include "Settings.h"
#include "BrowserSettings.h"
#include "ReturnValue.h"
#include "CefSharp.h"
#include "ClientAdapter.h"
#include "CefWebBrowser.h"

namespace CefSharp 
{

}

__declspec(dllexport ) int FakeExportedMethodToCauseLibFileToBeCreated()
{
    return 0;
}