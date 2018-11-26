namespace NoskheAPI_Beta.Models.Minimals.Output
{
    public class Score
    {
        public string UPI { get; set; }
        public string PharmacyName { get; set; }
        public int CustomerSatisfaction { get; set; }
        public int RankAmongPharmacies { get; set; }
        public int PackingAverageTimeInSeconds { get; set; }
    }
}