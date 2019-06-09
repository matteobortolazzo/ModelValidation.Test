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
        void CheckObject(Action<IModelObjectValidatorSetup<TModel>> setup, string expectedErrorMessage = null);
        void CheckProperty<TProperty>(Expression<Func<TModel, TProperty>> selector, Action<IModelPropertyValidatorSetup<TModel, TProperty>> setup);
    }

    internal class ModelTestSetup<TModel> : IModelTestSetup<TModel>
    {
        private readonly Func<TModel> _createValidModelFunc;
        private readonly List<IModelObjectValidator> _objectValidators;
        private readonly List<IModelPropertyValidator> _propertiesValidators;

        public ModelTestSetup(Func<TModel> createValidModelFunc)
        {
            _createValidModelFunc = createValidModelFunc;
            _objectValidators = new List<IModelObjectValidator>();
            _propertiesValidators = new List<IModelPropertyValidator>();
        }

        public void CheckObject(Action<IModelObjectValidatorSetup<TModel>> setup, string expectedErrorMessage = null)
        {
            var validator = new ModelObjectValidatorSetup<TModel>(expectedErrorMessage);
            setup(validator);
            _objectValidators.Add(validator);
        }

        public void CheckProperty<TProperty>(Expression<Func<TModel, TProperty>> selector, Action<IModelPropertyValidatorSetup<TModel, TProperty>> setup)
        {
            if (!(selector is LambdaExpression l) || !(l.Body is MemberExpression m))
            {
                throw new ArgumentException("Selector must return a property.", nameof(selector));
            }

            var validator = new ModelPropertyValidatorSetup<TModel, TProperty>(m.Member.Name);
            setup(validator);
            _propertiesValidators.Add(validator);
        }

        public void Run()
        {
            TModel model = _createValidModelFunc();
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), null, true);

            if (!isValid)
            {
                throw new ModelIsInvalidException("Object return by the creation function must be valid.");
            }

            foreach (IModelObjectValidator validator in _objectValidators)
            {
                validator.RunTest(_createValidModelFunc());
            }

            foreach (IModelPropertyValidator validator in _propertiesValidators)
            {
                foreach ((var Value, var Message) in validator.GetInvalidValues())
                {
                    validator.RunTest(_createValidModelFunc(), Value, Message);
                }
            }
        }
    }
}
