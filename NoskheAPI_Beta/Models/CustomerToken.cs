using System;

namespace NoskheAPI_Beta.Models
{
    public class CustomerToken
    {
        public int CustomerTokenId { get; set; }
        public string Token { get; set; }
        public bool IsValid { get; set; } // dar ayande bara inke jelo user ro begirim dar goje sabz
        public uint TokenRefreshRequests { get; set; } // tedade dafaate taghyeere token
        public uint LoginRequests { get; set; } // tedade loginha baraye track
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsAvailableInSignalR { get; set; } // bara inke toye hub login nakone dobare ta dardesar beshe
        // nav
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}