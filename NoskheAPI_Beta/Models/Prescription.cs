using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        public bool HasBeenAcceptedByPharmacy { get; set; }
        public string PictureUrl_1 { get; set; }
        public string PictureUrl_2 { get; set; }
        public string PictureUrl_3 { get; set; }
        public DateTime PicturesUploadDate { get; set; }
        /*
            n PrescriptionItem --> Tavasote darukhane por mishavad
            1 ShoppingCart
        */
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public List<PrescriptionItem> PrescriptionItems { get; set; }
    }
}