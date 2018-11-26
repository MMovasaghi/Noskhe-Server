namespace NoskheAPI_Beta.Models
{
    public class CosmeticShoppingCart
    {
        public int CosmeticId { get; set; }
        public Cosmetic Cosmetic { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int Quantity { get; set; }
    }
}