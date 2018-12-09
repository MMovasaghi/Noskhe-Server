using System;

namespace NoskheAPI_Beta.CustomExceptions.Pharmacy
{
    public class NoInformationException : Exception
    {
        public NoInformationException()
        {
        }

        public NoInformationException(string message)
            : base(message)
        {
        }

        public NoInformationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}