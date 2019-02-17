using System.Collections.Generic;

namespace NoskheAPI_Beta.Models.Minimals.Input
{
    public class ShoppingCart
    {
        public BrandType BrandPreference  { get; set; }
        public bool HasPregnancy { get; set; }
        public bool HasOtherDiseases { get; set; }
        public string Description { get; set; }
        public string PictureUrl_1 { get; set; }
        public string PictureUrl_2 { get; set; }
        public string PictureUrl_3 { get; set; }
        public double AddressLongitude { get; set; }
        public double AddressLatitude { get; set; }
        public string Address { get; set; }
        public Dictionary<int, int> MedicineIds { get; set; }
        public Dictionary<int, int> CosmeticIds { get; set; }
        public bool HasBeenRequested { get; set; } // true: search for pharmacy, false: save
    }
}