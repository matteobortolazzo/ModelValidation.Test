using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ModelValidation.Test.Exceptions;

namespace ModelValidation.Test
{
    internal interface IModelClassValidator
    {
        void RunTest(object model);
        object SetValues(object model);
    }

    public interface IModelClassValidatorSetup<TModel>
    {
        IModelClassValidatorSetup<TModel> IsInvalidWith<TProperty>(Expression<Func<TModel, TProperty>> selector, TProperty invalidValue);
    }

    internal class ModelClassValidatorSetup<TModel> : IModelClassValidatorSetup<TModel>, IModelClassValidator
    {
        private readonly Dictionary<PropertyInfo, object> _propertiesValues;
        private readonly string _expectedErrorMessage;

        public ModelClassValidatorSetup(string expectedErrorMessage)
        {
            _propertiesValues = new Dictionary<PropertyInfo, object>();
            _expectedErrorMessage = expectedErrorMessage;
        }

        public IModelClassValidatorSetup<TModel> IsInvalidWith<TProperty>(Expression<Func<TModel, TProperty>> selector, TProperty invalidValue)
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
            model = SetValues(model);

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

            if (isValid)
            {
                throw new ModelIsValidException("The model with the given properties must be invalid.");
            }

            if (!validationResults.Any())
            {

                throw new ModelIsValidException("The model with the given properties must be invalid.");
            }
            
            if (_expectedErrorMessage != null && !validationResults.Any(r => r.ErrorMessage == _expectedErrorMessage))
            {
                throw new InvalidErrorMessageException($"The model with the given properties must be invalid with message \"{_expectedErrorMessage}\".");
            }
        }

        public object SetValues(object model)
        {
            foreach(KeyValuePair<PropertyInfo, object> propertyValue in _propertiesValues)
            {
                propertyValue.Key.SetValue(model, propertyValue.Value);
            }

            return model;
        }
    }
}
