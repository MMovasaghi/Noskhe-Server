using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoMedicinesMatchedByUSCIExcpetion : Exception
    {
        public NoMedicinesMatchedByUSCIExcpetion()
        {
        }

        public NoMedicinesMatchedByUSCIExcpetion(string message)
            : base(message)
        {
        }

        public NoMedicinesMatchedByUSCIExcpetion(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}