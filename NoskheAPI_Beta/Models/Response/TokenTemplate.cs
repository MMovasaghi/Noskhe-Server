using System;

namespace NoskheAPI_Beta.Models.Response
{
    public class TokenTemplate
    {
        private long posixTime;
        public string Token { get; set; }
        public long ExpirationTime { get; set; }
    }
}