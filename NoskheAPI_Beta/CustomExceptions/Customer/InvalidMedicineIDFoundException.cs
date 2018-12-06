using System;

namespace NoskheAPI_Beta.CustomExceptions.Customer
{
    public class InvalidMedicineIDFoundException : Exception
    {
        public InvalidMedicineIDFoundException()
        {
        }

        public InvalidMedicineIDFoundException(string message)
            : base(message)
        {
        }

        public InvalidMedicineIDFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}