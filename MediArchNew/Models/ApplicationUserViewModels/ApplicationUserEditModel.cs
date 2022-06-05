using System;

namespace MediArch.Models.ApplicationUserViewModels
{
    public class ApplicationUserEditModel
    {
        public ApplicationUserEditModel()
        {
            // EF
        }
        
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }
        
        public string PhoneNumber { get; set; }

        public string Title { get; set; }

        public string CabinetAddress { get; set; }

        public ApplicationUserEditModel(string id, string firstName, string lastName, DateTime birthDate, string title, string cabinetAdress, string phoneNumber)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            Title = title;
            CabinetAddress = cabinetAdress;
        }
    }
}
