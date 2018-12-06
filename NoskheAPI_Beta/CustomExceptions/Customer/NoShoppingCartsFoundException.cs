using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoShoppingCartsFoundException : Exception
    {
        public NoShoppingCartsFoundException()
        {
        }

        public NoShoppingCartsFoundException(string message)
            : base(message)
        {
        }

        public NoShoppingCartsFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}