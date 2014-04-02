using System;
using System.ServiceModel;

namespace CefSharp.Internals
{
    [ServiceContract]
    public interface ISubProcessCallback
    {
        [OperationContract]
        void Error(Exception ex);
    }
}