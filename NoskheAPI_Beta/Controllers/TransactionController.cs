using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoskheAPI_Beta.Models;
using ZarinPalGateway;

namespace NoskheAPI_Beta.Controllers
{
    public class TransactionController : Controller
    {
        private static NoskheAPI_Beta.Models.NoskheContext db = new NoskheAPI_Beta.Models.NoskheContext();
        [Route("Transaction/{orderId}/Order")]
        public async Task<IActionResult> Order(int orderId, string Authority, string Status)
        {
            if (Authority != null && Status != null)
            {
                if (Status == "OK")
                {
                    ServicePointManager.Expect100Continue = false;
                    PaymentGatewayImplementationServicePortTypeClient zp = new PaymentGatewayImplementationServicePortTypeClient();
                    int price = 0;

                    var existingOrder = db.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();
                    db.Entry(existingOrder).Reference(o => o.ShoppingCart).Load();
                    price = existingOrder.Price;
                    
                    var request = await zp.PaymentVerificationAsync("9c82812c-08c8-11e8-ad5e-005056a205be", Authority, price); // BUG: amount of money is not defined
                    if (request.Body.Status == 100) // TODO: if money is paid correctly, then add to customer's wallet
                    {
                        var existingCustomer = db.Customers.Where(c => c.CustomerId == existingOrder.ShoppingCart.CustomerId).FirstOrDefault();
                        existingCustomer.Money += price;
                        db.SaveChanges();
                        
                        ViewBag.IsSuccess = true;
                        ViewBag.RefID = request.Body.RefID;
                    }
                    else
                    {
                        ViewBag.IsSuccess = false;
                        ViewBag.Description = $"پرداخت شما از طریق درگاه موفقیت آمیز نبوده است. چنانچه مبلغ از حساب شما کم شده باشد در 24 ساعت بعد به حساب شما باز خواهد گشت";
                    }
                }
                else if (Status == "NOK")
                {
                    ViewBag.IsSuccess = false;
                    ViewBag.IsStatusNOK = true;
                }
            }
            return View();
        }

        [Route("Transaction/{walletTransactionHistoryId}/Wallet")]
        public async Task<IActionResult> Wallet(int walletTransactionHistoryId, string Authority, string Status)
        {
            if (Authority != null && Status != null)
            {
                if (Status == "OK")
                {
                    ServicePointManager.Expect100Continue = false;
                    PaymentGatewayImplementationServicePortTypeClient zp = new PaymentGatewayImplementationServicePortTypeClient();

                    var existingWalletTransactionHistory = db.WalletTransactionHistories.Where(o => o.WalletTransactionHistoryId == walletTransactionHistoryId).FirstOrDefault();
                    
                    int price = existingWalletTransactionHistory.Price;
                    
                    var request = await zp.PaymentVerificationAsync("9c82812c-08c8-11e8-ad5e-005056a205be", Authority, existingWalletTransactionHistory.Price); // BUG: amount of money is not defined
                    var existingCustomer = db.Customers.Where(c => c.CustomerId == existingWalletTransactionHistory.CustomerId).FirstOrDefault();
                    if (request.Body.Status == 100) // TODO: if money is paid correctly, then add to customer's wallet
                    {
                        existingCustomer.Money += price;
                        existingWalletTransactionHistory.IsSuccessful = true;
                        db.SaveChanges();
                        
                        ViewBag.IsSuccess = true;
                        ViewBag.RefID = request.Body.RefID;
                    }
                    else
                    {
                        existingWalletTransactionHistory.IsSuccessful = false;
                        
                        ViewBag.IsSuccess = false;
                        ViewBag.Description = $"پرداخت شما از طریق درگاه موفقیت آمیز نبوده است. چنانچه مبلغ از حساب شما کم شده باشد در 24 ساعت بعد به حساب شما باز خواهد گشت";
                    }
                }
                else if (Status == "NOK")
                {
                    ViewBag.IsSuccess = false;
                    ViewBag.IsStatusNOK = true;
                }
            }
            return View();
        }
    }
}