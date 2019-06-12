using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ModelValidation.Test.Exceptions;

namespace ModelValidation.Test
{
    public interface IModelTestSetup<TModel>
    {
        void CheckClass(Action<IModelClassValidatorSetup<TModel>> setup, string expectedErrorMessage = null);
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

        public void Run(bool checkPropertiesCoverage, bool checkClassAttributesCoverage)
        {
            CheckIfModelIsValid();

            if (checkPropertiesCoverage)
            {
                CheckPropertiesCoverage();
            }

            if (checkClassAttributesCoverage)
            {
                CheckClassAttributesCoverage();
            }

            CheckClassLevelAttributes();
            CheckPropertyLevelAttributes();
        }

        private void CheckIfModelIsValid()
        {
            TModel validModel = _createValidModelFunc();
            var isValid = Validator.TryValidateObject(validModel, new ValidationContext(validModel), null, true);

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

        private void CheckClassAttributesCoverage()
        {
            var classValidationAttributes = typeof(TModel).GetCustomAttributes<ValidationAttribute>(true).ToList();
            foreach (ValidationAttribute validationAttribute in classValidationAttributes)
            {
                if (_classLevelValidators.All(v => validationAttribute.IsValid(v.SetValues(_createValidModelFunc()))))
                {
                    throw new ValidationAttributeNotTestedException($"{validationAttribute.GetType().Name} is not tested.");
                }
            }
        }

        private void CheckClassLevelAttributes()
        {
            foreach (IModelClassValidator validator in _classLevelValidators)
            {
                validator.RunTest(_createValidModelFunc());
            }
        }

        private void CheckPropertyLevelAttributes()
        {
            foreach ((PropertyInfo PropertyInfo, IModelPropertyValidator Validator) in _propertyLevelValidators)
            {
                if (Validator.CheckAttributesCoverage)
                {
                    IReadOnlyCollection<(object Value, string Message)> invalidValues = Validator.GetInvalidValues();
                    var propertyValidationAttributes = PropertyInfo.GetCustomAttributes<ValidationAttribute>(true).ToList();
                    foreach (ValidationAttribute validationAttribute in propertyValidationAttributes)
                    {
                        if (invalidValues.All(property => validationAttribute.IsValid(property.Value)))
                        {
                            throw new ValidationAttributeNotTestedException($"{validationAttribute.GetType().Name} is not tested.");
                        }
                    }
                }

                foreach ((var Value, var Message) in Validator.GetInvalidValues())
                {
                    Validator.RunTest(_createValidModelFunc(), Value, Message);
                }
            }
        }
    }
}
