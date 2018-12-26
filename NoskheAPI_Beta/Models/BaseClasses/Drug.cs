using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoskheAPI_Beta.Models.BaseClasses
{
    public class Drug
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ProductPictureUrl { get; set; }
    }
}
