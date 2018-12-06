using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoCosmeticsMatchedByUSCIExcpetion : Exception
    {
        public NoCosmeticsMatchedByUSCIExcpetion()
        {
        }

        public NoCosmeticsMatchedByUSCIExcpetion(string message)
            : base(message)
        {
        }

        public NoCosmeticsMatchedByUSCIExcpetion(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}