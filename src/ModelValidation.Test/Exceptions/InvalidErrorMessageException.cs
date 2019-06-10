using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Exceptions
{
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
