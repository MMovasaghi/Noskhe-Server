using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoOrdersMatchedByUOIException : Exception
    {
        public NoOrdersMatchedByUOIException()
        {
        }

        public NoOrdersMatchedByUOIException(string message)
            : base(message)
        {
        }

        public NoOrdersMatchedByUOIException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}