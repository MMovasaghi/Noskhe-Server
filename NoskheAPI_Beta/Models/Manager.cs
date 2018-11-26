using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NameOfFather { get; set; }
        public string MelliNumber { get; set; } // hamchenin betavanad panel modiriati khod ra dashte bashad
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime ProfilePictureUploadDate { get; set; }
        /*
            n Pharmacy
        */
        public List<Pharmacy> Pharmacies { get; set; }
    }
}