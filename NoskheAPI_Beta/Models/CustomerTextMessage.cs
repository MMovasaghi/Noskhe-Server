using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models
{
    public class CustomerTextMessage
    {
        public int CustomerTextMessageId { get; set; }
        public DateTime Date { get; set; }
        public CustomerTextMessageType Type { get; set; }
        public string Message { get; set; }
        public int NumberOfAttempts { get; set; }
        public bool Validated { get; set; }
        // nav
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}