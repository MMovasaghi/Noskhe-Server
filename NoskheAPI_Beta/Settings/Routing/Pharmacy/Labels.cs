namespace NoskheAPI_Beta.Settings.Routing.Pharmacy
{
    public static class Labels
    {
        // 1- connection
        public const string GetServerState = "server-state"; // "Get-Server-Status"
        public const string GetDbStatus = "db-state"; // "Get-Database-Status"
        public const string GetDateTime = "now"; // "Get-Datetime"
        // 2- login and profile
        public const string LoginWithEmailAndPass = "login"; // "Authenticate"
        public const string GetProfile = "profile"; // "Get-Details"
        public const string ToggleStateOfPharmacy = "toggle-state"; // "Change-Status"
        public const string GetScore = "score"; // "Get-Score"
        // 3- orders and settles
        public const string GetOrders = "orders"; // "Get-Orders"
        public const string GetSettles = "settles"; // "Get-Settles"
        public const string SetANewSettle = "new-settle"; // "Set-Settle"
        // 4- report
        public const string NumberOfOrdersInThisWeek = "orders-count-in-week"; // "Get-Weekly-Number-Of-Orders"
        public const string AverageTimeOfPackingInThisWeek = "packings-average-time-in-week"; // "Get-Weekly-Packing-Average-Time"
        public const string GetTopFivePharmacies = "top-five"; // "Get-Top-Five"
        // new functions goes here
        public const string ServiceResponse = "service-response";
        public const string InvoiceDetails = "invoice-details";
        public const string Logout = "logout";
    }
}