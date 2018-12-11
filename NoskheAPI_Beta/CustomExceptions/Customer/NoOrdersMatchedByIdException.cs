using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoOrdersMatchedByIdException : Exception
    {
        public NoOrdersMatchedByIdException()
        {
        }

        public NoOrdersMatchedByIdException(string message)
            : base(message)
        {
        }

        public NoOrdersMatchedByIdException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}