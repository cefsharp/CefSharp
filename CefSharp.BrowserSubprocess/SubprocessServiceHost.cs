using CefSharp.Internals;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace CefSharp.BrowserSubprocess
{
    public class SubprocessServiceHost : ServiceHost, ISubprocessCallback
    {
        public SubprocessProxy Service { get; private set; }

        public event Action<ISubprocessCallback> Initialized;

        public void Initialize(SubprocessProxy service)
        {
            Service = service;

            var handler = Initialized;
            if (handler != null)
            {
                handler(this);
            }
        }

        private SubprocessServiceHost()
            : base(typeof(SubprocessProxy), new Uri[0])
        {
        }

        public static SubprocessServiceHost Create(int parentProcessId, int browserId)
        {
            var host = CreateServiceHost();

            var serviceName = SubprocessProxyFactory.GetServiceName(parentProcessId, browserId);
            host.KillExistingServiceIfNeeded(serviceName);

            host.AddServiceEndpoint(
                typeof(ISubprocessProxy),
                new NetNamedPipeBinding(),
                new Uri(serviceName)
            );

            var dataContractResolver = new SubprocessDataContractResolver();
            var surrogate = new SubprocessSurrogates();

            host.ApplyOperationBehavior(
             (op) => new DataContractSerializerOperationBehavior(op),
                (b) =>
                {
                    b.DataContractResolver = dataContractResolver;
                    b.DataContractSurrogate = surrogate;
                }  );

            host.Open();
            return host;
        }

        private static SubprocessServiceHost CreateServiceHost()
        {
            var host = new SubprocessServiceHost();
            var serviceDebugBehavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();

            if (serviceDebugBehavior == null)
            {
                serviceDebugBehavior = new ServiceDebugBehavior
                {
                    IncludeExceptionDetailInFaults = true
                };
                host.Description.Behaviors.Add(serviceDebugBehavior);
            }
            else
            {
                serviceDebugBehavior.IncludeExceptionDetailInFaults = true;
            }

            return host;
        }

        private void KillExistingServiceIfNeeded(string serviceName)
        {
            // It might be that there is an existing process already bound to this port. We must get rid of that one, so that the
            // endpoint address gets available for us to use.
            try
            {
                var javascriptProxy = SubprocessProxyFactory.CreateSubprocessProxyClient(serviceName, this, TimeSpan.FromSeconds(1));
                javascriptProxy.Terminate();
            }
            catch
            {
                // We assume errors at this point are caused by things like the endpoint not being present (which will happen in
                // the first render subprocess instance).
            }
        }

        public void ApplyOperationBehavior<T>(Func<OperationDescription,T> behaviorFactory,Action<T> behaviorManipulation)
            where T : class, IOperationBehavior
        {
            foreach (ServiceEndpoint ep in Description.Endpoints)
            {
                foreach (OperationDescription op in ep.Contract.Operations)
                {
                    T behavior = op.Behaviors.Find<T>();
                    if (behavior == null)
                    {
                        behavior = behaviorFactory(op);
                        op.Behaviors.Add(behavior);
                    }
                    behaviorManipulation(behavior);
                }
            }
        }

        public object CallMethod(int objectId, string name, object[] parameters)
        {
            return Service.Callback.CallMethod(objectId, name, parameters);
        }

        public object GetProperty(int objectId, string name)
        {
            return Service.Callback.GetProperty(objectId, name);
        }

        public object SetProperty(int objectId, string name)
        {
            return Service.Callback.SetProperty(objectId, name);
        }
    }
}