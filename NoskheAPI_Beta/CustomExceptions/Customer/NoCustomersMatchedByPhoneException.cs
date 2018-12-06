using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoCustomersMatchedByPhoneException : Exception
    {
        public NoCustomersMatchedByPhoneException()
        {
        }

        public NoCustomersMatchedByPhoneException(string message)
            : base(message)
        {
        }

        public NoCustomersMatchedByPhoneException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}