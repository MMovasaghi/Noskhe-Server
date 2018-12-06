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

namespace NoskheAPI_Beta.Controllers
{
    [Authorize]
    [Route("desktop-api/[controller]")]
    [ApiController]
    public class PharmacyController : ControllerBase
    {
        /*
            GET : get-database-status
            └──── Parameter(s): none
            └─────── Output(s): single 'ResponseTemplate' model
            └──────── Error(s): DATABASE_FAILURE
            └───── Description: Checks whether the server can receive any data from database file or not.
            -----------------------------------------------------------------------------------------------------
            GET : get-server-status
            └──── Parameter(s): none
            └─────── Output(s): single 'ResponseTemplate' model
            └──────── Error(s): none
            └───── Description: Checks whether the server communicates properly to it's clients.
            -----------------------------------------------------------------------------------------------------
            GET : get-details
            └──── Parameter(s): 'upi' string
            └─────── Output(s): single 'Pharmacy' model, single 'ResponseTemplate' model
            └──────── Error(s): NO_PHARMACIES_MATCHED_THE_UPI, DATABASE_FAILURE
            └───── Description: Returns pharmacy details which can be used to show in profile.
            -----------------------------------------------------------------------------------------------------
            GET : get-orders
            └──── Parameter(s): 'upi' string
            └─────── Output(s): multiple 'Order' models, single 'ResponseTemplate' model
            └──────── Error(s): BAD_START_TIME_FORMAT, BAD_END_TIME_FORMAT, START_TIME_IS_GREATER_THAN_END_TIME, NO_RESPONSES_MATCHED_THE_UPI, DATABASE_FAILURE
            └───── Description: Returns a list of orders which are linked to the pharmacy.
            -----------------------------------------------------------------------------------------------------
            GET : get-score
            └──── Parameter(s): 'upi' string
            └─────── Output(s): single 'Score' model, single 'ResponseTemplate' model
            └──────── Error(s): NO_SCORES_MATCHED_THE_UPI, DATABASE_FAILURE
            └───── Description: Returns pharmacy score details.
            -----------------------------------------------------------------------------------------------------
            GET : get-settles
            └──── Parameter(s): 'upi' string
            └─────── Output(s): multiple 'Settle' models, single 'ResponseTemplate' model
            └──────── Error(s): NO_SETTLES_MATCHED_THE_UPI, DATABASE_FAILURE
            └───── Description: Gets a list of settles in which the pharmacy has submitted earlier.
            -----------------------------------------------------------------------------------------------------
            POST : set-settle
            └──── Parameter(s): single 'Settle' model
            └─────── Output(s): single 'ResponseTemplate' model
            └──────── Error(s): NO_PHARMACIES_MATCHED_THE_UPI, DATABASE_FAILURE
            └───── Description: Sets a settle for the pharmacy which has requested.
            -----------------------------------------------------------------------------------------------------
            GET : get-top-five
            └──── Parameter(s): 'upi' string
            └─────── Output(s): multiple 'Score' models, single 'ResponseTemplate' model
            └──────── Error(s): DATABASE_FAILURE
            └───── Description: Returns the top five pharmacy based on their score given during their activity.
            -----------------------------------------------------------------------------------------------------
            POST : authenticate
            └──── Parameter(s): credential string[] (credential[0]: email, credential[1]: password)
            └─────── Output(s): single 'ResponseTemplate' model
            └──────── Error(s): VERIFICATION_FAILED, DATABASE_FAILURE
            └───── Description: Checks whether the pharmacy client entered the right password or not.
                                Therefore, the application can redirect the client to pharmacy main page.
            -----------------------------------------------------------------------------------------------------
            PUT : change-status
            └──── Parameter(s): 'upi' string
            └─────── Output(s): single 'ResponseTemplate' model
            └──────── Error(s): NO_PHARMACIES_MATCHED_THE_UPI, DATABASE_FAILURE
            └───── Description: Used to change the status of the pharmacy to ON/OFF. Basically, ON means
                                it will receive new orders from customers and OFF means the pharamcy service
                                is unavailabe.
            -----------------------------------------------------------------------------------------------------
            GET : get-weekly-number-of-orders
            └──── Parameter(s): 'upi' string
            └─────── Output(s): multiple 'float' values, single 'ResponseTemplate' model
            └──────── Error(s): NO_PHARMACIES_MATCHED_THE_UPI, DATA_IS_NOT_AVAILABE, DATABASE_FAILURE
            └───── Description: Returns the daily number of orders in the recent week which a pharmacy has.
                                It is an array of 8 floats. The first 7 items are the number of orders of week days.
                                The last item is the average number of oreders of week.
            -----------------------------------------------------------------------------------------------------
            GET : get-weekly-packing-average-time            
            └──── Parameter(s): 'upi' string
            └─────── Output(s): multiple 'float' values, single 'ResponseTemplate' model
            └──────── Error(s): NO_PHARMACIES_MATCHED_THE_UPI, DATA_IS_NOT_AVAILABE, DATABASE_FAILURE
            └───── Description: Returns packing average time during the recent week, which is an array
                                of 8 floats. The first 7 items are the time for the week days.
                                The last item is for total packing average time of week.
            -----------------------------------------------------------------------------------------------------
            GET : get-upi         
            └──── Parameter(s): 'email' string
            └─────── Output(s): single 'string' value
            └──────── Error(s): NO_PHARMACIES_MATCHED_THE_EMAIL, DATABASE_FAILURE
            └───── Description: Resturn UPI that has that input Email address
        */

        private static NoskheContext db = new NoskheContext();

        private IPharmacyService _pharmacyService;
        private readonly AppSettings _appSettings;
        public PharmacyController(IPharmacyService pharmacyService, IOptions<AppSettings> appSettings)
        {
            _pharmacyService = pharmacyService;
            _appSettings = appSettings.Value;
        }
        public PharmacyController(IHubContext<NotificationHub> a)
        {
            HubContext = a;
        }

        // GET: desktop-api/pharmacy/get-database-status
        [HttpGet("Get-Database-Status")]
        public ActionResult GetDatabaseStatus()
        {
            try
            {
                var result = db.Customers.Select(s => s.CustomerId);
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATABASE_ERROR"
                });
            }

            // satisfying result
            return Ok(new ResponseTemplate {
                Success = true
            });
        }
        // GET: desktop-api/pharmacy/get-upi?email={EMAIL}"
        [HttpGet("Get-UPI")]
        public ActionResult<string> GetUPI(string email)
        {
            try
            {
                var response = db.Pharmacies.Where(q => q.Email == email).FirstOrDefault();
                if (response == null)
                {    // un-satisfying result
                    return BadRequest(new ResponseTemplate
                    {
                        Success = false,
                        Error = "NO_PHARMACIES_MATCHED_THE_EMAIL"
                    });
                }

                return response.UPI;
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new ResponseTemplate
                {
                    Success = false,
                    Error = "DATABASE_FAILURE"
                });
            }
        }
        // GET: desktop-api/pharmacy/get-datetime
        [HttpGet("Get-Datetime")]
        public ActionResult<DateTime> GetDatetime()
        {
            // satisfying result
            return DateTime.Now;
        }
        // GET: desktop-api/pharmacy/get-server-status
        [HttpGet("Get-Server-Status")]
        public ActionResult GetServerStatus()
        {
            // satisfying result
            return Ok(new ResponseTemplate {
                Success = true
            }); // Otherwise the connection is not established and 'Success' cannot be reached by the client. ('false' is meaningless)
        }
        // GET: desktop-api/pharmacy/get-details
        [HttpGet("Get-Details")]
        public ActionResult<Models.Minimals.Output.Pharmacy> GetDetails(string upi)
        {
            try
            {
                var search = db.Pharmacies.Where(q => q.UPI == upi).FirstOrDefault();
                if(search == null)
                    // un-satisfying result
                    return BadRequest(new ResponseTemplate {
                        Success = false,
                        Error = "NO_PHARMACIES_MATCHED_THE_UPI"
                    });
                
                return new Models.Minimals.Output.Pharmacy {
                    Name = search.Name,
                    UPI = upi,
                    Email = search.Email,
                    Phone = search.Phone,
                    ProfilePictureUrl = search.ProfilePictureUrl,
                    Address = search.Address,
                    IsAvailableNow = search.IsAvailableNow,
                    Credit = search.Credit,
                    ManagerName = db.Managers.Where(w => w.ManagerId == search.ManagerId).FirstOrDefault().FirstName + " " + db.Managers.Where(w => w.ManagerId == search.ManagerId).FirstOrDefault().LastName
                };
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATABASE_FAILURE"
                });
            }
        }
        
        // GET: desktop-api/pharmacy/get-orders
        [HttpGet("Get-Orders")]
        public ActionResult<IEnumerable<Models.Minimals.Output.Order>> GetOrders(string upi, string start="start-undifiend", string end="end-undifiend")
        {
            try
            {
                var responses = 
                    from record in db.Orders
                    where record.Pharmacy.UPI == upi
                    orderby record.Date
                    select record;
                
                List<Models.Minimals.Output.Order> ListOfOrders = new List<Models.Minimals.Output.Order>();

                if(responses != null)
                {
                    DateTime startTime = new DateTime();
                    DateTime endTime = new DateTime();

                    switch(start)
                    {
                        case "start-undefined":
                            startTime = DateTime.Parse("1990-01-01");
                            break;
                        case "start-of-week":
                            startTime = FirstAndLastWeekDayOfTheDay.ReturnFirstAndLastDate().First();
                            break;
                        default:
                            try
                            {
                                startTime = DateTime.Parse(start);
                                break;
                            }
                            catch
                            {
                                // un-satisfying result
                                return BadRequest(new ResponseTemplate {
                                    Success = false,
                                    Error = "BAD_START_TIME_FORMAT"
                                });
                            }
                        
                    }

                    switch(end)
                    {
                        case "end-undefined":
                            endTime = DateTime.Parse("2100-01-01");
                            break;
                        case "end-of-week":
                            endTime = FirstAndLastWeekDayOfTheDay.ReturnFirstAndLastDate().Last();
                            break;
                        default:
                            try
                            {
                                endTime = DateTime.Parse(end);
                                break;
                            }
                            catch
                            {
                                // un-satisfying result
                                return BadRequest(new ResponseTemplate {
                                    Success = false,
                                    Error = "BAD_END_TIME_FORMAT"
                                });
                            }
                    }

                    if(startTime >= endTime)
                        // un-satisfying result
                        return BadRequest(new ResponseTemplate {
                            Success = false,
                            Error = "START_TIME_IS_GREATER_THAN_END_TIME"
                        });

                    foreach (var response in responses)
                    {
                        if(response.Date < startTime && response.Date > endTime) continue;
                        decimal totalPriceWithoutShippingCost = 0M;
                        Models.Minimals.Output.Order tempOrder = new Models.Minimals.Output.Order();
                        tempOrder.UOI = response.UOI;
                        tempOrder.Date = response.Date;
                        tempOrder.HasPrescription = response.HasPrescription;
                        tempOrder.HasBeenAcceptedByCustomer = response.HasBeenAcceptedByCustomer;
                        tempOrder.HasBeenPaid = response.HasBeenPaid;
                        tempOrder.HasBeenDeliveredToCustomer = response.HasBeenDeliveredToCustomer;
                        tempOrder.HasBeenSettled = response.HasBeenSettled;
                        tempOrder.CourierName = response.Courier.FirstName + " " + response.Courier.LastName;
                        tempOrder.Address = response.ShoppingCart.Address;
                        tempOrder.Email = response.ShoppingCart.Customer.Email;
                        tempOrder.BrandPreference = response.ShoppingCart.Notation.BrandPreference;
                        tempOrder.HasPregnancy = response.ShoppingCart.Notation.HasPregnancy;
                        tempOrder.HasOtherDiseases = response.ShoppingCart.Notation.HasOtherDiseases;
                        tempOrder.Description = response.ShoppingCart.Notation.Description;
                        tempOrder.HasBeenAcceptedByPharmacy = response.ShoppingCart.Prescription.HasBeenAcceptedByPharmacy;
                        tempOrder.PictureUrl_1 = response.ShoppingCart.Prescription.PictureUrl_1;
                        tempOrder.PictureUrl_2 = response.ShoppingCart.Prescription.PictureUrl_2;
                        tempOrder.PictureUrl_3 = response.ShoppingCart.Prescription.PictureUrl_3;

                        var ListOfCosmetics =
                            from record in db.CosmeticShoppingCarts
                            where record.ShoppingCartId == response.ShoppingCartId
                            select record;
                        
                        if(ListOfCosmetics != null)
                        {
                            foreach (var item in ListOfCosmetics)
                            {
                                totalPriceWithoutShippingCost += (item.Cosmetic.Price * item.Quantity);
                                tempOrder.Cosmetics.Add(item.Cosmetic.Name, new string[] { item.Quantity.ToString(), item.Cosmetic.Price.ToString() });
                            }
                        }

                        var ListOfMedicines =
                            from record in db.MedicineShoppingCarts
                            where record.ShoppingCartId == response.ShoppingCartId
                            select record;
                        
                        if(ListOfMedicines != null)
                        {
                            foreach (var item in ListOfMedicines)
                            {
                                totalPriceWithoutShippingCost += (item.Medicine.Price * item.Quantity);
                                tempOrder.Medicines.Add(item.Medicine.Name, new string[] { item.Quantity.ToString(), item.Medicine.Price.ToString() });
                            }
                        }

                        var ListOfPrescriptionItems =
                            from record in db.PrescriptionItems
                            where record.Prescription.ShoppingCartId == response.ShoppingCartId
                            select record;
                        
                        if(ListOfPrescriptionItems != null)
                        {
                            foreach (var item in ListOfPrescriptionItems)
                            {
                                totalPriceWithoutShippingCost += (item.Price * item.Quantity);
                                tempOrder.PrescriptionItems.Add(item.Name, new string[] { item.Quantity.ToString(), item.Price.ToString() });
                            }
                        }

                        tempOrder.TotalPriceWithoutShippingCost = totalPriceWithoutShippingCost;
                        tempOrder.TotalPrice = response.Price;
                        
                        ListOfOrders.Add(tempOrder);
                    }
                }

                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "NO_RESPONSES_MATCHED_THE_UPI"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATABASE_FAILURE"
                });
            }
        }

        // GET: desktop-api/pharmacy/get-score
        [HttpGet("Get-Score")]
        public ActionResult<Models.Minimals.Output.Score> GetScore(string upi)
        {
            try
            {
                var search = db.Scores.Where(q => q.Pharmacy.UPI == upi).FirstOrDefault();
                if(search == null)
                    // un-satisfying result
                    return BadRequest(new ResponseTemplate {
                        Success = false,
                        Error = "NO_SCORES_MATCHED_THE_UPI"
                    });
                
                return new Models.Minimals.Output.Score {
                    UPI = search.Pharmacy.UPI,
                    CustomerSatisfaction = search.CustomerSatisfaction,
                    RankAmongPharmacies = search.RankAmongPharmacies,
                    PackingAverageTimeInSeconds = search.PackingAverageTimeInSeconds
                };
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATABASE_FAILURE"
                });
            }
        }

        // GET: desktop-api/pharmacy/get-settles
        [HttpGet("Get-Settles")]
        public ActionResult<IEnumerable<Models.Minimals.Output.Settle>> GetSettles(string upi, string start="start-undifiend", string end="end-undifiend")
        {
            try
            {
                var responses =
                    from record in db.Settles
                    where record.Pharmacy.UPI == upi
                    orderby record.Date
                    select new Models.Minimals.Output.Settle { USI = record.USI, CommissionCoefficient = record.CommissionCoefficient, NumberOfOrders = record.NumberOfOrders, Date = record.Date, HasBeenSettled = record.HasBeenSettled, Credit = record.Credit };
                if(responses != null) return responses.ToArray();

                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "NO_SETTLES_MATCHED_THE_UPI"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATABASE_FAILURE"
                });
            }
        }

        // POST: desktop-api/pharmacy/set-settle
        [HttpPost("Set-Settle")]
        public ActionResult SetSettle([FromBody] Models.Minimals.Input.Settle settle)
        {
            try
            {
                var response = db.Pharmacies.Where(q => q.UPI == settle.UPI).FirstOrDefault();
                if(response != null)
                {
                    db.Settles.Add(
                        new Settle {
                            USI = "123234", // TODO: USI generator
                            CommissionCoefficient = 23.3, // TODO: CommissionCoefficient calculator
                            NumberOfOrders = settle.NumberOfOrders,
                            Date = DateTime.Now,
                            Pharmacy = response,
                            HasBeenSettled = false,
                            Credit = settle.Credit
                        }
                    );
                    db.SaveChanges();

                    // satisfying result
                    return Ok(new ResponseTemplate {
                        Success = true
                    });
                }

                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "NO_PHARMACIES_MATCHED_THE_UPI"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATABASE_FAILURE"
                });
            }
        }
        
        // GET: desktop-api/pharmacy/get-top-five
        [HttpGet("Get-Top-Five")]
        public ActionResult<IEnumerable<Models.Minimals.Output.Score>> GetTopFive(string upi)
        {
            try
            {
                var TopFive =
                    from score in db.Scores
                    orderby score.RankAmongPharmacies descending
                    select new Models.Minimals.Output.Score {
                        CustomerSatisfaction = score.CustomerSatisfaction,
                        PharmacyName = score.Pharmacy.Name,
                        RankAmongPharmacies = score.RankAmongPharmacies,
                        PackingAverageTimeInSeconds = score.PackingAverageTimeInSeconds
                    };
                return TopFive.ToArray();
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATABASE_FAILURE"
                });
            }
        }

        // POST: desktop-api/pharmacy/authenticate
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public ActionResult Authenticate([FromBody] string[] credential)
        {
            try
            {
                var response = db.Pharmacies.Where(q => (q.Email == credential[0] && q.Password == credential[1])).FirstOrDefault();
                if(response != null)
                {
                    // satisfying result
                    return Ok(new ResponseTemplate {
                        Success = true
                    });
                }

                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "VERIFICATION_FAILED"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATABASE_FAILURE"
                });
            }
        }

        // PUT: desktop-api/pharmacy/change-status
        [HttpPut("Change-Status")]
        public ActionResult ChangeStatus([FromBody] string upi)
        {
            try
            {
                var response = db.Pharmacies.Where(q => q.UPI == upi).FirstOrDefault();
                if(response != null)
                {
                    if(response.IsAvailableNow == true)
                    {
                        response.IsAvailableNow = false;
                        db.SaveChanges();

                        // satisfying result
                        return Ok(new ResponseTemplate {
                            Success = true
                        });
                    }
                    else
                    {
                        response.IsAvailableNow = true;
                        db.SaveChanges();

                        // satisfying result
                        return Ok(new ResponseTemplate {
                            Success = true
                        });
                    }
                }

                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "NO_PHARMACIES_MATCHED_THE_UPI"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATABASE_FAILURE"
                });
            }
        }

        // GET: desktop-api/pharmacy/get-weekly-number-of-orders
        [HttpGet("Get-Weekly-Number-Of-Orders")]
        public ActionResult<IEnumerable<float>> GetWeeklyNumberOfOrders(string upi)
        {
            float[] weekFloats = new float[8];
            try
            {
                var responses =
                    from record in db.Orders
                    where record.Pharmacy.UPI == upi
                    orderby record.Date
                    where ( (record.Date >= FirstAndLastWeekDayOfTheDay.ReturnFirstAndLastDate().First())
                            && (record.Date <= FirstAndLastWeekDayOfTheDay.ReturnFirstAndLastDate().Last()) )
                    select record.Date;
                if(responses != null)
                {
                    var s0 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Saturday
                        select record;
                    var s1 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Sunday
                        select record;
                    var s2 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Monday
                        select record;
                    var s3 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Tuesday
                        select record;
                    var s4 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Wednesday
                        select record;
                    var s5 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Thursday
                        select record;
                    var s6 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Friday
                        select record;

                    if(s0 != null) weekFloats[0] = s0.Count();
                    else weekFloats[0] = 0;
                    if(s1 != null) weekFloats[1] = s1.Count();
                    else weekFloats[1] = 0;
                    if(s2 != null) weekFloats[2] = s2.Count();
                    else weekFloats[2] = 0;
                    if(s3 != null) weekFloats[3] = s3.Count();
                    else weekFloats[3] = 0;
                    if(s4 != null) weekFloats[4] = s4.Count();
                    else weekFloats[4] = 0;
                    if(s5 != null) weekFloats[5] = s5.Count();
                    else weekFloats[5] = 0;
                    if(s6 != null) weekFloats[6] = s6.Count();
                    else weekFloats[6] = 0;

                    weekFloats[7] = responses.Count();

                    // satsfied
                    return weekFloats;
                }

                if(db.Pharmacies.Where(q => q.UPI == upi).FirstOrDefault() == null)
                {
                    // un-satisfying result
                    return BadRequest(new ResponseTemplate {
                        Success = false,
                        Error = "NO_PHARMACIES_MATCHED_THE_UPI"
                    });
                }

                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATA_IS_NOT_AVAILABE"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATABASE_FAILURE"
                });
            }
        }

        // GET: desktop-api/pharmacy/get-weekly-packing-average-time
        [HttpGet("Get-Weekly-Packing-Average-Time")]
        public ActionResult<IEnumerable<float>> GetWeeklyPackingAverageTime(string upi)
        {
            float[] weekFloats = new float[8];
            try
            {
                var responses =
                    from record in db.Orders
                    where record.Pharmacy.UPI == upi
                    orderby record.Date
                    where ( (record.Date >= FirstAndLastWeekDayOfTheDay.ReturnFirstAndLastDate().First())
                            && (record.Date <= FirstAndLastWeekDayOfTheDay.ReturnFirstAndLastDate().Last()) )
                    select new { record.Date, record.PackingTime };
                if(responses != null)
                {
                    var s0 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Saturday
                        select record.PackingTime;
                    var s1 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Sunday
                        select record.PackingTime;
                    var s2 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Monday
                        select record.PackingTime;
                    var s3 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Tuesday
                        select record.PackingTime;
                    var s4 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Wednesday
                        select record.PackingTime;
                    var s5 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Thursday
                        select record.PackingTime;
                    var s6 =
                        from record in responses
                        where record.Date.DayOfWeek == DayOfWeek.Friday
                        select record.PackingTime;

                    if(s0 != null)
                    {
                        int sc = s0.Count();
                        float si = 0;
                        float sav = 0;
                        foreach(var time in s0)
                        {
                            si = (float) time / sc;
                            sav += si;
                            si = 0;
                        }
                        weekFloats[0] = sav;
                    }
                    else weekFloats[0] = 0;

                    if(s1 != null)
                    {
                        int sc = s1.Count();
                        float si = 0;
                        float sav = 0;
                        foreach(var time in s1)
                        {
                            si = (float) time / sc;
                            sav += si;
                            si = 0;
                        }
                        weekFloats[1] = sav;
                    }
                    else weekFloats[1] = 0;

                    if(s2 != null)
                    {
                        int sc = s2.Count();
                        float si = 0;
                        float sav = 0;
                        foreach(var time in s2)
                        {
                            si = (float) time / sc;
                            sav += si;
                            si = 0;
                        }
                        weekFloats[2] = sav;
                    }
                    else weekFloats[2] = 0;

                    if(s3 != null)
                    {
                        int sc = s3.Count();
                        float si = 0;
                        float sav = 0;
                        foreach(var time in s3)
                        {
                            si = (float) time / sc;
                            sav += si;
                            si = 0;
                        }
                        weekFloats[3] = sav;
                    }
                    else weekFloats[3] = 0;

                    if(s4 != null)
                    {
                        int sc = s4.Count();
                        float si = 0;
                        float sav = 0;
                        foreach(var time in s4)
                        {
                            si = (float) time / sc;
                            sav += si;
                            si = 0;
                        }
                        weekFloats[4] = sav;
                    }
                    else weekFloats[4] = 0;

                    if(s5 != null)
                    {
                        int sc = s5.Count();
                        float si = 0;
                        float sav = 0;
                        foreach(var time in s5)
                        {
                            si = (float) time / sc;
                            sav += si;
                            si = 0;
                        }
                        weekFloats[5] = sav;
                    }
                    else weekFloats[5] = 0;

                    if(s6 != null)
                    {
                        int sc = s6.Count();
                        float si = 0;
                        float sav = 0;
                        foreach(var time in s6)
                        {
                            si = (float) time / sc;
                            sav += si;
                            si = 0;
                        }
                        weekFloats[6] = sav;
                    }
                    else weekFloats[6] = 0;

                    int count = responses.Count();
                    float i = 0;
                    float average = 0;
                    foreach (var response in responses)
                    {
                        i = (float) response.PackingTime / count;
                        average += i;
                        i = 0;
                    }
                    weekFloats[7] = average;

                    // return to client
                    return weekFloats;
                }
                if(db.Pharmacies.Where(q => q.UPI == upi).FirstOrDefault() == null)
                {
                    // un-satisfying result
                    return BadRequest(new ResponseTemplate {
                        Success = false,
                        Error = "NO_PHARMACIES_MATCHED_THE_UPI"
                    });
                }

                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATA_IS_NOT_AVAILABE"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = "DATABASE_FAILURE"
                });
            }
        }

        private IHubContext<NotificationHub> HubContext { get; set; }

        [HttpGet("signalr")]
        public async Task<ActionResult> signalr(string identifier, string Error)
        {
            try
            {
                await this.HubContext.Clients.Group("amir").SendAsync("HandleNotification", "Hi Pashmam");
            }
            catch
            {
                return BadRequest(new ResponseTemplate { Success = false, Error = "UNABLE_TO_SEND_NOTIFICATION" });
            }

            return Ok(new ResponseTemplate { Success = true });
        }
    }
}