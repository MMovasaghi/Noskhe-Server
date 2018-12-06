namespace NoskheAPI_Beta.Settings.Routing.Customer
{
    public static class Labels
    {
        // 1-login
        public const string LoginWithEmailAndPass = "login"; // Old: "Authenticate"
        public const string LoginWithPhoneNumber = "login-by-phone"; // Old: "Authenticate-By-Phone"
        // 2-register
        public const string AddNewCustomer = "new-customer"; // Old: "Add-New"
        // 3-profile
        public const string GetProfileInformation = "get-profile"; // Old: "Get-Details"
        public const string EditExistingCustomerProfile = "edit-profile"; // Old: "Edit-Existing"
        // 4-shopping cart
        public const string AddNewShoppingCart = "add-shopping-cart"; // Old: "Add-New-Shopping-Cart"
        public const string GetCustomerShoppingCarts = "get-shopping-carts"; // Old: "Get-Shopping-Carts"
        public const string GetMedicinesOfAShoppingCart = "get-shopping-cart-medicines"; // Old: "Get-Medicines-By-USCI"
        public const string GetCosmeticsOfAShoppingCart = "get-shopping-cart-cosmetics"; // Old: "Get-Cosmetics-By-USCI"
        // 5-order
        public const string GetCustomerOrders = "get-orders"; // Old: "Get-Orders"
        // 5-sms
        public const string RequestSmsForForgetPassword = "request-forget-password"; // Old: "Send-SMS-Authentication-Code" + HAS PROBLEM
        public const string VerifySmsCodeForForgetPassword = "verify-forget-password"; // Old: "Verify-SMS-Authentication-Code" + HAS PROBLEM
        // 6-payment
        public const string CreatePaymentUrlForOrder = "new-payment"; // Old: "Create-New-Payment-Gateway"
        // 7-products
        public const string GetAllMedicines = "get-medicines"; // Old: "Get-Medicines"
        public const string GetAllCosmetics = "get-cosmetics"; // Old: "Get-Medicines"
        
        // new function route names here
    }
}