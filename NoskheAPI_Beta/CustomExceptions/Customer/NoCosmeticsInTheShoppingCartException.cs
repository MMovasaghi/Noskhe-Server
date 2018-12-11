using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoCosmeticsInTheShoppingCartException : Exception
    {
        public NoCosmeticsInTheShoppingCartException()
        {
        }

        public NoCosmeticsInTheShoppingCartException(string message)
            : base(message)
        {
        }

        public NoCosmeticsInTheShoppingCartException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}