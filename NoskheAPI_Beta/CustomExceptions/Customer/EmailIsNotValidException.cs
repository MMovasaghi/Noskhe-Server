using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class EmailIsNotValidException : Exception
    {
        public EmailIsNotValidException()
        {
        }

        public EmailIsNotValidException(string message)
            : base(message)
        {
        }

        public EmailIsNotValidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}