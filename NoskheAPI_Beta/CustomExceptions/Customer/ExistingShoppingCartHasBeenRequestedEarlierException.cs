using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class ExistingShoppingCartHasBeenRequestedEarlierException : Exception
    {
        public ExistingShoppingCartHasBeenRequestedEarlierException()
        {
        }

        public ExistingShoppingCartHasBeenRequestedEarlierException(string message)
            : base(message)
        {
        }

        public ExistingShoppingCartHasBeenRequestedEarlierException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}