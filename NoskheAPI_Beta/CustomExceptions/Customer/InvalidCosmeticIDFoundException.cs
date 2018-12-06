using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class InvalidCosmeticIDFoundException : Exception
    {
        public InvalidCosmeticIDFoundException()
        {
        }

        public InvalidCosmeticIDFoundException(string message)
            : base(message)
        {
        }

        public InvalidCosmeticIDFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}