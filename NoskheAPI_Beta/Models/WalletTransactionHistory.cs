using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models
{
    public class WalletTransactionHistory
    {
        public int WalletTransactionHistoryId { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public bool IsSuccessful { get; set; }
        // nav
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}