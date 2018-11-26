namespace NoskheAPI_Beta.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string IBAN { get; set; } // shomare hesab
        public string AccountOwnerName { get; set; } // name sahebe hesab
        public string BankName { get; set; } // name bank
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
    }
}