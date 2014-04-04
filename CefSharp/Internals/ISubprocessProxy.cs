using System.ServiceModel;

namespace CefSharp.Internals
{
    [ServiceContract(CallbackContract = typeof(ISubprocessCallback))]
    public interface ISubprocessProxy
    {
        [OperationContract]
        void Initialize();

        [OperationContract]
        object EvaluateScript(int frameId, string script, double timeout);

        [OperationContract]
        void Terminate();
    }
}
