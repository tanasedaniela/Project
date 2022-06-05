using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediArch.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

        public IEnumerable<IFormFile> File { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Title { get; set; }

        public string CabinetAddress { get; set; }

    }
}
