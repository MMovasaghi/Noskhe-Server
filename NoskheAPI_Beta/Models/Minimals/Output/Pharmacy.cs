namespace NoskheAPI_Beta.Models.Minimals.Output
{
    public class Pharmacy
    {
        public string Name { get; set; }
        public string UPI { get; set; } // code e sabt
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Address { get; set; }
        public bool IsAvailableNow { get; set; }
        public decimal Credit { get; set; }
        public string ManagerName { get; set; }
    }
}