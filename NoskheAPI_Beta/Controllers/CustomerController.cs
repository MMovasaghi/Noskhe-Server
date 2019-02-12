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

        // GET: mobile-api/customer/add-credit
        [HttpGet(Labels.AddCreditToWallet)]
        public async Task<ActionResult> AddCreditToWallet(int credit)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(await _customerService.AddCreditToWallet(credit, HttpContext.Request.Host));
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

        // GET: mobile-api/customer/request-service
        [HttpGet(Labels.RequestService)]
        public async Task<ActionResult> RequestService(int shoppingCartId)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(await _customerService.RequestService(_notificationService, _hubContext, shoppingCartId));
            }
            catch(NoPharmaciesAreProvidingServiceException npapse)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = npapse.Message
                });
            }
            catch(ExistingShoppingCartHasBeenRequestedEarlierException eschbree)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = eschbree.Message
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

        [AllowAnonymous]
        // POST: mobile-api/customer/request-phone-login
        [HttpPost(Labels.RequestPhoneLogin)]
        public ActionResult RequestPhoneLogin([FromBody] Models.Android.PhoneTemplate pt)
        {
            try
            {
                return Ok(_customerService.RequestPhoneLogin(pt));
            }
            catch(RepeatedTextMessageRequestsException rtmte)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = rtmte.Message
                });
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

        [AllowAnonymous]
        // POST: mobile-api/customer/verify-phone-login
        [HttpPost(Labels.VerifyPhoneLogin)]
        public ActionResult VerifyPhoneLogin([FromBody] Models.Android.VerifyPhoneTemplate vpt)
        {
            try
            {
                return Ok(_customerService.VerifyPhoneLogin(vpt, _appSettings));
            }
            catch(NoCustomersMatchedByPhoneException ncmbpe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ncmbpe.Message
                });
            }
            catch(TextMessageVerificationTimeExpiredException tmvtee)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = tmvtee.Message
                });
            }
            catch(TextMessageVerificationFailedException tmvfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = tmvfe.Message
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
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        [AllowAnonymous]
        // POST: mobile-api/customer/request-reset-password
        [HttpPost(Labels.RequestResetPassword)]
        public ActionResult RequestResetPassword([FromBody] Models.Android.PhoneTemplate pt)
        {
            try
            {
                return Ok(_customerService.RequestResetPassword(pt));
            }            
            catch(NoCustomersMatchedByPhoneException ncmbpe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ncmbpe.Message
                });
            }
            catch(RepeatedTextMessageRequestsException rtmre)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = rtmre.Message
                });
            }
            catch(NumberOfTextMessageTriesExceededException notmtee)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = notmtee.Message
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
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        [AllowAnonymous]
        // POST: mobile-api/customer/verify-reset-password
        [HttpPost(Labels.VerifyResetPassword)]
        public ActionResult VerifyResetPassword([FromBody] Models.Android.VerifyPhoneTemplate vpt)
        {
            try
            {
                return Ok(_customerService.VerifyResetPassword(vpt, _appSettings));
            }
            catch(NoCustomersMatchedByPhoneException ncmbpe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ncmbpe.Message
                });
            }
            catch(TextMessageVerificationFailedException tmvfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = tmvfe.Message
                });
            }
            catch(TextMessageVerificationTimeExpiredException tmvtee)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = tmvtee.Message
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
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // POST: mobile-api/customer/reset-password
        [HttpPost(Labels.ResetPassword)]
        public ActionResult ResetPassword([FromBody] Models.Android.ResetPasswordTemplate rpt)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_customerService.ResetPassword(rpt));
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

        [AllowAnonymous]
        // POST: mobile-api/customer/verify-phone-number
        [HttpPost(Labels.VerifyPhoneNumber)]
        public ActionResult VerifyPhoneNumber([FromBody] Models.Android.VerifyPhoneTemplate vpt)
        {
            try
            {
                return Ok(_customerService.VerifyPhoneNumber(vpt));
            }
            catch(NoCustomersMatchedByPhoneException ncmbpe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ncmbpe.Message
                });
            }
            catch(TextMessageVerificationFailedException tmvfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = tmvfe.Message
                });
            }
            catch(TextMessageVerificationTimeExpiredException tmvtee)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = tmvtee.Message
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
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // // GET: mobile-api/customer/logout
        // [HttpGet(Labels.Logout)]
        // public ActionResult Logout()
        // {
        //     try
        //     {
        //         GrabTokenFromHeader();
        //         return Ok(_customerService.Logout());
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

        private void GrabTokenFromHeader()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if(token == null || !token.StartsWith("Bearer")) throw new UnauthorizedAccessException();
            _customerService.RequestToken = token.Substring("Bearer ".Length).Trim();
        }
    }
}