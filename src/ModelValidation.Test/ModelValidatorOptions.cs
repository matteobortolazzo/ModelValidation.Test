using System;
using System.Collections.Generic;
using System.Text;
using ModelValidation.Test.Helpers;

namespace ModelValidation.Test
{
    public class ModelValidatorOptions
    {
        public bool CheckPropertiesCoverage { get; set; } = true;
        public bool CheckClassAttributesCoverage { get; set; } = true;
        public Action<IServiceProviderSetup> ServiceProviderSetupAction { get; set; } = null;
    }
}
