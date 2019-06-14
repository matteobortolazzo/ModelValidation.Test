using System;
using System.Collections.Generic;
using System.Text;
using ModelValidation.Test.Helpers;

namespace ModelValidation.Test
{
    /// <summary>
    /// Represent the model validator options to use during validation.
    /// </summary>
    public class ModelValidatorOptions
    {
        /// <summary>
        /// If true, the validator checks if all property are tested.
        /// </summary>
        public bool CheckPropertiesCoverage { get; set; } = true;

        /// <summary>
        /// If true, the validation checks that all class level attributes are tested.
        /// </summary>
        public bool CheckClassAttributesCoverage { get; set; } = true;

        /// <summary>
        /// Function to set up the service provider to use during validation.
        /// </summary>
        public Action<IServiceProviderSetup> ServiceProviderSetupAction { get; set; } = null;
    }
}
