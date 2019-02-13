namespace NoskheAPI_Beta.Models
{
    public class CustomerHubMap
    {
        public int CustomerHubMapId { get; set; }
        public string ConnectionID { get; set; }
        public bool Connected { get; set; }
        // nav
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}