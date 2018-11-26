using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models
{
    public class Cosmetic
    {
        public int CosmeticId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ProductPictureUrl { get; set; }
        public DateTime ProductPictureUploadDate { get; set; }
        /*
            n ShoppingCart
        */
        public List<CosmeticShoppingCart> CosmeticShoppingCarts { get; set; }
    }
}