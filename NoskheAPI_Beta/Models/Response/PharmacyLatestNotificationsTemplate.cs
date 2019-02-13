using System.Collections.Generic;

namespace NoskheAPI_Beta.Models.Response
{
    public class PharmacyLatestNotificationsTemplate
    {
        public bool Any { get; set; }
        public PharmacyReceptionObj PharmacyReceptionObj { get; set; }
    }
    public class PharmacyReceptionObj
    {
        public List<string[]> Content { get; set; }
    }
}