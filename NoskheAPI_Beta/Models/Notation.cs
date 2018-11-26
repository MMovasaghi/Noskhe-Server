namespace NoskheAPI_Beta.Models
{
    public class Notation
    {
        public int NotationId { get; set; }
        public BrandType BrandPreference  { get; set; }
        public bool HasPregnancy { get; set; }
        public bool HasOtherDiseases { get; set; }
        public string Description { get; set; }
        /*
            1 ShoppingCart
        */
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}