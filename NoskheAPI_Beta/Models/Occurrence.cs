using System;

namespace NoskheAPI_Beta.Models
{
    public class Occurrence
    {
        public int OccurrenceId { get; set; }
        public DateTime CustomerHasRequestedOrderOn { get; set; }
        public DateTime PharmacyHasAcceptedOrderOn { get; set; }
        public DateTime CustomerHasFinallyAcceptedOrderOn { get; set; }
        public DateTime PharmacyHasPackedOrderOn { get; set; }
        public DateTime CourierHasReceivedOrderFromPharmacyOn { get; set; }
        public DateTime CourierHasDeliveredOrderToCustomerOn { get; set; }
        public DateTime CustomerHasReceivedOrderFromCourierOn { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}