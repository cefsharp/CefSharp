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

///////////////////////////////// END OF NAMESPACE
}

__declspec(dllexport ) int FakeExportedMethodToCauseLibFileToBeCreated()
{
    return 0;
}