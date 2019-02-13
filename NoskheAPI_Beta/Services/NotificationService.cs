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
        Task P_PharmacyReception(IHubContext<NotificationHub> hubContext, int pharmacyId, /* int nofiticationId,*/ NoskheForFirstNotificationOnDesktop prescriptionDetails);   
        Task P_CheckAvailablity(IHubContext<NotificationHub> hubContext, int pharmacyId);   
        // Customer SignalR
        Task C_PharmacyInquiry(IHubContext<NotificationHub> hubContext, int customerId, /*int nofiticationId, */ string pharmacyName, string courierName, string phone); // for real time process
        Task C_InvoiceDetails(IHubContext<NotificationHub> hubContext, int customerId, int nofiticationId, decimal priceWithoutShippingCost, decimal shippingCost, string paymentUrl);
        Task C_CourierDetail(IHubContext<NotificationHub> hubContext, int customerId, int nofiticationId, string courierName, string phone); // not needed now
        Task C_CancellationReport(IHubContext<NotificationHub> hubContext, int nofiticationId, int customerId);
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

        public async Task P_PharmacyReception(IHubContext<NotificationHub> hubContext, int pharmacyId, /*int nofiticationId, */ NoskheForFirstNotificationOnDesktop prescriptionDetails)
        {
            await hubContext.Clients.Group("P" + pharmacyId.ToString()).SendAsync("PharmacyReception", /*nofiticationId, */ prescriptionDetails);
        }

        public async Task P_CheckAvailablity(IHubContext<NotificationHub> hubContext, int pharmacyId)
        {
            await hubContext.Clients.Group("P" + pharmacyId.ToString()).SendAsync("CheckAvailablity");
        }

        public async Task C_PharmacyInquiry(IHubContext<NotificationHub> hubContext, int customerId, /*int nofiticationId, */ string pharmacyName, string courierName, string phone)
        {
            await hubContext.Clients.Group("C" + customerId.ToString()).SendAsync("PharmacyInquiry", /*nofiticationId, */ pharmacyName, courierName, phone);
        }

        public async Task C_InvoiceDetails(IHubContext<NotificationHub> hubContext, int customerId, int nofiticationId, decimal priceWithoutShippingCost, decimal shippingCost, string paymentUrl)
        {
            await hubContext.Clients.Group("C" + customerId.ToString()).SendAsync("InvoiceDetails", nofiticationId, priceWithoutShippingCost, shippingCost, paymentUrl);
        }

        public Task C_CourierDetail(IHubContext<NotificationHub> hubContext, int customerId, int nofiticationId, string courierName, string phone)
        {
            throw new System.NotImplementedException();
        }

        public async Task C_CancellationReport(IHubContext<NotificationHub> hubContext, int nofiticationId, int customerId)
        {
            await hubContext.Clients.Group("C" + customerId.ToString()).SendAsync("CancellationReport");
        }
    }
}