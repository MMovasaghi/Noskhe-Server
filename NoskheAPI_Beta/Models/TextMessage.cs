using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models
{
    public class TextMessage
    {
        public int TextMessageId { get; set; }
        public DateTime Date { get; set; }
        public string VerificationCode { get; set; }
        public bool HasBeenExpired { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}