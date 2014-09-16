using System.Collections.Generic;
using System.Linq;

namespace CefSharp.BrowserSubprocess
{
    public class CefSubProcess : CefAppWrapper
    {
        public int? ParentProcessId { get; private set; }

        internal CefSubProcess(IEnumerable<string> args)
        {
            LocateParentProcessId(args);
        }

        private void LocateParentProcessId(IEnumerable<string> args)
        {
            // Format being parsed:
            // --channel=3828.2.1260352072\1102986608
            // We only really care about the PID (3828) part.
            const string channelPrefix = "--channel=";
            var channelArgument = args.SingleOrDefault(arg => arg.StartsWith(channelPrefix));
            if (channelArgument == null)
            {
                return;
            }

            var parentProcessId = channelArgument
                .Substring(channelPrefix.Length)
                .Split('.')
                .First();
            ParentProcessId = int.Parse(parentProcessId);
        }

        public override void OnBrowserCreated(CefBrowserWrapper cefBrowserWrapper)
        {
            
        }

        public override void OnBrowserDestroyed(CefBrowserWrapper cefBrowserWrapper)
        {
            
        }
    }
}