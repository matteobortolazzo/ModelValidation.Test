using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Exceptions
{
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
