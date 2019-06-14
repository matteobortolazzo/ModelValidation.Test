using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestsModelValidation.Test.Services;

namespace TestsModelValidation.Test.CustomAttributes
{
    public class IsJediPropertyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var jediService = (IJediService)validationContext.GetService(typeof(IJediService));

            if (jediService.IsJedi((bool)value))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Not a JEDI", new List<string> { "IsJedi" });
        }
    }
}
