﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace CefSharp.Internals
{
    [ServiceContract(CallbackContract = typeof(ISubProcessCallback))]
    public interface ISubProcessProxy
    {
        [OperationContract]
        void Initialize();

        [OperationContract]
        object EvaluateScript(int frameId, string script, double timeout);

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
