using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models.Minimals.Output
{
    public class Order
    {
        public string UOI { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPriceWithoutShippingCost { get; set; }
        public decimal TotalPrice { get; set; }
        public bool HasPrescription { get; set; }
        public bool HasBeenAcceptedByCustomer { get; set; } // ok budan ba poolesh
        public bool HasBeenPaid { get; set; }
        public bool HasBeenDeliveredToCustomer { get; set; }
        public bool HasBeenSettled { get; set; }
        public string CourierName { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyAddress { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public Dictionary<string, string[]> Cosmetics { get; set; }
        public Dictionary<string, string[]> Medicines { get; set; }
        public BrandType BrandPreference  { get; set; }
        public bool HasPregnancy { get; set; }
        public bool HasOtherDiseases { get; set; }
        public string Description { get; set; }
        public bool HasBeenAcceptedByPharmacy { get; set; }
        public string PictureUrl_1 { get; set; }
        public string PictureUrl_2 { get; set; }
        public string PictureUrl_3 { get; set; }
        public Dictionary<string, string[]> PrescriptionItems { get; set; }
        public int PackingTime { get; set; }
    }
}