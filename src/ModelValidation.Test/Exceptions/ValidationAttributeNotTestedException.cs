using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a validation attribute is not tested.
    /// </summary>
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
