using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class TextMessageVerificationFailedException : Exception
    {
        public TextMessageVerificationFailedException()
        {
        }

        public TextMessageVerificationFailedException(string message)
            : base(message)
        {
        }

        public TextMessageVerificationFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}