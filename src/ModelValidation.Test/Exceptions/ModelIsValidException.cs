using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the model is still valid after the test update.
    /// </summary>
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
