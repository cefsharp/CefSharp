using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public abstract class CefBrowserBase : ObjectBase
    {
        public int BrowserId { get; set; }
        public TaskFactory RenderThreadTaskFactory { get; set; }

        public Task<object> EvaluateScript(long frameId, string script)
        {
            return RenderThreadTaskFactory.StartNew(() =>
            {
                return DoEvaluateScript(frameId, script);
            }, TaskCreationOptions.AttachedToParent);
        }

        protected abstract object DoEvaluateScript(long frameId, string script);
    }
}
