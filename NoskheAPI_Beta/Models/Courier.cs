using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models
{
    public class Courier
    {
        public int CourierId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisterationDate { get; set; }
        public string NameOfFather { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string MelliNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime ProfilePictureUploadDate { get; set; }
        public bool IsAvailableNow { get; set; } // baraye tashkhise inke on/off hast
        public bool IsBusy { get; set; } // baraye inke vaghti on hast vali saresh sholughe
        /*
            n Order
        */
        public List<Order> Orders { get; set; }
    }
}