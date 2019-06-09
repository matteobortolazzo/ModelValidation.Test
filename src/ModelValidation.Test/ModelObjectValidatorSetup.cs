using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ModelValidation.Test.Exceptions;

namespace ModelValidation.Test
{
    internal interface IModelObjectValidator
    {
        void RunTest(object model);
    }

    public interface IModelObjectValidatorSetup<TModel>
    {
        IModelObjectValidatorSetup<TModel> IsInvalidWith<TProperty>(Expression<Func<TModel, TProperty>> selector, TProperty invalidValue);
    }

    internal class ModelObjectValidatorSetup<TModel> : IModelObjectValidatorSetup<TModel>, IModelObjectValidator
    {
        private readonly Dictionary<PropertyInfo, object> _propertiesValues;
        private readonly string _expectedErrorMessage;

        public ModelObjectValidatorSetup(string expectedErrorMessage)
        {
            _propertiesValues = new Dictionary<PropertyInfo, object>();
            _expectedErrorMessage = expectedErrorMessage;
        }

        public IModelObjectValidatorSetup<TModel> IsInvalidWith<TProperty>(Expression<Func<TModel, TProperty>> selector, TProperty invalidValue)
        {
            if (!(selector is LambdaExpression l) || !(l.Body is MemberExpression m))
            {
                throw new ArgumentException("Selector must return a property.", nameof(selector));
            }

            PropertyInfo propertyInfo = typeof(TModel).GetProperty(m.Member.Name);
            _propertiesValues.Add(propertyInfo, invalidValue);
            return this;
        }
        
        public void RunTest(object model)
        {
            foreach (KeyValuePair<PropertyInfo, object> propertyValue in _propertiesValues)
            {
                propertyValue.Key.SetValue(model, propertyValue.Value);
            }

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

            if (isValid)
            {
                throw new ModelIsValidException("The model with the given properties must be invalid.");
            }

            if (_expectedErrorMessage == null && !validationResults.Any())
            {
                throw new ModelIsValidException("The model with the given properties must be invalid.");
            }
            if (_expectedErrorMessage != null && !validationResults.Any(r => r.ErrorMessage == _expectedErrorMessage))
            {
                throw new ModelIsValidException($"The model with the given properties must be invalid with message \"{_expectedErrorMessage}\".");
            }
        }
    }
}
