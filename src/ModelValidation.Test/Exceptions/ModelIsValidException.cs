using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Exceptions
{
    public class ModelIsValidException : Exception
    {
        internal ModelIsValidException()
        {
        }

        internal ModelIsValidException(string message) : base(message)
        {
        }

        internal ModelIsValidException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
