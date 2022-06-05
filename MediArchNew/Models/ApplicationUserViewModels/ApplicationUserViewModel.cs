using System;

namespace MediArch.Models.ApplicationUserViewModels
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Title { get; set; }

        public string CabinetAddress { get; set; }
    }
}
