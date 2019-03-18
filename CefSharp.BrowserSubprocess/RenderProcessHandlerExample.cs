using CefSharp.RenderProcess;

namespace CefSharp.BrowserSubprocess
{
    public class RenderProcessHandlerExample : IRenderProcessHandler
    {
        void IRenderProcessHandler.OnContextCreated(IBrowser browser, IFrame frame, IV8Context context)
        {
            //TODO: browser and frame are currently null as there is no C++ wrapper in the render process currently.
            V8Exception ex;
            if(!context.Execute("Object.freeze(console)", "about:blank", 1, out ex))
            {
                //TODO: Do something with the exception
            }
        }

        void IRenderProcessHandler.OnContextReleased(IBrowser browser, IFrame frame, IV8Context context)
        {
            
        }
    }
}
