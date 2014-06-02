using System.Collections.Generic;

namespace CefSharp.BrowserSubprocess
{
    public class CefGpuProcess : CefSubProcess
    {
        public static new CefGpuProcess Instance
        {
            get { return (CefGpuProcess)CefSubProcess.Instance; }
        }

        public CefGpuProcess(IEnumerable<string> args) 
            : base(args)
        {
        }
    }
}
