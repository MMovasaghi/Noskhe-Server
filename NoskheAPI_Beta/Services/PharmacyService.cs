using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoskheAPI_Beta.Classes;
using NoskheAPI_Beta.CustomExceptions.Pharmacy;
using NoskheAPI_Beta.Models.Minimals.Output;
using NoskheAPI_Beta.Models.Response;
using NoskheAPI_Beta.Settings.ResponseMessages.Pharmacy;
using NoskheBackend_Beta.General;

namespace NoskheAPI_Beta.Services
{
    public interface IPharmacyService
    {
        ResponseTemplate GetDbStatus();
        ResponseTemplate GetDatetime();
        ResponseTemplate GetServerState();
        Models.Minimals.Output.Pharmacy GetProfile();
        IEnumerable<Models.Minimals.Output.Order> GetOrders(string start="start-undifiend", string end="end-undifiend");
        Models.Minimals.Output.Score GetScore();
        IEnumerable<Models.Minimals.Output.Settle> GetSettles();
        ResponseTemplate SetANewSettle(Models.Minimals.Input.Settle settle);
        IEnumerable<Models.Minimals.Output.Score> GetTopFivePharmacies();
        TokenTemplate LoginWithEmailAndPass(string[] credential, AppSettings appSettings);
        ResponseTemplate ToggleStateOfPharmacy();
        IEnumerable<float> NumberOfOrdersInThisWeek();
        IEnumerable<float> AverageTimeOfPackingInThisWeek();
        string RequestToken { get; set; } // motmaeninm hatman toye controller moeghdaresh set shode
        int GetPharmacyId();
        void TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
    }
    class PharmacyService : IPharmacyService
    {
        private static NoskheAPI_Beta.Models.NoskheContext db = new NoskheAPI_Beta.Models.NoskheContext();
        public string RequestToken { get; set; }

        public TokenTemplate LoginWithEmailAndPass(string[] credential, AppSettings appSettings)
        {
            try
            {
                var existingPharmacy = db.Pharmacies.Where(q => (q.Email == credential[0] && q.Password == credential[1])).FirstOrDefault();
                if(existingPharmacy != null)
                {
                    db.Entry(existingPharmacy).Reference(c => c.PharmacyToken).Load();
                    if(existingPharmacy.PharmacyToken != null) // hatman moghe AddNewCustomer sakhte shode hast
                    {
                        if(DateTime.UtcNow > existingPharmacy.PharmacyToken.ValidTo)
                        {
                            // token re-creation process -----------------------------------------------------------------
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[] 
                                {
                                    new Claim(ClaimTypes.Name, existingPharmacy.PharmacyId.ToString())
                                }),
                                Expires = DateTime.UtcNow.AddDays(1),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                            };
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            // ----------------------------------------------------------------------------------------
                            existingPharmacy.PharmacyToken.Token = tokenHandler.WriteToken(token);
                            existingPharmacy.PharmacyToken.ValidFrom = DateTime.UtcNow;
                            existingPharmacy.PharmacyToken.ValidTo = tokenDescriptor.Expires ?? DateTime.UtcNow.AddDays(1);
                            existingPharmacy.PharmacyToken.TokenRefreshRequests++; // afzoodane tedade dafaate avaz kardane token
                            db.SaveChanges();
                        }
                        existingPharmacy.PharmacyToken.LoginRequests++; // afzoodane tedade dafaate login
                        db.SaveChanges();

                        return new TokenTemplate {
                            Token = existingPharmacy.PharmacyToken.Token,
                            Expires = DateTimeOffset.Parse(existingPharmacy.PharmacyToken.ValidTo.ToString()).ToUnixTimeSeconds()
                        };
                    }
                    else // agar token pharmacy null bud
                    {
                        // adding new token
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[] 
                            {
                                new Claim(ClaimTypes.Name, existingPharmacy.PharmacyId.ToString())
                            }),
                            Expires = DateTime.UtcNow.AddDays(1),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var newPharmacyToken = new Models.PharmacyToken
                        {
                            Token = tokenHandler.WriteToken(token),
                            ValidFrom = DateTime.UtcNow,
                            ValidTo = tokenDescriptor.Expires ?? DateTime.UtcNow.AddDays(1),
                            Pharmacy = existingPharmacy,
                            IsValid = true
                        };
                        db.PharmacyTokens.Add(newPharmacyToken);
                        db.SaveChanges();

                        return new TokenTemplate {
                            Token = newPharmacyToken.Token,
                            Expires = DateTimeOffset.Parse(newPharmacyToken.ValidTo.ToString()).ToUnixTimeSeconds()
                        };
                    }
                }
                throw new LoginVerificationFailedException(ErrorCodes.LoginVerificationFailedExceptionMsg);
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public ResponseTemplate ToggleStateOfPharmacy()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var existingPharmacy = db.Pharmacies.Where(q => q.PharmacyId == GetPharmacyId()).FirstOrDefault();
                
                if(existingPharmacy.IsAvailableNow == true)
                {
                    existingPharmacy.IsAvailableNow = false;
                    db.SaveChanges();

                    return new ResponseTemplate {
                        Success = true,
                        Error = existingPharmacy.IsAvailableNow.ToString()
                    };
                }
                else
                {
                    existingPharmacy.IsAvailableNow = true;
                    db.SaveChanges();

                    return new ResponseTemplate {
                        Success = true,
                        Error = existingPharmacy.IsAvailableNow.ToString()
                    };
                }
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public ResponseTemplate GetDbStatus()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var result = db.Customers.Where(s => s.CustomerId == 1);
                return new ResponseTemplate {
                    Success = true
                };
            }
            catch
            {
                throw;
            }
        }

        public ResponseTemplate GetDatetime()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                return new ResponseTemplate {
                    Success = true,
                    Error = DateTime.Now.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        public Pharmacy GetProfile()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var search = db.Pharmacies.Where(q => q.PharmacyId == GetPharmacyId()).FirstOrDefault();
                
                return new Models.Minimals.Output.Pharmacy {
                    Name = search.Name,
                    UPI = search.UPI,
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
                throw;
            }
        }

        public IEnumerable<Order> GetOrders(string start="start-undifiend", string end="end-undifiend")
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var existingPharmacy = db.Pharmacies.Where(p => p.PharmacyId == GetPharmacyId()).FirstOrDefault();
                db.Entry(existingPharmacy).Collection(p => p.Orders).Load();
                
                List<Models.Minimals.Output.Order> ListOfOrders = new List<Models.Minimals.Output.Order>();

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
                            throw new InvalidTimeFormatException(ErrorCodes.InvalidTimeFormatExceptionMsg);
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
                            throw new InvalidTimeFormatException(ErrorCodes.InvalidTimeFormatExceptionMsg);
                        }
                }

                if(startTime >= endTime)
                    throw new InvalidTimeFormatException(ErrorCodes.InvalidTimeFormatExceptionMsg);

                foreach (var order in existingPharmacy.Orders)
                {
                    if(order.Date < startTime && order.Date > endTime) continue;
                    decimal totalPriceWithoutShippingCost = 0M;
                    Models.Minimals.Output.Order tempOrder = new Models.Minimals.Output.Order();
                    tempOrder.UOI = order.UOI;
                    tempOrder.Date = order.Date;
                    tempOrder.HasPrescription = order.HasPrescription;
                    tempOrder.HasBeenAcceptedByCustomer = order.HasBeenAcceptedByCustomer;
                    tempOrder.HasBeenPaid = order.HasBeenPaid;
                    tempOrder.HasBeenDeliveredToCustomer = order.HasBeenDeliveredToCustomer;
                    tempOrder.HasBeenSettled = order.HasBeenSettled;
                    tempOrder.CourierName = order.Courier.FirstName + " " + order.Courier.LastName;
                    tempOrder.Address = order.ShoppingCart.Address;
                    tempOrder.Email = order.ShoppingCart.Customer.Email;
                    tempOrder.BrandPreference = order.ShoppingCart.Notation.BrandPreference;
                    tempOrder.HasPregnancy = order.ShoppingCart.Notation.HasPregnancy;
                    tempOrder.HasOtherDiseases = order.ShoppingCart.Notation.HasOtherDiseases;
                    tempOrder.Description = order.ShoppingCart.Notation.Description;
                    tempOrder.HasBeenAcceptedByPharmacy = order.ShoppingCart.Prescription.HasBeenAcceptedByPharmacy;
                    tempOrder.PictureUrl_1 = order.ShoppingCart.Prescription.PictureUrl_1;
                    tempOrder.PictureUrl_2 = order.ShoppingCart.Prescription.PictureUrl_2;
                    tempOrder.PictureUrl_3 = order.ShoppingCart.Prescription.PictureUrl_3;

                    var ListOfCosmetics =
                        from record in db.CosmeticShoppingCarts
                        where record.ShoppingCartId == order.ShoppingCartId
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
                        where record.ShoppingCartId == order.ShoppingCartId
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
                        where record.Prescription.ShoppingCartId == order.ShoppingCartId
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
                    tempOrder.TotalPrice = order.Price;
                    
                    ListOfOrders.Add(tempOrder);
                }
                return ListOfOrders.ToArray();
            }
            catch
            {
                throw;
            }
        }

        public int GetPharmacyId()
        {
            try
            {
                // agar peida shod va valid bud vali timesh tamum shode bud -> SecurityToken"Expired"Exception
                // agar peida nashod -> "Unauthorized"AccessException
                var pharmacy = db.PharmacyTokens.Where(ct => ct.Token == RequestToken).FirstOrDefault();
                if(pharmacy == null || pharmacy.IsValid == false) throw new UnauthorizedAccessException();
                if(DateTime.UtcNow > pharmacy.ValidTo) throw new SecurityTokenExpiredException(ErrorCodes.SecurityTokenExpiredExceptionMsg);
                return pharmacy.PharmacyId;
            }
            catch
            {
                throw;
            }
        }

        public Score GetScore()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var existingPharmacy = db.Pharmacies.Where(p => p.PharmacyId == GetPharmacyId()).FirstOrDefault();
                db.Entry(existingPharmacy).Reference(p => p.Score).Load();

                // var search = db.Scores.Where(q => q.Pharmacy.PharmacyId == GetPharmacyId()).FirstOrDefault();
                
                return new Models.Minimals.Output.Score {
                    UPI = existingPharmacy.UPI,
                    CustomerSatisfaction = existingPharmacy.Score.CustomerSatisfaction,
                    RankAmongPharmacies = existingPharmacy.Score.RankAmongPharmacies,
                    PackingAverageTimeInSeconds = existingPharmacy.Score.PackingAverageTimeInSeconds
                };
            }
            catch
            {
                throw;
            }
        }

        public ResponseTemplate GetServerState()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                return new ResponseTemplate {
                    Success = true
                }; // Otherwise the connection is not established and 'Success' cannot be reached by the client. ('false' is meaningless)
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Settle> GetSettles()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var existingPharmacy = db.Pharmacies.Where(q => q.PharmacyId == GetPharmacyId()).FirstOrDefault();
                db.Entry(existingPharmacy).Collection(p => p.Settles).Load();

                var settles =
                    from settle in existingPharmacy.Settles
                    select new Models.Minimals.Output.Settle { USI = settle.USI, CommissionCoefficient = settle.CommissionCoefficient, NumberOfOrders = settle.NumberOfOrders, Date = settle.Date, HasBeenSettled = settle.HasBeenSettled, Credit = settle.Credit };
                
                if(existingPharmacy.Settles != null) return settles;
                throw new NoInformationException(ErrorCodes.NoInformationExceptionMsg);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Score> GetTopFivePharmacies()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
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
                throw;
            }
        }

        public IEnumerable<float> NumberOfOrdersInThisWeek()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                float[] weekFloats = new float[8];
                var existingPharmacy = db.Pharmacies.Where(q => q.PharmacyId == GetPharmacyId()).FirstOrDefault();
                db.Entry(existingPharmacy).Collection(p => p.Orders).Load();

                var orders =
                    from order in existingPharmacy.Orders
                    orderby order.Date
                    where ( (order.Date >= FirstAndLastWeekDayOfTheDay.ReturnFirstAndLastDate().First())
                            && (order.Date <= FirstAndLastWeekDayOfTheDay.ReturnFirstAndLastDate().Last()) )
                    select order.Date;
                if(orders != null)
                {
                    var s0 =
                        from record in orders
                        where record.Date.DayOfWeek == DayOfWeek.Saturday
                        select record;
                    var s1 =
                        from record in orders
                        where record.Date.DayOfWeek == DayOfWeek.Sunday
                        select record;
                    var s2 =
                        from record in orders
                        where record.Date.DayOfWeek == DayOfWeek.Monday
                        select record;
                    var s3 =
                        from record in orders
                        where record.Date.DayOfWeek == DayOfWeek.Tuesday
                        select record;
                    var s4 =
                        from record in orders
                        where record.Date.DayOfWeek == DayOfWeek.Wednesday
                        select record;
                    var s5 =
                        from record in orders
                        where record.Date.DayOfWeek == DayOfWeek.Thursday
                        select record;
                    var s6 =
                        from record in orders
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

                    weekFloats[7] = orders.Count();

                    // satsfied
                    return weekFloats;
                }

                throw new NoInformationException(ErrorCodes.NoInformationExceptionMsg);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<float> AverageTimeOfPackingInThisWeek()
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var existingPharmacy = db.Pharmacies.Where(q => q.PharmacyId == GetPharmacyId()).FirstOrDefault();
                db.Entry(existingPharmacy).Collection(p => p.Orders).Load();

                float[] weekFloats = new float[8];
                var orders =
                    from order in existingPharmacy.Orders
                    orderby order.Date
                    where ( (order.Date >= FirstAndLastWeekDayOfTheDay.ReturnFirstAndLastDate().First())
                            && (order.Date <= FirstAndLastWeekDayOfTheDay.ReturnFirstAndLastDate().Last()) )
                    select new { order.Date, order.PackingTime };
                if(orders != null)
                {
                    var s0 =
                        from record in orders
                        where record.Date.DayOfWeek == DayOfWeek.Saturday
                        select record.PackingTime;
                    var s1 =
                        from record in orders
                        where record.Date.DayOfWeek == DayOfWeek.Sunday
                        select record.PackingTime;
                    var s2 =
                        from record in orders
                        where record.Date.DayOfWeek == DayOfWeek.Monday
                        select record.PackingTime;
                    var s3 =
                        from record in orders
                        where record.Date.DayOfWeek == DayOfWeek.Tuesday
                        select record.PackingTime;
                    var s4 =
                        from record in orders
                        where record.Date.DayOfWeek == DayOfWeek.Wednesday
                        select record.PackingTime;
                    var s5 =
                        from record in orders
                        where record.Date.DayOfWeek == DayOfWeek.Thursday
                        select record.PackingTime;
                    var s6 =
                        from record in orders
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

                    int count = orders.Count();
                    float i = 0;
                    float average = 0;
                    foreach (var response in orders)
                    {
                        i = (float) response.PackingTime / count;
                        average += i;
                        i = 0;
                    }
                    weekFloats[7] = average;

                    // return to client
                    return weekFloats;
                }

                throw new NoInformationException(ErrorCodes.NoInformationExceptionMsg);
            }
            catch
            {
                throw;
            }
        }

        public ResponseTemplate SetANewSettle(Models.Minimals.Input.Settle settle)
        {
            try
            {
                TokenValidationHandler(); // REQUIRED for token protected requests in advance, NOT REQUIRED for non-protected requests
                var response = db.Pharmacies.Where(q => q.PharmacyId == GetPharmacyId()).FirstOrDefault();
                
                var newSettle = new Models.Settle {
                        // USI = "123234", // TODO: USI generator
                        CommissionCoefficient = 23.3, // TODO: CommissionCoefficient calculator
                        NumberOfOrders = settle.NumberOfOrders,
                        Date = DateTime.Now,
                        Pharmacy = response,
                        HasBeenSettled = false,
                        Credit = settle.Credit
                    };
                db.Settles.Add(newSettle);

                db.SaveChanges();

                return new ResponseTemplate {
                    Success = true,
                    Error = newSettle.SettleId.ToString()
                };
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public void TokenValidationHandler()
        {
            // agar token dar db bud -> invalid ya expired
            // agar token dar db nabud -> pharmacy vojud nadarad
            // ------
            // invalid YA namojud (null) --> unauthorized exception midahad
            // expired --> token expired exception midahad
            // ------
            // pas digar sharte null budane pharmacy dar edame code fayde nadarad va az ghabl hame chiz malum hast
            try
            {
                var pharmacy = db.PharmacyTokens.Where(ct => ct.Token == RequestToken).FirstOrDefault();
                if(pharmacy == null || pharmacy.IsValid == false) throw new UnauthorizedAccessException();
                if(DateTime.UtcNow > pharmacy.ValidTo) throw new SecurityTokenExpiredException(ErrorCodes.SecurityTokenExpiredExceptionMsg);
            }
            catch 
            {
                throw new APIUnhandledException(ErrorCodes.APIUnhandledExceptionMsg);
            }
        }
    }
}