using System;

namespace NoskheAPI_Beta.CustomExceptions.Pharmacy
{
    public class PendingRequestInProgressException : Exception
    {
        public PendingRequestInProgressException()
        {
        }

        public PendingRequestInProgressException(string message)
            : base(message)
        {
        }

        public PendingRequestInProgressException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}