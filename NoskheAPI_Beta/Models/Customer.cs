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
        public bool IsPhoneValidated { get; set; }
        public bool IsEmailValidated { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime ProfilePictureUploadDate { get; set; }
        public int Money { get; set; }
        /*
            n ShoppingCart
            1 TextMessage
        */
        public List<ShoppingCart> ShoppingCarts { get; set; }
        public List<CustomerTextMessage> CustomerTextMessages { get; set; }
        public CustomerToken CustomerToken { get; set; }
        public CustomerResetPasswordToken CustomerResetPasswordToken { get; set; }
        public CustomerHubMap CustomerHubMap { get; set; } // TODO: not needed for now, after mvp
        public CustomerNotification CustomerNotification { get; set; } // should it not be sent to customer, he or she will check and get the latest notifications
    }
}