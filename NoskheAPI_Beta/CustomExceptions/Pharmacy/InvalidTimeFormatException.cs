using System;

namespace NoskheAPI_Beta.CustomExceptions.Pharmacy
{
    public class InvalidTimeFormatException : Exception
    {
        public InvalidTimeFormatException()
        {
        }

        public InvalidTimeFormatException(string message)
            : base(message)
        {
        }

        public InvalidTimeFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}