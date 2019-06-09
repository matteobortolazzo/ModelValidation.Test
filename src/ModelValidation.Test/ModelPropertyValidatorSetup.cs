﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using ModelValidation.Test.Exceptions;

namespace ModelValidation.Test
{
    internal interface IModelPropertyValidator
    {
        void RunTest(object model, object value, string message);
        IReadOnlyCollection<(object Value, string Message)> GetInvalidValues();
    }

    public interface IModelPropertyValidatorSetup<TModel, TProperty>
    {
        IModelPropertyValidatorSetup<TModel, TProperty> IsInvalidWith(TProperty invalidValue, string expectedErrorMessage = null);
    }

    internal class ModelPropertyValidatorSetup<TModel, TProperty> : IModelPropertyValidatorSetup<TModel, TProperty>, IModelPropertyValidator
    {
        private readonly List<(object Value, string Message)> _propertiesValues;
        private readonly PropertyInfo _propertyInfo;

        public ModelPropertyValidatorSetup(string propertyName)
        {
            _propertiesValues = new List<(object Value, string Message)>();
            _propertyInfo = typeof(TModel).GetProperty(propertyName);
        }

        public IReadOnlyCollection<(object Value, string Message)> GetInvalidValues()
        {
            return _propertiesValues.AsReadOnly();
        }

        public IModelPropertyValidatorSetup<TModel, TProperty> IsInvalidWith(TProperty invalidValue, string expectedErrorMessage = null)
        {
            _propertiesValues.Add((invalidValue, expectedErrorMessage));
            return this;
        }

        public void RunTest(object model, object value, string expectedErrorMessage)
        {
            _propertyInfo.SetValue(model, value);

            var validationResults = new List<ValidationResult>(); 
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

            if (isValid)
            {
                throw new ModelIsValidException("The model with the given property must be invalid."); 
            }

            var propertyResults = validationResults.Where(r => r.MemberNames.Contains(_propertyInfo.Name)).ToList();
            if (expectedErrorMessage == null && !propertyResults.Any())
            {
                throw new PropertyIsValidException($"Property {_propertyInfo.Name} must be invalid.");
            }
            if (expectedErrorMessage != null && !propertyResults.Any(r => r.ErrorMessage == expectedErrorMessage))
            {
                throw new PropertyIsValidException($"Property {_propertyInfo.Name} must be invalid with error \"{expectedErrorMessage}\".");
            }
        }
    }
}