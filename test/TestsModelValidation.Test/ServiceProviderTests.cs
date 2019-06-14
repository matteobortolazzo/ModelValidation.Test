using System;
using System.Collections.Generic;
using System.Text;
using ModelValidation.Test;
using ModelValidation.Test.Exceptions;
using TestsModelValidation.Test.Models;
using TestsModelValidation.Test.Services;
using Xunit;

namespace TestsModelValidation.Test
{
    public class ServiceProviderTests
    {
        private readonly ModelValidatorOptions _skipConverageChecksOptions = new ModelValidatorOptions
        {
            CheckClassAttributesCoverage = false,
            CheckPropertiesCoverage = false
        };

        [Fact]
        public void Complete_Succeed()
        {
            ModelValidator.Test(
                () => new Skywalker
                {
                    Name = "Luke",
                    IsJedi = true
                },
                modelSetup =>
                {
                    modelSetup.CheckClass(os => os.IsInvalidWith(r => r.IsJedi, false));

                    modelSetup.CheckProperty(r => r.IsJedi, ps => ps.IsInvalidWith(false));
                },
                new ModelValidatorOptions
                {
                    ServiceProviderSetupAction = (s) =>
                    {
                        s.AddService<IJediService, JediService>();
                    }
                });
        }

        [Fact]
        public void ServiceNotRegisteredProperty_Throws_ServiceNotFoundException()
        {
            _ = Assert.Throws<ServiceNotFoundException>(() =>
            {
                ModelValidator.Test(
                () => new Skywalker
                {
                    Name = "Luke",
                    IsJedi = true
                },
                modelSetup =>
                {
                    modelSetup.CheckProperty(r => r.IsJedi, ps => ps.IsInvalidWith(false));
                },
                _skipConverageChecksOptions);
            });
        }

        [Fact]
        public void ServiceNotRegisteredClass_Throws_ServiceNotFoundException()
        {
            _ = Assert.Throws<ServiceNotFoundException>(() =>
            {
                ModelValidator.Test(
                () => new Skywalker
                {
                    Name = "Luke",
                    IsJedi = true
                },
                modelSetup =>
                {
                    modelSetup.CheckProperty(r => r.IsJedi, ps => ps.IsInvalidWith(false));
                });
            });
        }
    }
}
