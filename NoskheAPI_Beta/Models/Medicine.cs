using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models
{
    public class Medicine
    {
        public int MedicineId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ProductPictureUrl { get; set; }
        public DateTime ProductPictureUploadDate { get; set; }
        public MedicineType Type { get; set; }
        /*
            n ShoppingCart
        */
        public List<MedicineShoppingCart> MedicineShoppingCarts { get; set; }
    }
}