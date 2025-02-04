using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoskheAPI_Beta.Models;
using NoskheAPI_Beta.Models.Response;

namespace NoskheAPI_Beta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbController : ControllerBase
    {
        private static NoskheContext db = new NoskheContext();
        // GET api/values
        [HttpGet("init")]
        public ActionResult<string> init(string name)
        {
            var a = $"CustomerInit(): {CustomerInit()}";
            var b = $"ManagerInit(): {ManagerInit()}";
            var c = $"PharmacyInit(): {PharmacyInit()}";
            var d = $"CourierInit(): {CourierInit()}";
            var e = $"MedicineInit(): {MedicineInit()}";
            var f = $"CosmeticInit(): {CosmeticInit()}";
            var g = $"ShoppingCartInit(): {ShoppingCartInit()}";
            var h = $"NotationInit(): {NotationInit()}";
            var i = $"PrescriptionInit(): {PrescriptionInit()}";
            var j = $"PrescriptionItemInit(): {PrescriptionItemInit()}";
            var k = $"OrderInit(): {OrderInit()}";
            var n = $"ScoreInit(): {ScoreInit()}";
            var o = $"OccurrenceInit(): {OccurrenceInit()}";
            var p = $"AccountInit(): {AccountInit()}";
            var q = $"SettleInit(): {SettleInit()}";
            var r = $"TextMessageInit(): {TextMessageInit()}";
            var l = $"MedicineShoppingCartInit(): {MedicineShoppingCartInit()}";
            var m = $"CosmeticShoppingCartInit(): {CosmeticShoppingCartInit()}";
            return $"{a}\n{b}\n{c}\n{d}\n{e}\n{f}\n{g}\n{h}\n{i}\n{j}\n{k}\n{l}\n{m}\n{n}\n{o}\n{p}\n{q}\n{r}";
        }


        private bool CustomerInit()
        {
            try
            {
                if(db.Customers.Count() == 0)
                {
                    db.Customers.AddRange(
                        new Customer {
                            FirstName = "علی",
                            LastName = "ضیایی",
                            RegisterationDate = DateTime.Now,
                            Gender = Gender.Male,
                            Birthday = DateTime.Now,
                            Password = "test",
                            Email = "ali@yahoo.com",
                            Phone = "CustomerPh1",
                            ProfilePictureUrl = "CustomerPP1.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            Money = 0,
                            IsPhoneValidated = true
                        },
                        new Customer {
                            FirstName = "اشکان",
                            LastName = "محسنی",
                            RegisterationDate = DateTime.Now,
                            Gender = Gender.Male,
                            Birthday = DateTime.Now,
                            Password = "test",
                            Email = "ashkan@yahoo.com",
                            Phone = "CustomerPh2",
                            ProfilePictureUrl = "CustomerPP2.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            Money = 3500,
                            IsPhoneValidated = true
                        },
                        new Customer {
                            FirstName = "پدرام",
                            LastName = "اصغری",
                            RegisterationDate = DateTime.Now,
                            Gender = Gender.Male,
                            Birthday = DateTime.Now,
                            Password = "test",
                            Email = "pedram@yahoo.com",
                            Phone = "CustomerPh3",
                            ProfilePictureUrl = "CustomerPP3.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            Money = 4000,
                            IsPhoneValidated = true
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool ManagerInit()
        {
            try
            {
                if(db.Managers.Count() == 0)
                {
                    db.Managers.AddRange(
                        new Manager {
                            FirstName = "ManagerF_1",
                            LastName = "ManagerL_1",
                            NameOfFather = "NOF_1",
                            MelliNumber = "012345_1",
                            Gender = Gender.Male,
                            Password = "ManagerP_1",
                            Email = "ManagerE@1.com",
                            Phone = "ManagerPh_1",
                            ProfilePictureUrl = "ManagerPP_1.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                        },
                        new Manager {
                            FirstName = "ManagerF_2",
                            LastName = "ManagerL_2",
                            NameOfFather = "NOF_2",
                            MelliNumber = "012345_2",
                            Gender = Gender.Male,
                            Password = "ManagerP_2",
                            Email = "ManagerE@2.com",
                            Phone = "ManagerPh_2",
                            ProfilePictureUrl = "ManagerPP_2.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                        },
                        new Manager {
                            FirstName = "ManagerF_3",
                            LastName = "ManagerL_3",
                            NameOfFather = "NOF_3",
                            MelliNumber = "012345_3",
                            Gender = Gender.Male,
                            Password = "ManagerP_3",
                            Email = "ManagerE@3.com",
                            Phone = "ManagerPh_3",
                            ProfilePictureUrl = "ManagerPP_3.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private object PharmacyInit()
        {
            try
            {
                if(db.Pharmacies.Count() == 0)
                {
                    db.Pharmacies.AddRange(
                        new Pharmacy {
                            Name = "PharmacyN_1",
                            UPI = "PharmacyUPI_1",
                            RegisterationDate = DateTime.Now,
                            Password = "PharmacyP_1",
                            Email = "PharmacyE@1.com",
                            Phone = "PharmacyPh_1",
                            ProfilePictureUrl = "PharmacyPP_1.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            AddressLongitude = 28.21,
                            AddressLatitude = 28.29,
                            Address = "PharmacyA_1",
                            IsAvailableNow = false,
                            Manager = db.Managers.Where(s => s.Email == "ManagerE@1.com").FirstOrDefault(),
                            Credit = 23.57M
                        },
                        new Pharmacy {
                            Name = "PharmacyN_2",
                            UPI = "PharmacyUPI_2",
                            RegisterationDate = DateTime.Now,
                            Password = "PharmacyP_2",
                            Email = "PharmacyE@2.com",
                            Phone = "PharmacyPh_2",
                            ProfilePictureUrl = "PharmacyPP_2.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            AddressLongitude = 27.21,
                            AddressLatitude = 27.29,
                            Address = "PharmacyA_2",
                            IsAvailableNow = false,
                            Manager = db.Managers.Where(s => s.Email == "ManagerE@2.com").FirstOrDefault(),
                            Credit = 21.57M
                        },
                        new Pharmacy {
                            Name = "PharmacyN_3",
                            UPI = "PharmacyUPI_3",
                            RegisterationDate = DateTime.Now,
                            Password = "PharmacyP_3",
                            Email = "PharmacyE@3.com",
                            Phone = "PharmacyPh_3",
                            ProfilePictureUrl = "PharmacyPP_3.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            AddressLongitude = 26.21,
                            AddressLatitude = 26.29,
                            Address = "PharmacyA_3",
                            IsAvailableNow = false,
                            Manager = db.Managers.Where(s => s.Email == "ManagerE@3.com").FirstOrDefault(),
                            Credit = 22.57M
                        },
                        new Pharmacy {
                            Name = "PharmacyN_4",
                            UPI = "PharmacyUPI_4",
                            RegisterationDate = DateTime.Now,
                            Password = "PharmacyP_4",
                            Email = "PharmacyE@4.com",
                            Phone = "PharmacyPh_4",
                            ProfilePictureUrl = "PharmacyPP_4.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            AddressLongitude = 28.21,
                            AddressLatitude = 28.29,
                            Address = "PharmacyA_4",
                            IsAvailableNow = false,
                            Manager = db.Managers.Where(s => s.Email == "ManagerE@1.com").FirstOrDefault(),
                            Credit = 25.57M
                        },
                        new Pharmacy {
                            Name = "PharmacyN_5",
                            UPI = "PharmacyUPI_5",
                            RegisterationDate = DateTime.Now,
                            Password = "PharmacyP_5",
                            Email = "PharmacyE@5.com",
                            Phone = "PharmacyPh_5",
                            ProfilePictureUrl = "PharmacyPP_5.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            AddressLongitude = 27.21,
                            AddressLatitude = 27.29,
                            Address = "PharmacyA_5",
                            IsAvailableNow = false,
                            Manager = db.Managers.Where(s => s.Email == "ManagerE@2.com").FirstOrDefault(),
                            Credit = 26.57M
                        },
                        new Pharmacy {
                            Name = "PharmacyN_6",
                            UPI = "PharmacyUPI_6",
                            RegisterationDate = DateTime.Now,
                            Password = "PharmacyP_6",
                            Email = "PharmacyE@6.com",
                            Phone = "PharmacyPh_6",
                            ProfilePictureUrl = "PharmacyPP_6.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            AddressLongitude = 26.21,
                            AddressLatitude = 26.29,
                            Address = "PharmacyA_6",
                            IsAvailableNow = false,
                            Manager = db.Managers.Where(s => s.Email == "ManagerE@3.com").FirstOrDefault(),
                            Credit = 28.57M
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool CourierInit()
        {
            try
            {
                if(db.Couriers.Count() == 0)
                {
                    db.Couriers.AddRange(
                        new Courier {
                            FirstName = "CourierF_1",
                            LastName = "CourierL_1",
                            RegisterationDate = DateTime.Now,
                            NameOfFather = "NOF_1",
                            Gender = Gender.Male,
                            Birthday = DateTime.Now,
                            MelliNumber = "01234_1",
                            Password = "CourierP_1",
                            Email = "CourierE@1.com",
                            Phone = "CourierPh_1",
                            ProfilePictureUrl = "CourierPP_1.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            IsAvailableNow = true
                        },
                        new Courier {
                            FirstName = "CourierF_2",
                            LastName = "CourierL_2",
                            RegisterationDate = DateTime.Now,
                            NameOfFather = "NOF_2",
                            Gender = Gender.Male,
                            Birthday = DateTime.Now,
                            MelliNumber = "01234_2",
                            Password = "CourierP_2",
                            Email = "CourierE@2.com",
                            Phone = "CourierPh_2",
                            ProfilePictureUrl = "CourierPP_2.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            IsAvailableNow = true
                        },
                        new Courier {
                            FirstName = "CourierF_3",
                            LastName = "CourierL_3",
                            RegisterationDate = DateTime.Now,
                            NameOfFather = "NOF_3",
                            Gender = Gender.Male,
                            Birthday = DateTime.Now,
                            MelliNumber = "01234_3",
                            Password = "CourierP_3",
                            Email = "CourierE@3.com",
                            Phone = "CourierPh_3",
                            ProfilePictureUrl = "CourierPP_3.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            IsAvailableNow = true
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool MedicineInit()
        {
            try
            {
                if(db.Medicines.Count() == 0)
                {
                    db.Medicines.AddRange(
                        new Medicine {
                            Name = "Medicine_1",
                            Price = 700,
                            ProductPictureUrl = "Medicine_1.jpg",
                            ProductPictureUploadDate = DateTime.Now
                        },
                        new Medicine {
                            Name = "Medicine_2",
                            Price = 800,
                            ProductPictureUrl = "Medicine_2.jpg",
                            ProductPictureUploadDate = DateTime.Now
                        },
                        new Medicine {
                            Name = "Medicine_3",
                            Price = 23300,
                            ProductPictureUrl = "Medicine_3.jpg",
                            ProductPictureUploadDate = DateTime.Now
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool CosmeticInit()
        {
            try
            {
                if(db.Cosmetics.Count() == 0)
                {
                    db.Cosmetics.AddRange(
                        new Cosmetic {
                            Name = "Cosmetic_1",
                            Price = 500,
                            ProductPictureUrl = "test1.jpg",
                            ProductPictureUploadDate = DateTime.Now
                        },
                        new Cosmetic {
                            Name = "Cosmetic_2",
                            Price = 400,
                            ProductPictureUrl = "test2.jpg",
                            ProductPictureUploadDate = DateTime.Now
                        },
                        new Cosmetic {
                            Name = "Cosmetic_3",
                            Price = 600,
                            ProductPictureUrl = "test3.jpg",
                            ProductPictureUploadDate = DateTime.Now
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool NotationInit()
        {
            try
            {
                if(db.Notations.Count() == 0)
                {
                    db.Notations.AddRange(
                        new Notation {
                            BrandPreference = BrandType.Global,
                            HasPregnancy = true,
                            HasOtherDiseases = true,
                            Description = "NotationD_1",
                            ShoppingCartId = 1
                        },
                        new Notation {
                            BrandPreference = BrandType.Global,
                            HasPregnancy = false,
                            HasOtherDiseases = true,
                            Description = "NotationD_2",
                            ShoppingCartId = 2
                        },
                        new Notation {
                            BrandPreference = BrandType.Global,
                            HasPregnancy = false,
                            HasOtherDiseases = false,
                            Description = "NotationD_3",
                            ShoppingCartId = 3
                        },
                        new Notation {
                            BrandPreference = BrandType.Local,
                            HasPregnancy = true,
                            HasOtherDiseases = true,
                            Description = "NotationD_4",
                            ShoppingCartId = 4
                        },
                        new Notation {
                            BrandPreference = BrandType.Global,
                            HasPregnancy = false,
                            HasOtherDiseases = false,
                            Description = "NotationD_5",
                            ShoppingCartId = 3
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool ShoppingCartInit()
        {
            try
            {
                if(db.ShoppingCarts.Count() == 0)
                {
                    db.ShoppingCarts.AddRange(
                        new ShoppingCart {
                            USCI = "ShoppingCartUSCI_001",
                            Date = DateTime.Now,
                            AddressLongitude = 23.23,
                            AddressLatitude = 23.12,
                            Address = "ShoppingCartA_1",
                            HasBeenRequested = true,
                            CustomerId = 1
                        },
                        new ShoppingCart {
                            USCI = "ShoppingCartUSCI_002",
                            Date = DateTime.Now,
                            AddressLongitude = 21.23,
                            AddressLatitude = 21.12,
                            Address = "ShoppingCartA_2",
                            HasBeenRequested = true,
                            CustomerId = 2
                        },
                        new ShoppingCart {
                            USCI = "ShoppingCartUSCI_003",
                            Date = DateTime.Now,
                            AddressLongitude = 20.23,
                            AddressLatitude = 20.12,
                            Address = "ShoppingCartA_3",
                            HasBeenRequested = true,
                            CustomerId = 3
                        },
                        new ShoppingCart {
                            USCI = "ShoppingCartUSCI_004",
                            Date = DateTime.Now,
                            AddressLongitude = 19.23,
                            AddressLatitude = 19.12,
                            Address = "ShoppingCartA_4",
                            HasBeenRequested = true,
                            CustomerId = 3
                        },
                        new ShoppingCart {
                            USCI = "ShoppingCartUSCI_005",
                            Date = DateTime.Now,
                            AddressLongitude = 18.23,
                            AddressLatitude = 18.12,
                            Address = "ShoppingCartA_5",
                            HasBeenRequested = true,
                            CustomerId = 3
                        },
                        new ShoppingCart {
                            USCI = "ShoppingCartUSCI_006",
                            Date = DateTime.Now,
                            AddressLongitude = 17.23,
                            AddressLatitude = 17.12,
                            Address = "ShoppingCartA_6",
                            HasBeenRequested = true,
                            CustomerId = 2
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool PrescriptionInit()
        {
            try
            {
                if(db.Prescriptions.Count() == 0)
                {
                    db.Prescriptions.AddRange(
                        new Prescription {
                            HasBeenAcceptedByPharmacy = true,
                            PictureUrl_1 = "PrescriptionP_1.jpg",
                            PicturesUploadDate = DateTime.Now,
                            ShoppingCartId = 1
                        },
                        new Prescription {
                            HasBeenAcceptedByPharmacy = false,
                            PictureUrl_1 = "PrescriptionP_2.jpg",
                            PicturesUploadDate = DateTime.Now,
                            ShoppingCartId = 2
                        },
                        new Prescription {
                            HasBeenAcceptedByPharmacy = true,
                            PictureUrl_1 = "PrescriptionP_3.jpg",
                            PicturesUploadDate = DateTime.Now,
                            ShoppingCartId = 3
                        },
                        new Prescription {
                            HasBeenAcceptedByPharmacy = false,
                            PictureUrl_1 = "PrescriptionP_4.jpg",
                            PicturesUploadDate = DateTime.Now,
                            ShoppingCartId = 4
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool PrescriptionItemInit()
        {
            try
            {
                if(db.PrescriptionItems.Count() == 0)
                {
                    db.PrescriptionItems.AddRange(
                        new PrescriptionItem {
                            Name = "PrescriptionItem_1",
                            Quantity = 4,
                            Price = 45000,
                            PrescriptionId = 1
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_2",
                            Quantity = 1,
                            Price = 2300,
                            PrescriptionId = 2
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_3",
                            Quantity = 3,
                            Price = 5000,
                            PrescriptionId = 3
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_4",
                            Quantity = 2,
                            Price = 950,
                            PrescriptionId = 4
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_5",
                            Quantity = 1,
                            Price = 80400,
                            PrescriptionId = 1
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_6",
                            Quantity = 5,
                            Price = 93200,
                            PrescriptionId = 2
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_7",
                            Quantity = 3,
                            Price = 11500,
                            PrescriptionId = 3
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_8",
                            Quantity = 4,
                            Price = 1400,
                            PrescriptionId = 4
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_9",
                            Quantity = 2,
                            Price = 11350,
                            PrescriptionId = 1
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool OrderInit()
        {
            try
            {
                if(db.Orders.Count() == 0)
                {
                    db.Orders.AddRange(
                        new Order {
                            UOI = "OrderUOI_1",
                            Date = DateTime.Now,
                            Price = 232,
                            HasPrescription = true,
                            HasBeenSetToACourier = true,
                            HasBeenAcceptedByCustomer = true,
                            HasBeenReceivedByCustomer = true,
                            HasBeenDeliveredToCustomer = true,
                            HasBeenSettled = false,
                            HasBeenPaid = true,
                            SettlementDate = DateTime.Now,
                            ShoppingCartId = 1,
                            CourierId = 1,
                            PharmacyId = 1
                        },
                        new Order {
                            UOI = "OrderUOI_2",
                            Date = DateTime.Now,
                            Price = 233,
                            HasPrescription = true,
                            HasBeenSetToACourier = true,
                            HasBeenAcceptedByCustomer = true,
                            HasBeenReceivedByCustomer = true,
                            HasBeenDeliveredToCustomer = true,
                            HasBeenSettled = false,
                            HasBeenPaid = true,
                            SettlementDate = DateTime.Now,
                            ShoppingCartId = 2,
                            CourierId = 2,
                            PharmacyId = 2
                        },
                        new Order {
                            UOI = "OrderUOI_3",
                            Date = DateTime.Now,
                            Price = 234,
                            HasPrescription = true,
                            HasBeenSetToACourier = true,
                            HasBeenAcceptedByCustomer = true,
                            HasBeenReceivedByCustomer = true,
                            HasBeenDeliveredToCustomer = true,
                            HasBeenSettled = false,
                            HasBeenPaid = false,
                            SettlementDate = DateTime.Now,
                            ShoppingCartId = 3,
                            CourierId = 3,
                            PharmacyId = 3
                        },
                        new Order {
                            UOI = "OrderUOI_4",
                            Date = DateTime.Now,
                            Price = 235,
                            HasPrescription = true,
                            HasBeenSetToACourier = true,
                            HasBeenAcceptedByCustomer = true,
                            HasBeenReceivedByCustomer = true,
                            HasBeenDeliveredToCustomer = true,
                            HasBeenSettled = false,
                            HasBeenPaid = false,
                            SettlementDate = DateTime.Now,
                            ShoppingCartId = 1,
                            CourierId = 1,
                            PharmacyId = 1
                        },
                        new Order {
                            UOI = "OrderUOI_5",
                            Date = DateTime.Now,
                            Price = 236,
                            HasPrescription = true,
                            HasBeenSetToACourier = true,
                            HasBeenAcceptedByCustomer = true,
                            HasBeenReceivedByCustomer = true,
                            HasBeenDeliveredToCustomer = true,
                            HasBeenSettled = true,
                            HasBeenPaid = true,
                            SettlementDate = DateTime.Now,
                            ShoppingCartId = 2,
                            CourierId = 2,
                            PharmacyId = 2
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool MedicineShoppingCartInit()
        {
            try
            {
                if(db.MedicineShoppingCarts.Count() == 0)
                {
                    db.MedicineShoppingCarts.AddRange(
                        new MedicineShoppingCart {
                            ShoppingCartId = 1,
                            MedicineId = 2,
                            Quantity = 1
                        },
                        new MedicineShoppingCart {
                            ShoppingCartId = 2,
                            MedicineId = 2,
                            Quantity = 2
                        },
                        new MedicineShoppingCart {
                            ShoppingCartId = 3,
                            MedicineId = 2,
                            Quantity = 3
                        },
                        new MedicineShoppingCart {
                            ShoppingCartId = 2,
                            MedicineId = 1,
                            Quantity = 4
                        },
                        new MedicineShoppingCart {
                            ShoppingCartId = 3,
                            MedicineId = 3,
                            Quantity = 3
                        },
                        new MedicineShoppingCart {
                            ShoppingCartId = 2,
                            MedicineId = 3,
                            Quantity = 2
                        },
                        new MedicineShoppingCart {
                            ShoppingCartId = 1,
                            MedicineId = 1,
                            Quantity = 1
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool CosmeticShoppingCartInit()
        {
            try
            {
                if(db.CosmeticShoppingCarts.Count() == 0)
                {
                    db.CosmeticShoppingCarts.AddRange(
                        new CosmeticShoppingCart {
                            ShoppingCartId = 3,
                            CosmeticId = 3,
                            Quantity = 1
                        },
                        new CosmeticShoppingCart {
                            ShoppingCartId = 1,
                            CosmeticId = 3,
                            Quantity = 2
                        },
                        new CosmeticShoppingCart {
                            ShoppingCartId = 3,
                            CosmeticId = 1,
                            Quantity = 3
                        },
                        new CosmeticShoppingCart {
                            ShoppingCartId = 2,
                            CosmeticId = 3,
                            Quantity = 4
                        },
                        new CosmeticShoppingCart {
                            ShoppingCartId = 3,
                            CosmeticId = 2,
                            Quantity = 4
                        },
                        new CosmeticShoppingCart {
                            ShoppingCartId = 1,
                            CosmeticId = 2,
                            Quantity = 3
                        },
                        new CosmeticShoppingCart {
                            ShoppingCartId = 2,
                            CosmeticId = 1,
                            Quantity = 2
                        },
                        new CosmeticShoppingCart {
                            ShoppingCartId = 2,
                            CosmeticId = 2,
                            Quantity = 1
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool ScoreInit()
        {
            try
            {
                if(db.Scores.Count() == 0)
                {
                    db.Scores.AddRange(
                        new Score {
                            CustomerSatisfaction = 1,
                            PackingAverageTimeInSeconds = 14,
                            RankAmongPharmacies = 1,
                            PharmacyId = 1
                        },
                        new Score {
                            CustomerSatisfaction = 2,
                            PackingAverageTimeInSeconds = 13,
                            RankAmongPharmacies = 2,
                            PharmacyId = 2
                        },
                        new Score {
                            CustomerSatisfaction = 5,
                            PackingAverageTimeInSeconds = 12,
                            RankAmongPharmacies = 3,
                            PharmacyId = 3
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        
        private bool OccurrenceInit()
        {
            try
            {
                if(db.Occurrences.Count() == 0)
                {
                    db.Occurrences.AddRange(
                        new Occurrence {
                            CustomerHasRequestedOrderOn = DateTime.Now,
                            OrderId = 1
                        },
                        new Occurrence {
                            CustomerHasRequestedOrderOn = DateTime.Now,
                            OrderId = 2
                        },
                        new Occurrence {
                            CustomerHasRequestedOrderOn = DateTime.Now,
                            OrderId = 3
                        },
                        new Occurrence {
                            CustomerHasRequestedOrderOn = DateTime.Now,
                            OrderId = 3
                        },
                        new Occurrence {
                            CustomerHasRequestedOrderOn = DateTime.Now,
                            OrderId = 1
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool AccountInit()
        {
            try
            {
                if(db.Accounts.Count() == 0)
                {
                    db.Accounts.AddRange(
                        new Account {
                            IBAN = "IBAN_1",
                            AccountOwnerName = "AccountOwnerName_1",
                            BankName = "BankName_1",
                            PharmacyId = 1
                        },
                        new Account {
                            IBAN = "IBAN_2",
                            AccountOwnerName = "AccountOwnerName_2",
                            BankName = "BankName_2",
                            PharmacyId = 2
                        },
                        new Account {
                            IBAN = "IBAN_3",
                            AccountOwnerName = "AccountOwnerName_3",
                            BankName = "BankName_3",
                            PharmacyId = 3
                        },
                        new Account {
                            IBAN = "IBAN_4",
                            AccountOwnerName = "AccountOwnerName_4",
                            BankName = "BankName_4",
                            PharmacyId = 3
                        },
                        new Account {
                            IBAN = "IBAN_5",
                            AccountOwnerName = "AccountOwnerName_5",
                            BankName = "BankName_5",
                            PharmacyId = 1
                        },
                        new Account {
                            IBAN = "IBAN_6",
                            AccountOwnerName = "AccountOwnerName_6",
                            BankName = "BankName_6",
                            PharmacyId = 3
                        }
                    );
                    
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool SettleInit()
        {
            try
            {
                if(db.Settles.Count() == 0)
                {
                    db.Settles.AddRange(
                        new Settle {
                            USI = "SettleUSI_1",
                            CommissionCoefficient = 0.04,
                            Date = DateTime.Now,
                            NumberOfOrders = 42,
                            PharmacyId = 2,
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_2",
                            CommissionCoefficient = 0.05,
                            Date = DateTime.Now,
                            NumberOfOrders = 12,
                            PharmacyId = 1,
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_3",
                            CommissionCoefficient = 0.06,
                            Date = DateTime.Now,
                            NumberOfOrders = 23,
                            PharmacyId = 3,
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_4",
                            CommissionCoefficient = 0.07,
                            Date = DateTime.Now,
                            NumberOfOrders = 62,
                            PharmacyId = 2,
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_5",
                            CommissionCoefficient = 0.08,
                            Date = DateTime.Now,
                            NumberOfOrders = 21,
                            PharmacyId = 3,
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_6",
                            CommissionCoefficient = 0.09,
                            Date = DateTime.Now,
                            NumberOfOrders = 30,
                            PharmacyId = 1,
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_7",
                            CommissionCoefficient = 0.1,
                            Date = DateTime.Now,
                            NumberOfOrders = 16,
                            PharmacyId = 3,
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_8",
                            CommissionCoefficient = 0.11,
                            Date = DateTime.Now,
                            NumberOfOrders = 19,
                            PharmacyId = 2,
                            HasBeenSettled = false
                        }
                    );
                    
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool TextMessageInit()
        {
            try
            {
                if(db.CustomerTextMessages.Count() == 0)
                {
                    db.CustomerTextMessages.AddRange(
                        new CustomerTextMessage {
                            Date = DateTime.Now,
                            CustomerId = 1,
                            Message = "VerificationCode_1"
                        },
                        new CustomerTextMessage {
                            Date = DateTime.Now,
                            CustomerId = 2,
                            Message = "VerificationCode_2"
                        },
                        new CustomerTextMessage {
                            Date = DateTime.Now,
                            CustomerId = 3,
                            Message = "VerificationCode_3"
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}