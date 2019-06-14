using System;
using ModelValidation.Test.Helpers;

namespace ModelValidation.Test
{
    /// <summary>
    /// Class used to test model validation.
    /// </summary>
    public static class ModelValidator
    {
        /// <summary>
        /// Tests the validation of a class.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="createValidModelFunc">Function that return a valid model object.</param>
        /// <param name="setupAction">Function to configure the tests.</param>
        /// <param name="options">Options to use during the validation.</param>
        public static void Test<TModel>(
            Func<TModel> createValidModelFunc, 
            Action<IModelTestSetup<TModel>> setupAction,
            ModelValidatorOptions options = null) where TModel : class
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            if (options == null)
            {
                options = new ModelValidatorOptions();
            }

            var setup = new ModelTestSetup<TModel>(createValidModelFunc);
            setupAction(setup);

            setup.Run(options);
        }
    }
}
