using System;

namespace NoskheAPI_Beta.CustomExceptions.Management
{
    public class NoMedicinesAvailableException : Exception
    {
        public NoMedicinesAvailableException()
        {
        }

        public NoMedicinesAvailableException(string message)
            : base(message)
        {
        }

        public NoMedicinesAvailableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}