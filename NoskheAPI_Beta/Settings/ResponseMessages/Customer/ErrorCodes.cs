namespace NoskheAPI_Beta.Settings.ResponseMessages.Customer
{
    public static class ErrorCodes
    {
        // Custom Exception Error Code Messages For Customer
        public static string APIUnhandledExceptionMsg = "CEC1"; // CustomerErrorCode1
        public static string DatabaseFailureExceptionMsg = "CEC2";
        public static string DuplicateCustomerExceptionMsg = "CEC3";
        public static string InvalidItemIdFoundExceptionMsg = "CEC4";
        public static string EmailIsNotValidExceptionMsg = "CEC5";
        public static string NoCosmeticsAvailabeExceptionMsg = "CEC6";
        public static string NoCosmeticsInTheShoppingCartExceptionMsg = "CEC7";
        public static string NoCustomersMatchedByPhoneExceptionMsg = "CEC8";
        public static string NoMedicinesAvailabeExceptionMsg = "CEC9";
        public static string RepeatedTextMessageRequestsExceptionMsg = "CEC10";
        public static string NoOrdersFoundExceptionMsg = "CEC11";
        public static string TextMessageVerificationTimeExpiredExceptionMsg = "CEC12";
        public static string NoShoppingCartsFoundExceptionMsg = "CEC13";
        public static string PaymentGatewayFailureExceptionMsg = "CEC14";
        public static string LoginVerificationFailedExceptionMsg = "CEC15";
        public static string EditProfileRuleExceptionMsg = "CEC16";
        public static string SmsVerificationFailedExceptionMsg = "CEC17"; // todo: remove this
        public static string RegistrationRuleExceptionMsg = "CEC18";
        public static string TextMessageVerificationFailedExceptionMsg = "CEC19";
        public static string NumberOfTextMessageTriesExceededExceptionMsg = "CEC20";
        public static string ExistingShoppingCartHasBeenRequestedEarlierExceptionMsg = "CEC21";
        public static string NoPharmaciesAreProvidingServiceExceptionMsg = "CEC22";
        public static string NoNotificationsMatchedByIdExceptionMsg = "CEC23"; // TODO: add to postman
        public static string PhoneNumberIsNotVerifiedExceptionMsg = "CEC24"; // TODO: add to postman
        // C# Standard Exception Error Code Messages
        public static string SecurityTokenExpiredExceptionMsg = "CEC0";
    }
}