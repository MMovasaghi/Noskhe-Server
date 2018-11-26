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
    class Descriptive
    {
        public bool Success { get; set; }
        // AfterMVP: More detailed error which client can distinguish.
        // public ErrorType ErrorType { get; set; }
        public string Message { get; set; }
    }
}