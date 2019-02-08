namespace NoskheAPI_Beta.Models
{
    public enum PharmacyCancellationReason
    {
        OtherReason,
        NotEnoughInformation,
        BadPrescription // TODO: Prescription => HasBeenAcceptedByPharmacy = false
    }
}