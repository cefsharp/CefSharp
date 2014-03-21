using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CefSharp.Internals
{
    public class CefSubprocess : ManagedCefApp
    {
        #region Singleton pattern

        public static CefSubprocess Instance { get; private set; }

        public CefSubprocess()
        {
            Instance = this;
        }

        protected override void DoDispose(bool isDisposing)
        {
            DisposeMember(ref javascriptServiceHost);
            DisposeMember(ref browser);

            Instance = null;

            base.DoDispose(isDisposing);
        }

        #endregion

        public void FindParentProcessId(IEnumerable<string> args)
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


        private SubProcessServiceHost javascriptServiceHost;
        private CefBrowserBase browser;

        public int? ParentProcessId { get; private set; }

        public CefBrowserBase Browser
        {
            get { return browser; }
        }

        public SubProcessServiceHost ServiceHost
        {
            get { return javascriptServiceHost; }
        }

        public override void OnBrowserCreated(CefBrowserBase cefBrowserWrapper)
        {
            browser = cefBrowserWrapper;

            if (ParentProcessId == null)
            {
                return;
            }

            Task.Factory.StartNew(() => javascriptServiceHost = SubProcessServiceHost.Create(ParentProcessId.Value, cefBrowserWrapper.BrowserId));
        }
    }
}