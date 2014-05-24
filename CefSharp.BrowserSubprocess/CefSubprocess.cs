using System.Collections.Generic;
using System.Linq;

namespace CefSharp.BrowserSubprocess
{
    public class CefSubprocess : CefAppWrapper
    {
        public int? ParentProcessId { get; private set; }

        public static new CefSubprocess Instance 
        {
            get { return (CefSubprocess)CefAppWrapper.Instance; }
        }

        public static CefSubprocess Create(IEnumerable<string> args)
        {
            const string typePrefix = "--type=";
            var typeArgument = args.SingleOrDefault(arg => arg.StartsWith(typePrefix));

            var type = typeArgument.Substring(typePrefix.Length);
            
            switch (type)
            {
                case "renderer":
                    return new CefRenderprocess(args);
                case "gpu-process":
                    return new CefGpuprocess(args);
                default:
                    return new CefSubprocess(args);
            }
        }

        protected CefSubprocess(IEnumerable<string> args)
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
    }
}