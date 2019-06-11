using System;
using System.Collections.Generic;
using System.Text;

namespace ModelValidation.Test.Exceptions
{
    public class PropertiesNotTestedException : Exception
    {
        public PropertiesNotTestedException()
        {
        }

        public PropertiesNotTestedException(string message) : base(message)
        {
        }

        public PropertiesNotTestedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
