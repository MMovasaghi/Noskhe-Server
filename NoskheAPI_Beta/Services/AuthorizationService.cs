using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoskheAPI_Beta.Services
{
    public interface IAuthorizationService
    {
        bool CheckNotExpired(DateTime toTime);
        int ClientId { get; set; }
    }
    class AuthorizationService : IAuthorizationService
    {
        public int ClientId { get; set; }

        public bool CheckNotExpired(DateTime toTime)
        {
            if(DateTime.UtcNow > toTime) return false;
            return true;
        }
    }
}