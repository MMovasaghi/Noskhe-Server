using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class NoMedicinesAvailabeException : Exception
    {
        public NoMedicinesAvailabeException()
        {
        }

        public NoMedicinesAvailabeException(string message)
            : base(message)
        {
        }

        public NoMedicinesAvailabeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}