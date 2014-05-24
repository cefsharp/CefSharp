using System.ServiceModel;

namespace CefSharp.Internals
{
    [ServiceContract(CallbackContract = typeof(IRenderprocess))]
    public interface IBrowserProcess
    {
        [OperationContract]
        object CallMethod(int objectId, string name, object[] parameters);

        [OperationContract]
        object GetProperty(int objectId, string name);

        [OperationContract]
        void SetProperty(int objectId, string name, object value);
        
        [OperationContract]
        JavascriptObject GetRegisteredJavascriptObjects();
    }
}