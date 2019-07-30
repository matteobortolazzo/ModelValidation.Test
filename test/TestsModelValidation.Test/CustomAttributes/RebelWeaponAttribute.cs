using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestsModelValidation.Test.Models;

namespace TestsModelValidation.Test.CustomAttributes
{
    public class RebelWeaponAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var weapon = (Weapon)value;

            if (weapon == null)
            {
                return true;
            }
            return weapon.Color == "Green";
        }
    }
}
