using System;
using Microsoft.AspNetCore.Identity;

namespace MediArch.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }
        
        public string Title { get; set; }

        public string CabinetAddress { get; set; }

        public bool ActiveAccount { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
