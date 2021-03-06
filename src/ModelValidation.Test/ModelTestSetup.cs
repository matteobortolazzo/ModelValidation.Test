﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ModelValidation.Test.Exceptions;
using ModelValidation.Test.Extensions;
using ModelValidation.Test.Helpers;

namespace ModelValidation.Test
{
    /// <summary>
    /// Class to configure all validation tests.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IModelTestSetup<TModel>
    {
        /// <summary>
        /// Checks that the model with the given setup is invalid.
        /// </summary>
        /// <param name="setup">Function to setup class level validation.</param>
        /// <param name="expectedErrorMessage">The expected error message.</param>
        void CheckClass(Action<IModelClassValidatorSetup<TModel>> setup, string expectedErrorMessage = null);

        /// <summary>
        ///  Checks that the model with the given property setup is invalid.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="selector"></param>
        /// <param name="setup">Function to setup property level validation.</param>
        /// <param name="checkAttributesCoverage">If true, checks that all property's attributes are tested.</param>
        void CheckProperty<TProperty>(Expression<Func<TModel, TProperty>> selector, Action<IModelPropertyValidatorSetup<TModel, TProperty>> setup, bool checkAttributesCoverage = true);
    }

    internal class ModelTestSetup<TModel> : IModelTestSetup<TModel>
    {
        private readonly Func<TModel> _createValidModelFunc;
        private readonly List<IModelClassValidator> _classLevelValidators;
        private readonly List<(PropertyInfo PropertyInfo, IModelPropertyValidator Validator)> _propertyLevelValidators;

        public ModelTestSetup(Func<TModel> createValidModelFunc)
        {
            _createValidModelFunc = createValidModelFunc;
            _classLevelValidators = new List<IModelClassValidator>();
            _propertyLevelValidators = new List<(PropertyInfo PropertyInfo, IModelPropertyValidator Validator)>();
        }

        public void CheckClass(Action<IModelClassValidatorSetup<TModel>> setup, string expectedErrorMessage = null)
        {
            var validator = new ModelClassValidatorSetup<TModel>(expectedErrorMessage);
            setup(validator);
            _classLevelValidators.Add(validator);
        }

        public void CheckProperty<TProperty>(Expression<Func<TModel, TProperty>> selector, Action<IModelPropertyValidatorSetup<TModel, TProperty>> setup, bool checkAttributesCoverage = true)
        {
            if (!(selector is LambdaExpression l) || !(l.Body is MemberExpression m))
            {
                throw new ArgumentException("Selector must return a property.", nameof(selector));
            }

            PropertyInfo propertyInfo = typeof(TModel).GetProperty(m.Member.Name);
            var validator = new ModelPropertyValidatorSetup<TModel, TProperty>(propertyInfo, checkAttributesCoverage);
            setup(validator);
            _propertyLevelValidators.Add((propertyInfo, validator));
        }

        public void Run(ModelValidatorOptions options)
        {
            var serviceProvider = new TestServiceProvider();
            options.ServiceProviderSetupAction?.Invoke(serviceProvider);

            CheckIfModelIsValid(serviceProvider);

            if (options.CheckPropertiesCoverage)
            {
                CheckPropertiesCoverage();
            }

            if (options.CheckClassAttributesCoverage)
            {
                CheckClassAttributesCoverage(serviceProvider);
            }

            CheckClassLevelAttributes(serviceProvider);
            CheckPropertyLevelAttributes(serviceProvider);
        }

        private void CheckIfModelIsValid(IServiceProvider serviceProvider)
        {
            TModel validModel = _createValidModelFunc();
            var isValid = Validator.TryValidateObject(validModel, new ValidationContext(validModel, serviceProvider, null), null, true);

            if (!isValid)
            {
                throw new ModelIsInvalidException("Object return by the creation function must be valid.");
            }
        }

        private void CheckPropertiesCoverage()
        {
            // Get all properties with validation attributes
            var allProperties = typeof(TModel).GetProperties().Where(p => p.GetCustomAttributes<ValidationAttribute>(true).Any()).ToList();
            var notTestedProperties = allProperties.Except(_propertyLevelValidators.Select(p => p.PropertyInfo)).ToList();

            if (notTestedProperties.Any())
            {
                throw new PropertiesNotTestedException($"One or more properties are not tested: {string.Join(", ", notTestedProperties.Select(p => p.Name))}.");
            }
        }

        private void CheckClassAttributesCoverage(IServiceProvider serviceProvider)
        {
            var classValidationAttributes = typeof(TModel).GetCustomAttributes<ValidationAttribute>(true).ToList();
            foreach (ValidationAttribute validationAttribute in classValidationAttributes)
            {
                var validationContext = new ValidationContext(_createValidModelFunc(), serviceProvider, null);
                if (_classLevelValidators.All(model => 
                    {
                        var invalidModel = model.SetValues(_createValidModelFunc());
                        try
                        {
                            validationAttribute.Validate(invalidModel, validationContext);
                            return true;
                        }
                        catch (ValidationException)
                        {
                            return false;
                        }
                    }))
                {
                    throw new ValidationAttributeNotTestedException($"{validationAttribute.GetType().Name} is not tested.");
                }
            }
        }

        private void CheckClassLevelAttributes(IServiceProvider serviceProvider)
        {
            foreach (IModelClassValidator validator in _classLevelValidators)
            {
                validator.RunTest(_createValidModelFunc(), serviceProvider);
            }
        }

        private void CheckPropertyLevelAttributes(IServiceProvider serviceProvider)
        {
            foreach ((PropertyInfo PropertyInfo, IModelPropertyValidator Validator) in _propertyLevelValidators)
            {
                if (Validator.CheckAttributesCoverage)
                {
                    IReadOnlyCollection<(object TransformFunction, string Message)> invalidValues = Validator.GetInvalidValues();
                    var propertyValidationAttributes = PropertyInfo.GetCustomAttributes<ValidationAttribute>(true).ToList();
                    foreach (ValidationAttribute validationAttribute in propertyValidationAttributes)
                    {
                        TModel validModel = _createValidModelFunc();
                        var validationContext = new ValidationContext(validModel, serviceProvider, null);
                        if (invalidValues.All(property => 
                            {
                                var propertyValue = property.TransformFunction.GetUpdatedProperty(validModel, PropertyInfo);

                                try
                                {
                                    validationAttribute.Validate(propertyValue, validationContext);
                                    return true;
                                }
                                catch (ValidationException)
                                {
                                    return false;
                                }
                            }))
                        {
                            throw new ValidationAttributeNotTestedException($"{validationAttribute.GetType().Name} is not tested.");
                        }
                    }
                }

                foreach ((var Action, var Message) in Validator.GetInvalidValues())
                {
                    var validModel = _createValidModelFunc();
                    Validator.RunTest(validModel, Action, Message, serviceProvider);
                }
            }
        }
    }
}
