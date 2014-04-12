using System.ServiceModel;

namespace CefSharp.Internals
{
    [ServiceContract]
    public interface ISubprocessCallback
    {
        [OperationContract]
        object CallMethod(int objectId, string name, object[] parameters);

        [OperationContract]
        object GetProperty(int objectId, string name);

        [OperationContract]
        object SetProperty(int objectId, string name);
    }
}