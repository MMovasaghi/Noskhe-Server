using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoMedicinesInTheShoppingCartException : Exception
    {
        public NoMedicinesInTheShoppingCartException()
        {
        }

        public NoMedicinesInTheShoppingCartException(string message)
            : base(message)
        {
        }

        public NoMedicinesInTheShoppingCartException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}