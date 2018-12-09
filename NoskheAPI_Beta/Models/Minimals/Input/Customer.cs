using System;

namespace NoskheAPI_Beta.Models.Minimals.Input
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; } // lozum nadare bara taghyeer
        public string ProfilePictureUrl { get; set; }
    }
}