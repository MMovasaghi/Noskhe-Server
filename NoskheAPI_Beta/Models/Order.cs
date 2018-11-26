using System;

namespace NoskheAPI_Beta.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UOI { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public bool HasPrescription { get; set; }
        public bool HasBeenSetToACourier { get; set; }
        public bool HasBeenAcceptedByCustomer { get; set; } // ok budan ba poolesh
        public bool HasBeenPaid { get; set; }
        public bool HasBeenReceivedByCustomer { get; set; } // moshtari begirad
        public bool HasBeenDeliveredToCustomer { get; set; } // mototri bedahad
        /* new */
        public bool HasBeenSettled { get; set; }
        public DateTime SettlementDate { get; set; }
        public int PackingTime { get; set; }
        /* new */
        /*
            1 Courier
            1 Shopping Cart
            1 Pharmacy
        */
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int CourierId { get; set; }
        public Courier Courier { get; set; } // ----> dar hengame sqakhte order be yek courier temp vasl mikonim vali badan taeein mishavad (tavasote edite courier id)
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public Occurrence Occurrence { get; set; }
    }
}