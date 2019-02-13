namespace NoskheAPI_Beta.Models
{
    public class PharmacyHubMap
    {
        public int PharmacyHubMapId { get; set; }
        public string ConnectionID { get; set; }
        public bool Connected { get; set; }
        // nav
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
    }
}