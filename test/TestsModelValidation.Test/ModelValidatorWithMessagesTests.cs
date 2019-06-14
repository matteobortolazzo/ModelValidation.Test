using System;
using ModelValidation.Test;
using ModelValidation.Test.Exceptions;
using TestsModelValidation.Test.Models;
using Xunit;

namespace TestsModelValidation.Test
{
    public class ModelValidatorWithMessagesTests
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
                () => new Stormtrooper
                {
                    IsCloned = true,
                    Leader = "Palpatine"
                },
                modelSetup =>
                {
                    modelSetup.CheckClass(os => os.IsInvalidWith(r => r.IsCloned, false), "Trooper must be a clone.");

                    modelSetup.CheckProperty(r => r.Leader, ps => ps.IsInvalidWith(null, "Sith leader is required."));
                },
                _skipConverageChecksOptions);
        }

        [Fact]
        public void ObjectWrongMessage_Throws_ModelIsValidException()
        {
            _ = Assert.Throws<InvalidErrorMessageException>(() =>
            {
                ModelValidator.Test(
                     () => new Stormtrooper
                     {
                         IsCloned = true,
                         Leader = "Palpatine"
                     },
                     modelSetup =>
                     {
                         modelSetup.CheckClass(os => os.IsInvalidWith(r => r.IsCloned, false), "Trooper should be a clone.");
                     },
                    _skipConverageChecksOptions);   
            });
        }

        [Fact]
        public void PropertyWrongMessage_Throws_PropertyIsValidException()
        {
            _ = Assert.Throws<InvalidErrorMessageException>(() =>
            {
                ModelValidator.Test(
                    () => new Stormtrooper
                    {
                        IsCloned = true,
                        Leader = "Palpatine"
                    },
                    modelSetup =>
                    {
                        modelSetup.CheckProperty(r => r.Leader, ps => ps.IsInvalidWith(null, "Leader is required."));
                    },
                    _skipConverageChecksOptions);
            });
        }


        [Fact]
        public void InvalidModelFunction_Throws_ModelIsInvalidException()
        {
            _ = Assert.Throws<ModelIsInvalidException>(() =>
            {
                ModelValidator.Test(
                   () => new Stormtrooper
                   {
                       IsCloned = false,
                       Leader = "Palpatine"
                   },
                   modelSetup =>
                   {
                   },
                    _skipConverageChecksOptions);
            });            
        }

        [Fact]
        public void ValidModelAfterUpdates_Throws_ModelIsValidException()
        {
            _ = Assert.Throws<ModelIsValidException>(() =>
            {
                ModelValidator.Test(
                   () => new Stormtrooper
                   {
                       IsCloned = true,
                       Leader = "Palpatine"
                   },
                   modelSetup =>
                   {
                       modelSetup.CheckProperty(r => r.Leader, ps => ps
                           .IsInvalidWith(null)            // Required
                           .IsInvalidWith("Darth Vader")); // Valid
                   },
                    _skipConverageChecksOptions);
            });
        }
    }
}
