using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ModelValidation.Test.Extensions
{
    internal static class ActionExtensions
    {
        public static object GetUpdatedProperty<TModel>(this object transformFunction, TModel validModel, PropertyInfo propertyInfo)
        {
            Type funcType = typeof(Func<,>);
            Type funcGenericType = funcType.MakeGenericType(new Type[] { propertyInfo.PropertyType, propertyInfo.PropertyType });

            if (transformFunction.GetType() != funcGenericType)
            {
                throw new ArgumentException("Input must be of type Func<T,T>.", nameof(transformFunction));
            }

            MethodInfo invokeFunction = funcGenericType.GetMethod(nameof(Func<object, object>.Invoke));

            var valueToUpdate = propertyInfo.GetValue(validModel);
            valueToUpdate = invokeFunction.Invoke(transformFunction, new object[] { valueToUpdate });

            propertyInfo.SetValue(validModel, valueToUpdate);

            return valueToUpdate;
        }
    }
}
