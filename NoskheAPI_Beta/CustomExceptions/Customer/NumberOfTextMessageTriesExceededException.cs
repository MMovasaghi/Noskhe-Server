using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NumberOfTextMessageTriesExceededException : Exception
    {
        public NumberOfTextMessageTriesExceededException()
        {
        }

        public NumberOfTextMessageTriesExceededException(string message)
            : base(message)
        {
        }

        public NumberOfTextMessageTriesExceededException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}