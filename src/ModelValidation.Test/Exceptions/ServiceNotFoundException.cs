using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the reqeusted services has not been registered in the options.
    /// </summary>
    public class ServiceNotFoundException : Exception
    {
        internal ServiceNotFoundException()
        {
        }

        internal ServiceNotFoundException(string message) : base(message)
        {
        }

        internal ServiceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
