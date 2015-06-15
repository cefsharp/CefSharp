using System.ServiceModel;

namespace CefSharp.Internals
{
    [ServiceContract]
    public interface IRenderProcess
    {
        [OperationContract]
        void Done();
    }
}
