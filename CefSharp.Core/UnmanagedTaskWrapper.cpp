
#include "Stdafx.h"
#include "UnmanagedTaskWrapper.h"


namespace CefSharp
{
    void CefTaskWrapper::Execute()
    {
        try 
        {
            _scheduler->ExecuteTask(this);
        }
        catch ( Exception^ ex )
        {
            
        }
    };
}