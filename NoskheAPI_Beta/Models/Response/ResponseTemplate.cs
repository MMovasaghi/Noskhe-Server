namespace NoskheAPI_Beta.Models.Response
{
    // AfterMVP: More detailed error which client can distinguish.
    // public enum ErrorType
    // {
    //     API,
    //     Server,
    //     Database,
    //     Unknown
    // }
    public class ResponseTemplate
    {
        public bool Success { get; set; }
        // AfterMVP: More detailed error which client can distinguish.
        // public ErrorType ErrorType { get; set; }
        public string Error { get; set; }
    }
}