using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models.Minimals.Output
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public string USCI { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public Dictionary<string, string[]> Cosmetics { get; set; }
        public Dictionary<string, string[]> Medicines { get; set; }
        public BrandType BrandPreference  { get; set; }
        public bool HasPregnancy { get; set; }
        public bool HasOtherDiseases { get; set; }
        public string Description { get; set; }
        public string PictureUrl_1 { get; set; }
        public string PictureUrl_2 { get; set; }
        public string PictureUrl_3 { get; set; }
        public decimal TotalPriceWithoutPrescription { get; set; }
        public bool HasBeenRequested { get; set; }
    }
}