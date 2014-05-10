using System.ServiceModel;

namespace CefSharp.Internals
{
    [ServiceContract(CallbackContract = typeof(ISubprocessCallback))]
    public interface ISubprocessProxy
    {
        [OperationContract]
        void Initialize();

        // TODO: Jan Eggers (and the rest of the world, basically) is right. We should do away with this method and instead use
        // a Task-based approach. The question is just how to make that work over the WCF channel? But perhaps this isn't really
        // a big problem if we just find a reasonable way to correlate the result back to the caller...
        [OperationContract]
        object EvaluateScript(int frameId, string script, double timeout);

        [OperationContract]
        void Terminate();

        [OperationContract]
        void RegisterJavascriptObjects(JavascriptObject obj);
    }
}
