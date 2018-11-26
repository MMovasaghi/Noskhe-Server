using System;

namespace NoskheAPI_Beta.Models.Minimals.Output
{
    public class Settle
    {
        public string USI { get; set; }
        public double CommissionCoefficient { get; set; }
        public int NumberOfOrders { get; set; }
        public DateTime Date { get; set; }
        public bool HasBeenSettled { get; set; }
        public decimal Credit { get; set; }
    }
}