using System.Collections.Generic;

namespace CefSharp.BrowserSubprocess
{
    public class CefGpuProcess : CefSubProcess
    {
        public CefGpuProcess(IEnumerable<string> args)
            : base(args)
        {
        }
    }
}
