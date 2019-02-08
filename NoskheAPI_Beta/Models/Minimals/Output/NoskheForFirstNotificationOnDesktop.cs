using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoskheAPI_Beta.Models.Minimals.Output
{
    public class NoskheForFirstNotificationOnDesktop
    {
        public List<string> Picture_Urls { set; get; }
        public Notation Notation { get; set; }
        public List<Cosmetic> Cosmetics { set; get; }
        public List<Medicine> Medicions { set; get; }
        public Customer Customer { get; set; }
    }
}
