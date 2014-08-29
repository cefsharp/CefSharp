using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    [ServiceContract]
    public interface IRenderProcess 
    {
        [OperationContract]
        Task<object> EvaluateScript(int frameId, string script, TimeSpan timeout);
    }
}
