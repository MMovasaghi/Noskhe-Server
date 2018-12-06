using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class SmsVerificationFailedException : Exception
    {
        public SmsVerificationFailedException()
        {
        }

        public SmsVerificationFailedException(string message)
            : base(message)
        {
        }

        public SmsVerificationFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}