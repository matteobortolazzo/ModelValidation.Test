using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Extensions
{
    public static class ModelPropertyValidatorSetupExtentions
    {
        public static IModelPropertyValidatorSetup<TModel, TProperty> IsRequired<TModel, TProperty>(this IModelPropertyValidatorSetup<TModel, TProperty> setup)
            where TProperty : class
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            return setup.IsInvalidWith(null);
        }

        public static IModelPropertyValidatorSetup<TModel, string> HasMaxLenght<TModel>(this IModelPropertyValidatorSetup<TModel, string> setup, int maxLenght)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            var testStr = new string('0', maxLenght + 1);
            return setup.IsInvalidWith(testStr);
        }

        public static IModelPropertyValidatorSetup<TModel, string> HasMinLenght<TModel>(this IModelPropertyValidatorSetup<TModel, string> setup, int minLenght)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            var testStr = new string('0', minLenght - 1);
            return setup.IsInvalidWith(testStr);
        }

        public static IModelPropertyValidatorSetup<TModel, int> InRange<TModel>(this IModelPropertyValidatorSetup<TModel, int> setup, int min, int max)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            return setup
                .IsInvalidWith(min - 1)
                .IsInvalidWith(max + 1);
        }

        public static IModelPropertyValidatorSetup<TModel, double> InRange<TModel>(this IModelPropertyValidatorSetup<TModel, double> setup, double min, double max)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            return setup
                .IsInvalidWith(min - 1)
                .IsInvalidWith(max + 1);
        }
    }
}
