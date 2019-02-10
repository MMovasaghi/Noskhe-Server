namespace NoskheAPI_Beta.Settings.ResponseMessages.Customer
{
    public static class ErrorCodes
    {
        // Custom Exception Error Code Messages For Customer
        public static string APIUnhandledExceptionMsg = "CEC1"; // CustomerErrorCode1
        public static string DatabaseFailureExceptionMsg = "CEC2";
        public static string DuplicateCustomerExceptionMsg = "CEC3";
        public static string InvalidCosmeticIDFoundExceptionMsg = "CEC4";
        public static string InvalidMedicineIDFoundExceptionMsg = "CEC5";
        public static string NoCosmeticsAvailabeExceptionMsg = "CEC6";
        public static string NoCosmeticsInTheShoppingCartExceptionMsg = "CEC7";
        public static string NoCustomersMatchedByPhoneExceptionMsg = "CEC8";
        public static string NoMedicinesAvailabeExceptionMsg = "CEC9";
        public static string NoMedicinesInTheShoppingCartExceptionMsg = "CEC10"; // TODO: remove this (DEPRECATED)
        public static string NoOrdersFoundExceptionMsg = "CEC11";
        public static string NoOrdersMatchedByIdException = "CEC12"; // TODO: remove this (DEPRECATED)
        public static string NoShoppingCartsFoundExceptionMsg = "CEC13";
        public static string PaymentGatewayFailureExceptionMsg = "CEC14";
        public static string LoginVerificationFailedExceptionMsg = "CEC15";
        public static string SmsVerificationExpiredExceptionMsg = "CEC16";
        public static string SmsVerificationFailedExceptionMsg = "CEC17";
        public static string EmailAndPhoneAreNullExceptionMsg = "CEC18";
        // C# Standard Exception Error Code Messages
        public static string SecurityTokenExpiredExceptionMsg = "CEC0";
    }
}