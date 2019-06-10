using System;
using System.Collections.Generic;
using System.Text;
using ModelValidation.Test;
using ModelValidation.Test.Extensions;
using TestsModelValidation.Test.Models;
using Xunit;

namespace TestsModelValidation.Test
{
    public class ModelPropertyValidatorSetupExtensionsTests
    {
        [Fact]
        public void IsRequried_Succeed()
        {
            ModelValidator.Test(
                () => new Rebel
                {
                    Name = "Luke",
                    Surname = "Skywalker",
                    Age = 18
                },
                modelSetup =>
                {
                    modelSetup.CheckProperty(r => r.Name, ps => ps.IsRequired().IsInvalidWith("Lukelongname"));
                });
        }

        [Fact]
        public void HasMaxLenght_Succeed()
        {
            ModelValidator.Test(
                () => new Rebel
                {
                    Name = "Luke",
                    Surname = "Skywalker",
                    Age = 18
                },
                modelSetup =>
                {
                    modelSetup.CheckProperty(r => r.Name, ps => ps.IsInvalidWith(null).HasMaxLenght(10));
                });
        }

        [Fact]
        public void InRange_Succeed()
        {
            ModelValidator.Test(
                () => new Rebel
                {
                    Name = "Luke",
                    Surname = "Skywalker",
                    Age = 18
                },
                modelSetup =>
                {
                    modelSetup.CheckProperty(r => r.Age, ps => ps.InRange(10, 900));
                });
        }
    }
}
