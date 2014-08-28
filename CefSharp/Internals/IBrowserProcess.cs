using System.ServiceModel;

namespace CefSharp.Internals
{
    [ServiceContract(CallbackContract = typeof(IRenderProcess))]
    public interface IBrowserProcess
    {
        [OperationContract]
        object CallMethod(long objectId, string name, object[] parameters);

        [OperationContract]
        object GetProperty(long objectId, string name);

        [OperationContract]
        void SetProperty(long objectId, string name, object value);
        
        [OperationContract]
        JavascriptObject GetRegisteredJavascriptObjects();
    }
}