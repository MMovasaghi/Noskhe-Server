using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoskheAPI_Beta.Classes;
using NoskheAPI_Beta.Classes.UIGenerator;
using NoskheAPI_Beta.CustomExceptions.Customer;
using NoskheAPI_Beta.Models;
using NoskheAPI_Beta.Models.Minimals.Output;
using NoskheAPI_Beta.Models.Response;
using NoskheBackend_Beta.Encryption;
using ZarinPalGateway;
using NoskheAPI_Beta.Settings.ResponseMessages.Customer;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.SignalR;
using NoskheAPI_Beta.Classes.Communication;
using NoskheAPI_Beta.Models.Android;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Globalization;

namespace NoskheAPI_Beta.Services
{
    public interface ICustomerService
    {
        // 1- usci generators are wrong because of concurrency
        // 2- dar EditExistingCustomer baraye taghyeer email va phone va password che eghdamati konim?
        Models.Minimals.Output.Customer GetProfileInformation();
        IEnumerable<Models.Minimals.Output.ShoppingCart> GetCustomerShoppingCarts();
        IEnumerable<Models.Minimals.Output.Order> GetCustomerOrders();
        IEnumerable<Models.Minimals.Output.Cosmetic> GetAllCosmetics();
        IEnumerable<Models.Minimals.Output.Medicine> GetAllMedicines();
        TokenTemplate LoginWithEmailAndPass(Models.Android.AuthenticateTemplate at, AppSettings appSettings);
        // ResponseTemplate LoginWithPhoneNumber(Models.Android.AuthenticateByPhoneTemplate abp, AppSettings appSettings); // TODO: Login with phone -> *Will be fixed #3*
        // bool RequestSmsForForgetPassword();
        // bool VerifySmsCodeForForgetPassword();
        Task<TokenTemplate> AddNewCustomer(Models.Android.AddNewTemplate an, AppSettings appSettings);
        bool EditExistingCustomerProfile(Models.Android.EditExistingTemplate ee);
        StatusAndIdTemplate AddNewShoppingCart(Models.Android.AddNewShoppingCartTemplate ansc);
        // Task<string> CreatePaymentUrlForOrder(int id, HostString hostIp); // TODO: Check konim ke in id male customer hast ya na // kolan lazem nist
        string RequestToken { get; set; } // motmaeninm hatman toye controller moeghdaresh set shode
        int GetCustomerId();
        CreditTemplate WalletInquiry();
        Task<ResponseTemplate> RequestService(INotificationService notificationService, IHubContext<NotificationHub> hubContext, int shoppingCartId);
        Task<AddCreditTemplate> AddCreditToWallet(int credit, HostString hostIp);
        Task<ResponseTemplate> RequestPhoneLogin(Models.Android.PhoneTemplate pt);
        TokenTemplate VerifyPhoneLogin(Models.Android.VerifyPhoneTemplate vpt, AppSettings appSettings);
        Task<ResponseTemplate> RequestResetPassword(Models.Android.PhoneTemplate pt);
        TokenTemplate VerifyResetPassword(Models.Android.VerifyPhoneTemplate vpt, AppSettings appSettings);
        ResponseTemplate ResetPassword(ResetPasswordTemplate rp);        
        ResponseTemplate VerifyPhoneNumber(Models.Android.VerifyPhoneTemplate vpt);
        CustomerLatestNotificationsTemplate LatestNotifications(INotificationService notificationService, IHubContext<NotificationHub> hubContext);        
        ResponseTemplate NotificationResponse(int notificationId);        
        void TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
    }
    class CustomerService : ICustomerService
    {
        public static string KAVENEGAR_SMS_TOKEN = "3463437075492B4E4E4453565136542B684156365559716C556572476250716A";
        public static string DEFUALT_PROFILE_PIC_URL = "http://www.someweb.com/MY_PROFILE_PIC.jpg";
        public static string AUTHRIZATION_TEMPLATE_NAME = "verify";
        public static string LOGIN_TEMPLATE_NAME = "login";
        private static NoskheAPI_Beta.Models.NoskheContext db = new NoskheAPI_Beta.Models.NoskheContext();
        public string RequestToken { get; set; }

        public async Task<TokenTemplate> AddNewCustomer(Models.Android.AddNewTemplate an, AppSettings appSettings)
        {
            try
            {
                Models.Customer foundCustomer;
                // empty password or phone
                if(string.IsNullOrEmpty(an.CustomerObj.Phone) || string.IsNullOrEmpty(an.CustomerObj.Password))
                {
                    throw new RegistrationRuleException(ErrorCodes.RegistrationRuleExceptionMsg);
                }
                
                if(!string.IsNullOrEmpty(an.CustomerObj.Email))
                {
                    if(!IsValidEmail(an.CustomerObj.Email))
                    {
                        throw new EmailIsNotValidException(ErrorCodes.EmailIsNotValidExceptionMsg);
                    }
                }

                try
                {
                    long.Parse(an.CustomerObj.Phone);
                }
                catch
                {
                    throw new RegistrationRuleException(ErrorCodes.RegistrationRuleExceptionMsg);
                }

                if(an.CustomerObj.Phone.Length != 11)
                {
                    throw new RegistrationRuleException(ErrorCodes.RegistrationRuleExceptionMsg);
                }
                // duplicate email
                if(!string.IsNullOrEmpty(an.CustomerObj.Email))
                {
                    foundCustomer = db.Customers.Where(c => c.Email == an.CustomerObj.Email).FirstOrDefault();
                    if(foundCustomer != null) throw new DuplicateCustomerException(ErrorCodes.DuplicateCustomerExceptionMsg);
                    foundCustomer = null;
                }

                foundCustomer = db.Customers.Where(c => c.Phone == an.CustomerObj.Phone).FirstOrDefault();

                if(foundCustomer == null)
                {
                    // adding new user
                    var newUser = new Models.Customer {
                            FirstName = an.CustomerObj.FirstName,
                            LastName = an.CustomerObj.LastName,
                            Gender = an.CustomerObj.Gender,
                            Birthday = an.CustomerObj.Birthday,
                            Email = an.CustomerObj.Email ?? "",
                            Password = an.CustomerObj.Password,
                            Phone = an.CustomerObj.Phone,
                            ProfilePictureUrl = DEFUALT_PROFILE_PIC_URL,
                            RegisterationDate = DateTime.Now,
                            IsPhoneValidated = false,
                            IsEmailValidated = false
                        };
                    db.Customers.Add(newUser);
                    // db.SaveChanges(); // TODO: agar usere jadid add shod vali add kardane token expception dad bayad che konim?
                    
                    Random rnd = new Random();
                    var code = rnd.Next(11111, 99999);
                    db.CustomerTextMessages.Add(
                        new CustomerTextMessage {
                            Customer = newUser,
                            Date = DateTime.Now,
                            Message = code.ToString(),
                            Type = CustomerTextMessageType.VerifyPhoneNumber,
                            Validated = false
                        }
                    );
                    // // (*) sms
                    var client = new HttpClient();
                    var response = new HttpResponseMessage();
                    response = await client.GetAsync($"https://api.kavenegar.com/v1/{KAVENEGAR_SMS_TOKEN}/verify/lookup.json?receptor={newUser.Phone}&token={code}&template={AUTHRIZATION_TEMPLATE_NAME}");


                    // adding new token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] 
                        {
                            new Claim(ClaimTypes.Name, "C" + newUser.CustomerId.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var newCustomerToken = new CustomerToken {
                        Token = tokenHandler.WriteToken(token),
                        ValidFrom = DateTime.UtcNow,
                        ValidTo = tokenDescriptor.Expires ?? DateTime.UtcNow.AddDays(7),
                        Customer = newUser,
                        IsValid = true,
                    };
                    db.CustomerTokens.Add(newCustomerToken);
                    db.SaveChanges();

                    return new TokenTemplate {
                        Token = newCustomerToken.Token,
                        Expires = DateTimeOffset.Parse(newCustomerToken.ValidTo.ToString()).ToUnixTimeSeconds()
                    };
                }
                throw new DuplicateCustomerException(ErrorCodes.DuplicateCustomerExceptionMsg);
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public StatusAndIdTemplate AddNewShoppingCart(Models.Android.AddNewShoppingCartTemplate ansc)
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests

                var foundCustomer = db.Customers.Where(q => q.CustomerId == GetCustomerId()).FirstOrDefault();

                var customerShoppingCart =
                    new Models.ShoppingCart {
                            USCI = "someUSCI", // TODO: USCI generator
                            Date = DateTime.Now,
                            AddressLatitude = ansc.ShoppingCartObj.AddressLatitude,
                            AddressLongitude = ansc.ShoppingCartObj.AddressLongitude,
                            Address = ansc.ShoppingCartObj.Address,
                            HasBeenRequested = false,
                            Customer = foundCustomer
                        };
                db.ShoppingCarts.Add(customerShoppingCart);
                db.SaveChanges();
                
                List<int> medIds = new List<int>();
                List<int> cosmIds = new List<int>();
                foreach (var id in ansc.ShoppingCartObj.MedicineIds)
                {
                    if(db.Medicines.Find(id) != null)
                    {
                        medIds.Add(id);
                    }
                    else throw new InvalidItemIdFoundException(ErrorCodes.InvalidItemIdFoundExceptionMsg);
                }

                foreach (var id in ansc.ShoppingCartObj.CosmeticIds)
                {
                    if(db.Cosmetics.Find(id) != null)
                    {
                        cosmIds.Add(id);
                    }
                    else throw new InvalidItemIdFoundException(ErrorCodes.InvalidItemIdFoundExceptionMsg);
                }

                foreach (var id in medIds)
                {
                    db.MedicineShoppingCarts.Add(
                        new MedicineShoppingCart {
                            Medicine = db.Medicines.Where(e => e.MedicineId == id).FirstOrDefault(),
                            ShoppingCartId = customerShoppingCart.ShoppingCartId
                        }
                    );
                }
                foreach (var id in cosmIds)
                {
                    db.CosmeticShoppingCarts.Add(
                        new CosmeticShoppingCart {
                            Cosmetic = db.Cosmetics.Where(e => e.CosmeticId == id).FirstOrDefault(),
                            ShoppingCartId = customerShoppingCart.ShoppingCartId
                        }
                    );
                }
                db.SaveChanges();

                // var sh = db.ShoppingCarts.Where(s => s.Date == customerShoppingCart.Date).FirstOrDefault();
                // TODO: MAYBE INPUT IS NULL
                db.Prescriptions.Add(
                    new Prescription {
                        HasBeenAcceptedByPharmacy = false,
                        PictureUrl_1 = ansc.ShoppingCartObj.PictureUrl_1 ?? "",
                        PictureUrl_2 = ansc.ShoppingCartObj.PictureUrl_2 ?? "",
                        PictureUrl_3 = ansc.ShoppingCartObj.PictureUrl_3 ?? "",
                        PicturesUploadDate = DateTime.Now,
                        ShoppingCartId = customerShoppingCart.ShoppingCartId
                    }
                );

                // TODO: MAYBE INPUT IS NULL
                db.Notations.Add(
                    new Notation {
                        BrandPreference = ansc.ShoppingCartObj.BrandPreference,
                        Description = ansc.ShoppingCartObj.Description ?? "",
                        HasOtherDiseases = ansc.ShoppingCartObj.HasOtherDiseases,
                        HasPregnancy = ansc.ShoppingCartObj.HasPregnancy,
                        ShoppingCartId = customerShoppingCart.ShoppingCartId
                    }
                );
                db.SaveChanges();

                // satisfying result
                return new StatusAndIdTemplate {
                    Success = true,
                    Id = customerShoppingCart.ShoppingCartId.ToString()
                };
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public TokenTemplate LoginWithEmailAndPass(Models.Android.AuthenticateTemplate at, AppSettings appSettings)
        {
            // agar email/pass dorost bud va valid bud vali timesh tamum bud tokene jadid tolid mikonim
            try
            {
                var existingCustomer = db.Customers.Where(q => (q.Email == at.Email && q.Password == at.Password)).FirstOrDefault();
                if(existingCustomer != null)
                {
                    if(existingCustomer.IsPhoneValidated == true) return LoginHandler(existingCustomer, appSettings);
                    else throw new PhoneNumberIsNotVerifiedException(ErrorCodes.PhoneNumberIsNotVerifiedExceptionMsg);
                }
                throw new LoginVerificationFailedException(ErrorCodes.LoginVerificationFailedExceptionMsg);
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public bool EditExistingCustomerProfile(Models.Android.EditExistingTemplate ee)
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var foundCustomer = db.Customers.Where(q => q.CustomerId == GetCustomerId()).FirstOrDefault();
                if(!foundCustomer.IsPhoneValidated) throw new PhoneNumberIsNotVerifiedException(ErrorCodes.PhoneNumberIsNotVerifiedExceptionMsg);
                
                if(string.IsNullOrEmpty(ee.CustomerObj.Phone) || string.IsNullOrEmpty(ee.CustomerObj.Password))
                {
                    throw new EditProfileRuleException(ErrorCodes.EditProfileRuleExceptionMsg);
                }
                
                if(!string.IsNullOrEmpty(ee.CustomerObj.Email))
                {
                    if(!IsValidEmail(ee.CustomerObj.Email))
                    {
                        throw new EmailIsNotValidException(ErrorCodes.EmailIsNotValidExceptionMsg);
                    }
                }

                try
                {
                    long.Parse(ee.CustomerObj.Phone);
                }
                catch
                {
                    throw new EditProfileRuleException(ErrorCodes.EditProfileRuleExceptionMsg);
                }

                if(ee.CustomerObj.Phone.Length != 11)
                {
                    throw new EditProfileRuleException(ErrorCodes.EditProfileRuleExceptionMsg);
                }
                // duplicate email
                if(!string.IsNullOrEmpty(ee.CustomerObj.Email))
                {
                    var foundCustomerByEmail = db.Customers.Where(c => c.Email == ee.CustomerObj.Email && c.CustomerId != GetCustomerId()).FirstOrDefault();
                    if(foundCustomerByEmail != null) throw new DuplicateCustomerException(ErrorCodes.DuplicateCustomerExceptionMsg);
                    foundCustomerByEmail = null;
                }
                
                if(ee.CustomerObj.Phone != foundCustomer.Phone) foundCustomer.IsPhoneValidated = false;
                if(ee.CustomerObj.Email != foundCustomer.Email) foundCustomer.IsEmailValidated = false;

                foundCustomer.FirstName = ee.CustomerObj.FirstName;
                foundCustomer.LastName = ee.CustomerObj.LastName;
                foundCustomer.Gender = ee.CustomerObj.Gender;
                foundCustomer.Birthday = ee.CustomerObj.Birthday;
                foundCustomer.Email = ee.CustomerObj.Email;
                foundCustomer.Password = ee.CustomerObj.Password;
                foundCustomer.Phone = ee.CustomerObj.Phone;
                if(ee.CustomerObj.ProfilePictureUrl != foundCustomer.ProfilePictureUrl)
                {
                    foundCustomer.ProfilePictureUrl = ee.CustomerObj.ProfilePictureUrl;
                    foundCustomer.ProfilePictureUploadDate = DateTime.Now;
                }
                db.SaveChanges();

                return true;
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public IEnumerable<Models.Minimals.Output.Cosmetic> GetAllCosmetics()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var responses =
                    from record in db.Cosmetics
                    select new Models.Minimals.Output.Cosmetic { CosmeticId = record.CosmeticId, Name = record.Name, Price = record.Price, ProductPictureUrl = record.ProductPictureUrl };
                if(responses != null) return responses.ToArray();
                
                throw new NoCosmeticsAvailabeException(ErrorCodes.NoCosmeticsAvailabeExceptionMsg);
            }
            catch
            {
                throw;
            }
        }

        public Models.Minimals.Output.Customer GetProfileInformation()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var response = db.Customers.Where(q => q.CustomerId == GetCustomerId()).FirstOrDefault();
                
                if(!response.IsPhoneValidated) throw new PhoneNumberIsNotVerifiedException(ErrorCodes.PhoneNumberIsNotVerifiedExceptionMsg);

                return new Models.Minimals.Output.Customer {
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    Gender = response.Gender,
                    Birthday = response.Birthday,
                    Email = response.Email,
                    Phone = response.Phone,
                    ProfilePictureUrl = response.ProfilePictureUrl
                };
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Models.Minimals.Output.Medicine> GetAllMedicines()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var responses =
                    from record in db.Medicines
                    select new Models.Minimals.Output.Medicine { MedicineId = record.MedicineId, Name = record.Name, Price = record.Price, ProductPictureUrl = record.ProductPictureUrl };
                if(responses != null) return responses.ToArray();

                throw new NoMedicinesAvailabeException(ErrorCodes.NoMedicinesAvailabeExceptionMsg);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Models.Minimals.Output.Order> GetCustomerOrders()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var foundCustomer = db.Customers.Where(c => c.CustomerId == GetCustomerId()).FirstOrDefault();
                if(foundCustomer == null)
                {
                    throw new NoOrdersFoundException(ErrorCodes.NoOrdersFoundExceptionMsg);
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
                    output.Email = foundCustomer.Email;
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
            catch
            {
                throw;
            }
        }

        public IEnumerable<Models.Minimals.Output.ShoppingCart> GetCustomerShoppingCarts()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var foundCustomer = db.Customers.Where(c => c.CustomerId == GetCustomerId()).FirstOrDefault();

                db.Entry(foundCustomer).Collection(c => c.ShoppingCarts).Query()
                    .Include(s => s.MedicineShoppingCarts)
                        .ThenInclude(m => m.Medicine)
                    .Include(s => s.CosmeticShoppingCarts)
                        .ThenInclude(m => m.Cosmetic)
                    .Include(s => s.Prescription)
                    .Include(s => s.Notation)
                    .Load();

                if(foundCustomer.ShoppingCarts == null)
                {
                    throw new NoShoppingCartsFoundException(ErrorCodes.NoShoppingCartsFoundExceptionMsg);
                }

                List<Models.Minimals.Output.ShoppingCart> outputs = new List<Models.Minimals.Output.ShoppingCart>();

                foreach (var selectedShoppingCart in foundCustomer.ShoppingCarts)
                {
                    decimal totalPriceWithoutPrescription = 0M;
                    Models.Minimals.Output.ShoppingCart output = new Models.Minimals.Output.ShoppingCart();
                    output.ShoppingCartId = selectedShoppingCart.ShoppingCartId;
                    output.USCI = selectedShoppingCart.USCI;
                    output.Date = selectedShoppingCart.Date;
                    output.Address = selectedShoppingCart.Address;
                    output.Email = foundCustomer.Email;
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
                throw;
            }
        }

        public List<DistanceObj> PharmaciesNearCustomer(INotificationService notificationService, IHubContext<NotificationHub> hubContext, int shoppingCartId)
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var existingShoppingCart = db.ShoppingCarts.Where(sh => sh.ShoppingCartId == shoppingCartId).FirstOrDefault();
                if (existingShoppingCart == null) throw new Exception();
                var pharmaciesLocation = db.Pharmacies.Where(p => (p.IsAvailableNow == true && p.PendingRequests < 5)).Select(p => new { PharmacyId = p.PharmacyId, Lat = p.AddressLatitude, Lon = p.AddressLongitude, Name = p.Name });

                List<DistanceObj> nearPharmacies = new List<DistanceObj>();
                GeoCoordinate shLoc = new GeoCoordinate(existingShoppingCart.AddressLatitude, existingShoppingCart.AddressLongitude);
                GeoCoordinate phLoc = new GeoCoordinate();

                foreach (var pharmacyLocation in pharmaciesLocation)
                {
                    // // check availablity of pharmacies
                    // await notificationService.P_CheckAvailablity(hubContext, pharmacyLocation.PharmacyId);

                    phLoc = new GeoCoordinate(pharmacyLocation.Lat, pharmacyLocation.Lon);
                    nearPharmacies.Add(new DistanceObj { Distance = shLoc.GetDistanceTo(phLoc), PharmacyId = pharmacyLocation.PharmacyId, Name = pharmacyLocation.Name });
                }
                var sorted = nearPharmacies.OrderBy(p => p.Distance).ToList();
                
                // foreach (var item in sorted)
                // {
                //     // if the pharmacy response is
                //     var existingPharmacy = db.Pharmacies.Where(p => p.PharmacyId == item.PharmacyId).FirstOrDefault();
                //     if(existingPharmacy.IsAvailableNow == false) sorted.Remove(item);
                // }

                // sorted.RemoveAll(item => item.Distance > 100000); // TODO: tehran values for appropriate measurements
                var trustedPharmacy = db.Pharmacies.Where(p => p.Name == "PharmacyN_1").FirstOrDefault(); // todo: whatif it was null :||
                // check if the trustedPharmacy is not available in found pharmacy list, in order to 
                if(sorted.Where(p => p.PharmacyId == trustedPharmacy.PharmacyId).FirstOrDefault() == null) sorted.Add(new DistanceObj { PharmacyId = trustedPharmacy.PharmacyId, Distance = shLoc.GetDistanceTo(new GeoCoordinate(trustedPharmacy.AddressLatitude, trustedPharmacy.AddressLongitude)) });
                
                if(sorted.Count == 0) throw new NoPharmaciesAreProvidingServiceException(ErrorCodes.NoPharmaciesAreProvidingServiceExceptionMsg);

                if(sorted.Count > 9)
                {
                    sorted.RemoveRange(9, sorted.Count - 7);
                }

                return sorted;
            }
            catch
            {

                throw;
            }
        }

        public CreditTemplate WalletInquiry()
        {
            TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
            return new CreditTemplate {
                Success = true,
                Credit = db.Customers.Where(c => c.CustomerId == GetCustomerId()).FirstOrDefault().Money
            };
        }

        public async Task<ResponseTemplate> RequestService(INotificationService notificationService, IHubContext<NotificationHub> hubContext, int shoppingCartId)
        {
            // (1) pharmaciesnearme
            // (2) signalr to first pharmacy
            // (3) return list of pharmacies ids or errors
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var existingServiceMapping = db.ServiceMappings.Where(sm => sm.ShoppingCartId == shoppingCartId).FirstOrDefault();
                // (1)
                if(existingServiceMapping == null)
                {
                    var pharmaciesQueue = PharmaciesNearCustomer(notificationService, hubContext, shoppingCartId);
                
                    var firstPharmacy = db.Pharmacies.Where(p => p.PharmacyId == pharmaciesQueue.First().PharmacyId).FirstOrDefault(); // TODO: decrement and increment control
                    firstPharmacy.PendingRequests++;

                    string foundPharmaciesString = "";
                    foreach (var pharmacy in pharmaciesQueue)
                    {
                        foundPharmaciesString += (pharmacy.PharmacyId + ",");
                    }
                    // remove last comma
                    if(foundPharmaciesString != "") foundPharmaciesString = foundPharmaciesString.Remove(foundPharmaciesString.Length - 1);

                    db.ServiceMappings.Add(
                        new ServiceMapping {
                            ShoppingCartId = shoppingCartId,
                            FoundPharmacies = foundPharmaciesString,
                            PrimativePharmacyId = pharmaciesQueue.First().PharmacyId,
                            PharmacyServiceStatus = PharmacyServiceStatus.Pending
                        }
                    );
                    db.SaveChanges();
                    // // save notifaction record
                    // var newPharmacyNotification = new PharmacyNotification {
                    //         PharmacyId = pharmaciesQueue.First().PharmacyId,
                    //         HasRecieved = false,
                    //         Date = DateTime.Now,
                    //         Type = PharmacyNotificationType.PharmacyReception
                    //     };
                    // db.PharmacyNotifications.Add(newPharmacyNotification);
                    // db.SaveChanges();
                    // (2)
                    var newItem = PrepareObject(shoppingCartId);
                    await notificationService.P_PharmacyReception(hubContext, pharmaciesQueue.First().PharmacyId, newItem);
                    // (3)
                    return new ResponseTemplate {
                        Success = true
                    };
                }
                throw new ExistingShoppingCartHasBeenRequestedEarlierException(ErrorCodes.ExistingShoppingCartHasBeenRequestedEarlierExceptionMsg);
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public async Task<AddCreditTemplate> AddCreditToWallet(int credit, HostString hostIp)
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var foundCustomer = db.Customers.Where(c => c.CustomerId == GetCustomerId()).FirstOrDefault();

                // add a record to wallet history
                var newWalletHistory = new WalletTransactionHistory {
                        Customer = foundCustomer,
                        Date = DateTime.Now,
                        Price = credit,
                        IsSuccessful = false
                    };
                db.WalletTransactionHistories.Add(newWalletHistory);
                db.SaveChanges();

                string gender = foundCustomer.Gender == Models.Gender.Male ? "آقای" : "خانم";
                string description = $"شارژ کیف پول کاربر {gender} {foundCustomer.FirstName} {foundCustomer.LastName} - اپلیکیشن نسخه";
                ServicePointManager.Expect100Continue = false;
                PaymentGatewayImplementationServicePortTypeClient zp = new PaymentGatewayImplementationServicePortTypeClient();
                var request = await zp.PaymentRequestAsync("9c82812c-08c8-11e8-ad5e-005056a205be", credit, description, "amirmohammad.biuki@gmail.com", "09102116894", $"http://{hostIp}/Transaction/{newWalletHistory.WalletTransactionHistoryId}/Wallet");
                string paymentUrl = "";
                if (request.Body.Status == 100)
                    paymentUrl = "https://zarinpal.com/pg/StartPay/" + request.Body.Authority;
                else
                    throw new PaymentGatewayFailureException(ErrorCodes.PaymentGatewayFailureExceptionMsg);
                return new AddCreditTemplate {
                    Success = true,
                    Url = paymentUrl
                };
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public NoskheForFirstNotificationOnDesktop PrepareObject(int shoppingCartId) // TODO: TRY CATCH, SAVE IN DATABASE FOR FURTHER USE AND NEXT PHARMACY RECEPTION
        {
            var existingShoppingCart = db.ShoppingCarts.Where(sc => sc.ShoppingCartId == shoppingCartId).FirstOrDefault();
            if(existingShoppingCart.Customer == null) db.Entry(existingShoppingCart).Reference(sc => sc.Customer).Load();
            if(existingShoppingCart.Prescription == null)db.Entry(existingShoppingCart).Reference(sc => sc.Prescription).Load();
            if(existingShoppingCart.Notation == null) db.Entry(existingShoppingCart).Reference(sc => sc.Notation).Load();
            if(existingShoppingCart.MedicineShoppingCarts == null) db.Entry(existingShoppingCart).Collection(sc => sc.MedicineShoppingCarts).Query()
                .Include(msc => msc.Medicine).Load();
            if(existingShoppingCart.CosmeticShoppingCarts == null) db.Entry(existingShoppingCart).Collection(sc => sc.CosmeticShoppingCarts).Query()
                .Include(msc => msc.Cosmetic).Load();
            
            var cosmetics = new List<Models.Minimals.Output.Cosmetic>();
            foreach (var cosmetic in existingShoppingCart.CosmeticShoppingCarts)
            {
                cosmetics.Add(new Models.Minimals.Output.Cosmetic {
                    Name = cosmetic.Cosmetic.Name,
                    Number = cosmetic.Quantity,
                    Price = cosmetic.Cosmetic.Price,
                    ProductPictureUrl = cosmetic.Cosmetic.ProductPictureUrl
                });
            }
            var medicines = new List<Models.Minimals.Output.Medicine>();
            foreach (var medicine in existingShoppingCart.MedicineShoppingCarts)
            {
                medicines.Add(new Models.Minimals.Output.Medicine {
                    Name = medicine.Medicine.Name,
                    Number = medicine.Quantity,
                    Price = medicine.Medicine.Price,
                    ProductPictureUrl = medicine.Medicine.ProductPictureUrl
                });
            }
            var picUrls = new List<string> { existingShoppingCart.Prescription.PictureUrl_1, existingShoppingCart.Prescription.PictureUrl_2, existingShoppingCart.Prescription.PictureUrl_3 };
            
            NoskheForFirstNotificationOnDesktop newItem = new NoskheForFirstNotificationOnDesktop
            {
                Customer = new Models.Minimals.Output.Customer
                {
                    FirstName = existingShoppingCart.Customer.FirstName,
                    LastName = existingShoppingCart.Customer.LastName,
                    Birthday = existingShoppingCart.Customer.Birthday,
                    Email = existingShoppingCart.Customer.Email,
                    Phone = existingShoppingCart.Customer.Phone,
                    Gender = existingShoppingCart.Customer.Gender,
                    ProfilePictureUrl = existingShoppingCart.Customer.ProfilePictureUrl
                },
                Cosmetics = cosmetics,
                Medicines = medicines,
                Picture_Urls = picUrls,
                Notation = existingShoppingCart.Notation
            };
            newItem.Notation.ShoppingCart = null; // if not -> message is not going to be sent to pharmacy through signalr
            return newItem;
        }

        public async Task<ResponseTemplate> RequestPhoneLogin(PhoneTemplate pt)
        {
            try
            {
                try
                {
                    long.Parse(pt.Phone);
                }
                catch
                {
                    throw new NoCustomersMatchedByPhoneException(ErrorCodes.RegistrationRuleExceptionMsg);
                }

                if(pt.Phone.Length != 11)
                {
                    throw new NoCustomersMatchedByPhoneException(ErrorCodes.RegistrationRuleExceptionMsg);
                }

                var existingCustomer = db.Customers.Where(c => c.Phone == pt.Phone).FirstOrDefault();
                if(existingCustomer != null)
                {
                    if(existingCustomer.IsPhoneValidated == false) throw new PhoneNumberIsNotVerifiedException(ErrorCodes.PhoneNumberIsNotVerifiedExceptionMsg);
                    db.Entry(existingCustomer).Collection(c => c.CustomerTextMessages).Query();
                    var existingLoginRequests = from record in existingCustomer.CustomerTextMessages
                        where record.Type == CustomerTextMessageType.Login
                        select record;
                    if(existingLoginRequests.Count() != 0)
                    {
                        var lastMessage = existingLoginRequests.Last();
                        var difference = DateTime.Now.Subtract(lastMessage.Date).TotalMinutes;
                        if(difference < 10)
                        {
                            throw new RepeatedTextMessageRequestsException(ErrorCodes.RepeatedTextMessageRequestsExceptionMsg);
                        }
                    }
                    
                    Random rnd = new Random();
                    var code = rnd.Next(11111, 99999);
                    db.CustomerTextMessages.Add(
                        new CustomerTextMessage {
                            Customer = existingCustomer,
                            Message = code.ToString(),
                            Date = DateTime.Now,
                            Type = CustomerTextMessageType.Login,
                            Validated = false
                        }
                    );
                    db.SaveChanges();

                    // TODO: (*) sms
                    var client = new HttpClient();
                    var response = new HttpResponseMessage();
                    response = await client.GetAsync($"https://api.kavenegar.com/v1/{KAVENEGAR_SMS_TOKEN}/verify/lookup.json?receptor={existingCustomer.Phone}&token={code}&template={LOGIN_TEMPLATE_NAME}");

                    return new ResponseTemplate {
                        Success = true
                    };
                }
                throw new NoCustomersMatchedByPhoneException(ErrorCodes.NoCustomersMatchedByPhoneExceptionMsg);
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public TokenTemplate VerifyPhoneLogin(VerifyPhoneTemplate vpt, AppSettings appSettings)
        {
            try
            {
                try
                {
                    long.Parse(vpt.Phone);
                }
                catch
                {
                    throw new NoCustomersMatchedByPhoneException(ErrorCodes.RegistrationRuleExceptionMsg);
                }

                if(vpt.Phone.Length != 11)
                {
                    throw new NoCustomersMatchedByPhoneException(ErrorCodes.RegistrationRuleExceptionMsg);
                }

                var existingCustomer = db.Customers.Where(c => c.Phone == vpt.Phone).FirstOrDefault();
                if(existingCustomer != null)
                {
                    db.Entry(existingCustomer).Collection(c => c.CustomerTextMessages).Query();
                    var existingLoginRequests = from record in existingCustomer.CustomerTextMessages
                        where record.Type == CustomerTextMessageType.Login
                        select record;
                    if(existingLoginRequests.Count() != 0)
                    {
                        var lastMessage = existingLoginRequests.Last();
                        var difference = DateTime.Now.Subtract(lastMessage.Date).TotalMinutes;
                        if(difference < 10)
                        {
                            if(vpt.VerificationCode == lastMessage.Message)
                            {
                                lastMessage.Validated = true;
                                db.SaveChanges();
                                return LoginHandler(existingCustomer, appSettings);
                            }
                            lastMessage.NumberOfAttempts++;
                            db.SaveChanges();
                            throw new TextMessageVerificationFailedException(ErrorCodes.TextMessageVerificationFailedExceptionMsg);
                        }
                        throw new TextMessageVerificationTimeExpiredException(ErrorCodes.TextMessageVerificationTimeExpiredExceptionMsg);
                    }
                    throw new UnauthorizedAccessException(); // if there is no request for phone login : odd one!
                }
                throw new NoCustomersMatchedByPhoneException(ErrorCodes.NoCustomersMatchedByPhoneExceptionMsg);
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public async Task<ResponseTemplate> RequestResetPassword(PhoneTemplate pt)
        {
            try
            {
                var existingCustomer = db.Customers.Where(c => c.Phone == pt.Phone).FirstOrDefault();
                if(existingCustomer != null)
                {
                    db.Entry(existingCustomer).Collection(c => c.CustomerTextMessages).Load();
                    var existingLoginRequests = from record in existingCustomer.CustomerTextMessages
                        where record.Type == CustomerTextMessageType.ForgetPassword
                        select record;
                    if(existingLoginRequests.Count() != 0)
                    {
                        var lastMessage = existingLoginRequests.Last();
                        var difference = DateTime.Now.Subtract(lastMessage.Date).TotalMinutes;
                        var numberOfRequestsPerDay = 0;
                        foreach (var textMessageRequest in existingLoginRequests)
                        {
                            if(DateTime.Now.Subtract(textMessageRequest.Date).TotalHours < 24) numberOfRequestsPerDay++;
                        }
                        if(difference < 30)
                        {
                            throw new RepeatedTextMessageRequestsException(ErrorCodes.RepeatedTextMessageRequestsExceptionMsg);
                        }
                        if(lastMessage.NumberOfAttempts > 4)
                        {
                            throw new NumberOfTextMessageTriesExceededException(ErrorCodes.NumberOfTextMessageTriesExceededExceptionMsg);
                        }
                        if(numberOfRequestsPerDay > 3)
                        {
                            throw new UnauthorizedAccessException();
                        }
                    }
                    Random rnd = new Random();
                    var code = rnd.Next(11111, 99999);
                    db.CustomerTextMessages.Add(
                        new CustomerTextMessage {
                            Customer = existingCustomer,
                            Message = code.ToString(),
                            Date = DateTime.Now,
                            Type = CustomerTextMessageType.ForgetPassword,
                            Validated = false
                        }
                    );
                    db.SaveChanges();
                    
                    // TODO: (*) sms
                    var client = new HttpClient();
                    var response = new HttpResponseMessage();
                    response = await client.GetAsync($"https://api.kavenegar.com/v1/{KAVENEGAR_SMS_TOKEN}/verify/lookup.json?receptor={existingCustomer.Phone}&token={code}&template={AUTHRIZATION_TEMPLATE_NAME}");

                    return new ResponseTemplate {
                        Success = true
                    };
                }
                throw new NoCustomersMatchedByPhoneException(ErrorCodes.NoCustomersMatchedByPhoneExceptionMsg);
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public TokenTemplate VerifyResetPassword(VerifyPhoneTemplate vpt, AppSettings appSettings)
        {
            try
            {
                var existingCustomer = db.Customers.Where(c => c.Phone == vpt.Phone).FirstOrDefault();
                if(existingCustomer != null)
                {
                    db.Entry(existingCustomer).Collection(c => c.CustomerTextMessages).Load();
                    var existingLoginRequests = from record in existingCustomer.CustomerTextMessages
                        where record.Type == CustomerTextMessageType.ForgetPassword
                        select record;
                    if(existingLoginRequests.Count() != 0)
                    {
                        var lastMessage = existingLoginRequests.Last();
                        var difference = DateTime.Now.Subtract(lastMessage.Date).TotalMinutes;
                        if(difference < 10)
                        {
                            if(vpt.VerificationCode == lastMessage.Message)
                            {
                                lastMessage.Validated = true;
                                db.SaveChanges();
                                return ResetPasswordHandler(existingCustomer, appSettings);
                            }
                            lastMessage.NumberOfAttempts++;
                            db.SaveChanges();
                            throw new TextMessageVerificationFailedException(ErrorCodes.TextMessageVerificationFailedExceptionMsg);
                        }
                        throw new TextMessageVerificationTimeExpiredException(ErrorCodes.TextMessageVerificationTimeExpiredExceptionMsg);
                    }
                    throw new UnauthorizedAccessException();
                }
                throw new NoCustomersMatchedByPhoneException(ErrorCodes.NoCustomersMatchedByPhoneExceptionMsg);
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public ResponseTemplate ResetPassword(ResetPasswordTemplate rpt)
        {
            try
            {
                ResetPasswordTokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var existingCustomer = db.Customers.Where(c => c.CustomerId == GetCustomerResetPasswordId()).FirstOrDefault();
                existingCustomer.Password = rpt.NewPassword;
                db.SaveChanges();
                return new ResponseTemplate {
                    Success = true
                };
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        // taeede shomare mobile bad az sabte naam, baraye login kardan
        public ResponseTemplate VerifyPhoneNumber(VerifyPhoneTemplate vpt)
        {
            try
            {
                var existingCustomer = db.Customers.Where(c => c.Phone == vpt.Phone).FirstOrDefault();
                if(existingCustomer != null)
                {
                    // if(existingCustomer.CustomerTextMessages != null) db.Entry(existingCustomer).Collection(c => c.CustomerTextMessages).Query();
                    // var existingLoginRequests = from record in existingCustomer.CustomerTextMessages
                    //     where record.Type == CustomerTextMessageType.VerifyPhoneNumber
                    //     select record;
                    var existingLoginRequests = db.CustomerTextMessages.Where(ctm => ctm.CustomerId == existingCustomer.CustomerId).ToList();
                    if(existingLoginRequests.Count() != 0)
                    {
                        var lastMessage = existingLoginRequests.Last();
                        var difference = DateTime.Now.Subtract(lastMessage.Date).TotalMinutes;
                        if(difference < 10)
                        {
                            if(vpt.VerificationCode == lastMessage.Message)
                            {
                                // verify ok
                                lastMessage.Validated = true;
                                existingCustomer.IsPhoneValidated = true;
                                lastMessage.NumberOfAttempts++;

                                db.SaveChanges();

                                return new ResponseTemplate {
                                    Success = true
                                };
                            }

                            // user entered wrong code
                            lastMessage.NumberOfAttempts++;
                            db.SaveChanges();
                            throw new TextMessageVerificationFailedException(ErrorCodes.TextMessageVerificationFailedExceptionMsg);
                        }

                        // user didn't type code in defined time
                        throw new TextMessageVerificationTimeExpiredException(ErrorCodes.TextMessageVerificationTimeExpiredExceptionMsg);
                    }
                    throw new UnauthorizedAccessException();
                }
                throw new NoCustomersMatchedByPhoneException(ErrorCodes.NoCustomersMatchedByPhoneExceptionMsg);
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public CustomerLatestNotificationsTemplate LatestNotifications(INotificationService notificationService, IHubContext<NotificationHub> hubContext)
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var latestNotifications = db.CustomerNotifications.Where(cn => cn.CustomerId == GetCustomerId() && cn.HasRecieved == false).ToList();
                var latestNotificationsTemplate = new CustomerLatestNotificationsTemplate();
                if(latestNotifications.Count != 0)
                {
                    latestNotificationsTemplate.Any = true; // there is at least one notification
                    foreach (var notification in latestNotifications)
                    {
                        switch (notification.Type)
                        {
                            case CustomerNotificationType.CancellationReport:
                                latestNotificationsTemplate.CancellationReportObj.Content.Add(new string[] {
                                        notification.Date.ToString(),
                                        notification.Arg1 ?? "",
                                        notification.Arg2 ?? "",
                                        notification.Arg3 ?? "",
                                        notification.Arg4 ?? "",
                                        notification.Arg5 ?? "",
                                        notification.Arg6 ?? "",
                                    });
                                break;
                            case CustomerNotificationType.CourierDetail:
                                latestNotificationsTemplate.CourierDetailObj.Content.Add(new string[] {
                                        notification.Date.ToString(),
                                        notification.Arg1 ?? "",
                                        notification.Arg2 ?? "",
                                        notification.Arg3 ?? "",
                                        notification.Arg4 ?? "",
                                        notification.Arg5 ?? "",
                                        notification.Arg6 ?? "",
                                    });
                                break;
                            case CustomerNotificationType.InvoiceDetails:
                                latestNotificationsTemplate.InvoiceDetailsObj.Content.Add(new string[] {
                                        notification.Date.ToString(),
                                        notification.Arg1 ?? "",
                                        notification.Arg2 ?? "",
                                        notification.Arg3 ?? "",
                                        notification.Arg4 ?? "",
                                        notification.Arg5 ?? "",
                                        notification.Arg6 ?? "",
                                    });
                                break;
                            case CustomerNotificationType.PharmacyInquiry:
                                latestNotificationsTemplate.PharmacyInquiryObj.Content.Add(new string[] {
                                        notification.Date.ToString(),
                                        notification.Arg1 ?? "",
                                        notification.Arg2 ?? "",
                                        notification.Arg3 ?? "",
                                        notification.Arg4 ?? "",
                                        notification.Arg5 ?? "",
                                        notification.Arg6 ?? "",
                                    });
                                break;
                        }
                        notification.HasRecieved = true;
                    }
                    db.SaveChanges();
                }
                latestNotificationsTemplate.Any = false; // there is no unread notifications available
                return latestNotificationsTemplate;
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public ResponseTemplate NotificationResponse(int notificationId)
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var existingNotification = db.CustomerNotifications.Where(cn => cn.CustomerNotificationId == notificationId).FirstOrDefault();
                if(existingNotification != null)
                {
                    existingNotification.HasRecieved = true;
                    db.SaveChanges();
                    return new ResponseTemplate {
                        Success = true
                    };
                }
                throw new NoNotificationsMatchedByIdException(ErrorCodes.NoNotificationsMatchedByIdExceptionMsg);
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public TokenTemplate ResetPasswordHandler(Models.Customer existingCustomer, AppSettings appSettings)
        {
            db.Entry(existingCustomer).Reference(c => c.CustomerResetPasswordToken).Load();
            if(existingCustomer.CustomerResetPasswordToken != null) // hatman moghe AddNewCustomer sakhte shode hast
            {
                if(DateTime.UtcNow > existingCustomer.CustomerResetPasswordToken.ValidTo)
                {
                    // token re-creation process -----------------------------------------------------------------
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] 
                        {
                            new Claim(ClaimTypes.Name, "CRP" + existingCustomer.CustomerId.ToString()) // customer reset password
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(2),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    // ----------------------------------------------------------------------------------------
                    existingCustomer.CustomerResetPasswordToken.Token = tokenHandler.WriteToken(token);
                    existingCustomer.CustomerResetPasswordToken.ValidFrom = DateTime.UtcNow;
                    existingCustomer.CustomerResetPasswordToken.ValidTo = tokenDescriptor.Expires ?? DateTime.UtcNow.AddMinutes(2);
                    existingCustomer.CustomerResetPasswordToken.TokenRefreshRequests++; // afzoodane tedade dafaate avaz kardane token
                    db.SaveChanges();
                }

                return new TokenTemplate {
                    Token = existingCustomer.CustomerResetPasswordToken.Token,
                    Expires = DateTimeOffset.Parse(existingCustomer.CustomerResetPasswordToken.ValidTo.ToString()).ToUnixTimeSeconds()
                };
            }
            else // agar token null bud
            {
                // adding new token
                // token re-creation process -----------------------------------------------------------------
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    {
                        new Claim(ClaimTypes.Name, "CRP" + existingCustomer.CustomerId.ToString()) // customer reset password
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var newCustomerResetPasswordToken = new Models.CustomerResetPasswordToken
                {
                    Token = tokenHandler.WriteToken(token),
                    ValidFrom = DateTime.UtcNow,
                    ValidTo = tokenDescriptor.Expires ?? DateTime.UtcNow.AddMinutes(2),
                    Customer = existingCustomer,
                    IsValid = true,
                };
                db.CustomerResetPasswordToken.Add(newCustomerResetPasswordToken);
                db.SaveChanges();

                return new TokenTemplate {
                    Token = newCustomerResetPasswordToken.Token,
                    Expires = DateTimeOffset.Parse(newCustomerResetPasswordToken.ValidTo.ToString()).ToUnixTimeSeconds()
                };
            }
        }

        public TokenTemplate LoginHandler(Models.Customer existingCustomer, AppSettings appSettings)
        {
            db.Entry(existingCustomer).Reference(c => c.CustomerToken).Load();
            if(existingCustomer.CustomerToken != null) // hatman moghe AddNewCustomer sakhte shode hast
            {
                if(DateTime.UtcNow > existingCustomer.CustomerToken.ValidTo)
                {
                    // token re-creation process -----------------------------------------------------------------
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] 
                        {
                            new Claim(ClaimTypes.Name, "C" + existingCustomer.CustomerId.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    // ----------------------------------------------------------------------------------------
                    existingCustomer.CustomerToken.Token = tokenHandler.WriteToken(token);
                    existingCustomer.CustomerToken.ValidFrom = DateTime.UtcNow;
                    existingCustomer.CustomerToken.ValidTo = tokenDescriptor.Expires ?? DateTime.UtcNow.AddDays(7);
                    existingCustomer.CustomerToken.TokenRefreshRequests++; // afzoodane tedade dafaate avaz kardane token
                    db.SaveChanges();
                }
                existingCustomer.CustomerToken.LoginRequests++; // afzoodane tedade dafaate login
                db.SaveChanges();

                return new TokenTemplate {
                    Token = existingCustomer.CustomerToken.Token,
                    Expires = DateTimeOffset.Parse(existingCustomer.CustomerToken.ValidTo.ToString()).ToUnixTimeSeconds()
                };
            }
            else // agar token pharmacy null bud
            {
                // adding new token
                // token re-creation process -----------------------------------------------------------------
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    {
                        new Claim(ClaimTypes.Name, "C" + existingCustomer.CustomerId.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var newCustomerToken = new Models.CustomerToken
                {
                    Token = tokenHandler.WriteToken(token),
                    ValidFrom = DateTime.UtcNow,
                    ValidTo = tokenDescriptor.Expires ?? DateTime.UtcNow.AddDays(7),
                    Customer = existingCustomer,
                    IsValid = true,
                };
                db.CustomerTokens.Add(newCustomerToken);
                db.SaveChanges();

                return new TokenTemplate {
                    Token = newCustomerToken.Token,
                    Expires = DateTimeOffset.Parse(newCustomerToken.ValidTo.ToString()).ToUnixTimeSeconds()
                };
            }
        }

        public void TokenValidationHandler()
        {
            // agar token dar db bud -> invalid ya expired
            // agar token dar db nabud -> customer vojud nadarad
            // ------
            // invalid YA namojud (null) --> unauthorized exception midahad
            // expired --> token expired exception midahad
            // ------
            // pas digar sharte null budane customer dar edame code fayde nadarad va az ghabl hame chiz malum hast
            try
            {
                var customer = db.CustomerTokens.Where(ct => ct.Token == RequestToken).FirstOrDefault();
                if(customer == null || customer.IsValid == false) throw new UnauthorizedAccessException();
                if(DateTime.UtcNow > customer.ValidTo) throw new SecurityTokenExpiredException(ErrorCodes.SecurityTokenExpiredExceptionMsg);
            }
            catch 
            {
                throw;
                // throw new APIUnhandledException(ErrorCodes.APIUnhandledExceptionMsg);
            }
        }

        public void ResetPasswordTokenValidationHandler()
        {
            // agar token dar db bud -> invalid ya expired
            // agar token dar db nabud -> customer vojud nadarad
            // ------
            // invalid YA namojud (null) --> unauthorized exception midahad
            // expired --> token expired exception midahad
            // ------
            // pas digar sharte null budane customer dar edame code fayde nadarad va az ghabl hame chiz malum hast
            try
            {
                var customer = db.CustomerResetPasswordToken.Where(ct => ct.Token == RequestToken).FirstOrDefault();
                if(customer == null || customer.IsValid == false) throw new UnauthorizedAccessException();
                if(DateTime.UtcNow > customer.ValidTo) throw new SecurityTokenExpiredException(ErrorCodes.SecurityTokenExpiredExceptionMsg);
            }
            catch 
            {
                throw;
                // throw new APIUnhandledException(ErrorCodes.APIUnhandledExceptionMsg);
            }
        }

        public int GetCustomerId()
        {
            try
            {
                // agar peida shod va valid bud vali timesh tamum shode bud -> SecurityToken"Expired"Exception
                // agar peida nashod -> "Unauthorized"AccessException
                var customer = db.CustomerTokens.Where(ct => ct.Token == RequestToken).FirstOrDefault();
                if(customer == null || customer.IsValid == false) throw new UnauthorizedAccessException();
                if(DateTime.UtcNow > customer.ValidTo) throw new SecurityTokenExpiredException(ErrorCodes.SecurityTokenExpiredExceptionMsg);
                return customer.CustomerId;
            }
            catch
            {
                throw;
            }
        }

        public int GetCustomerResetPasswordId()
        {
            try
            {
                // agar peida shod va valid bud vali timesh tamum shode bud -> SecurityToken"Expired"Exception
                // agar peida nashod -> "Unauthorized"AccessException
                var customer = db.CustomerResetPasswordToken.Where(ct => ct.Token == RequestToken).FirstOrDefault();
                if(customer == null || customer.IsValid == false) throw new UnauthorizedAccessException();
                if(DateTime.UtcNow > customer.ValidTo) throw new SecurityTokenExpiredException(ErrorCodes.SecurityTokenExpiredExceptionMsg);
                return customer.CustomerId;
            }
            catch
            {
                throw;
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}

namespace NoskheAPI_Beta.Services
{
    public class DistanceObj
    {
        public int PharmacyId { get; set; }
        public double Distance { get; set; }
        public string Name { get; set; }
    }
}