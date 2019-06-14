using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the validation error is not the one expected.
    /// </summary>
    public class InvalidErrorMessageException : Exception
    {
        internal InvalidErrorMessageException()
        {
        }

        internal InvalidErrorMessageException(string message) : base(message)
        {
        }

        internal InvalidErrorMessageException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
