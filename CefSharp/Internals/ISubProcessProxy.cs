using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    [ServiceContract(CallbackContract = typeof(ISubProcessCallback))]
    public interface ISubProcessProxy : IDisposable
    {
        [OperationContract]
        void Initialize();

        [OperationContract]
        Task<object> EvaluateScript(long frameId, string script);
        
        [OperationContract]
        void Terminate();
    }

    [ServiceContract]
    public interface ISubProcessCallback
    {
        [OperationContract]
        void Error(Exception ex);
    }
}
