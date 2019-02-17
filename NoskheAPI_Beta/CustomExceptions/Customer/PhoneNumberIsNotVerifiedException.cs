using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class PhoneNumberIsNotVerifiedException : Exception
    {
        public PhoneNumberIsNotVerifiedException()
        {
        }

        public PhoneNumberIsNotVerifiedException(string message)
            : base(message)
        {
        }

        public PhoneNumberIsNotVerifiedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}