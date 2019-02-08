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
        public ActionResult<IEnumerable<string>> init(string name)
        {
            return new List<string> {
                $"CustomerInit(): {CustomerInit()}",
                $"CourierInit(): {CourierInit()}",
                $"MedicineInit(): {MedicineInit()}",
                $"CosmeticInit(): {CosmeticInit()}",
                $"ManagerInit(): {ManagerInit()}",
                $"PharmacyInit(): {PharmacyInit()}",
                $"ShoppingCartInit(): {ShoppingCartInit()}",
                $"NotationInit(): {NotationInit()}",
                $"PrescriptionInit(): {PrescriptionInit()}",
                $"PrescriptionItemInit(): {PrescriptionItemInit()}",
                $"OrderInit(): {OrderInit()}",
                $"MedicineShoppingCartInit(): {MedicineShoppingCartInit()}",
                $"CosmeticShoppingCartInit(): {CosmeticShoppingCartInit()}",
                $"ScoreInit(): {ScoreInit()}",
                $"OccurrenceInit(): {OccurrenceInit()}",
                $"AccountInit(): {AccountInit()}",
                $"SettleInit(): {SettleInit()}",         
                $"TextMessageInit(): {TextMessageInit()}"
            };
        }


        private bool CustomerInit()
        {
            try
            {
                if(db.Customers.Count() == 0)
                {
                    db.Customers.AddRange(
                        new Customer {
                            FirstName = "CustomerF1",
                            LastName = "CustomerL1",
                            RegisterationDate = DateTime.Now,
                            Gender = Gender.Male,
                            Birthday = DateTime.Now,
                            Password = "CustomerP1",
                            Email = "CustomerE@1.com",
                            Phone = "CustomerPh1",
                            ProfilePictureUrl = "CustomerPP1.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            Money = 3.45M
                        },
                        new Customer {
                            FirstName = "CustomerF2",
                            LastName = "CustomerL2",
                            RegisterationDate = DateTime.Now,
                            Gender = Gender.Male,
                            Birthday = DateTime.Now,
                            Password = "CustomerP2",
                            Email = "CustomerE@2.com",
                            Phone = "CustomerPh2",
                            ProfilePictureUrl = "CustomerPP2.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            Money = 4.45M
                        },
                        new Customer {
                            FirstName = "CustomerF3",
                            LastName = "CustomerL3",
                            RegisterationDate = DateTime.Now,
                            Gender = Gender.Male,
                            Birthday = DateTime.Now,
                            Password = "CustomerP3",
                            Email = "CustomerE@3.com",
                            Phone = "CustomerPh3",
                            ProfilePictureUrl = "CustomerPP3.jpg",
                            ProfilePictureUploadDate = DateTime.Now,
                            Money = 5.45M
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
                            IsAvailableNow = true,
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
                            IsAvailableNow = true,
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
                            IsAvailableNow = true,
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
                            IsAvailableNow = true,
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
                            IsAvailableNow = true,
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
                            IsAvailableNow = true,
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
            return false;
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
                            Price = 3.82M,
                            ProductPictureUrl = "Medicine_1.jpg",
                            ProductPictureUploadDate = DateTime.Now
                        },
                        new Medicine {
                            Name = "Medicine_2",
                            Price = 4.82M,
                            ProductPictureUrl = "Medicine_2.jpg",
                            ProductPictureUploadDate = DateTime.Now
                        },
                        new Medicine {
                            Name = "Medicine_3",
                            Price = 5.82M,
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
                            Price = 3.82M,
                            ProductPictureUrl = "test1.jpg",
                            ProductPictureUploadDate = DateTime.Now
                        },
                        new Cosmetic {
                            Name = "Cosmetic_2",
                            Price = 4.82M,
                            ProductPictureUrl = "test2.jpg",
                            ProductPictureUploadDate = DateTime.Now
                        },
                        new Cosmetic {
                            Name = "Cosmetic_3",
                            Price = 5.82M,
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
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_001").FirstOrDefault()
                        },
                        new Notation {
                            BrandPreference = BrandType.Global,
                            HasPregnancy = false,
                            HasOtherDiseases = true,
                            Description = "NotationD_2",
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_002").FirstOrDefault()
                        },
                        new Notation {
                            BrandPreference = BrandType.Global,
                            HasPregnancy = false,
                            HasOtherDiseases = false,
                            Description = "NotationD_3",
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_003").FirstOrDefault()
                        },
                        new Notation {
                            BrandPreference = BrandType.Local,
                            HasPregnancy = true,
                            HasOtherDiseases = true,
                            Description = "NotationD_4",
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_004").FirstOrDefault()
                        },
                        new Notation {
                            BrandPreference = BrandType.Global,
                            HasPregnancy = false,
                            HasOtherDiseases = false,
                            Description = "NotationD_5",
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_005").FirstOrDefault()
                        },
                        new Notation {
                            BrandPreference = BrandType.Local,
                            HasPregnancy = false,
                            HasOtherDiseases = false,
                            Description = "NotationD_6",
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_006").FirstOrDefault()
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
                            Customer = db.Customers.Where(s => s.Email == "CustomerE@1.com").FirstOrDefault()
                        },
                        new ShoppingCart {
                            USCI = "ShoppingCartUSCI_002",
                            Date = DateTime.Now,
                            AddressLongitude = 21.23,
                            AddressLatitude = 21.12,
                            Address = "ShoppingCartA_2",
                            HasBeenRequested = true,
                            Customer = db.Customers.Where(s => s.Email == "CustomerE@2.com").FirstOrDefault()
                        },
                        new ShoppingCart {
                            USCI = "ShoppingCartUSCI_003",
                            Date = DateTime.Now,
                            AddressLongitude = 20.23,
                            AddressLatitude = 20.12,
                            Address = "ShoppingCartA_3",
                            HasBeenRequested = true,
                            Customer = db.Customers.Where(s => s.Email == "CustomerE@1.com").FirstOrDefault()
                        },
                        new ShoppingCart {
                            USCI = "ShoppingCartUSCI_004",
                            Date = DateTime.Now,
                            AddressLongitude = 19.23,
                            AddressLatitude = 19.12,
                            Address = "ShoppingCartA_4",
                            HasBeenRequested = true,
                            Customer = db.Customers.Where(s => s.Email == "CustomerE@3.com").FirstOrDefault()
                        },
                        new ShoppingCart {
                            USCI = "ShoppingCartUSCI_005",
                            Date = DateTime.Now,
                            AddressLongitude = 18.23,
                            AddressLatitude = 18.12,
                            Address = "ShoppingCartA_5",
                            HasBeenRequested = true,
                            Customer = db.Customers.Where(s => s.Email == "CustomerE@2.com").FirstOrDefault()
                        },
                        new ShoppingCart {
                            USCI = "ShoppingCartUSCI_006",
                            Date = DateTime.Now,
                            AddressLongitude = 17.23,
                            AddressLatitude = 17.12,
                            Address = "ShoppingCartA_6",
                            HasBeenRequested = true,
                            Customer = db.Customers.Where(s => s.Email == "CustomerE@1.com").FirstOrDefault()
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
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_001").FirstOrDefault()
                        },
                        new Prescription {
                            HasBeenAcceptedByPharmacy = false,
                            PictureUrl_1 = "PrescriptionP_2.jpg",
                            PicturesUploadDate = DateTime.Now,
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_002").FirstOrDefault()
                        },
                        new Prescription {
                            HasBeenAcceptedByPharmacy = true,
                            PictureUrl_1 = "PrescriptionP_3.jpg",
                            PicturesUploadDate = DateTime.Now,
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_003").FirstOrDefault()
                        },
                        new Prescription {
                            HasBeenAcceptedByPharmacy = false,
                            PictureUrl_1 = "PrescriptionP_4.jpg",
                            PicturesUploadDate = DateTime.Now,
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_004").FirstOrDefault()
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
                            Price = 4.82M,
                            Prescription = db.Prescriptions.Where(s => s.PictureUrl_1 == "PrescriptionP_1.jpg").FirstOrDefault()
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_2",
                            Quantity = 1,
                            Price = 5.82M,
                            Prescription = db.Prescriptions.Where(s => s.PictureUrl_1 == "PrescriptionP_1.jpg").FirstOrDefault()
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_3",
                            Quantity = 3,
                            Price = 6.82M,
                            Prescription = db.Prescriptions.Where(s => s.PictureUrl_1 == "PrescriptionP_1.jpg").FirstOrDefault()
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_4",
                            Quantity = 2,
                            Price = 7.82M,
                            Prescription = db.Prescriptions.Where(s => s.PictureUrl_1 == "PrescriptionP_2.jpg").FirstOrDefault()
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_5",
                            Quantity = 1,
                            Price = 8.82M,
                            Prescription = db.Prescriptions.Where(s => s.PictureUrl_1 == "PrescriptionP_2.jpg").FirstOrDefault()
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_6",
                            Quantity = 5,
                            Price = 9.82M,
                            Prescription = db.Prescriptions.Where(s => s.ShoppingCart.Address == "ShoppingCartA_4").FirstOrDefault()
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_7",
                            Quantity = 3,
                            Price = 11.82M,
                            Prescription = db.Prescriptions.Where(s => s.ShoppingCart.Address == "ShoppingCartA_4").FirstOrDefault()
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_8",
                            Quantity = 4,
                            Price = 14.82M,
                            Prescription = db.Prescriptions.Where(s => s.ShoppingCart.Address == "ShoppingCartA_3").FirstOrDefault()
                        },
                        new PrescriptionItem {
                            Name = "PrescriptionItem_9",
                            Quantity = 2,
                            Price = 41.82M,
                            Prescription = db.Prescriptions.Where(s => s.ShoppingCart.Address == "ShoppingCartA_2").FirstOrDefault()
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
                            Price = 232.26M,
                            HasPrescription = true,
                            HasBeenSetToACourier = true,
                            HasBeenAcceptedByCustomer = true,
                            HasBeenReceivedByCustomer = true,
                            HasBeenDeliveredToCustomer = true,
                            HasBeenSettled = false,
                            HasBeenPaid = true,
                            SettlementDate = DateTime.Now,
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_001").FirstOrDefault(), // magar inke ye kode bezarim barash
                            Courier = db.Couriers.Where(s => s.Email == "CourierE@1.com").FirstOrDefault(),
                            Pharmacy = db.Pharmacies.Where(s => s.Email == "PharmacyE@1.com").FirstOrDefault()
                        },
                        new Order {
                            UOI = "OrderUOI_2",
                            Date = DateTime.Now,
                            Price = 233.26M,
                            HasPrescription = true,
                            HasBeenSetToACourier = true,
                            HasBeenAcceptedByCustomer = true,
                            HasBeenReceivedByCustomer = true,
                            HasBeenDeliveredToCustomer = true,
                            HasBeenSettled = false,
                            HasBeenPaid = true,
                            SettlementDate = DateTime.Now,
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_002").FirstOrDefault(), // magar inke ye kode bezarim barash
                            Courier = db.Couriers.Where(s => s.Email == "CourierE@2.com").FirstOrDefault(),
                            Pharmacy = db.Pharmacies.Where(s => s.Email == "PharmacyE@1.com").FirstOrDefault()
                        },
                        new Order {
                            UOI = "OrderUOI_3",
                            Date = DateTime.Now,
                            Price = 234.26M,
                            HasPrescription = true,
                            HasBeenSetToACourier = true,
                            HasBeenAcceptedByCustomer = true,
                            HasBeenReceivedByCustomer = true,
                            HasBeenDeliveredToCustomer = true,
                            HasBeenSettled = false,
                            HasBeenPaid = false,
                            SettlementDate = DateTime.Now,
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_003").FirstOrDefault(), // magar inke ye kode bezarim barash
                            Courier = db.Couriers.Where(s => s.Email == "CourierE@3.com").FirstOrDefault(),
                            Pharmacy = db.Pharmacies.Where(s => s.Email == "PharmacyE@2.com").FirstOrDefault()
                        },
                        new Order {
                            UOI = "OrderUOI_4",
                            Date = DateTime.Now,
                            Price = 235.26M,
                            HasPrescription = true,
                            HasBeenSetToACourier = true,
                            HasBeenAcceptedByCustomer = true,
                            HasBeenReceivedByCustomer = true,
                            HasBeenDeliveredToCustomer = true,
                            HasBeenSettled = false,
                            HasBeenPaid = false,
                            SettlementDate = DateTime.Now,
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_004").FirstOrDefault(), // magar inke ye kode bezarim barash
                            Courier = db.Couriers.Where(s => s.Email == "CourierE@3.com").FirstOrDefault(),
                            Pharmacy = db.Pharmacies.Where(s => s.Email == "PharmacyE@2.com").FirstOrDefault()
                        },
                        new Order {
                            UOI = "OrderUOI_5",
                            Date = DateTime.Now,
                            Price = 236.26M,
                            HasPrescription = true,
                            HasBeenSetToACourier = true,
                            HasBeenAcceptedByCustomer = true,
                            HasBeenReceivedByCustomer = true,
                            HasBeenDeliveredToCustomer = true,
                            HasBeenSettled = true,
                            HasBeenPaid = true,
                            SettlementDate = DateTime.Now,
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_005").FirstOrDefault(), // magar inke ye kode bezarim barash
                            Courier = db.Couriers.Where(s => s.Email == "CourierE@2.com").FirstOrDefault(),
                            Pharmacy = db.Pharmacies.Where(s => s.Email == "PharmacyE@3.com").FirstOrDefault()
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
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_001").FirstOrDefault(),
                            Medicine = db.Medicines.Where(s => s.Name == "Medicine_1").FirstOrDefault(),
                            Quantity = 3
                        },
                        new MedicineShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_002").FirstOrDefault(),
                            Medicine = db.Medicines.Where(s => s.Name == "Medicine_2").FirstOrDefault(),
                            Quantity = 3
                        },
                        new MedicineShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_002").FirstOrDefault(),
                            Medicine = db.Medicines.Where(s => s.Name == "Medicine_3").FirstOrDefault(),
                            Quantity = 3
                        },
                        new MedicineShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_003").FirstOrDefault(),
                            Medicine = db.Medicines.Where(s => s.Name == "Medicine_2").FirstOrDefault(),
                            Quantity = 3
                        },
                        new MedicineShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_003").FirstOrDefault(),
                            Medicine = db.Medicines.Where(s => s.Name == "Medicine_1").FirstOrDefault(),
                            Quantity = 3
                        },
                        new MedicineShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_003").FirstOrDefault(),
                            Medicine = db.Medicines.Where(s => s.Name == "Medicine_3").FirstOrDefault(),
                            Quantity = 3
                        },
                        new MedicineShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_004").FirstOrDefault(),
                            Medicine = db.Medicines.Where(s => s.Name == "Medicine_3").FirstOrDefault(),
                            Quantity = 3
                        },
                        new MedicineShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_006").FirstOrDefault(),
                            Medicine = db.Medicines.Where(s => s.Name == "Medicine_1").FirstOrDefault(),
                            Quantity = 3
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
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_001").FirstOrDefault(),
                            Cosmetic = db.Cosmetics.Where(s => s.Name == "Cosmetic_1").FirstOrDefault(),
                            Quantity = 4
                        },
                        new CosmeticShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_002").FirstOrDefault(),
                            Cosmetic = db.Cosmetics.Where(s => s.Name == "Cosmetic_2").FirstOrDefault(),
                            Quantity = 4
                        },
                        new CosmeticShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_002").FirstOrDefault(),
                            Cosmetic = db.Cosmetics.Where(s => s.Name == "Cosmetic_3").FirstOrDefault(),
                            Quantity = 4
                        },
                        new CosmeticShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_003").FirstOrDefault(),
                            Cosmetic = db.Cosmetics.Where(s => s.Name == "Cosmetic_2").FirstOrDefault(),
                            Quantity = 4
                        },
                        new CosmeticShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_003").FirstOrDefault(),
                            Cosmetic = db.Cosmetics.Where(s => s.Name == "Cosmetic_1").FirstOrDefault(),
                            Quantity = 4
                        },
                        new CosmeticShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_003").FirstOrDefault(),
                            Cosmetic = db.Cosmetics.Where(s => s.Name == "Cosmetic_3").FirstOrDefault(),
                            Quantity = 4
                        },
                        new CosmeticShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_004").FirstOrDefault(),
                            Cosmetic = db.Cosmetics.Where(s => s.Name == "Cosmetic_3").FirstOrDefault(),
                            Quantity = 4
                        },
                        new CosmeticShoppingCart {
                            ShoppingCart = db.ShoppingCarts.Where(s => s.USCI == "ShoppingCartUSCI_006").FirstOrDefault(),
                            Cosmetic = db.Cosmetics.Where(s => s.Name == "Cosmetic_1").FirstOrDefault(),
                            Quantity = 4
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
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_1").FirstOrDefault()
                        },
                        new Score {
                            CustomerSatisfaction = 2,
                            PackingAverageTimeInSeconds = 13,
                            RankAmongPharmacies = 2,
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_2").FirstOrDefault()
                        },
                        new Score {
                            CustomerSatisfaction = 5,
                            PackingAverageTimeInSeconds = 12,
                            RankAmongPharmacies = 3,
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_3").FirstOrDefault()
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
                            Order = db.Orders.Where(p => p.UOI == "OrderUOI_1").FirstOrDefault()
                        },
                        new Occurrence {
                            CustomerHasRequestedOrderOn = DateTime.Now,
                            Order = db.Orders.Where(p => p.UOI == "OrderUOI_2").FirstOrDefault()
                        },
                        new Occurrence {
                            CustomerHasRequestedOrderOn = DateTime.Now,
                            Order = db.Orders.Where(p => p.UOI == "OrderUOI_3").FirstOrDefault()
                        },
                        new Occurrence {
                            CustomerHasRequestedOrderOn = DateTime.Now,
                            Order = db.Orders.Where(p => p.UOI == "OrderUOI_4").FirstOrDefault()
                        },
                        new Occurrence {
                            CustomerHasRequestedOrderOn = DateTime.Now,
                            Order = db.Orders.Where(p => p.UOI == "OrderUOI_5").FirstOrDefault()
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
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_1").FirstOrDefault()
                        },
                        new Account {
                            IBAN = "IBAN_2",
                            AccountOwnerName = "AccountOwnerName_2",
                            BankName = "BankName_2",
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_2").FirstOrDefault()
                        },
                        new Account {
                            IBAN = "IBAN_3",
                            AccountOwnerName = "AccountOwnerName_3",
                            BankName = "BankName_3",
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_3").FirstOrDefault()
                        },
                        new Account {
                            IBAN = "IBAN_4",
                            AccountOwnerName = "AccountOwnerName_4",
                            BankName = "BankName_4",
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_4").FirstOrDefault()
                        },
                        new Account {
                            IBAN = "IBAN_5",
                            AccountOwnerName = "AccountOwnerName_5",
                            BankName = "BankName_5",
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_5").FirstOrDefault()
                        },
                        new Account {
                            IBAN = "IBAN_6",
                            AccountOwnerName = "AccountOwnerName_6",
                            BankName = "BankName_6",
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_6").FirstOrDefault()
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
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_1").FirstOrDefault(),
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_2",
                            CommissionCoefficient = 0.05,
                            Date = DateTime.Now,
                            NumberOfOrders = 12,
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_5").FirstOrDefault(),
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_3",
                            CommissionCoefficient = 0.06,
                            Date = DateTime.Now,
                            NumberOfOrders = 23,
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_1").FirstOrDefault(),
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_4",
                            CommissionCoefficient = 0.07,
                            Date = DateTime.Now,
                            NumberOfOrders = 62,
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_3").FirstOrDefault(),
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_5",
                            CommissionCoefficient = 0.08,
                            Date = DateTime.Now,
                            NumberOfOrders = 21,
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_6").FirstOrDefault(),
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_6",
                            CommissionCoefficient = 0.09,
                            Date = DateTime.Now,
                            NumberOfOrders = 30,
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_2").FirstOrDefault(),
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_7",
                            CommissionCoefficient = 0.1,
                            Date = DateTime.Now,
                            NumberOfOrders = 16,
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_2").FirstOrDefault(),
                            HasBeenSettled = false
                        },
                        new Settle {
                            USI = "SettleUSI_8",
                            CommissionCoefficient = 0.11,
                            Date = DateTime.Now,
                            NumberOfOrders = 19,
                            Pharmacy = db.Pharmacies.Where(p => p.UPI == "PharmacyUPI_3").FirstOrDefault(),
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
                if(db.TextMessages.Count() == 0)
                {
                    db.TextMessages.AddRange(
                        new TextMessage {
                            Date = DateTime.Now,
                            Customer = db.Customers.Where(q => q.Email == "CustomerE@1.com").FirstOrDefault(),
                            HasBeenLocked = false,
                            VerificationCode = "VerificationCode_1"
                        },
                        new TextMessage {
                            Date = DateTime.Now,
                            Customer = db.Customers.Where(q => q.Email == "CustomerE@2.com").FirstOrDefault(),
                            HasBeenLocked = false,
                            VerificationCode = "VerificationCode_2"
                        },
                        new TextMessage {
                            Date = DateTime.Now,
                            Customer = db.Customers.Where(q => q.Email == "CustomerE@3.com").FirstOrDefault(),
                            HasBeenLocked = true,
                            VerificationCode = "VerificationCode_3"
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