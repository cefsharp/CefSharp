using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp.BrowserSubprocess
{
    public class CefSubprocess : ManagedCefApp
    {
        #region Singleton pattern

        public static CefSubprocess Instance { get; private set; }

        public CefSubprocess(IEnumerable<string> args)
        {
            Instance = this;
           
            LocateParentProcessId(args);
        }

        #endregion

        protected override void DoDispose(bool isDisposing)
        {
            DisposeMember(ref javascriptServiceHost);
            DisposeMember(ref browser);

            Instance = null;

            base.DoDispose(isDisposing);
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

        private SubprocessServiceHost javascriptServiceHost;
        private CefBrowserBase browser;

        public int? ParentProcessId { get; private set; }

        public CefBrowserBase Browser
        {
            get { return browser; }
        }

        public SubprocessServiceHost ServiceHost
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

            Task.Factory.StartNew(() => javascriptServiceHost = SubprocessServiceHost.Create(ParentProcessId.Value, cefBrowserWrapper.BrowserId));
        }

        public int Run()
        {
            var wrapper = new CefAppWrapper(OnBrowserCreated);
            return wrapper.Run();
        }
    }
}