using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Exceptions
{
    /// <summary>
    /// The exception that is thrown a property is not tested.
    /// </summary>
    public class PropertiesNotTestedException : Exception
    {
        internal PropertiesNotTestedException()
        {
        }

        internal PropertiesNotTestedException(string message) : base(message)
        {
        }

        internal PropertiesNotTestedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
