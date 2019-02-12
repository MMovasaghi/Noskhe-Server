using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models
{
    public class CourierTextMessage
    {
        public int CourierTextMessageId { get; set; }
        public DateTime Date { get; set; }
        public CourierTextMessageType Type { get; set; }
        public string Message { get; set; }
        public int CourierId { get; set; }
        public Courier Courier { get; set; }
    }
}