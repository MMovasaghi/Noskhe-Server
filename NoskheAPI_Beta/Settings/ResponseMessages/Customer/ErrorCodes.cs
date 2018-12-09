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
        public static string NoCosmeticsMatchedByUSCIExcpetionMsg = "CEC7";
        // public static string NoCustomersFoundExceptionMsg = "CEC8";
        public static string NoCustomersMatchedByPhoneExceptionMsg = "CEC9";
        public static string NoMedicinesAvailabeExceptionMsg = "CEC10";
        public static string NoMedicinesMatchedByUSCIExcpetionMsg = "CEC11";
        public static string NoOrdersFoundExceptionMsg = "CEC12";
        public static string NoOrdersMatchedByUOIExceptionMsg = "CEC13";
        public static string NoShoppingCartsFoundExceptionMsg = "CEC14";
        public static string PaymentGatewayFailureExceptionMsg = "CEC15";
        public static string LoginVerificationFailedExceptionMsg = "CEC16";
        public static string SmsVerificationExpiredExceptionMsg = "CEC17";
        public static string SmsVerificationFailedExceptionMsg = "CEC18";
        // C# Standard Exception Error Code Messages
        public static string SecurityTokenExpiredExceptionMsg = "CEC0";
    }
}