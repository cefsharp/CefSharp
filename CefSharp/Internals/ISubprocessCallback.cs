using System;
using System.ServiceModel;

namespace CefSharp.Internals
{
    [ServiceContract]
    public interface ISubprocessCallback
    {
        [OperationContract]
        void Error(Exception ex);
    }
}