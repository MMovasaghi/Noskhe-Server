using System;

namespace NoskheAPI_Beta.Models
{
    public class PharmacyNotification
    {
        public int PharmacyNotificationId { get; set; }
        public bool HasRecieved { get; set; }
        public PharmacyNotificationType Type  { get; set; }
        public DateTime Date { get; set; }
        /*
            1 Pharmacy
        */
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
    }
}