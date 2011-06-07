#include "stdafx.h"

#include "Settings.h"
#include "BrowserSettings.h"
#include "ReturnValue.h"
#include "CefSharp.h"
#include "ClientAdapter.h"
#include "CefFormsWebBrowser.h"

namespace CefSharp 
{

}

__declspec(dllexport ) int FakeExportedMethodToCauseLibFileToBeCreated()
{
    return 0;
}