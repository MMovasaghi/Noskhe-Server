using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class DatabaseFailureException : Exception
    {
        public DatabaseFailureException()
        {
        }

        public DatabaseFailureException(string message)
            : base(message)
        {
        }

        public DatabaseFailureException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}