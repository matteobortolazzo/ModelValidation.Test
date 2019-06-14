using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Helpers
{
    /// <summary>
    /// Class to configure a service provider to use during validation.
    /// </summary>
    public interface IServiceProviderSetup
    {
        /// <summary>
        /// Adds a service of the type specified in TService with an implementation type specified in TImplementation.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>A reference to this instance.</returns>
        IServiceProviderSetup AddService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// Adds a service of the type specified in TService with the provided implementation.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementation">The implementation instance to use.</param>
        /// <returns>A reference to this instance.</returns>
        IServiceProviderSetup AddService<TService>(TService implementation)
            where TService : class;

        /// <summary>
        /// Adds a service of the type specified in serviceType with an implementation type specified in implementationType.
        /// </summary>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <returns>A reference to this instance.</returns>
        IServiceProviderSetup AddService(Type serviceType, Type implementationType);

        /// <summary>
        /// Adds a service of the type specified in serviceType with the provided implementation.
        /// </summary>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="implementation">The implementation instance to use.</param>
        /// <returns>A reference to this instance.</returns>
        IServiceProviderSetup AddService(Type serviceType, object implementation);
    }
}
