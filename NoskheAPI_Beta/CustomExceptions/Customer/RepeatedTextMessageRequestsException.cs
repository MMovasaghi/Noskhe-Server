using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class RepeatedTextMessageRequestsException : Exception
    {
        public RepeatedTextMessageRequestsException()
        {
        }

        public RepeatedTextMessageRequestsException(string message)
            : base(message)
        {
        }

        public RepeatedTextMessageRequestsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}