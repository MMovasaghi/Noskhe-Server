namespace NoskheAPI_Beta.Services
{
    public interface IPharmacyService
    {
        int PharmacyId { get; set; }
    }
    class PharmacyService : IPharmacyService
    {
        public int PharmacyId { get; set; }
    }
}