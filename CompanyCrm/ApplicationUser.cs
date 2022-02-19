using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CompanyCrm
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }   
        public string GoogleId { get; set; }
    }
}