using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the property is still valid after the test update.
    /// </summary>
    public class PropertyIsValidException : Exception
    {
        internal PropertyIsValidException()
        {
        }

        internal PropertyIsValidException(string message) : base(message)
        {
        }

        internal PropertyIsValidException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
