using System;

namespace ModelValidation.Test
{
    public static class ModelValidator
    {
        public static void Test<TModel>(Func<TModel> createValidModelFunc, Action<IModelTestSetup<TModel>> setupAction) where TModel : class
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var setup = new ModelTestSetup<TModel>(createValidModelFunc);
            setupAction(setup);
            setup.Run();
        }
    }
}
