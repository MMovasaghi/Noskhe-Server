using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoCosmeticsAvailabeException : Exception
    {
        public NoCosmeticsAvailabeException()
        {
        }

        public NoCosmeticsAvailabeException(string message)
            : base(message)
        {
        }

        public NoCosmeticsAvailabeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}