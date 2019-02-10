namespace NoskheAPI_Beta.Models.Android
{
    public class AuthenticateTemplate
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AddNewTemplate
    {
        public Minimals.Input.Customer CustomerObj { get; set; }        
    }
    public class AddNewShoppingCartTemplate
    {
        public Minimals.Input.ShoppingCart ShoppingCartObj { get; set; }        
    }
    public class EditExistingTemplate
    {
        public Minimals.Input.Customer CustomerObj { get; set; }
    }
    public class PhoneTemplate
    {
        public string Phone { get; set; }
    }
    public class VerifyPhoneTemplate
    {
        public string Phone { get; set; }
        public string VerificationCode { get; set; }
    }
    public class ResetPasswordTemplate
    {
        public string NewPassword { get; set; }
    }
}