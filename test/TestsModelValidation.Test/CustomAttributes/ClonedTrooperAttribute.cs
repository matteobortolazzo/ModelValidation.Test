using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestsModelValidation.Test.Models;

namespace TestsModelValidation.Test.CustomAttributes
{
    public class ClonedTrooperAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var trooper = (Stormtrooper)value;
            return trooper.IsCloned;
        }
    }
}
