using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoPharmaciesAreProvidingServiceException : Exception
    {
        public NoPharmaciesAreProvidingServiceException()
        {
        }

        public NoPharmaciesAreProvidingServiceException(string message)
            : base(message)
        {
        }

        public NoPharmaciesAreProvidingServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}