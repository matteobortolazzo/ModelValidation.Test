using System;
using System.Collections.Generic;
using System.Text;
using ModelValidation.Test.Exceptions;

namespace ModelValidation.Test.Helpers
{
    internal class TestServiceProvider : IServiceProvider, IServiceProviderSetup
    {
        private readonly Dictionary<Type, object> _services;

        public TestServiceProvider()
        {
            _services = new Dictionary<Type, object>();
        }

        public IServiceProviderSetup AddService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            var implementation = Activator.CreateInstance(typeof(TImplementation));
            _services.Add(typeof(TService), implementation);
            return this;
        }

        public IServiceProviderSetup AddService<TService>(TService implementation)
            where TService : class
        {
            _services.Add(typeof(TService), implementation);
            return this;
        }

        public IServiceProviderSetup AddService(Type serviceType, Type implementationType)
        {
            var implementation = Activator.CreateInstance(implementationType);
            _services.Add(serviceType, implementation);
            return this;
        }

        public IServiceProviderSetup AddService(Type serviceType, object implementation)
        {
            _services.Add(serviceType, implementation);
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (_services.ContainsKey(serviceType))
            {
                return _services[serviceType];
            }

            throw new ServiceNotFoundException($"Service of type {serviceType.Name} is not registered.");
        }
    }
}
