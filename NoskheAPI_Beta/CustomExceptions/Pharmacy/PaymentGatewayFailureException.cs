using System;

namespace NoskheAPI_Beta.CustomExceptions.Pharmacy
{
    public class PaymentGatewayFailureException : Exception
    {
        public PaymentGatewayFailureException()
        {
        }

        public PaymentGatewayFailureException(string message)
            : base(message)
        {
        }

        public PaymentGatewayFailureException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}