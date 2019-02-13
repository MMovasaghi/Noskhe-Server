using System.Collections.Generic;

namespace NoskheAPI_Beta.Models.Response
{
    public class CustomerLatestNotificationsTemplate
    {
        public bool Any { get; set; }
        public CancellationReportObj CancellationReportObj { get; set; }
        public CourierDetailObj CourierDetailObj { get; set; }
        public InvoiceDetailsObj InvoiceDetailsObj { get; set; }
        public PharmacyInquiryObj PharmacyInquiryObj { get; set; }
    }
    public class CancellationReportObj
    {
        public List<string[]> Content { get; set; }
    }
    public class CourierDetailObj
    {
        public List<string[]> Content { get; set; }
    }
    public class InvoiceDetailsObj
    {
        public List<string[]> Content { get; set; }
    }
    public class PharmacyInquiryObj
    {
        public List<string[]> Content { get; set; }
    }
}