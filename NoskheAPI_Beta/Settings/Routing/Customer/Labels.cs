namespace NoskheAPI_Beta.Settings.Routing.Customer
{
    public static class Labels
    {
        // 1-login
        public const string LoginWithEmailAndPass = "login"; // Old: "Authenticate"
        // phone login
        public const string RequestPhoneLogin = "request-phone-login";
        public const string VerifyPhoneLogin = "verify-phone-login";
        // reset password
        public const string RequestResetPassword = "request-reset-password";
        public const string VerifyResetPassword = "verify-reset-password";
        public const string ResetPassword = "reset-password";
        // verify phone
        public const string VerifyPhoneNumber = "verify-phone-number";
        // 2-register
        public const string AddNewCustomer = "new-customer"; // Old: "Add-New"
        // 3-profile
        public const string GetProfileInformation = "profile"; // Old: "Get-Details"
        public const string EditExistingCustomerProfile = "edit-profile"; // Old: "Edit-Existing"
        // 4-shopping cart
        public const string AddNewShoppingCart = "new-shopping-cart"; // Old: "Add-New-Shopping-Cart"
        public const string GetCustomerShoppingCarts = "shopping-carts"; // Old: "Get-Shopping-Carts"
        // 5-order
        public const string GetCustomerOrders = "orders"; // Old: "Get-Orders"
        // 5-sms
        public const string RequestSmsForForgetPassword = "request-forget-password"; // Old: "Send-SMS-Authentication-Code" + HAS PROBLEM
        public const string VerifySmsCodeForForgetPassword = "verify-forget-password"; // Old: "Verify-SMS-Authentication-Code" + HAS PROBLEM
        // 6-payment
        // 7-products
        public const string GetAllMedicines = "all-medicines"; // Old: "Get-Medicines"
        public const string GetAllCosmetics = "all-cosmetics"; // Old: "Get-Medicines"
        
        // new function route names here
        public const string AddCreditToWallet = "add-credit";
        public const string WalletInquiry = "wallet";
        public const string RequestService = "request-service";
    }
}