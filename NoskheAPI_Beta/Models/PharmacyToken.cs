using System;

namespace NoskheAPI_Beta.Models
{
    public class PharmacyToken
    {
        public int PharmacyTokenId { get; set; }
        public string Token { get; set; }
        public bool IsValid { get; set; } // dar ayande bara inke jelo logine pharmacy ro begirim dar goje sabz
        public uint TokenRefreshRequests { get; set; } // tedade dafaate taghyeere token
        public uint LoginRequests { get; set; } // tedade loginha baraye track
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        // nav
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
    }
}