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
        Task C_PharmacyInquiry(IHubContext<NotificationHub> hubContext, int customerId, string pharmacyName, string courierName, string phone, bool finalized);
        Task C_PaymentDetail(IHubContext<NotificationHub> hubContext, int customerId);
        Task C_CourierDetail(IHubContext<NotificationHub> hubContext, int customerId, string courierName, string phone);
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
            // await
        }

        public Task C_PharmacyInquiry(IHubContext<NotificationHub> hubContext, int customerId, string pharmacyName, string courierName, string phone, bool finalized)
        {
            throw new System.NotImplementedException();
        }

        public Task C_PaymentDetail(IHubContext<NotificationHub> hubContext, int customerId)
        {
            throw new System.NotImplementedException();
        }

        public Task C_CourierDetail(IHubContext<NotificationHub> hubContext, int customerId, string courierName, string phone)
        {
            throw new System.NotImplementedException();
        }
    }
}