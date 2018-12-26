using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoskheAPI_Beta.Models.Minimals.Output
{
    public class NoskheForFirstNotificationOnDesktop
    {
        public List<string> Picture_Urls { set; get; }
        public List<CosmeticOutput> Cosmetics { set; get; }
        public List<MedicineOutput> Medicions { set; get; }
        public CustomerOutput Customer { get; set; }
    }
}
