using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Exceptions
{
    public class ValidationAttributeNotTestedException : Exception
    {
        internal ValidationAttributeNotTestedException()
        {
        }

        internal ValidationAttributeNotTestedException(string message) : base(message)
        {
        }

        internal ValidationAttributeNotTestedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
