namespace NoskheAPI_Beta.Models
{
    public class MedicineShoppingCart
    {
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int Quantity { get; set; }
    }
}