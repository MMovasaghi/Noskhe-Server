using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class SmsVerificationExpiredException : Exception
    {
        public SmsVerificationExpiredException()
        {
        }

        public SmsVerificationExpiredException(string message)
            : base(message)
        {
        }

        public SmsVerificationExpiredException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}