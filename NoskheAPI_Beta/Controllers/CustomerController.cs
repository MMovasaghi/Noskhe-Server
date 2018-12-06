using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoskheBackend_Beta.Encryption;
using NoskheAPI_Beta.Models;
using NoskheAPI_Beta.Models.Response;
using ZarinPalGateway;
using Microsoft.EntityFrameworkCore;
using NoskheAPI_Beta.Classes.UIGenerator;
using Microsoft.AspNetCore.Http;
using NoskheAPI_Beta.Classes;
using Microsoft.AspNetCore.Authorization;
using NoskheAPI_Beta.Services;
using NoskheAPI_Beta.CustomExceptions.Customer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NoskheAPI_Beta.Settings.ResponseMessages.Customer;
using NoskheAPI_Beta.Settings.Routing.Customer;

namespace NoskheAPI_Beta.Controllers
{
    // 1- funce sms ha kolan kharab hast
    // 3- AuthenticateByPhone kharab ast
    [Authorize]
    [Route("mobile-api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerService _customerService;
        private readonly AppSettings _appSettings;
        public CustomerController(ICustomerService customerService, IOptions<AppSettings> appSettings)
        {
            _customerService = customerService;
            _appSettings = appSettings.Value;
        }
        // GET: mobile-api/customer/get-details
        [HttpGet(Labels.GetProfileInformation)]
        public ActionResult<Models.Minimals.Output.Customer> GetDetails()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.GetDetails());
            }
            catch(NoCustomersFoundException ncfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ncfe.Message
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(SecurityTokenExpiredException stee)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = stee.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // GET: mobile-api/customer/get-shopping-carts
        [HttpGet(Labels.GetCustomerShoppingCarts)]
        public ActionResult<IEnumerable<Models.Minimals.Output.ShoppingCart>> GetShoppingCarts()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.GetShoppingCarts());
            }
            catch(NoShoppingCartsFoundException nscfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = nscfe.Message
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(SecurityTokenExpiredException stee)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = stee.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }
        
        // GET: mobile-api/customer/get-orders
        [HttpGet(Labels.GetCustomerOrders)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Order>> GetOrders()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.GetMedicines());
            }
            catch(NoOrdersFoundException nofe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = nofe.Message
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(SecurityTokenExpiredException stee)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = stee.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }
        
        // GET: mobile-api/customer/get-cosmetics
        [AllowAnonymous]
        [HttpGet(Labels.GetAllCosmetics)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Cosmetic>> GetCosmetics()
        {
            try
            {
                return Ok(_customerService.GetCosmetics());
            }
            catch(NoCosmeticsAvailabeException ncae)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ncae.Message
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // GET: mobile-api/customer/get-cosmetics-by-usci
        [HttpGet(Labels.GetCosmeticsOfAShoppingCart)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Cosmetic>> GetCosmetics(string usci)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.GetShoppingCartCosmetics(usci));
            }
            catch(NoCosmeticsMatchedByUSCIExcpetion ncmbue)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ncmbue.Message
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(SecurityTokenExpiredException stee)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = stee.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }
        
        // GET: mobile-api/customer/get-medicines
        [AllowAnonymous]
        [HttpGet(Labels.GetAllMedicines)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Medicine>> GetMedicines()
        {
            try
            {
                return Ok(_customerService.GetMedicines());
            }
            catch(NoMedicinesAvailabeException nmae)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = nmae.Message
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // GET: mobile-api/customer/get-medicines-by-usci
        [HttpGet(Labels.GetMedicinesOfAShoppingCart)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Medicine>> GetMedicines(string usci)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.GetShoppingCartMedicines(usci));
            }
            catch(NoMedicinesMatchedByUSCIExcpetion nmmbue)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = nmmbue.Message
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(SecurityTokenExpiredException stee)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = stee.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // POST: mobile-api/customer/authenticate
        [AllowAnonymous]
        [HttpPost(Labels.LoginWithEmailAndPass)] //string[] credential
        public ActionResult<TokenTemplate> Authenticate([FromBody] Models.Android.AuthenticateTemplate at)
        {
            try
            {
                return Ok(_customerService.Authenticate(at, _appSettings));
            }
            catch(LoginVerificationFailedException vfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = vfe.Message
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // POST: mobile-api/customer/authenticate-by-phone
        [AllowAnonymous]
        [HttpPost(Labels.LoginWithPhoneNumber)]
        public ActionResult Authenticate([FromBody] Models.Android.AuthenticateByPhoneTemplate abp)
        {
            try
            {
                return Ok(_customerService.AuthenticateByPhone(abp, _appSettings));
            }
            catch(NoCustomersMatchedByPhoneException ncmbpe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ncmbpe.Message
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // POST: mobile-api/customer/send-sms-authentication-code
        [HttpPost(Labels.RequestSmsForForgetPassword)]
        public ActionResult SendSMS([FromBody] Models.Android.SendSmsAuthenticationCodeTemplate ssac)
        {
            throw new NotImplementedException();
            // TODO: HasBeenExpired -> true
            // TODO: Sms Api
        }

        // POST: mobile-api/customer/verify-sms-authentication-code
        [HttpPost(Labels.VerifySmsCodeForForgetPassword)]
        public ActionResult VerifySMSAuthenticationCode([FromBody] Models.Android.VerifySmsAuthenticationCodeTemplate vsac)
        {
            throw new NotImplementedException();
            // try
            // {
            //     var response = db.TextMessages.Where(q => (q.Customer.Phone == vsac.Phone && q.VerificationCode == vsac.VerificationCode)).FirstOrDefault();
            //     if(response != null)
            //     {
            //         if(response.HasBeenExpired == false)
            //         {
            //             response.HasBeenExpired = true; // vaghti ke dorost bude YA vaghte masalan 2 daghighe tamum shode bashe
            //             db.SaveChanges();

            //             // satisfying result
            //             return Ok(new ResponseTemplate {
            //                 Success = true
            //             });
            //         }

            //         return BadRequest(new ResponseTemplate {
            //             Success = false,
            //             Error = "VERIFICATION_EXPIRED"
            //         });
            //     }

            //     return BadRequest(new ResponseTemplate {
            //         Success = false,
            //         Error = "VERIFICATION_FAILED"
            //     });
            // }
            // catch
            // {
            //     return BadRequest(new ResponseTemplate {
            //         Success = false,
            //         Error = "DATABASE_FAILURE"
            //     });
            // }
        }

        // POST: mobile-api/customer/add-new
        [AllowAnonymous]
        [HttpPost(Labels.AddNewCustomer)]
        public ActionResult<TokenTemplate> AddNew([FromBody] Models.Android.AddNewTemplate an)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.AddNewCustomer(an, _appSettings));
            }
            catch(DuplicateCustomerException dce)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dce.Message
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // PUT: mobile-api/customer/edit-existing
        [HttpPut(Labels.EditExistingCustomerProfile)]
        public ActionResult EditExisting([FromBody] Models.Android.EditExistingTemplate ee)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.EditExistingCustomer(ee));
            }
            catch(NoCustomersFoundException ncfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ncfe.Message
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(SecurityTokenExpiredException stee)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = stee.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }
        
        // POST: mobile-api/customer/add-new-shopping-cart
        [HttpPost(Labels.AddNewShoppingCart)]
        public ActionResult AddNewShoppingCart([FromBody] Models.Android.AddNewShoppingCartTemplate ansc)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.AddNewShoppingCart(ansc));
            }
            catch(NoCustomersFoundException ncfe)
            {
                return  BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ncfe.Message
                });
            }
            catch(InvalidCosmeticIDFoundException icife)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = icife.Message // TODO: ADD THIS TO DOC
                });
            }
            catch(InvalidMedicineIDFoundException imife)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = imife.Message // TODO: ADD THIS TO DOC
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(SecurityTokenExpiredException stee)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = stee.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // GET: mobile-api/customer/create-new-payment-gateway
        [HttpGet(Labels.CreatePaymentUrlForOrder)]
        public async Task<ActionResult<string>> CreateNewPaymentGateway(string uoi)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(await _customerService.CreateNewPaymentGateway(uoi, HttpContext.Request.Host));
            }
            catch(NoOrdersMatchedByUOIException nombue)
            {
                return BadRequest(new ResponseTemplate
                {
                    Success = false,
                    Error = nombue.Message
                });
            }
            catch(PaymentGatewayFailureException pgfe)
            {
                return BadRequest(new ResponseTemplate
                {
                    Success = false,
                    Error = pgfe.Message
                });
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(SecurityTokenExpiredException stee)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = stee.Message
                });
            }
            catch(DatabaseFailureException dfe)
            {
                return BadRequest(new ResponseTemplate
                {
                    Success = false,
                    Error = dfe.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        private void GrabTokenFromHeader()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if(token == null || !token.StartsWith("Bearer")) throw new UnauthorizedAccessException();
            _customerService.RequestToken = token.Substring("Bearer ".Length).Trim();
        }
    }
}