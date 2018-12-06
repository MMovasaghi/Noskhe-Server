using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoCustomersFoundException : Exception
    {
        public NoCustomersFoundException()
        {
        }

        public NoCustomersFoundException(string message)
            : base(message)
        {
        }

        public NoCustomersFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}