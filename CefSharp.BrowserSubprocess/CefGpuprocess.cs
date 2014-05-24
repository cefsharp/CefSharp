using System.Collections.Generic;

namespace CefSharp.BrowserSubprocess
{
    public class CefGpuprocess : CefSubprocess
    {
        public static new CefGpuprocess Instance
        {
            get { return (CefGpuprocess)CefSubprocess.Instance; }
        }

        public CefGpuprocess(IEnumerable<string> args) 
            : base(args)
        {
        }
    }
}
