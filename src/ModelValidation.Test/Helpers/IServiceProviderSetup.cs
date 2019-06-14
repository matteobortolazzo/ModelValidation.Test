using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Helpers
{
    public interface IServiceProviderSetup
    {
        IServiceProviderSetup AddService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;
        IServiceProviderSetup AddService<TService>(TService implementation)
            where TService : class;
        IServiceProviderSetup AddService(Type serviceType, Type implementationType);
        IServiceProviderSetup AddService(Type serviceType, object implementation);
    }
}
