using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestsModelValidation.Test.Models;

namespace TestsModelValidation.Test.CustomAttributes
{
    public class YoungSkywalkerAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var model = (Rebel)value;
            // Doesn't mean that people over 25 are old.
            return model.Surname == "Skywalker" && model.Age < 25;
        }
    }
}
