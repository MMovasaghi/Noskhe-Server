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
using Microsoft.AspNetCore.SignalR;
using NoskheAPI_Beta.Classes.Communication;

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
        private INotificationService _notificationService;
        private readonly AppSettings _appSettings;
        private IHubContext<NotificationHub> _hubContext { get; set; }
        public CustomerController(ICustomerService customerService, INotificationService notificationService, IOptions<AppSettings> appSettings, IHubContext<NotificationHub> hubContext)
        {
            _customerService = customerService;
            _notificationService = notificationService;
            _appSettings = appSettings.Value;
            _hubContext = hubContext;
        }
        // GET: mobile-api/customer/profile
        [HttpGet(Labels.GetProfileInformation)]
        public ActionResult<Models.Minimals.Output.Customer> GetProfileInformation()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.GetProfileInformation());
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

        // GET: mobile-api/customer/shopping-carts
        [HttpGet(Labels.GetCustomerShoppingCarts)]
        public ActionResult<IEnumerable<Models.Minimals.Output.ShoppingCart>> GetCustomerShoppingCarts()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.GetCustomerShoppingCarts());
            }
            catch(NoShoppingCartsFoundException nscfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = nscfe.Message
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
        
        // GET: mobile-api/customer/orders
        [HttpGet(Labels.GetCustomerOrders)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Order>> GetCustomerOrders()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.GetCustomerOrders());
            }
            catch(NoOrdersFoundException nofe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = nofe.Message
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
        
        // GET: mobile-api/customer/all-cosmetics
        // [AllowAnonymous]
        [HttpGet(Labels.GetAllCosmetics)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Cosmetic>> GetAllCosmetics()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.GetAllCosmetics());
            }
            catch(NoCosmeticsAvailabeException ncae)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ncae.Message
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

        // GET: mobile-api/customer/shopping-cart-cosmetics
        [HttpGet(Labels.GetCosmeticsOfAShoppingCart)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Cosmetic>> GetCosmeticsOfAShoppingCart(int id)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.GetCosmeticsOfAShoppingCart(id));
            }
            catch(NoCosmeticsInTheShoppingCartException ncitsce)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ncitsce.Message
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
        
        // GET: mobile-api/customer/all-medicines
        // [AllowAnonymous]
        [HttpGet(Labels.GetAllMedicines)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Medicine>> GetAllMedicines()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.GetAllMedicines());
            }
            catch(NoMedicinesAvailabeException nmae)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = nmae.Message
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

        // GET: mobile-api/customer/shopping-cart-medicines
        [HttpGet(Labels.GetMedicinesOfAShoppingCart)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Medicine>> GetMedicinesOfAShoppingCart(int id)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.GetMedicinesOfAShoppingCart(id));
            }
            catch(NoMedicinesInTheShoppingCartException nmitsce)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = nmitsce.Message
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

        // POST: mobile-api/customer/login
        [AllowAnonymous]
        [HttpPost(Labels.LoginWithEmailAndPass)] //string[] credential
        public ActionResult<TokenTemplate> LoginWithEmailAndPass([FromBody] Models.Android.AuthenticateTemplate at)
        {
            try
            {
                return Ok(_customerService.LoginWithEmailAndPass(at, _appSettings));
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
            catch(UnauthorizedAccessException) // NOTE: if and only if the user has already registerd but the token hasn't been set for that user. Like db sample users (dar hengame sabte nam token tolid mishavad va emkan nadarad ke user bedune token bashad, vali agar token nadasht bayad unauthorized begirad)
            {
                return Unauthorized();
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // POST: mobile-api/customer/login-by-phone
        [AllowAnonymous]
        [HttpPost(Labels.LoginWithPhoneNumber)]
        public ActionResult LoginWithPhoneNumber([FromBody] Models.Android.AuthenticateByPhoneTemplate abp)
        {
            try
            {
                return Ok(_customerService.LoginWithPhoneNumber(abp, _appSettings));
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

        // POST: mobile-api/customer/request-forget-password
        [HttpPost(Labels.RequestSmsForForgetPassword)]
        public ActionResult RequestSmsForForgetPassword([FromBody] Models.Android.SendSmsAuthenticationCodeTemplate ssac)
        {
            throw new NotImplementedException();
            // TODO: HasBeenExpired -> true
            // TODO: Sms Api
        }

        // POST: mobile-api/customer/verify-forget-password
        [HttpPost(Labels.VerifySmsCodeForForgetPassword)]
        public ActionResult VerifySmsCodeForForgetPassword([FromBody] Models.Android.VerifySmsAuthenticationCodeTemplate vsac)
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

        // POST: mobile-api/customer/new-customer
        [AllowAnonymous]
        [HttpPost(Labels.AddNewCustomer)]
        public ActionResult<TokenTemplate> AddNewCustomer([FromBody] Models.Android.AddNewTemplate an)
        {
            try
            {
                return Ok(_customerService.AddNewCustomer(an, _appSettings));
            }
            catch(DuplicateCustomerException dce)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dce.Message
                });
            }
            catch(EmailAndPhoneAreNullException eapane)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = eapane.Message
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

        // PUT: mobile-api/customer/edit-profile
        [HttpPut(Labels.EditExistingCustomerProfile)]
        public ActionResult EditExistingCustomerProfile([FromBody] Models.Android.EditExistingTemplate ee)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.EditExistingCustomerProfile(ee));
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
        
        // POST: mobile-api/customer/new-shopping-cart
        [HttpPost(Labels.AddNewShoppingCart)]
        public ActionResult AddNewShoppingCart([FromBody] Models.Android.AddNewShoppingCartTemplate ansc)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.AddNewShoppingCart(ansc));
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

        // GET: mobile-api/customer/new-payment
        [HttpGet(Labels.CreatePaymentUrlForOrder)]
        public async Task<ActionResult<string>> CreatePaymentUrlForOrder(int id)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(await _customerService.CreatePaymentUrlForOrder(id, HttpContext.Request.Host));
            }
            catch(NoOrdersMatchedByIdException nombie)
            {
                return BadRequest(new ResponseTemplate
                {
                    Success = false,
                    Error = nombie.Message
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
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // GET: mobile-api/customer/add-credit
        [HttpGet(Labels.AddCreditToWallet)]
        public async Task<ActionResult<string>> AddCreditToWallet(int credit)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(await _customerService.AddCreditToWallet(credit, HttpContext.Request.Host));
            }
            catch(NoOrdersMatchedByIdException nombie)
            {
                return BadRequest(new ResponseTemplate
                {
                    Success = false,
                    Error = nombie.Message
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
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // GET: mobile-api/customer/wallet
        [HttpGet(Labels.WalletInquiry)]
        public ActionResult WalletInquiry()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.WalletInquiry());
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

        // Signalr Based Methods

        // // GET: mobile-api/customer/pay
        // [HttpGet(Labels.PayTheOrder)]
        // public ActionResult PayTheOrder(int orderId)
        // {
        //     try
        //     {
        //         GrabTokenFromHeader();
        //         return Ok(_customerService.PayTheOrder(orderId));
        //     }
        //     catch(DatabaseFailureException dfe)
        //     {
        //         return BadRequest(new ResponseTemplate {
        //             Success = false,
        //             Error = dfe.Message
        //         });
        //     }
        //     catch(UnauthorizedAccessException)
        //     {
        //         return Unauthorized();
        //     }
        //     catch(SecurityTokenExpiredException stee)
        //     {
        //         return BadRequest(new ResponseTemplate {
        //             Success = false,
        //             Error = stee.Message
        //         });
        //     }
        //     catch
        //     {
        //         return BadRequest(new ResponseTemplate {
        //             Success = false,
        //             Error = ErrorCodes.APIUnhandledExceptionMsg
        //         });
        //     }
        // }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // GET: mobile-api/customer/request-service
        [HttpGet(Labels.RequestService)]
        public ActionResult RequestService(int shoppingCartId)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.RequestService(_notificationService, _hubContext, shoppingCartId));
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
        
        private void GrabTokenFromHeader()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if(token == null || !token.StartsWith("Bearer")) throw new UnauthorizedAccessException();
            _customerService.RequestToken = token.Substring("Bearer ".Length).Trim();
        }
    }
}