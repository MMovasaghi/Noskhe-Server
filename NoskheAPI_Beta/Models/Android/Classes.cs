namespace NoskheAPI_Beta.Models.Android
{
    public class AuthenticateTemplate
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AuthenticateByPhoneTemplate
    {
        public string Phone { get; set; }
    }
    public class SendSmsAuthenticationCodeTemplate
    {
        public string Phone { get; set; }
    }
    public class VerifySmsAuthenticationCodeTemplate
    {
        public string Phone { get; set; }
        public string VerificationCode { get; set; }
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
}