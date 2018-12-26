using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoskheAPI_Beta.Models.BaseClasses
{
    public class PersonBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
