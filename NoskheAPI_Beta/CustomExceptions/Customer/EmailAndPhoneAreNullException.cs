using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class EmailAndPhoneAreNullException : Exception
    {
        public EmailAndPhoneAreNullException()
        {
        }

        public EmailAndPhoneAreNullException(string message)
            : base(message)
        {
        }

        public EmailAndPhoneAreNullException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}