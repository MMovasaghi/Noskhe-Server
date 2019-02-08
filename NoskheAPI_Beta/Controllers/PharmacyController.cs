using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NoskheBackend_Beta.General;
using NoskheAPI_Beta.Models;
using NoskheAPI_Beta.Models.Response;
using NoskheAPI_Beta.Classes.Communication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using NoskheAPI_Beta.Services;
using Microsoft.Extensions.Options;
using NoskheAPI_Beta.Classes;
using NoskheAPI_Beta.Settings.Routing.Pharmacy;
using Microsoft.IdentityModel.Tokens;
using NoskheAPI_Beta.CustomExceptions.Pharmacy;
using NoskheAPI_Beta.Settings.ResponseMessages.Pharmacy;

namespace NoskheAPI_Beta.Controllers
{
    [Authorize]
    [Route("desktop-api/[controller]")]
    [ApiController]
    public class PharmacyController : ControllerBase
    {
        private IHubContext<NotificationHub> _hubContext { get; set; }
        private IPharmacyService _pharmacyService;
        private readonly AppSettings _appSettings;
        public PharmacyController(IPharmacyService pharmacyService, IOptions<AppSettings> appSettings, IHubContext<NotificationHub> hubContext)
        {
            _pharmacyService = pharmacyService;
            _appSettings = appSettings.Value;
            _hubContext = hubContext;
        }

        // GET: desktop-api/pharmacy/db-state
        [HttpGet(Labels.GetDbStatus)]
        public ActionResult GetDbStatus()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.GetDbStatus());
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

        // GET: desktop-api/pharmacy/now
        [HttpGet(Labels.GetDateTime)]
        public ActionResult<ResponseTemplate> GetDateTime()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.GetDatetime());
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
        // GET: desktop-api/pharmacy/server-state
        [HttpGet(Labels.GetServerState)]
        public ActionResult GetServerState()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.GetServerState());
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
        
        // GET: desktop-api/pharmacy/profile
        [HttpGet(Labels.GetProfile)]
        public ActionResult<Models.Minimals.Output.Pharmacy> GetProfile(string upi)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.GetProfile());
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
        
        // GET: desktop-api/pharmacy/orders
        [HttpGet(Labels.GetOrders)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Order>> GetOrders(string start="start-undefined", string end="end-undefined")
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.GetOrders(start, end));
            }
            catch(InvalidTimeFormatException itfe)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = itfe.Message
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

        // GET: desktop-api/pharmacy/score
        [HttpGet(Labels.GetScore)]
        public ActionResult<Models.Minimals.Output.Score> GetScore()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.GetScore());
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

        // GET: desktop-api/pharmacy/settles
        [HttpGet(Labels.GetSettles)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Settle>> GetSettles(string start="start-undifiend", string end="end-undifiend")
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.GetSettles());
            }
            catch(NoInformationException nie)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = nie.Message
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

        // POST: desktop-api/pharmacy/new-settle
        [HttpPost(Labels.SetANewSettle)]
        public ActionResult SetANewSettle([FromBody] Models.Minimals.Input.Settle settle)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.SetANewSettle(settle));
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
        
        // GET: desktop-api/pharmacy/top-five
        [HttpGet(Labels.GetTopFivePharmacies)]
        public ActionResult<IEnumerable<Models.Minimals.Output.Score>> GetTopFivePharmacies(string upi)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.GetTopFivePharmacies());
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

        // POST: desktop-api/pharmacy/login
        [AllowAnonymous]
        [HttpPost(Labels.LoginWithEmailAndPass)]
        public ActionResult LoginWithEmailAndPass([FromBody] string[] credential)
        {
            try
            {
                return Ok(_pharmacyService.LoginWithEmailAndPass(credential, _appSettings));
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

        // PUT: desktop-api/pharmacy/toggle-state
        [HttpPut(Labels.ToggleStateOfPharmacy)]
        public ActionResult ToggleStateOfPharmacy()
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.ToggleStateOfPharmacy());
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

        // GET: desktop-api/pharmacy/orders-count-in-week
        [HttpGet(Labels.NumberOfOrdersInThisWeek)]
        public ActionResult<IEnumerable<float>> NumberOfOrdersInThisWeek(string upi)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.NumberOfOrdersInThisWeek());
            }
            catch(NoInformationException nie)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = nie.Message
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

        // GET: desktop-api/pharmacy/packings-average-time-in-week
        [HttpGet(Labels.AverageTimeOfPackingInThisWeek)]
        public ActionResult<IEnumerable<float>> AverageTimeOfPackingInThisWeek(string upi)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.AverageTimeOfPackingInThisWeek());
            }
            catch(NoInformationException nie)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = nie.Message
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

        /////////////////////////////////////////////////////////////////////////////
        // GET: desktop-api/pharmacy/service-response
        [HttpGet(Labels.ServiceResponse)]
        public ActionResult ServiceResponse(int shoppingCartId)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.ServiceResponse(shoppingCartId));
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

        // GET: desktop-api/pharmacy/invoice-details
        [HttpGet(Labels.InvoiceDetails)]
        public ActionResult InvoiceDetails(int shoppingCartId)
        {
            try
            {
                GrabTokenFromHeader();
                return Ok(_pharmacyService.InvoiceDetails(shoppingCartId));
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
            _pharmacyService.RequestToken = token.Substring("Bearer ".Length).Trim();
        }
    }
}