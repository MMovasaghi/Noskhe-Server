using System;

namespace NoskheAPI_Beta.CustomExceptions.Pharmacy
{
    public class LoginVerificationFailedException : Exception
    {
        public LoginVerificationFailedException()
        {
        }

        public LoginVerificationFailedException(string message)
            : base(message)
        {
        }

        public LoginVerificationFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}