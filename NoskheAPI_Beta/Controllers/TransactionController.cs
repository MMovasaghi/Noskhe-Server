using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZarinPalGateway;

namespace NoskheAPI_Beta.Controllers
{
    public class TransactionController : Controller
    {
        [Route("Transaction/Report")]
        public async Task<IActionResult> Report(string Authority, string Status)
        {
            if (Authority != null && Status != null)
            {
                if (Status == "OK")
                {
                    ServicePointManager.Expect100Continue = false;
                    PaymentGatewayImplementationServicePortTypeClient zp = new PaymentGatewayImplementationServicePortTypeClient();
                    var request = await zp.PaymentVerificationAsync("9c82812c-08c8-11e8-ad5e-005056a205be", Authority, 100);
                    if (request.Body.Status == 100)
                    {
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
    }
}