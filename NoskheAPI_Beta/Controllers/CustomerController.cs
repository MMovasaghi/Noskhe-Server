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

namespace NoskheAPI_Beta.Controllers
{
    [Route("android-api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        /*
            GET: get-details
            └──── Parameter(s): 'email' string
            └─────── Output(s): single 'Customer' model, single 'Descriptive' model
            └──────── Error(s): NO_CUSTOMERS_MATCHED_THE_EMAIL, DATABASE_FAILURE
            └───── Description: Returns customer information which can be used in profile.
            -----------------------------------------------------------------------------------------------------
            GET: get-shopping-carts
            └──── Parameter(s): 'email' string
            └─────── Output(s): multiple 'ShoppingCart' models, single 'Descriptive' model
            └──────── Error(s): NO_SHOPPING_CARTS_MATCHED_THE_EMAIL, DATABASE_FAILURE
            └───── Description: Returns customer shopping carts which can be used to show in application.
            -----------------------------------------------------------------------------------------------------
            GET: get-orders
            └──── Parameter(s): 'email' string
            └─────── Output(s): multiple 'Order' models, single 'Descriptive' model
            └──────── Error(s): NO_ORDERS_MATCHED_THE_EMAIL, DATABASE_FAILURE
            └───── Description: Returns customer orders which can be used to show in application.
            -----------------------------------------------------------------------------------------------------
            GET: get-cosmetics
            └──── Parameter(s): none
            └─────── Output(s): multiple 'Cosmetic' models, single 'Descriptive' model
            └──────── Error(s): NO_COSMETICS_AVAILABE, DATABASE_FAILURE
            └───── Description: Returns all cosmetics stored in database which can be used to setup availabe cosmetic list to buy.
            -----------------------------------------------------------------------------------------------------
            GET: get-cosmetics
            └───── Parameter(s): 'usci' string
            └─────── Output(s): multiple 'Cosmetic' models, single 'Descriptive' model
            └──────── Error(s): NO_COSMETICS_MATCHED_THE_USCI, DATABASE_FAILURE
            └───── Description: Returns a list of cosmetics of a shopping cart which is owned by customer.
            -----------------------------------------------------------------------------------------------------
            GET: get-list-of-cosmetic-ids
            └──── Parameter(s): 'usci' string
            └─────── Output(s): multiple 'int' values, single 'Descriptive' model
            └──────── Error(s): NO_COSMETICS_MATCHED_THE_USCI, DATABASE_FAILURE
            └───── Description: Returns a list of cosmetic IDs in which customer has added to his/her own shopping cart.
            -----------------------------------------------------------------------------------------------------
            GET: get-medicines
            └──── Parameter(s): none
            └─────── Output(s): multiple 'Modicine' models, single 'Descriptive' model
            └──────── Error(s): NO_MEDICINES_AVAILABE, DATABASE_FAILURE
            └───── Description: Returns all medicines stored in database which can be used to setup availabe medicine list to buy.
            -----------------------------------------------------------------------------------------------------
            GET: get-medicines
            └──── Parameter(s): 'usci' string
            └─────── Output(s): multiple 'Modicine' models, single 'Descriptive' model
            └──────── Error(s): NO_MEDICINES_MATCHED_THE_USCI, DATABASE_FAILURE
            └───── Description: Returns a list of medicines of a shopping cart which is owned by customer.
            -----------------------------------------------------------------------------------------------------
            GET: get-list-of-medicine-ids
            └──── Parameter(s): 'usci' string
            └─────── Output(s): multiple 'int' values, single 'Descriptive' model
            └──────── Error(s): NO_MEDICINES_MATCHED_THE_USCI, DATABASE_FAILURE
            └───── Description: Returns a list of medicine IDs in which customer has added to his/her own shopping cart.
            -----------------------------------------------------------------------------------------------------
            POST: authenticate
            └──── Parameter(s): 'credential' string[] (credential[0]: email, credential[1]: password)
            └─────── Output(s): single 'Descriptive' model
            └──────── Error(s): VERIFICATION_FAILED, DATABASE_FAILURE
            └───── Description: Checks whether the customer entered right login credentials or not in order to take the customer to his/her own page.
            -----------------------------------------------------------------------------------------------------
            POST: authenticate
            └──── Parameter(s): 'phone' string
            └─────── Output(s): single 'Descriptive' model
            └──────── Error(s): NO_CUSTOMERS_MATCHED_THE_PHONE, DATABASE_FAILURE
            └───── Description: Checks whether the customer entered right phone number or not in order to send SMS to his/her mobile. Also, this generates a verification in the database to check later.
            -----------------------------------------------------------------------------------------------------
            POST: send-sms-authentication-code
            └──── Parameter(s): 'phone' string
            └─────── Output(s): single 'Descriptive' model
            └──────── Error(s): , DATABASE_FAILURE
            └───── Description: Sends SMS to the customer phone number.
            -----------------------------------------------------------------------------------------------------
            POST: verify-sms-authentication-code
            └──── Parameter(s): 'pair' string[] (pair[0]: phone, pair[1]: verification code)
            └─────── Output(s): single 'Descriptive' model
            └──────── Error(s): VERIFICATION_EXPIRED, VERIFICATION_FAILED, DATABASE_FAILURE
            └───── Description: Checks whether customer entered the right verification code or not.
            -----------------------------------------------------------------------------------------------------
            POST: add-new
            └──── Parameter(s): single 'Customer' model
            └─────── Output(s): single 'Descriptive' model
            └──────── Error(s): DUPLICATE_CUSTOMER, DATABASE_FAILURE
            └───── Description: Adds a new customer to the application.
            -----------------------------------------------------------------------------------------------------
            PUT: edit-existing
            └──── Parameter(s): single 'Customer' model
            └─────── Output(s): single 'Descriptive' model
            └──────── Error(s): NO_CUSTOMERS_MATCHED_THE_EMAIL, DATABASE_FAILURE
            └───── Description: Edits detais of an existing customer.
            -----------------------------------------------------------------------------------------------------
            POST: add-new-shopping-cart
            └──── Parameter(s): single 'ShoppingCart' model
            └─────── Output(s): single 'Descriptive' model
            └──────── Error(s): NO_CUSTOMERS_MATCHED_THE_EMAIL, DATABASE_FAILURE
            └───── Description: Adds a new shopping cart for a customer.
        */
        private static NoskheContext db = new NoskheContext();
        
        // GET: android-api/customer/get-details
        [HttpGet("Get-Details")]
        public ActionResult<Models.Minimals.Output.Customer> GetDetails(string email)
        {
            try
            {
                var response = db.Customers.Where(q => q.Email == email).FirstOrDefault();
                if(response != null) return new Models.Minimals.Output.Customer {
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    Gender = response.Gender,
                    Birthday = response.Birthday,
                    Email = response.Email,
                    Phone = response.Phone,
                    ProfilePictureUrl = response.ProfilePictureUrl
                };

                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "NO_CUSTOMERS_MATCHED_THE_EMAIL"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }

        // GET: android-api/customer/get-shopping-carts
        [HttpGet("Get-Shopping-Carts")]
        public ActionResult<IEnumerable<Models.Minimals.Output.ShoppingCart>> GetShoppingCarts(string email)
        {
            try
            {
                var foundCustomer = db.Customers.Where(c => c.Email == email).FirstOrDefault();
                if(foundCustomer == null)
                {
                    // un-satisfying result
                    return BadRequest(new Descriptive {
                        Success = false,
                        Message = "NO_SHOPPING_CARTS_MATCHED_THE_EMAIL"
                    });
                }

                db.Entry(foundCustomer).Collection(c => c.ShoppingCarts).Query()
                    .Include(s => s.MedicineShoppingCarts)
                        .ThenInclude(m => m.Medicine)
                    .Include(s => s.CosmeticShoppingCarts)
                        .ThenInclude(m => m.Cosmetic)
                    .Include(s => s.Prescription)
                    .Include(s => s.Notation)
                    .Load();

                List<Models.Minimals.Output.ShoppingCart> outputs = new List<Models.Minimals.Output.ShoppingCart>();

                foreach (var selectedShoppingCart in foundCustomer.ShoppingCarts)
                {
                    decimal totalPriceWithoutPrescription = 0M;
                    Models.Minimals.Output.ShoppingCart output = new Models.Minimals.Output.ShoppingCart();
                    output.ShoppingCartId = selectedShoppingCart.ShoppingCartId;
                    output.USCI = selectedShoppingCart.USCI;
                    output.Date = selectedShoppingCart.Date;
                    output.Address = selectedShoppingCart.Address;
                    output.Email = email;
                    output.Cosmetics = new Dictionary<string, string[]>();
                    output.Medicines = new Dictionary<string, string[]>();
                    output.HasBeenRequested = selectedShoppingCart.HasBeenRequested;
                    
                    if(selectedShoppingCart.CosmeticShoppingCarts != null)
                    {
                        foreach (var item in selectedShoppingCart.CosmeticShoppingCarts)
                        {
                            totalPriceWithoutPrescription += (item.Cosmetic.Price * item.Quantity);
                            output.Cosmetics.Add(item.Cosmetic.Name, new string[] { item.Quantity.ToString(), item.Cosmetic.Price.ToString() });
                       }
                    }

                    
                    if(selectedShoppingCart.MedicineShoppingCarts != null)
                    {
                        foreach (var item in selectedShoppingCart.MedicineShoppingCarts)
                        {
                            totalPriceWithoutPrescription += (item.Medicine.Price * item.Quantity);
                            output.Medicines.Add(item.Medicine.Name, new string[] { item.Quantity.ToString(), item.Medicine.Price.ToString() });
                        }
                    }
                    
                    if(selectedShoppingCart.Notation != null)
                    {
                        output.BrandPreference = selectedShoppingCart.Notation.BrandPreference;
                        output.HasPregnancy = selectedShoppingCart.Notation.HasPregnancy;
                        output.HasOtherDiseases = selectedShoppingCart.Notation.HasOtherDiseases;
                        output.Description = selectedShoppingCart.Notation.Description;
                    }
                    
                    if(selectedShoppingCart.Prescription != null)
                    {
                        output.PictureUrl_1 = selectedShoppingCart.Prescription.PictureUrl_1;
                        output.PictureUrl_2 = selectedShoppingCart.Prescription.PictureUrl_2;
                        output.PictureUrl_3 = selectedShoppingCart.Prescription.PictureUrl_3;
                    }
                    
                    output.TotalPriceWithoutPrescription = totalPriceWithoutPrescription;
                    
                    outputs.Add(output);
                }
                return outputs.ToArray();
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }
        
        // GET: android-api/customer/get-orders
        [HttpGet("Get-Orders")]
        public ActionResult<IEnumerable<Models.Minimals.Output.Order>> GetOrders(string email)
        {
            try
            {
                var foundCustomer = db.Customers.Where(c => c.Email == email).FirstOrDefault();
                if(foundCustomer == null)
                {
                    // un-satisfying result
                    return BadRequest(new Descriptive {
                        Success = false,
                        Message = "NO_ORDERS_MATCHED_THE_EMAIL"
                    });
                }

                db.Entry(foundCustomer).Collection(c => c.ShoppingCarts).Query()
                    .Include(s => s.Notation)
                    .Include(s => s.MedicineShoppingCarts)
                        .ThenInclude(m => m.Medicine)
                    .Include(s => s.CosmeticShoppingCarts)
                        .ThenInclude(m => m.Cosmetic)
                    .Include(s => s.Prescription)
                        .ThenInclude(s => s.PrescriptionItems)
                    .Include(s => s.Order)
                        .ThenInclude(s => s.Courier)
                    .Include(s => s.Order)
                        .ThenInclude(s => s.Pharmacy)
                    .Include(s => s.Order)
                        .ThenInclude(s => s.Occurrence)
                    .Include(s => s.Order)
                        .ThenInclude(s => s.Courier)
                    .Load();

                List<Models.Minimals.Output.Order> outputs = new List<Models.Minimals.Output.Order>();

                foreach (var selectedShoppingCart in foundCustomer.ShoppingCarts)
                {
                    if(selectedShoppingCart.Order == null) continue;
                    decimal totalPriceWithoutShippingCost = 0M;
                    Models.Minimals.Output.Order output = new Models.Minimals.Output.Order();
                    output.UOI = selectedShoppingCart.Order.UOI;
                    output.Date = selectedShoppingCart.Order.Date;
                    output.HasPrescription = selectedShoppingCart.Order.HasPrescription;
                    output.HasBeenAcceptedByCustomer = selectedShoppingCart.Order.HasBeenAcceptedByCustomer;
                    output.HasBeenPaid = selectedShoppingCart.Order.HasBeenPaid;
                    output.HasBeenDeliveredToCustomer = selectedShoppingCart.Order.HasBeenDeliveredToCustomer;
                    output.HasBeenSettled = selectedShoppingCart.Order.HasBeenSettled;
                    output.CourierName = selectedShoppingCart.Order.Courier.FirstName + " " + selectedShoppingCart.Order.Courier.LastName;
                    output.PharmacyName = selectedShoppingCart.Order.Pharmacy.Name;
                    output.PharmacyAddress = selectedShoppingCart.Order.Pharmacy.Address;
                    output.Address = selectedShoppingCart.Address;
                    output.Email = foundCustomer.Email; // or 'email', the given paramether
                    output.Cosmetics = new Dictionary<string, string[]>();
                    output.Medicines = new Dictionary<string, string[]>();
                    output.PrescriptionItems = new Dictionary<string, string[]>();

                    if(selectedShoppingCart.Notation != null)
                    {
                        output.BrandPreference = selectedShoppingCart.Notation.BrandPreference;
                        output.HasPregnancy = selectedShoppingCart.Notation.HasPregnancy;
                        output.HasOtherDiseases = selectedShoppingCart.Notation.HasOtherDiseases;
                    }
                    if(selectedShoppingCart.Prescription != null)
                    {
                        output.HasBeenAcceptedByPharmacy = selectedShoppingCart.Prescription.HasBeenAcceptedByPharmacy;
                        output.PictureUrl_1 = selectedShoppingCart.Prescription.PictureUrl_1;
                        output.PictureUrl_2 = selectedShoppingCart.Prescription.PictureUrl_2;
                        output.PictureUrl_3 = selectedShoppingCart.Prescription.PictureUrl_3;
                    }
                    
                    if(selectedShoppingCart.CosmeticShoppingCarts != null)
                    {
                        foreach (var item in selectedShoppingCart.CosmeticShoppingCarts)
                        {
                            totalPriceWithoutShippingCost += (item.Cosmetic.Price * item.Quantity);
                            output.Cosmetics.Add(item.Cosmetic.Name, new string[] { item.Quantity.ToString(), item.Cosmetic.Price.ToString() }); // !ERROR!
                        }
                    }
                    
                    if(selectedShoppingCart.MedicineShoppingCarts != null)
                    {
                        foreach (var item in selectedShoppingCart.MedicineShoppingCarts)
                        {
                            totalPriceWithoutShippingCost += (item.Medicine.Price * item.Quantity);
                            output.Medicines.Add(item.Medicine.Name, new string[] { item.Quantity.ToString(), item.Medicine.Price.ToString() }); // !ERROR!
                        }
                    }
                    
                    if(selectedShoppingCart.Prescription != null)
                    {
                        if(selectedShoppingCart.Prescription.PrescriptionItems != null)
                        {
                            foreach (var item in selectedShoppingCart.Prescription.PrescriptionItems)
                            {
                                totalPriceWithoutShippingCost += (item.Price * item.Quantity);
                                output.PrescriptionItems.Add(item.Name, new string[] { item.Quantity.ToString(), item.Price.ToString() });
                            }
                        }
                    }

                    output.TotalPriceWithoutShippingCost = totalPriceWithoutShippingCost;
                    output.TotalPrice = selectedShoppingCart.Order.Price;
                    
                    outputs.Add(output);
                    // }
                }

                    
                return outputs.ToArray();
            }
            catch(Exception)
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }
        
        // GET: android-api/customer/get-cosmetics
        [HttpGet("Get-Cosmetics")]
        public ActionResult<IEnumerable<Models.Minimals.Output.Cosmetic>> GetCosmetics()
        {
            try
            {
                var responses =
                    from record in db.Cosmetics
                    select new Models.Minimals.Output.Cosmetic { Name = record.Name, Price = record.Price, ProductPictureUrl = record.ProductPictureUrl };
                if(responses != null) return responses.ToArray();
                
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "NO_COSMETICS_AVAILABE"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }

        // GET: android-api/customer/get-cosmetics-by-usci
        [HttpGet("Get-Cosmetics-By-USCI")]
        public ActionResult<IEnumerable<Models.Minimals.Output.Cosmetic>> GetCosmetics(string usci)
        {
            try
            {
                var responses =
                    from record in db.CosmeticShoppingCarts
                    // TODO: #LOADING
                    where record.ShoppingCart.USCI == usci
                    select new Models.Minimals.Output.Cosmetic { Name = record.Cosmetic.Name, Price = record.Cosmetic.Price, ProductPictureUrl = record.Cosmetic.ProductPictureUrl };
                if(responses != null) return responses.ToArray();

                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "NO_COSMETICS_MATCHED_THE_USCI"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }

        // GET: android-api/customer/get-list-of-cosmetic-ids
        [HttpGet("Get-List-Of-Cosmetic-IDs")]
        public ActionResult<IEnumerable<int>> GetListOfCosmeticIDs(string usci)
        {
            try
            {
                var responses =
                    from record in db.CosmeticShoppingCarts
                    // TODO: #LOADING
                    where record.ShoppingCart.USCI == usci
                    select record.CosmeticId;

                if(responses != null) return responses.ToArray();

                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "NO_COSMETICS_MATCHED_THE_USCI"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }
        
        // GET: android-api/customer/get-medicines
        [HttpGet("Get-Medicines")]
        public ActionResult<IEnumerable<Models.Minimals.Output.Modicine>> GetMedicines()
        {
            try
            {
                var responses =
                    from record in db.Medicines
                    select new Models.Minimals.Output.Modicine { Name = record.Name, Price = record.Price, ProductPictureUrl = record.ProductPictureUrl };
                if(responses != null) return responses.ToArray();

                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "NO_MEDICINES_AVAILABE"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }

        // GET: android-api/customer/get-medicines-by-usci
        [HttpGet("Get-Medicines-By-USCI")]
        public ActionResult<IEnumerable<Models.Minimals.Output.Modicine>> GetMedicines(string usci)
        {
            try
            {
                var responses =
                    from record in db.MedicineShoppingCarts
                    // TODO: #LOADING
                    where record.ShoppingCart.USCI == usci
                    select new Models.Minimals.Output.Modicine { Name = record.Medicine.Name, Price = record.Medicine.Price, ProductPictureUrl = record.Medicine.ProductPictureUrl };
                if(responses != null) return responses.ToArray();

                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "NO_MEDICINES_MATCHED_THE_USCI"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }

        // GET: android-api/customer/get-list-of-medicine-ids
        [HttpGet("Get-List-Of-Medicine-IDs")]
        public ActionResult<IEnumerable<int>> GetListOfMedicineIDs(string usci)
        {
            try
            {
                var responses =
                    from record in db.MedicineShoppingCarts
                    // TODO: #LOADING
                    where record.ShoppingCart.USCI == usci
                    select record.MedicineId;

                if(responses != null) return responses.ToArray();

                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "NO_MEDICINES_MATCHED_THE_USCI"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }

        // POST: android-api/customer/authenticate
        [HttpPost("Authenticate")] //string[] credential
        public ActionResult Authenticate([FromBody] Models.Android.AuthenticateTemplate at)
        {
            try
            {
                var response = db.Customers.Where(q => (q.Email == at.Email && q.Password == at.Password)).FirstOrDefault();
                if(response != null)
                {
                    // satisfying result
                    return Ok(new Descriptive {
                        Success = true
                    });
                }

                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "VERIFICATION_FAILED"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }

        // POST: android-api/customer/authenticate-by-phone
        [HttpPost("Authenticate-By-Phone")]
        public ActionResult Authenticate([FromBody] Models.Android.AuthenticateByPhoneTemplate abp)
        {
            try
            {
                var response = db.Customers.Where(q => q.Phone == abp.Phone).FirstOrDefault();
                if(response != null)
                {
                    db.TextMessages.Add(
                        new TextMessage {
                            Date = DateTime.Now,
                            VerificationCode = "221312", // TODO: Rand generator
                            Customer = response,
                            HasBeenExpired = false
                        }
                    );
                    db.SaveChanges();

                    // satisfying result
                    return Ok(new Descriptive {
                        Success = true
                    });
                }

                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "NO_CUSTOMERS_MATCHED_THE_PHONE"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }

        // POST: android-api/customer/send-sms-authentication-code
        [HttpPost("Send-SMS-Authentication-Code")]
        public ActionResult SendSMS([FromBody] Models.Android.SendSmsAuthenticationCodeTemplate ssac)
        {
            throw new NotImplementedException();
            // TODO: HasBeenExpired -> true
            // TODO: Sms Api
        }

        // POST: android-api/customer/verify-sms-authentication-code
        [HttpPost("Verify-SMS-Authentication-Code")]
        public ActionResult VerifySMSAuthenticationCode([FromBody] Models.Android.VerifySmsAuthenticationCodeTemplate vsac)
        {
            try
            {
                var response = db.TextMessages.Where(q => (q.Customer.Phone == vsac.Phone && q.VerificationCode == vsac.VerificationCode)).FirstOrDefault();
                if(response != null)
                {
                    if(response.HasBeenExpired == false)
                    {
                        response.HasBeenExpired = true; // vaghti ke dorost bude YA vaghte masalan 2 daghighe tamum shode bashe
                        db.SaveChanges();

                        // satisfying result
                        return Ok(new Descriptive {
                            Success = true
                        });
                    }

                    // un-satisfying result
                    return BadRequest(new Descriptive {
                        Success = false,
                        Message = "VERIFICATION_EXPIRED"
                    });
                }

                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "VERIFICATION_FAILED"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }

        // POST: android-api/customer/add-new
        [HttpPost("Add-New")]
        public ActionResult AddNew([FromBody] Models.Android.AddNewTemplate an)
        {
            try
            {
                var foundCustomer = db.Customers.Where(q => (q.Email == an.CustomerObj.Email && q.Phone == an.CustomerObj.Phone)).FirstOrDefault();
                if(foundCustomer == null)
                {
                    db.Customers.Add(
                        new Customer {
                            FirstName = an.CustomerObj.FirstName,
                            LastName = an.CustomerObj.LastName,
                            Gender = an.CustomerObj.Gender,
                            Birthday = an.CustomerObj.Birthday,
                            Email = an.CustomerObj.Email,
                            Password = SHA256_Algorithm.Compute(an.CustomerObj.Password),
                            Phone = an.CustomerObj.Phone,
                            ProfilePictureUrl = an.CustomerObj.ProfilePictureUrl,
                            // server-side decisions
                            RegisterationDate = DateTime.Now
                        }
                    );
                    db.SaveChanges();

                    // satisfying result
                    return Ok(new Descriptive {
                        Success = true
                    });
                }

                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DUPLICATE_CUSTOMER"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }

        // PUT: android-api/customer/edit-existing
        [HttpPut("Edit-Existing")]
        public ActionResult EditExisting([FromBody] Models.Android.EditExistingTemplate ee)
        {
            try
            {
                var foundCustomer = db.Customers.Where(q => (q.Email == ee.CustomerObj.Email && q.Phone == ee.CustomerObj.Phone)).FirstOrDefault();
                if(foundCustomer != null)
                {
                    foundCustomer.FirstName = ee.CustomerObj.FirstName;
                    foundCustomer.LastName = ee.CustomerObj.LastName;
                    foundCustomer.Gender = ee.CustomerObj.Gender;
                    foundCustomer.Birthday = ee.CustomerObj.Birthday;
                    foundCustomer.Email = ee.CustomerObj.Email;
                    foundCustomer.Password = SHA256_Algorithm.Compute(ee.CustomerObj.Password);
                    foundCustomer.Phone = ee.CustomerObj.Phone;
                    if(ee.CustomerObj.ProfilePictureUrl != foundCustomer.ProfilePictureUrl)
                    {
                        foundCustomer.ProfilePictureUrl = ee.CustomerObj.ProfilePictureUrl;
                        foundCustomer.ProfilePictureUploadDate = DateTime.Now;
                    }
                    db.SaveChanges();

                    // satisfying result
                    return Ok(new Descriptive {
                        Success = true
                    });
                }

                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "NO_CUSTOMERS_MATCHED_THE_EMAIL"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }
        
        // POST: android-api/customer/add-new-shopping-cart
        [HttpPost("Add-New-Shopping-Cart")]
        public ActionResult AddNewShoppingCart([FromBody] Models.Android.AddNewShoppingCartTemplate ansc)
        {
            try
            {
                var foundCustomer = db.Customers.Where(q => q.Email == ansc.ShoppingCartObj.Email).FirstOrDefault();
                if(foundCustomer == null)
                    return  BadRequest(new Descriptive {
                        Success = false,
                        Message = "NO_CUSTOMERS_MATCHED_THE_EMAIL"
                    });

                var customerShoppingCart =
                    new ShoppingCart {
                            USCI = USCIG.Generate(), // TODO: USCI generator
                            Date = DateTime.Now,
                            AddressLatitude = ansc.ShoppingCartObj.AddressLatitude,
                            AddressLongitude = ansc.ShoppingCartObj.AddressLongitude,
                            Address = ansc.ShoppingCartObj.Address,
                            HasBeenRequested = false,
                            Customer = foundCustomer
                        };

                db.ShoppingCarts.Add(customerShoppingCart);
                db.SaveChanges();
                
                foreach (var id in ansc.ShoppingCartObj.MedicineIds)
                {
                    try
                    {
                        db.MedicineShoppingCarts.Add(
                        new MedicineShoppingCart {
                                Medicine = db.Medicines.Where(w => w.MedicineId == id).FirstOrDefault(),
                                ShoppingCartId = customerShoppingCart.ShoppingCartId
                            }
                        );
                        db.SaveChanges();
                    }
                    catch
                    {
                        // un-satisfying result
                        return BadRequest(new Descriptive {
                            Success = false,
                            Message = "INVALID_MEDICINE_ID" // TODO: ADD THIS TO DOC
                        });
                    }
                }

                foreach (var id in ansc.ShoppingCartObj.CosmeticIds)
                {
                    try
                    {    
                        db.CosmeticShoppingCarts.Add(
                            new CosmeticShoppingCart {
                                Cosmetic = db.Cosmetics.Where(e => e.CosmeticId == id).FirstOrDefault(),
                                ShoppingCartId = customerShoppingCart.ShoppingCartId
                            }
                        );
                        db.SaveChanges();
                    }
                    catch
                    {
                        // un-satisfying result
                        return BadRequest(new Descriptive {
                            Success = false,
                            Message = "INVALID_COSMETIC_ID" // TODO: ADD THIS TO DOC
                        });
                    }
                }

                // TODO: MAYBE INPUT IS NULL
                db.Notations.Add(
                    new Notation {
                        BrandPreference = ansc.ShoppingCartObj.BrandPreference,
                        Description = ansc.ShoppingCartObj.Description,
                        HasOtherDiseases = ansc.ShoppingCartObj.HasOtherDiseases,
                        HasPregnancy = ansc.ShoppingCartObj.HasPregnancy,
                        ShoppingCart = customerShoppingCart
                    }
                );
                db.SaveChanges();

                // TODO: MAYBE INPUT IS NULL
                db.Prescriptions.Add(
                    new Prescription {
                        HasBeenAcceptedByPharmacy = false,
                        PictureUrl_1 = ansc.ShoppingCartObj.PictureUrl_1,
                        PictureUrl_2 = ansc.ShoppingCartObj.PictureUrl_2,
                        PictureUrl_3 = ansc.ShoppingCartObj.PictureUrl_3, 
                        PicturesUploadDate = DateTime.Now,
                        ShoppingCart = customerShoppingCart
                    }
                );
                db.SaveChanges();

                // satisfying result
                return Ok(new Descriptive {
                    Success = true
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }

        // GET: android-api/customer/create-new-payment-gateway
        [HttpGet("Create-New-Payment-Gateway")]
        public async Task<ActionResult<string>> CreateNewPaymentGateway(string uoi)
        {
            try
            {
                var response = db.Orders.Where(q => q.UOI == uoi).FirstOrDefault();

                if (response != null)
                {
                    var sc = db.ShoppingCarts.Where(w => w.ShoppingCartId == response.ShoppingCartId).FirstOrDefault();
                    var c = db.Customers.Where(e => e.CustomerId == sc.CustomerId).FirstOrDefault();
                    string gender = c.Gender == Gender.Male ? "آقای" : "خانم";
                    //return "ok";
                    ////response.ShoppingCart.Customer.FirstName + " " + response.ShoppingCart.Customer.LastName
                    string description = $"پرداخت سفارش به کد {response.UOI} به نام {gender} {c.FirstName} {c.LastName} - اپلیکیشن نسخه";
                    ServicePointManager.Expect100Continue = false;
                    PaymentGatewayImplementationServicePortTypeClient zp = new PaymentGatewayImplementationServicePortTypeClient();
                    var request = await zp.PaymentRequestAsync("9c82812c-08c8-11e8-ad5e-005056a205be", (int)(response.Price), description, "amirmohammad.biuki@gmail.com", "09102116894", $"http://{HttpContext.Request.Host}/Transaction/Report");

                    if (request.Body.Status == 100)
                    {
                        return "https://zarinpal.com/pg/StartPay/" + request.Body.Authority;
                    }

                    // un-satisfying result
                    return BadRequest(new Descriptive
                    {
                        Success = false,
                        Message = "PAYMENT_GATEWAY_FAILURE"
                    });
                }

                // un-satisfying result
                return BadRequest(new Descriptive
                {
                    Success = false,
                    Message = "NO_ORDERS_MATCHED_THE_UOI"
                });
            }
            catch
            {
                // un-satisfying result
                return BadRequest(new Descriptive
                {
                    Success = false,
                    Message = "DATABASE_FAILURE"
                });
            }
        }
    }
}