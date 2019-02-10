using System;

namespace NoskheAPI_Beta.Models
{
    public class CustomerResetPasswordToken
    {
        public int CustomerResetPasswordTokenId { get; set; }
        public string Token { get; set; }
        public bool IsValid { get; set; } // dar ayande bara inke jelo user ro begirim dar goje sabz
        public uint TokenRefreshRequests { get; set; } // tedade dafaate taghyeere token
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        // nav
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}