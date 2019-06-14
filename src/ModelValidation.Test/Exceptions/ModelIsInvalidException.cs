using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the input model is invalid.
    /// </summary>
    public class ModelIsInvalidException : Exception
    {
        internal ModelIsInvalidException()
        {
        }

        internal ModelIsInvalidException(string message) : base(message)
        {
        }

        internal ModelIsInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
