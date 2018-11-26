using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models.Minimals.Output
{
    public class CustomerRequest
    {
        public string UOI { get; set; }
        public string PictureUrl_1 { get; set; }
        public string PictureUrl_2 { get; set; }
        public string PictureUrl_3 { get; set; }
        public Dictionary<string, string[]> Cosmetics { get; set; }
        public Dictionary<string, string[]> Medicines { get; set; }
    }
}