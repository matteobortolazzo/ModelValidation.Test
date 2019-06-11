using System;
using ModelValidation.Test;
using ModelValidation.Test.Exceptions;
using TestsModelValidation.Test.Models;
using Xunit;

namespace TestsModelValidation.Test
{
    public class ModelValidatorWithMessagesTests
    {
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
                    modelSetup.CheckObject(os => os.IsInvalidWith(r => r.IsCloned, false), "Trooper must be a clone.");

                    modelSetup.CheckProperty(r => r.Leader, ps => ps.IsInvalidWith(null, "Sith leader is required."));
                },
                false); ;
        }

        [Fact]
        public void ObjectWrongMessage_Throws_ModelIsValidException()
        {
            _ = Assert.Throws<InvalidErrorMessageException>(() =>
               ModelValidator.Test(
                    () => new Stormtrooper
                    {
                        IsCloned = true,
                        Leader = "Palpatine"
                    },
                    modelSetup =>
                    {
                        modelSetup.CheckObject(os => os.IsInvalidWith(r => r.IsCloned, false), "Trooper should be a clone.");
                    },
                    false));
        }

        [Fact]
        public void PropertyWrongMessage_Throws_PropertyIsValidException()
        {
            _ = Assert.Throws<InvalidErrorMessageException>(() =>
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
                    false));
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
                   false);
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
                   false);
            });
        }
    }
}
