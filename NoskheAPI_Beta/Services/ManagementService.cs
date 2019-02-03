using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NoskheAPI_Beta.CustomExceptions.Management;
using NoskheAPI_Beta.Models;
using NoskheAPI_Beta.Models.Response;
using NoskheAPI_Beta.Settings.ResponseMessages.Management;

namespace NoskheAPI_Beta.Services
{
    public interface IManagementService
    {
        ResponseTemplate AddNewMedicine(Medicine newMedcine);
        ResponseTemplate EditMedicine(Medicine existingMedcine);
        object ShowAllMedicines();
    }
    class ManagementService : IManagementService
    {
        private static NoskheAPI_Beta.Models.NoskheContext db = new NoskheAPI_Beta.Models.NoskheContext();
        public ResponseTemplate AddNewMedicine(Medicine newMedcine)
        {
            try
            {
                Medicine newMed = new Medicine() { Name = newMedcine.Name, Price = 100, ProductPictureUploadDate = DateTime.Now, Type = newMedcine.Type };
                db.Medicines.Add(newMed);
                db.SaveChanges();
                return new ResponseTemplate
                {
                    Success = true
                };
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public ResponseTemplate EditMedicine(Medicine existingMedcine)
        {
            try
            {
                var foundMedicine = db.Medicines.Where(q => q.MedicineId == existingMedcine.MedicineId).FirstOrDefault();
                if(foundMedicine == null)
                {
                    throw new NoMedicinesAvailableException(ErrorCodes.NoMedicinesAvailableExceptionMsg);
                }

                foundMedicine.Name = existingMedcine.Name;
                foundMedicine.Price = existingMedcine.Price;
                foundMedicine.Type = existingMedcine.Type;
                foundMedicine.ProductPictureUrl = existingMedcine.ProductPictureUrl;
                foundMedicine.ProductPictureUploadDate = existingMedcine.ProductPictureUploadDate;

                db.SaveChanges();
                return new ResponseTemplate
                {
                    Success = true
                };
            }
            catch(DbUpdateException)
            {
                throw new DatabaseFailureException(ErrorCodes.DatabaseFailureExceptionMsg);
            }
        }

        public object ShowAllMedicines()
        {
            return db.Medicines.ToList();
        }
    }
}
