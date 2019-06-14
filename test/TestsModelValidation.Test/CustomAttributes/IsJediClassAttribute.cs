using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestsModelValidation.Test.Models;
using TestsModelValidation.Test.Services;

namespace TestsModelValidation.Test.CustomAttributes
{
    public class IsJediClassAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var jediService = (IJediService)validationContext.GetService(typeof(IJediService));

            var jedi = (Skywalker)value;

            if (jediService.IsJedi(jedi.IsJedi))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Not a JEDI", new List<string> { "IsJedi" });
        }
    }
}
