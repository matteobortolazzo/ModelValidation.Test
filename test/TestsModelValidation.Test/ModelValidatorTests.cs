using System;
using ModelValidation.Test;
using ModelValidation.Test.Exceptions;
using TestsModelValidation.Test.Models;
using Xunit;

namespace TestsModelValidation.Test
{
    public class ModelValidatorTests
    {
        [Fact]
        public void Complete_Succeed()
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
                    modelSetup.CheckObject(os => os.IsInvalidWith(r => r.Surname, "Organa"));
                    modelSetup.CheckObject(os => os.IsInvalidWith(r => r.Age, 42));

                    modelSetup.CheckProperty(r => r.Name, ps => ps.IsInvalidWith(null).IsInvalidWith("Lukelongname"));
                    modelSetup.CheckProperty(r => r.Age, ps => ps.IsInvalidWith(901).IsInvalidWith(9));
                });
        }

        [Fact]
        public void RequiredAttributeNotTested_Throws_ValidationAttributeNotTestedException()
        {
            _ = Assert.Throws<ValidationAttributeNotTestedException>(() =>
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
                       modelSetup.CheckProperty(r => r.Name, ps => ps
                           .IsInvalidWith("01234567890")); // MaxLenght
                   });
            });
        }

        [Fact]
        public void InvalidModelFunction_Throws_ModelIsInvalidException()
        {
            _ = Assert.Throws<ModelIsInvalidException>(() =>
            {
                ModelValidator.Test(
                   () => new Rebel
                   {
                       Name = "Luke",
                       Age = 9
                   },
                   modelSetup =>
                   {
                   });
            });            
        }

        [Fact]
        public void ValidModelAfterUpdates_Throws_ModelIsValidException()
        {
            _ = Assert.Throws<ModelIsValidException>(() =>
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
                       modelSetup.CheckProperty(r => r.Name, ps => ps
                           .IsInvalidWith(null)          // Required
                           .IsInvalidWith("01234567890") // MaxLenght
                           .IsInvalidWith("Anakin"));    // Valid
                   });
            });
        }

        [Fact]
        public void ValidPropertyAfterUpdates_Throws_ValidPropertyException()
        {
            _ = Assert.Throws<PropertyIsValidException>(() =>
            {
                var rebel = new Rebel
                {
                    Name = "Luke",
                    Surname = "Skywalker",
                    Age = 18
                };
                ModelValidator.Test(
                   () => rebel,
                   modelSetup =>
                   {
                       modelSetup.CheckProperty(r => r.Age, ps => ps.IsInvalidWith(9));
                       modelSetup.CheckProperty(r => r.Name, ps => ps
                           .IsInvalidWith(null)          // Required
                           .IsInvalidWith("01234567890") // MaxLenght
                           .IsInvalidWith("Anakin"));    // Valid
                   });
            });
        }
    }
}
