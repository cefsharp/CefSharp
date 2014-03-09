using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace CefSharp.Internals
{
    public class CefSubprocessBase : CefAppBase
    {
        #region Singleton pattern

        public static CefSubprocessBase Instance { get; private set; }

        public CefSubprocessBase()
        {
            Instance = this;
        }

        protected override void DoDispose(bool isDisposing)
        {
            DisposeMember(ref _javascriptServiceHost);
            DisposeMember(ref _browser);

            Instance = null;

            base.DoDispose(isDisposing);
        }

        #endregion

        public void FindParentProcessId(IEnumerable<string> args)
        {
            // Format being parsed:
            // --channel=3828.2.1260352072\1102986608
            // We only really care about the PID (3828) part.
            var channelPrefix = "--channel=";
            var channelArgument = args.SingleOrDefault(arg => arg.StartsWith(channelPrefix));
            if(channelArgument == null)
            {
                return;
            }

            var parentProcessId = channelArgument
                .Substring(channelPrefix.Length)
                .Split('.')
                .First();
            ParentProcessId = int.Parse(parentProcessId);
        }


        private SubProcessServiceHost _javascriptServiceHost;
        private CefBrowserBase _browser;

        public int? ParentProcessId { get; private set; }

        public CefBrowserBase Browser
        {
            get { return _browser; }
        }

        public SubProcessServiceHost ServiceHost
        {
            get { return _javascriptServiceHost; }
        }


        public override void OnBrowserCreated(CefBrowserBase cefBrowserWrapper)
        {
            _browser = cefBrowserWrapper;

            if(ParentProcessId == null)
            {
                return;
            }

            Task.Factory.StartNew(() => _javascriptServiceHost = SubProcessServiceHost.Create(ParentProcessId.Value, cefBrowserWrapper.BrowserId));
        }
    }
}