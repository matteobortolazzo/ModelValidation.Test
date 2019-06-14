using System;
using System.Collections.Generic;
using System.Text;
using TestsModelValidation.Test.CustomAttributes;

namespace TestsModelValidation.Test.Models
{
    [IsJediClass]
    public class Skywalker
    {
        public string Name { get; set; }

        [IsJediProperty]
        public bool IsJedi { get; set; }
    }
}
