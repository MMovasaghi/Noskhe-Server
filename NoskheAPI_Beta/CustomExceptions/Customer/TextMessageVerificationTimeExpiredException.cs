using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class TextMessageVerificationTimeExpiredException : Exception
    {
        public TextMessageVerificationTimeExpiredException()
        {
        }

        public TextMessageVerificationTimeExpiredException(string message)
            : base(message)
        {
        }

        public TextMessageVerificationTimeExpiredException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}