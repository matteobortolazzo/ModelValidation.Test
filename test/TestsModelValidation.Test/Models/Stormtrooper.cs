using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestsModelValidation.Test.CustomAttributes;

namespace TestsModelValidation.Test.Models
{
    [ClonedTrooper(ErrorMessage = "Trooper must be a clone.")]
    public class Stormtrooper
    {
        public bool IsCloned { get; set; }

        [Required(ErrorMessage = "Sith leader is required.")]
        public string Leader { get; set; }
    }
}
