using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public string USCI { get; set; }
        public DateTime Date { get; set; }
        public double AddressLongitude { get; set; }
        public double AddressLatitude { get; set; }
        public string Address { get; set; }
        public bool HasBeenRequested { get; set; }
        // -- Nav Properties --
        // public int NotationId { get; set; }
        // public Notation Notation { get; set; }
        // public int PrescriptionId { get; set; }
        // public Prescription Prescription { get; set; }
        // public int MyProperty { get; set; }
        /*
            1 Notation
            n Medicine
            n Cosmetic
            1 Prescription
            1 Order
            1 Customer
        */
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<CosmeticShoppingCart> CosmeticShoppingCarts { get; set; }
        public List<MedicineShoppingCart> MedicineShoppingCarts { get; set; }
        public Order Order { get; set; }
        public Notation Notation { get; set; }
        public Prescription Prescription { get; set; }
        public ServiceMapping PharmacyMapping { get; set; }
    }
}