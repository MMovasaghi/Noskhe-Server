using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class RegistrationRuleException : Exception
    {
        public RegistrationRuleException()
        {
        }

        public RegistrationRuleException(string message)
            : base(message)
        {
        }

        public RegistrationRuleException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}