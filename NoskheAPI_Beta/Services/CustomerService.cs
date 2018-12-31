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
        IEnumerable<Models.Minimals.Output.Cosmetic> GetCosmeticsOfAShoppingCart(int id);
        IEnumerable<Models.Minimals.Output.Medicine> GetAllMedicines();
        IEnumerable<Models.Minimals.Output.Medicine> GetMedicinesOfAShoppingCart(int id);
        TokenTemplate LoginWithEmailAndPass(Models.Android.AuthenticateTemplate at, AppSettings appSettings);
        bool LoginWithPhoneNumber(Models.Android.AuthenticateByPhoneTemplate abp, AppSettings appSettings); // TODO: Login with phone -> *Will be fixed #3*
        bool RequestSmsForForgetPassword();
        bool VerifySmsCodeForForgetPassword();
        TokenTemplate AddNewCustomer(Models.Android.AddNewTemplate an, AppSettings appSettings);
        bool EditExistingCustomerProfile(Models.Android.EditExistingTemplate ee);
        ResponseTemplate AddNewShoppingCart(Models.Android.AddNewShoppingCartTemplate ansc);
        Task<string> CreatePaymentUrlForOrder(int id, HostString hostIp); // TODO: Check konim ke in id male customer hast ya na
        string RequestToken { get; set; } // motmaeninm hatman toye controller moeghdaresh set shode
        int GetCustomerId();
        void TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
        IEnumerable<FoundPharmaciesTemplate> PharmaciesNearCustomer(Models.Android.AddNewShoppingCartTemplate ansc);
        ResponseTemplate WalletInquiry();
        ResponseTemplate PayTheOrder(int orderId);
    }
    class CustomerService : ICustomerService
    {
        private static NoskheAPI_Beta.Models.NoskheContext db = new NoskheAPI_Beta.Models.NoskheContext();
        public string RequestToken { get; set; }

        public TokenTemplate AddNewCustomer(Models.Android.AddNewTemplate an, AppSettings appSettings)
        {
            try
            {
                var foundCustomer = db.Customers.Where(q => (q.Email == an.CustomerObj.Email && q.Phone == an.CustomerObj.Phone)).FirstOrDefault();
                if(foundCustomer == null)
                {
                    // adding new user
                    var newUser = new Models.Customer {
                            FirstName = an.CustomerObj.FirstName,
                            LastName = an.CustomerObj.LastName,
                            Gender = an.CustomerObj.Gender,
                            Birthday = an.CustomerObj.Birthday,
                            Email = an.CustomerObj.Email,
                            Password = an.CustomerObj.Password,
                            Phone = an.CustomerObj.Phone,
                            ProfilePictureUrl = an.CustomerObj.ProfilePictureUrl,
                            // server-side decisions
                            RegisterationDate = DateTime.Now
                        };
                    db.Customers.Add(newUser);
                    db.SaveChanges(); // TODO: agar usere jadid add shod vali add kardane token expception dad bayad che konim?

                    // adding new token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] 
                        {
                            new Claim(ClaimTypes.Name, newUser.CustomerId.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var newCustomerToken = new CustomerToken {
                        Token = tokenHandler.WriteToken(token),
                        ValidFrom = DateTime.UtcNow,
                        ValidTo = tokenDescriptor.Expires ?? DateTime.UtcNow.AddDays(1),
                        Customer = newUser,
                        IsValid = true,
                        IsAvailableInSignalR = false
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

        public ResponseTemplate AddNewShoppingCart(Models.Android.AddNewShoppingCartTemplate ansc)
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
                        throw new InvalidMedicineIDFoundException(ErrorCodes.InvalidMedicineIDFoundExceptionMsg);
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
                        throw new InvalidCosmeticIDFoundException(ErrorCodes.InvalidCosmeticIDFoundExceptionMsg);
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
                return new ResponseTemplate {
                    Success = true,
                    Error = customerShoppingCart.ShoppingCartId.ToString()
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
                                    new Claim(ClaimTypes.Name, existingCustomer.CustomerId.ToString())
                                }),
                                Expires = DateTime.UtcNow.AddDays(1),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                            };
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            // ----------------------------------------------------------------------------------------
                            existingCustomer.CustomerToken.Token = tokenHandler.WriteToken(token);
                            existingCustomer.CustomerToken.ValidFrom = DateTime.UtcNow;
                            existingCustomer.CustomerToken.ValidTo = tokenDescriptor.Expires ?? DateTime.UtcNow.AddDays(1);
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
                    throw new UnauthorizedAccessException();
                }
                throw new LoginVerificationFailedException(ErrorCodes.LoginVerificationFailedExceptionMsg);
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public bool LoginWithPhoneNumber(Models.Android.AuthenticateByPhoneTemplate abp, AppSettings appSettings)
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
                    return true;
                }

                // un-satisfying result
                throw new NoCustomersMatchedByPhoneException(ErrorCodes.NoCustomersMatchedByPhoneExceptionMsg);
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public async Task<string> CreatePaymentUrlForOrder(int id, HostString hostIp)
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var response = db.Orders.Where(q => q.OrderId == id).FirstOrDefault();

                if (response != null)
                {
                    var sc = db.ShoppingCarts.Where(w => w.ShoppingCartId == response.ShoppingCartId).FirstOrDefault();
                    var c = db.Customers.Where(e => e.CustomerId == sc.CustomerId).FirstOrDefault();
                    string gender = c.Gender == Gender.Male ? "آقای" : "خانم";
                    string description = $"پرداخت سفارش به کد {response.UOI} به نام {gender} {c.FirstName} {c.LastName} - اپلیکیشن نسخه";
                    ServicePointManager.Expect100Continue = false;
                    PaymentGatewayImplementationServicePortTypeClient zp = new PaymentGatewayImplementationServicePortTypeClient();
                    var request = await zp.PaymentRequestAsync("9c82812c-08c8-11e8-ad5e-005056a205be", (int)(response.Price), description, "amirmohammad.biuki@gmail.com", "09102116894", $"http://{hostIp}/Transaction/Report");

                    if (request.Body.Status == 100)
                    {
                        return "https://zarinpal.com/pg/StartPay/" + request.Body.Authority;
                    }

                    throw new PaymentGatewayFailureException(ErrorCodes.PaymentGatewayFailureExceptionMsg);
                }
                throw new NoOrdersMatchedByIdException(ErrorCodes.NoOrdersMatchedByIdException);
            }
            catch
            {
                throw;
            }
        }

        public bool EditExistingCustomerProfile(Models.Android.EditExistingTemplate ee)
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var foundCustomer = db.Customers.Where(q => q.CustomerId == GetCustomerId()).FirstOrDefault();
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

        public Models.Minimals.Output.Customer GetProfileInformation()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var response = db.Customers.Where(q => q.CustomerId == GetCustomerId()).FirstOrDefault();
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

        public IEnumerable<Models.Minimals.Output.Cosmetic> GetCosmeticsOfAShoppingCart(int id)
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var existingCustomer = db.Customers.Where(c => c.CustomerId == GetCustomerId()).FirstOrDefault();
                db.Entry(existingCustomer).Collection(c => c.ShoppingCarts).Query()
                    .Include(sc => sc.CosmeticShoppingCarts)
                        .ThenInclude(msc => msc.Cosmetic)
                    .Load();

                var existingShoppingCart = existingCustomer.ShoppingCarts.Where(sc => sc.ShoppingCartId == id).FirstOrDefault();
                if(existingShoppingCart != null)
                {
                    var cosmetics =
                        from cosmetic in existingShoppingCart.CosmeticShoppingCarts
                        where cosmetic.ShoppingCartId == id
                        select new Models.Minimals.Output.Cosmetic { CosmeticId = cosmetic.Cosmetic.CosmeticId, Name = cosmetic.Cosmetic.Name, Price = cosmetic.Cosmetic.Price, ProductPictureUrl = cosmetic.Cosmetic.ProductPictureUrl };
                    if(cosmetics != null) return cosmetics.ToArray();
                    throw new NoCosmeticsInTheShoppingCartException(ErrorCodes.NoCosmeticsInTheShoppingCartExceptionMsg); // Khali hast
                }
                throw new UnauthorizedAccessException(); // NOTE: angah in customer id eshtebah zade va nabayad shopping carte kase dige ro neshun bede
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Models.Minimals.Output.Medicine> GetMedicinesOfAShoppingCart(int id)
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var existingCustomer = db.Customers.Where(c => c.CustomerId == GetCustomerId()).FirstOrDefault();
                db.Entry(existingCustomer).Collection(c => c.ShoppingCarts).Query()
                    .Include(sc => sc.MedicineShoppingCarts)
                        .ThenInclude(msc => msc.Medicine)
                    .Load();

                var existingShoppingCart = existingCustomer.ShoppingCarts.Where(sc => sc.ShoppingCartId == id).FirstOrDefault();
                if(existingShoppingCart != null)
                {
                    var medicines =
                        from medicine in existingShoppingCart.MedicineShoppingCarts
                        where medicine.ShoppingCartId == id
                        select new Models.Minimals.Output.Medicine { MedicineId = medicine.Medicine.MedicineId, Name = medicine.Medicine.Name, Price = medicine.Medicine.Price, ProductPictureUrl = medicine.Medicine.ProductPictureUrl };
                    if(medicines != null) return medicines.ToArray();
                    throw new NoMedicinesInTheShoppingCartException(ErrorCodes.NoMedicinesInTheShoppingCartExceptionMsg); // Khali hast
                }
                throw new UnauthorizedAccessException(); // NOTE: angah in customer id eshtebah zade va nabayad shopping carte kase dige ro neshun bede
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

        public bool RequestSmsForForgetPassword()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                throw new NotImplementedException();
            }
            catch
            {
                throw;
            }
        }

        public bool VerifySmsCodeForForgetPassword()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                throw new NotImplementedException();
            }
            catch
            {
                throw;
            }
            // catch(SmsVerificationFailedException vfe)
            // {
            //     throw new SmsVerificationFailedException(vfe.Message);
            // }
            // catch(SmsVerificationExpiredException vee)
            // {
            //     throw new SmsVerificationExpiredException(vee.Message);
            // }
        }

        public IEnumerable<FoundPharmaciesTemplate> PharmaciesNearCustomer(Models.Android.AddNewShoppingCartTemplate ansc)
        {
            /* 
            * 1- find 6 nearest phrmacies based on location and distance & 2 trusted pharmacy
            * 2- if a pharmacy status is not ready skip
            * 3- if all pharmacies were busy or not ready return busy error
            * 4- otherwise save records to ShoppingCartMapping (ShoppingCartId+CustomerId+PharmacyIds) table & return list of FoundPharmaciesTemplate
            * 4- invoke NotificationService.PharmacyShoppingCartReception(ShoppingCartMappingId)
            */
            // 1
            // 2
            // 3
            // 4
            // 5
            throw new NotImplementedException();
        }

        public ResponseTemplate WalletInquiry()
        {
            throw new NotImplementedException();
        }

        public ResponseTemplate PayTheOrder(int orderId)
        {
            throw new NotImplementedException();
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
                throw new APIUnhandledException(ErrorCodes.APIUnhandledExceptionMsg);
            }
        }
    }
}