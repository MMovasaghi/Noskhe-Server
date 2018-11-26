using System;

namespace NoskheAPI_Beta.Models
{
    public class Settle
    {
        public int SettleId { get; set; }
        public string USI { get; set; }
        public double CommissionCoefficient { get; set; }
        public int NumberOfOrders { get; set; }
        public DateTime Date { get; set; }
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public bool HasBeenSettled { get; set; }
        public decimal Credit { get; set; }
    }
}