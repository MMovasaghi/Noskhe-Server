using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoOrdersFoundException : Exception
    {
        public NoOrdersFoundException()
        {
        }

        public NoOrdersFoundException(string message)
            : base(message)
        {
        }

        public NoOrdersFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}