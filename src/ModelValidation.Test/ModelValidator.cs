using System;
using ModelValidation.Test.Helpers;

namespace ModelValidation.Test
{
    public static class ModelValidator
    {
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
