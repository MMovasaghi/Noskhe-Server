using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class DuplicateCustomerException : Exception
    {
        public DuplicateCustomerException()
        {
        }

        public DuplicateCustomerException(string message)
            : base(message)
        {
        }

        public DuplicateCustomerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}