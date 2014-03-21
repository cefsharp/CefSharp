#include "Stdafx.h"

#include "Wrappers\CefAppWrapper.h"

namespace CefSharp
{
    CefAppWrapper::CefAppWrapper(CefSubprocessBase^ managedApp)
    {
        _managedApp = managedApp;
        cefApp = new CefRefPtr<CefAppUnmanagedWrapper>(new CefAppUnmanagedWrapper(this));
    }

    int CefAppWrapper::Run(array<String^>^ args)
    {
        _managedApp->FindParentProcessId(args);

        auto hInstance = Process::GetCurrentProcess()->Handle;

        CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());

        return CefExecuteProcess(cefMainArgs, *(CefRefPtr<CefApp>*)cefApp);
    }
}