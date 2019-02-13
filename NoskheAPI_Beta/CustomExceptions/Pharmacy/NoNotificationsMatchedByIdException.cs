using System;

namespace NoskheAPI_Beta.CustomExceptions.Pharmacy
{
    public class NoNotificationsMatchedByIdException : Exception
    {
        public NoNotificationsMatchedByIdException()
        {
        }

        public NoNotificationsMatchedByIdException(string message)
            : base(message)
        {
        }

        public NoNotificationsMatchedByIdException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}