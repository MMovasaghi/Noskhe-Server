using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class InvalidItemIdFoundException : Exception
    {
        public InvalidItemIdFoundException()
        {
        }

        public InvalidItemIdFoundException(string message)
            : base(message)
        {
        }

        public InvalidItemIdFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}