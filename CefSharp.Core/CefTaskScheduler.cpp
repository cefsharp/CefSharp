
#include "Stdafx.h"
#include "CefTaskScheduler.h"


namespace CefSharp
{
    void CefTaskWrapper::Execute()
    {
        try 
        {
            _scheduler->ExecuteTask(this);
        }
        catch ( Exception^ )
        {
            
        }
    };
}