using System;

namespace NoskheAPI_Beta.CustomExceptions.Pharmacy
{
    public class APIUnhandledException : Exception
    {
        public APIUnhandledException()
        {
        }

        public APIUnhandledException(string message)
            : base(message)
        {
        }

        public APIUnhandledException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}