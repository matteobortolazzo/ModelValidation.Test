using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestsModelValidation.Test.CustomAttributes;

namespace TestsModelValidation.Test.Models
{
    [YoungSkywalker]
    public class Rebel
    {
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Range(10, 900)]
        public int Age { get; set; }
    }
}
