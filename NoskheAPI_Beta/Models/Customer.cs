using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisterationDate { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime ProfilePictureUploadDate { get; set; }
        public decimal Money { get; set; }
        /*
            n ShoppingCart
            1 TextMessage
        */
        public List<ShoppingCart> ShoppingCarts { get; set; }
        public TextMessage TextMessage { get; set; }
    }
}