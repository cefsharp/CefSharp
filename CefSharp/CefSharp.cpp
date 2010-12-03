// This is the main DLL file.

#include "stdafx.h"

#include "Settings.h"
#include "BrowserSettings.h"
#include "ReturnValue.h"
#include "CefSharp.h"
#include "HandlerAdapter.h"
#include "BrowserControl.h"

namespace CefSharp 
{

/*
public ref class WindowInfo
{
internal:
    CAutoNativePtr<CefWindowInfo> _windowInfo;

public:
    WindowInfo(IntPtr handle)
    {		
        _windowInfo = new CefWindowInfo();

        HWND hWnd = static_cast<HWND>(handle.ToPointer());
        RECT rect;
        GetClientRect(hWnd, &rect);
        _windowInfo->SetAsChild(hWnd, rect);
    }
};*/

public ref class CEF sealed
{
public:
    static property String^ CefSharpVersion
    {
        String^ get()
        {
            return "0.1";
        }
    }

    static property String^ CefVersion
    {
        String^ get()
        {
            return "trunk r149";
        }
    }

    static property String^ ChromiumVersion
    {
        String^ get()
        {
            return "trunk r66269";
        }
    }

    static bool Initialize(Settings^ settings, BrowserSettings^ browserSettings)
    {
        return CefInitialize(*settings->_cefSettings, *browserSettings->_browserSettings);
    }

    static void Shutdown()
    {
        CefShutdown();
    }
};


///////////////////////////////// END OF NAMESPACE
}

__declspec(dllexport ) int FakeExportedMethodToCauseLibFileToBeCreated()
{
    return 0;
}