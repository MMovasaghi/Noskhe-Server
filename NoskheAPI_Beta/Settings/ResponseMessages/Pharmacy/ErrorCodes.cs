namespace NoskheAPI_Beta.Settings.ResponseMessages.Pharmacy
{
    public static class ErrorCodes
    {
        // Custom Exception Error Code Messages For Pharmacy
        public static string APIUnhandledExceptionMsg = "PEC1";
        public static string DatabaseFailureExceptionMsg = "PEC2";
        public static string InvalidTimeFormatExceptionMsg = "PEC3";
        public static string LoginVerificationFailedExceptionMsg = "PEC4";
        public static string NoInformationExceptionMsg = "PEC5";
        public static string PaymentGatewayFailureExceptionMsg = "PEC6";
        public static string PendingRequestInProgressExceptionMsg = "PEC7";
        public static string NoNotificationsMatchedByIdExceptionMsg = "PEC8"; // TODO: add to postman
        // C# Standard Exception Error Code Messages
        public static string SecurityTokenExpiredExceptionMsg = "PEC0";
    }
}