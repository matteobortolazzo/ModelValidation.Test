using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Extensions
{
    /// <summary>
    /// Extensions methods to provide a faster developement of properties tests.
    /// </summary>
    public static class ModelPropertyValidatorSetupExtentions
    {
        /// <summary>
        /// Tests that a property cannot be null.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="setup">The property validation setup to update.</param>
        /// <returns>The instance of the setup.</returns>
        public static IModelPropertyValidatorSetup<TModel, TProperty> IsRequired<TModel, TProperty>(this IModelPropertyValidatorSetup<TModel, TProperty> setup)
            where TProperty : class
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            return setup.IsInvalidWith(null);
        }

        /// <summary>
        /// Tests that the value of the property has a maximum amount of characters.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="setup">The property validation setup to update.</param>
        /// <param name="maxLenght">The maximum amount of characters for the property.</param>
        /// <returns>The instance of the setup.</returns>
        public static IModelPropertyValidatorSetup<TModel, string> HasMaxLenght<TModel>(this IModelPropertyValidatorSetup<TModel, string> setup, int maxLenght)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            var testStr = new string('0', maxLenght + 1);
            return setup.IsInvalidWith(testStr);
        }


        /// <summary>
        /// Tests that the value of the property has a minimum amount of characters.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="setup">The property validation setup to update.</param>
        /// <param name="minLenght">The minimum amount of characters for the property.</param>
        /// <returns>The instance of the setup.</returns>
        public static IModelPropertyValidatorSetup<TModel, string> HasMinLenght<TModel>(this IModelPropertyValidatorSetup<TModel, string> setup, int minLenght)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            var testStr = new string('0', minLenght - 1);
            return setup.IsInvalidWith(testStr);
        }

        /// <summary>
        /// Tests that the numberic value of the property has a minimum value.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="setup">The property validation setup to update.</param>
        /// <param name="min">The minumun value for the property.</param>
        /// <returns>The instance of the setup.</returns>
        public static IModelPropertyValidatorSetup<TModel, int> HasMinValue<TModel>(this IModelPropertyValidatorSetup<TModel, int> setup, int min)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            return setup
                .IsInvalidWith(min - 1);
        }

        /// <summary>
        /// Tests that the numberic value of the property has a minimum value.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="setup">The property validation setup to update.</param>
        /// <param name="max">The maximum value for the property.</param>
        /// <returns>The instance of the setup.</returns>
        public static IModelPropertyValidatorSetup<TModel, int> HasMaxValue<TModel>(this IModelPropertyValidatorSetup<TModel, int> setup, int max)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            return setup
                .IsInvalidWith(max + 1);
        }

        /// <summary>
        /// Tests that the numberic value of the property is in a range of values.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="setup">The property validation setup to update.</param>
        /// <param name="min">The minumun value for the property.</param>
        /// <param name="max">The maximum value for the property.</param>
        /// <returns>The instance of the setup.</returns>
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

        /// <summary>
        /// Tests that the numberic value of the property has a minimum value.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="setup">The property validation setup to update.</param>
        /// <param name="min">The minumun value for the property.</param>
        /// <returns>The instance of the setup.</returns>
        public static IModelPropertyValidatorSetup<TModel, double> HasMinValue<TModel>(this IModelPropertyValidatorSetup<TModel, double> setup, double min)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            return setup
                .IsInvalidWith(min - 1);
        }
        
        /// <summary>
        /// Tests that the numberic value of the property has a minimum value.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="setup">The property validation setup to update.</param>
        /// <param name="max">The maximum value for the property.</param>
        /// <returns>The instance of the setup.</returns>
        public static IModelPropertyValidatorSetup<TModel, double> HasMaxValue<TModel>(this IModelPropertyValidatorSetup<TModel, double> setup, double max)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            return setup
                .IsInvalidWith(max + 1);
        }

        /// <summary>
        /// Tests that the numberic value of the property is in a range of values.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="setup">The property validation setup to update.</param>
        /// <param name="min">The minumun value for the property.</param>
        /// <param name="max">The maximum value for the property.</param>
        /// <returns>The instance of the setup.</returns>
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
