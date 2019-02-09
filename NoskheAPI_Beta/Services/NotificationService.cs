using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NoskheAPI_Beta.Classes.Communication;
using NoskheAPI_Beta.Models.Minimals.Output;
using NoskheAPI_Beta.Models.Response;

namespace NoskheAPI_Beta.Services
{
    public interface INotificationService
    {
        // Pharmacy SignalR
        Task P_PharmacyReception(IHubContext<NotificationHub> hubContext, int pharmacyId, NoskheForFirstNotificationOnDesktop prescriptionDetails);   
        // Customer SignalR
        Task C_PharmacyInquiry(IHubContext<NotificationHub> hubContext, int customerId, string pharmacyName, string courierName, string phone, bool finalized); // for real time process
        Task C_InvoiceDetails(IHubContext<NotificationHub> hubContext, int customerId, decimal priceWithoutShippingCost, decimal shippingCost, string paymentUrl);
        Task C_CourierDetail(IHubContext<NotificationHub> hubContext, int customerId, string courierName, string phone); // not needed now
    }
    class NotificationService : INotificationService
    {
        // P_PharmacyReception;
        // PharmacyShoppingCartReception: find ShoppingCartMappingId record & send signal to its first pharmacy
        public void CallPharmacy(int pid)
        {
            
        }
        public void CallCustomer(int pid)
        {
            
        }

        public async Task P_PharmacyReception(IHubContext<NotificationHub> hubContext, int pharmacyId, NoskheForFirstNotificationOnDesktop prescriptionDetails)
        {
            await hubContext.Clients.Group("P" + pharmacyId.ToString()).SendAsync("PharmacyReception", prescriptionDetails);
        }

        public Task C_PharmacyInquiry(IHubContext<NotificationHub> hubContext, int customerId, string pharmacyName, string courierName, string phone, bool finalized)
        {
            throw new System.NotImplementedException();
        }

        public async Task C_InvoiceDetails(IHubContext<NotificationHub> hubContext, int customerId, decimal priceWithoutShippingCost, decimal shippingCost, string paymentUrl)
        {
            await hubContext.Clients.Group("C" + customerId.ToString()).SendAsync("InvoiceDetails", priceWithoutShippingCost, shippingCost, paymentUrl);
        }

        public Task C_CourierDetail(IHubContext<NotificationHub> hubContext, int customerId, string courierName, string phone)
        {
            throw new System.NotImplementedException();
        }
    }
}