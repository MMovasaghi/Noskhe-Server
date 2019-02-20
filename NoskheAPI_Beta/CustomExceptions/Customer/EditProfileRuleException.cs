using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class EditProfileRuleException : Exception
    {
        public EditProfileRuleException()
        {
        }

        public EditProfileRuleException(string message)
            : base(message)
        {
        }

        public EditProfileRuleException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}