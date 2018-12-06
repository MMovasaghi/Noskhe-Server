namespace NoskheAPI_Beta.Settings.ResponseMessages.Customer
{
    public static class ErrorCodes
    {
        // Custom Exception Error Code Messages
        public static string APIUnhandledExceptionMsg = "EC1";
        public static string DatabaseFailureExceptionMsg = "EC2";
        public static string DuplicateCustomerExceptionMsg = "EC3";
        public static string InvalidCosmeticIDFoundExceptionMsg = "EC4";
        public static string InvalidMedicineIDFoundExceptionMsg = "EC5";
        public static string NoCosmeticsAvailabeExceptionMsg = "EC6";
        public static string NoCosmeticsMatchedByUSCIExcpetionMsg = "EC7";
        public static string NoCustomersFoundExceptionMsg = "EC8";
        public static string NoCustomersMatchedByPhoneExceptionMsg = "EC9";
        public static string NoMedicinesAvailabeExceptionMsg = "EC10";
        public static string NoMedicinesMatchedByUSCIExcpetionMsg = "EC11";
        public static string NoOrdersFoundExceptionMsg = "EC12";
        public static string NoOrdersMatchedByUOIExceptionMsg = "EC13";
        public static string NoShoppingCartsFoundExceptionMsg = "EC14";
        public static string PaymentGatewayFailureExceptionMsg = "EC15";
        public static string LoginVerificationFailedExceptionMsg = "EC16";
        public static string SmsVerificationExpiredExceptionMsg = "EC17";
        public static string SmsVerificationFailedExceptionMsg = "EC18";
        // C# Standard Exception Error Code Messages
        public static string SecurityTokenExpiredExceptionMsg = "EC0";
    }
}