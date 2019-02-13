using System;

namespace NoskheAPI_Beta.Models
{
    public class CustomerNotification
    {
        public int CustomerNotificationId { get; set; }
        public bool HasRecieved { get; set; }
        public CustomerNotificationType Type { get; set; }
        public DateTime Date { get; set; }
        public string Arg1 { get; set; }
        public string Arg2 { get; set; }
        public string Arg3 { get; set; }
        public string Arg4 { get; set; }
        public string Arg5 { get; set; }
        public string Arg6 { get; set; }
        /*
            1 Customer
        */
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}