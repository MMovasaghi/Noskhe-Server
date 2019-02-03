using System;

namespace NoskheAPI_Beta.CustomExceptions.Management
{
    public class DuplicateMedicineException : Exception
    {
        public DuplicateMedicineException()
        {
        }

        public DuplicateMedicineException(string message)
            : base(message)
        {
        }

        public DuplicateMedicineException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}