using System.Collections.Generic;

namespace NoskheAPI_Beta.Models.Minimals.Input
{
    public class PrescriptionInvoice
    {
        public int ShoppingCartId { get; set; }
        public List<PrescriptionItem> PrescriptionItems { get; set; }
    }
}